using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkRunnerCon : MonoBehaviour
{
    [SerializeField] private NetworkEvents netEvents;

    private void Awake(){
        FindObjectOfType<NetworkGameCon>().SetNetEvents(netEvents);
    }
}
