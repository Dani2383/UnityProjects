using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VidaBoss : MonoBehaviour
{
  //sistema dropeo
  Rigidbody rb;
    public float pushOnDamageSpeed = 1;
    public GameObject dropSystem;
    public int health = 10;
    GameObject jugador;
    AudioSource sonido;
    // Start is called before the first frame update
    Animator _animator;
    public bool muerto = false;
 

      
    void Start()
    {
         jugador = GameObject.FindGameObjectWithTag("Player");
        dropSystem = GameObject.FindGameObjectWithTag("inventario");
        sonido = GameObject.Find("golpeEsther").GetComponent<AudioSource>();
        _animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Dano")){
            StartCoroutine(rutina());
        }
    }

    public void LoseHealth(acciones_Street street)
    {
        print("AAAAAAAAA");
        sonido.Play();
         
        pushOnDamage();
        health--;
        
        //animacion?

        if (health == 0)
        {
            _animator.SetBool("Muerto", true);
            muerto = true;
            gameObject.GetComponent<NavMeshAgent>().speed = 0;
            street.EnemyKO();
            Drop();
            StartCoroutine(muerte());
            //animacion muerte
            //Destroy(gameObject);
        }
        _animator.SetBool("Hit", true);
    }

    private IEnumerator muerte(){
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
    private IEnumerator rutina(){
        yield return new WaitForSeconds(0.2f);
        _animator.SetBool("Hit", false);

    }

    public void Drop(){
        dropSystem.GetComponent<Drops>().Drop(transform.position);
    }

    public void pushOnDamage(){      
        rb.AddForce(jugador.transform.forward * pushOnDamageSpeed,ForceMode.Impulse);
        Debug.Log("Me empujan");

        print("Forward" + jugador.transform.forward);
     /* 
       Quaternion position = jugador.transform.rotation;
      Vector3 movimiento = position * Vector3.forward;
     print(movimiento);
                   Vector3 move = movimiento * pushOnDamageSpeed;
                    print(move);  */
//transform.Translate(jugador.transform.forward * pushOnDamageSpeed * Time.deltaTime);
    }
}
