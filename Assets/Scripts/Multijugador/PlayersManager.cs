using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayersManager : NetworkSingleton<PlayersManager>
{
    private NetworkVariable<int> _currentPlayers = new();
    // private NetworkVariable<int> _currentPlayers = new(NetworkVariableReadPermission.OwnerOnly);
    // private NetworkVariable<int> _currentPlayers = new(NetworkVariableReadPermission.Everyone);

    public int CurrentPlayers => _currentPlayers.Value;

    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += clientID =>
        {
            // This script will be in EVERY NODE, but
            // Only the server can modify this variable, so we need to ask before
            if (!IsServer) return;
            
            Logger.singleton.LogInfo($"{clientID} just connected...");
            _currentPlayers.Value += 1;

        };

        NetworkManager.Singleton.OnClientDisconnectCallback += clientID =>
        {
            // This script will be in EVERY NODE, but
            // Only the server can modify this variable, so we need to ask before
            if (!IsServer) return;
            
            Logger.singleton.LogInfo($"{clientID} just disconnected...");
            _currentPlayers.Value -= 1;

        };
    }

}
