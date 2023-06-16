using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletTrigger : MonoBehaviour
{


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Target")) {
            other.GetComponent<targetOnHitted>().OnHitted();
            Destroy(gameObject);
        }
    }
}
