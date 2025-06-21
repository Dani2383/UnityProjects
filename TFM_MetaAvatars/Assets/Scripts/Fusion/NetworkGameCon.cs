using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;

public class NetworkGameCon : MonoBehaviour, INetworkRunnerCallbacks
{
    static GameMode gameMode = 0;
    public bool serverBool;
    public NetworkObject playerPrefab;
    [SerializeField] private NetworkDebugStart networkStart;

    public void ConnectToFusion(){

        gameMode = GameMode.AutoHostOrClient;

        switch( gameMode )
        {
        case GameMode.Client:
            Debug.Log("StartClient() " + gameMode.ToString());
            networkStart.StartClient();
            break;
        case GameMode.Host:
            Debug.Log("StartHost() " + gameMode.ToString());
            networkStart.StartHost();
            break;
        case GameMode.Single:
            Debug.Log("StartSingle() " + gameMode.ToString());
            networkStart.StartSinglePlayer();
            break;
        case GameMode.Shared:
            Debug.Log("StartShared() " + gameMode.ToString());
            networkStart.StartSharedClient();
            break;
        case GameMode.AutoHostOrClient:
            Debug.LogWarning("***********StartAutoClient() " + gameMode.ToString());
            networkStart.StartAutoClient();
            break;
        }
    }
    public void SetNetEvents(NetworkEvents nEvents){
        nEvents.OnConnectedToServer.AddListener(OnConnectedToServer);
        nEvents.PlayerJoined.AddListener(OnPlayerJoined);
        nEvents.PlayerLeft.AddListener(OnPlayerLeft);
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.LogWarning("*********OnConnectedToServer");
        SpawnPlayer(runner, runner.LocalPlayer);
    }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
         Debug.LogWarning("*************OnPlayerJoined");
         if(runner.GameMode == GameMode.Host) serverBool = true;
         SpawnPlayer(runner, player);
         
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
         Debug.Log("OnPlayerLeft");
    }

    void SpawnPlayer(NetworkRunner runner, PlayerRef playerref){
        Debug.LogWarning("*****SpawnPlayer()");
        runner.Spawn(playerPrefab, null, null, playerref);
    }
#region Eventos
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        throw new NotImplementedException();
    }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        throw new NotImplementedException();
    }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }
    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        throw new NotImplementedException();
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        throw new NotImplementedException();
    }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }
    public void OnSceneLoadDone(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }
    public void OnSceneLoadStart(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        throw new NotImplementedException();
    }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        throw new NotImplementedException();
    }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }


#endregion eventos

}
