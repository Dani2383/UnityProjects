using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillArrow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(){
        //Physics2D.IgnoreLayerCollision(6,8, true);
    }
    private void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "Flecha"){
            Destroy(col.gameObject);
        }
    }
}
