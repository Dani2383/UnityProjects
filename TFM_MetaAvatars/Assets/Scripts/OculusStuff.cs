using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Platform;
using Oculus.Avatar2;
using System;
using Photon.Pun;
using Photon.Realtime;

public class OculusStuff : MonoBehaviourPunCallbacks
{

    public UInt64 _userId = 0;
    public string appId;
    public int playerNum;
    public Transform[] playerSpawnPoints = new Transform[2];
    public Transform startRig;
    public GameObject playerObject, playerPrefab;
    public bool serverBool;
    public string sampleAvatar = "sampleAvatar";
    //public StreamingAvatar _streamingAvatar;
    void Awake(){
        Debug.LogWarning("1");
        try {

            Core.AsyncInitialize();
            Entitlements.IsUserEntitledToApplication().OnComplete(EntitlementCallback);
            Debug.LogWarning("2");
        } catch(UnityException e) {

            Debug.LogError("Platform failed to initialize due to exception.");
            Debug.LogException(e);
            // Immediately quit the application.
            UnityEngine.Application.Quit();
        }
    }
    void EntitlementCallback (Message msg) {
        Debug.LogWarning("3");
    if (msg.IsError) {// User failed entitlement check
        Debug.LogWarning("4");
        UnityEngine.Application.Quit();
    } else { // User passed entitlement check
        // Log the succeeded entitlement check for debugging.
        Debug.LogWarning("Game on!");
        StartCoroutine(StartOvrPlatform());
    }
  }

    IEnumerator StartOvrPlatform(){
    Debug.Log("Entro aqui no?");
     if (OvrPlatformInit.status == OvrPlatformInitStatus.NotStarted)
        {
            OvrPlatformInit.InitializeOvrPlatform();
        }

        while (OvrPlatformInit.status != OvrPlatformInitStatus.Succeeded)
        {
            if (OvrPlatformInit.status == OvrPlatformInitStatus.Failed)
            {
                OvrAvatarLog.LogError($"Error initializing OvrPlatform. Falling back to local avatar", sampleAvatar);
                //LoadLocalAvatar();
                yield break;
            }

            yield return null;
        }

        // user ID == 0 means we want to load logged in user avatar from CDN
        if (_userId == 0){
            // Get User ID
            Users.GetLoggedInUser().OnComplete(message =>
            {
                if (!message.IsError)
                {
                    _userId = message.Data.ID;

                    //_streamingAvatar.gameObject.SetActive(true);
                    //_streamingAvatar.StartAvatar(this);
                    //Build multiplayer login room


                }
                else
                {
                    var e = message.GetError();
                    Debug.LogError(e.Message);
                    OvrAvatarLog.LogError($"Error loading CDN avatar: {e.Message}. Falling back to local avatar", sampleAvatar);
                }
            });

        }
  }

    public void ConectToServer(){

PhotonNetwork.SendRate = 30;
PhotonNetwork.SerializationRate = 20;
PhotonNetwork.AutomaticallySyncScene = true;

PhotonNetwork.ConnectUsingSettings();
}

    public override void OnConnectedToMaster()
{
    base.OnConnectedToMaster();
    Photon.Realtime.RoomOptions roomOptions = new Photon.Realtime.RoomOptions{
        MaxPlayers = 2,
        IsVisible = true,
        IsOpen = true,
    };

    PhotonNetwork.JoinOrCreateRoom("GameRoom", roomOptions, TypedLobby.Default);
}

    public override void OnCreatedRoom()
{
    base.OnCreatedRoom();
    serverBool = true;

}

    public override void OnJoinedRoom(){
        base.OnJoinedRoom();

        playerNum = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        if(playerNum <= playerSpawnPoints.Length) startRig = playerSpawnPoints[playerNum].transform;
        else startRig = playerSpawnPoints[0].transform;

        SpawnPlayer();
    }

    private void SpawnPlayer(){
        object[] userID0 = new object[1]{Convert.ToInt64(_userId)};
        playerObject = PhotonNetwork.Instantiate(playerPrefab.name, startRig.position, startRig.rotation, 0, userID0);
        startRig.gameObject.SetActive(false);
    }
}
