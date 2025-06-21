using System.Collections;
using UnityEngine;
using Oculus.Avatar2;
//using Photon.Pun;
using System;

public class StreamingAvatar : OvrAvatarEntity
{
    public OculusStuff networkController;
    public XRPlayer player;
    //PhotonView view;
    WaitForSeconds waitTime = new WaitForSeconds(.08f);
    // public GameObject mainCam;

    // Start is called before the first frame update

    protected override void Awake(){
        _userId = player.userID;
        PreloadAvatar();
        base.Awake();
    }

    public void PreloadAvatar(){

        //if im the local avatar
        if(player.authBool){
            SetIsLocal(true);
            _creationInfo.features = Oculus.Avatar2.CAPI.ovrAvatar2EntityFeatures.Preset_Default;
        } else {
            SetIsLocal(false);
            _creationInfo.features = Oculus.Avatar2.CAPI.ovrAvatar2EntityFeatures.Preset_Remote;
        }        
    }

    public void StartLoadingAvatar(){

        SetBodyTracking(FindObjectOfType<SampleInputManager>());
        SetLipSync(FindObjectOfType<OvrAvatarLipSyncContext>());
        StartCoroutine(LoadAvatarWithID());
    }
    IEnumerator LoadAvatarWithID(){
        var hasAvatarRequest = OvrAvatarManager.Instance.UserHasAvatarAsync(_userId);
        while (!hasAvatarRequest.IsCompleted) { yield return null; }

        LoadUser();
    }    

}
