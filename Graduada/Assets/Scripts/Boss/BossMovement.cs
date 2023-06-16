using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMovement : MonoBehaviour
{
 
    public float lookRadius = 20f;
    public float life = 100.00f;
    NavMeshAgent agent;

    public BossAtack miraPJ;
    Transform target;
    GameObject jugador;
    private Animator _animator;
   

    // Start is called before the first frame update
    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
        target = jugador.transform;
         agent = GetComponent<NavMeshAgent>();
        _animator = gameObject.GetComponent<Animator>();
        miraPJ = FindObjectOfType<BossAtack>();
    }

    // Update is called once per frame
    void Update()
    {
     //   float distance = Vector3.Distance(target.position, transform.position);
   
       if(miraPJ.miraPJ) transform.LookAt(jugador.transform);        
     /*  
       if (distance <= lookRadius)
        {
            _animator.SetBool("Detectado", true);
            //Debug.Log("Voy a correr");
            if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Correr")){
               // Debug.Log("Corro");
            }
            agent.SetDestination(target.position);
        }  */
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

    }
}
