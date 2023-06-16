using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossAtack : MonoBehaviour
{    public GameObject proyectil;
    private float launchVelocity = 1200f;  //1000f
    private int shoot = 0;
    private float shootingDistance = 20f;
    private GameObject ball, ballDer, ballDel, ballAtr, ballIzq;
    Animator _animator;
    Vector3 posicionProyectil, posicionProyectilDel, posicionProyectilAtr, posicionProyectilDer, posicionProyectilIzq;
    GameObject target;
    public vidaEnemigo vida;
    public bool miraPJ;
    int attackMode = 1;
    private float time = 0.0f;
    // Start is called before the first frame update
    void Start()
    {   
        miraPJ = true;
        target = GameObject.FindGameObjectWithTag("Player");
        _animator = gameObject.GetComponent<Animator>();
        StartCoroutine(destruir(3f,null));
        
    }

    // Update is called once per frame
    void Update()
    {


        vida = FindObjectOfType<vidaEnemigo>();
        
        if(vida.health >= 5){
            attack1();
            _animator.SetBool("Fase2", false);
        } 

        
        if( vida.health < 5){
            _animator.SetBool("Fase2", true);
            attack2();
        }
       
      
    }
    //Ataque 1 simplemente dispara bolsas de basura
    void attack1(){
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (shoot == 1 && distance <= shootingDistance)
            {   
            Debug.Log("Ataco");
            _animator.SetBool("Disparo", true);
            shoot = 0;
            StartCoroutine(disparar1());
            
            }
    }
    //Ataque 2 dispara circulo 
    void attack2(){
      //float distance = Vector3.Distance(target.transform.position, transform.position);
      if (shoot == 1)
            {   
            Debug.Log("Ataco");
            _animator.SetBool("Disparo", true);
            shoot = 0;
            StartCoroutine(disparar2());
            StartCoroutine(destruir(1.3f,ball));
            }
            
            
            
    }
    //Dispara hacia adelante
    IEnumerator disparar1(){
        yield return new WaitForSeconds(0.4f);
        Vector3 desp = new Vector3(1f,1f,1f);
        posicionProyectil = new Vector3(transform.position.x + transform.forward.x * desp.x, 2,
                                        transform.position.z + transform.forward.z * desp.z);

        ball = Instantiate(proyectil, posicionProyectil, transform.rotation);
        ball.GetComponent<Rigidbody>().AddRelativeForce(new Vector3
                                                (0, 0, launchVelocity));
        _animator.SetBool("Disparo", false);
        StartCoroutine(destruir(1.3f,ball));
    }

    //Dispara en circulo
    IEnumerator disparar2(){
        yield return new WaitForSeconds(0.6f);
        Vector3 desp = new Vector3(2f,2f,2f);

        posicionProyectilDel = new Vector3(transform.position.x + transform.forward.x * desp.x, 2, transform.position.z + transform.forward.z * desp.z);
        posicionProyectilAtr = new Vector3(transform.position.x - transform.forward.x * desp.x, 2, transform.position.z - transform.forward.z * desp.z);
        posicionProyectilIzq = new Vector3(transform.position.x + transform.forward.x * desp.x, 2, transform.position.z - transform.forward.z * desp.z);
        posicionProyectilDer = new Vector3(transform.position.x - transform.forward.x * desp.x, 2, transform.position.z + transform.forward.z * desp.z);
        
        //miraPJ = false;
        //transform.RotateAround(transform.position, transform.up, Time.deltaTime * 500f);

        ballDel = Instantiate(proyectil, posicionProyectilDel, transform.rotation);
        ballAtr = Instantiate(proyectil, posicionProyectilAtr, transform.rotation);
        ballIzq = Instantiate(proyectil, posicionProyectilIzq, transform.rotation);
        ballDer = Instantiate(proyectil, posicionProyectilDer, transform.rotation);

        ballDel.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(launchVelocity, 0, launchVelocity));
        ballAtr.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(-launchVelocity, 0, -launchVelocity));
        ballIzq.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(launchVelocity, 0, -launchVelocity));
        ballDer.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(-launchVelocity, 0, launchVelocity));
        
        _animator.SetBool("Disparo", false);
    }

    IEnumerator destruir(float time, GameObject ball)
    {
        yield return new WaitForSeconds(time);
        if (ball != null) Destroy(ball);
        if(!gameObject.GetComponent<vidaEnemigo>().muerto) shoot = 1;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, shootingDistance);

    }
}
