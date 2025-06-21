using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Platform;
using Oculus.Avatar2;
using System;
//using Photon.Pun;
//using Photon.Realtime;

public class OculusStuff : MonoBehaviour
{

    public UInt64 _userId = 0;
    public string appId;
    public int playerNum;
    public Transform startRig;
    public bool serverBool;
    public string sampleAvatar = "sampleAvatar";

    [SerializeField] private NetworkGameCon netGameCon;
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
        // user ID == 0 means we want to load logged in user avatar from CDN
        Debug.LogWarning("Entro aqui no?");

        if (OvrPlatformInit.status == OvrPlatformInitStatus.NotStarted) OvrPlatformInit.InitializeOvrPlatform();

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

        Users.GetLoggedInUser().OnComplete(message => {
            if(message.IsError){
                var e = message.GetError();
                Debug.LogError(e.Message);
                OvrAvatarLog.LogError($"Error loading CDN avatar: {e.Message}. Falling back to local avatar", sampleAvatar);
            } else{
                _userId = message.Data.ID; 

                //Build multiplayer login room
                netGameCon.ConnectToFusion();
            } 
        });

    }

    public void PlayerSpawned(){
        //object[] userID0 = new object[1]{Convert.ToInt64(_userId)};
        startRig.gameObject.SetActive(false);
    }
}
