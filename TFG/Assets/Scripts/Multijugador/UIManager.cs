using TMPro;
using Unity.Netcode;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public TextMeshProUGUI playersInGameText;
    
    private void Start()
    {   
        // Since we'll be changing between different Unity applications
        // This helps in seeing which window is focused
        Cursor.visible = true;
    }

    private void Update()
    {
        playersInGameText.text = $"Current players: {PlayersManager.singleton.CurrentPlayers}";
    }

    public async void StartHost()
    {
        
        // True => Connection success
        if (NetworkManager.Singleton.StartHost())
        {
            Logger.singleton.LogInfo("Host started...");
        }
        else
        {
            Logger.singleton.LogInfo("Host could not be started...");
        }
    }

    public async void StartClient()
    {
        // True => Connection success
        if (NetworkManager.Singleton.StartClient())
        {
            Logger.singleton.LogInfo("Client started...");
        }
        else
        {
            Logger.singleton.LogInfo("Client could not be started...");
        }
    }

    public void StartServer()
    {
        // True => Connection success
        if (NetworkManager.Singleton.StartServer())
        {
            Logger.singleton.LogInfo("Server started...");
        }
        else
        {
            Logger.singleton.LogInfo("Server could not be started...");
        }
    }

}
