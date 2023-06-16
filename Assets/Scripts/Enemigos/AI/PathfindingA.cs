using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class PathfindingA : MonoBehaviour
{

    [NonSerialized] public Transform target;
    public float speed;
    public float nextWaypointDistance = 3f;
    public bool chasing = false;
    public Vector2 direction;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    Seeker seeker;
    AiAgent agent;
    Rigidbody2D rb;

    void Start()
    {
        if(this.transform.name == "Skeleton") speed = 40;
        if(this.transform.name == "Boss") speed = 50;
        else speed = 60;
        seeker = GetComponent<Seeker>();
        agent = GetComponent<AiAgent>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
        InvokeRepeating("UpdatePath", 0f, 0.1f);
        
    }

    void UpdatePath(){
        if (seeker.IsDone()) seeker.StartPath(rb.position, target.position, OnPathComplete);
    }
    void OnPathComplete(Path p){
        // Función que se llama una vez se calcula el camino, si no hay errores se coge ese camino
        // y reiniciamos el progreso en el mismo.
        if(!p.error){
            path = p;
            currentWaypoint = 0;
        }
    }

    // void Update(){
    //     //if(Math.Abs(rb.velocity.x) < 0.01f) rb.velocity = new Vector2(0f, rb.velocity.y);

    // }
    void FixedUpdate()
    {
        if (chasing)
        {
            if(path == null) return;
            //Comprobamos que el waypoint en el que nos situamos no sea mayor que la cantidad de waypoints exstentes
            if(currentWaypoint >= path.vectorPath.Count){
                reachedEndOfPath = true;
                return;
            }else {
                reachedEndOfPath = false;
            }
            // Obtenemos la direccion que tiene que seguir el enemigo (pos sig waypoint - pos enemigo)
            direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position);
            if((direction.x > 0.1f && !agent.facingRight) || (direction.x < -0.1f && agent.facingRight )) agent.ChangeFacingDirection();
            direction = direction.normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            if(gameObject.transform.tag != "Boss") {
                Vector2 velocity = rb.velocity;
                velocity.x = force.x;
                rb.velocity = velocity;
            }else {
                rb.velocity = force;
            }
            

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
            if(distance < nextWaypointDistance) currentWaypoint++;
        }
    }
}
