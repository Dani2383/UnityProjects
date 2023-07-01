using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Avatar2;
using Photon.Pun;
using System;

public class StreamingAvatar : OvrAvatarEntity
{
    public OculusStuff networkController;
    PhotonView view;
    // Start is called before the first frame update

    protected override void Awake(){
        StartLoadingAvatar();
        base.Awake();
    }
    public void StartAvatar(OculusStuff nc)
    {
        networkController = nc;
        _userId = networkController._userId;
        Debug.LogError(_userId);
        StartCoroutine(LoadAvatarWithID());
        
    }

    public void StartLoadingAvatar(){
        //get userID
        PhotonView parentView = GetComponentInParent<PhotonView>();
        object[] args = parentView.InstantiationData;
        Int64 avatarId = (Int64)args[0];
        _userId = Convert.ToUInt64(avatarId);

        //Avatar view for streaming
        view = GetComponent<PhotonView>();

        //if im the local avatar
        if(view.IsMine){
            SetIsLocal(true);
            _creationInfo.features = Oculus.Avatar2.CAPI.ovrAvatar2EntityFeatures.Preset_Default;
        } else {
            SetIsLocal(false);
            _creationInfo.features = Oculus.Avatar2.CAPI.ovrAvatar2EntityFeatures.Preset_Remote;
        }

        StartCoroutine(LoadAvatarWithID());
    }
    IEnumerator LoadAvatarWithID(){
        var hasAvatarRequest = OvrAvatarManager.Instance.UserHasAvatarAsync(_userId);
        while (!hasAvatarRequest.IsCompleted) { yield return null; }

        LoadUser();
    }
}
