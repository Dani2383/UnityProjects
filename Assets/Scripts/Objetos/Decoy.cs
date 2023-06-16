using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoy : MonoBehaviour
{

    Rigidbody2D rb;
    bool hasHitted;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if(!hasHitted){
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void OnCollisionEnter2D(Collision2D col){
        hasHitted = true;
        rb.velocity = Vector2.zero;
        StartCoroutine(Disappear());
    }

    private IEnumerator Disappear(){
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
