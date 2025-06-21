using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using Fusion;
public class XRPlayer : NetworkBehaviour
{
    //public XROrigin origin;
    //public MagicTractorBeam[] mtbs = new MagicTractorBeam[2];
    private OculusStuff metaCon;
    public GameObject CamRig;
    public StreamingAvatar avatar;
    public byte[] avatarBytes;
    [Networked] public UInt64 userID { get; set; }
    public string goName;
    bool enableBeam, jump;
    public bool createdBool, authBool;


    public override void Spawned(){
        Debug.Log("*****SPAWNED XRRIG");
        metaCon = FindObjectOfType<OculusStuff>();

        if(Object.HasInputAuthority){
            authBool = true;
            userID = metaCon._userId;
            RPC_ReceiveID(userID);
        } else {
            SetRemoteVars();
        }
        metaCon.PlayerSpawned();
        StartCoroutine(StartAvatar());
    }

    private void SetRemoteVars(){
        CamRig.SetActive(false);
        GetComponent<PlayerInput>().enabled = false;
    }

    IEnumerator StartAvatar(){
        
        if(Object.HasInputAuthority){
            //Hacer lo que queramos que haga el avatar
            GetComponent<DeviceBasedSnapTurnProvider>().system = FindObjectOfType<LocomotionSystem>();
            //FindObjectOfType<LocomotionSystem>().xrOrigin = origin;
            //FindObjectOfType<NetworkTeleportationArea>.player = this;
        }
        
        yield return new WaitForSeconds(2f);

        avatar.gameObject.SetActive(true);
        avatar.StartLoadingAvatar();
    }

    IEnumerator StreamAvatarData(){
        yield return new WaitForSeconds(3f);
        avatarBytes = avatar.RecordStreamData(avatar.activeStreamLod);
        RPC_AddRemoteData(avatarBytes);
        StartCoroutine(StreamAvatarData());
    }

    public void AvatarCreated(){
        if(Object.HasInputAuthority) StartCoroutine(StreamAvatarData());
        createdBool = true;
    }

    
    //Metodo para obtener el RPC mandado antes
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All, InvokeLocal = false)]
    public void RPC_AddRemoteData(byte[] aBytes){
        
        if(createdBool) avatar.ApplyStreamData(aBytes);
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.All, InvokeLocal = true)]
    public void RPC_ReceiveID(UInt64 id){
        if(Object.HasStateAuthority) userID = id;

    }


    // public void OnLeftHandGrab(InputValue value)
    // {
    //     Debug.Log("left hand grab input system");
    //     if (enableBeam)
    //     {
    //         enableBeam = false;
    //         foreach (var mtb in mtbs)
    //         {
    //             mtb.EnableBeam();
    //         }
    //     }
    // }

    // public void OnRightHandGrab(InputValue value)
    // {
    //     Debug.Log("right hand grab input system");
    //     if (enableBeam)
    //     {
    //         enableBeam = false;
    //         foreach (var mtb in mtbs)
    //         {
    //             mtb.EnableBeam();
    //         }
    //     }
    // }

    // public void OnThumbStickDown(InputValue value)
    // {
    //     if (!enableBeam)
    //     {
    //         enableBeam = true;
    //         foreach (var mtb in mtbs)
    //         {
    //             mtb.EnableBeam();
    //         }
    //     }  
    // }

}
