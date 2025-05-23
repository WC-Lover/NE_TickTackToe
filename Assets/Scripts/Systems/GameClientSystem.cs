using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;

[UpdateAfter(typeof(GoInGameClientSystem))]
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
partial struct GameClientSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntitiesReferences entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        // OnConnectedEvent
        foreach (RefRO<OnConnectedEvent> onConnectedEvent in SystemAPI.Query<RefRO<OnConnectedEvent>>())
        {
            RefRW<GameClientData> gameClientData = SystemAPI.GetSingletonRW<GameClientData>();
            if (onConnectedEvent.ValueRO.connectionId == 1)
            {
                gameClientData.ValueRW.localPlayerType = PlayerType.Cross;
            }
            else
            {
                gameClientData.ValueRW.localPlayerType = PlayerType.Circle;
            }
        }

        // GameStartedRpc
        foreach ((
            RefRO<GameStartedRpc> onConnectedEvent,
            Entity entity)
            in SystemAPI.Query<RefRO<GameStartedRpc>>().WithEntityAccess())
        {

            DOTSEventsMonoBehaviour.Instance.TriggerOnGameStarted();

            entityCommandBuffer.DestroyEntity(entity);
        }

        // GameWinRpc
        foreach ((
            RefRO<GameWinRpc> gameWinRpc,
            Entity entity)
            in SystemAPI.Query<RefRO<GameWinRpc>>().WithEntityAccess())
        {
            DOTSEventsMonoBehaviour.Instance.TriggerOnGameWin(gameWinRpc.ValueRO.playerType);
            GameClientData gameClientData = SystemAPI.GetSingleton<GameClientData>();
            if (gameWinRpc.ValueRO.playerType == gameClientData.localPlayerType)
            {
                entityCommandBuffer.Instantiate(entitiesReferences.winSFXPrefabEntity);
            }
            else
            {
                entityCommandBuffer.Instantiate(entitiesReferences.loseSFXPrefabEntity);
            }
            entityCommandBuffer.DestroyEntity(entity);
        }

        // RematchRpc
        foreach ((
            RefRO<RematchRpc> rematchRpc,
            Entity entity)
            in SystemAPI.Query<RefRO<RematchRpc>>().WithAll<ReceiveRpcCommandRequest>().WithEntityAccess())
        {
            DOTSEventsMonoBehaviour.Instance.TriggerOnGameRematch();

            entityCommandBuffer.DestroyEntity(entity);
        }

        // GameTieRpc
        foreach ((
            RefRO<GameTieRpc> gameTieRpc,
            Entity entity)
            in SystemAPI.Query<RefRO<GameTieRpc>>().WithAll<ReceiveRpcCommandRequest>().WithEntityAccess())
        {

            DOTSEventsMonoBehaviour.Instance.TriggerOnGameTie();

            entityCommandBuffer.DestroyEntity(entity);
        }

        // ClickedOnGridPositionRpc
        foreach ((
            RefRO<ClickedOnGridPositionRpc> gameTieRpc,
            Entity entity)
            in SystemAPI.Query<RefRO<ClickedOnGridPositionRpc>>().WithAll<ReceiveRpcCommandRequest>().WithEntityAccess())
        {

            entityCommandBuffer.Instantiate(entitiesReferences.placeSFXPrefabEntity);

            entityCommandBuffer.DestroyEntity(entity);
        }

        entityCommandBuffer.Playback(state.EntityManager);
    }
}
