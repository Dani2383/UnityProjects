using System;
using Fusion;
using Fusion.Sockets;
using System.Threading.Tasks;

public class FusionStartCon : NetworkDebugStart
{
    protected override Task InitializeNetworkRunner( NetworkRunner runner, GameMode gameMode, NetAddress address, SceneRef scene, Action<NetworkRunner> initialized )
    {
        return runner.StartGame( new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            SessionName = DefaultRoomName,
            Initialized = initialized,
            ObjectPool = runner.GetComponent<INetworkObjectPool>()
        } );
    }
}
