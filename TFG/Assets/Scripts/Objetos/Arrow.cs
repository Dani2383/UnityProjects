using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Vector3 mousePosition;
    private Rigidbody2D rb;
    public float force = 0.2f;
    Vector3 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Flecha");
        rb = GetComponent<Rigidbody2D>();
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
        StartCoroutine(Disappear());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log(other.gameObject.transform.name);
     }
    private IEnumerator Disappear(){
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }


}

