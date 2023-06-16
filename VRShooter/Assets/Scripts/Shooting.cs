using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private OVRGrabbable ovrGrabbable;
    public OVRInput.Button shootingButton;
    public SimpleShoot simpleShoot;
    private AudioSource gunAudio;

    // Start is called before the first frame update
    void Start(){
        ovrGrabbable = GetComponent<OVRGrabbable>();
        gunAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update(){
        if(ovrGrabbable.isGrabbed && OVRInput.GetDown(shootingButton, ovrGrabbable.grabbedBy.GetController())) {
            simpleShoot.StartShoot();
            gunAudio.Play();
        }
    }
}
