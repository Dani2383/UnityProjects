using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AiChaseState : AiState
{
    public PathfindingA enemyPathfinding;
    public AiStateId GetId(){
        return AiStateId.ChasePlayer;
    }

    public void Enter(AiAgent agent){
        enemyPathfinding = agent.GetComponent<PathfindingA>();
        agent.animator.SetBool("isMoving", true);
        enemyPathfinding.chasing = true;
       
    }

    public void Update(AiAgent agent){
        
        if (DialogueManager.GetInstance().DialogueIsPlaying) {
            agent.animator.SetBool("isMoving", false);
            enemyPathfinding.chasing = false;
            return;
        }

        agent.animator.SetBool("isMoving", true);

        Vector3 playerDirection = agent.playerTransform.position - agent.transform.position;
        if ((playerDirection.magnitude > agent.maxSightDistance + 0.2f) || agent.player.GetComponent<Movement>().isHidded == true){
            enemyPathfinding.chasing = false;
            agent.stateMachine.ChangeState(AiStateId.Patrol);
        }
        else {
            Collider2D player = Physics2D.OverlapCircle(agent.attackPos.position, 0.15f, agent.playerLayerMask);
            if(player != null){
                enemyPathfinding.chasing = false;
                agent.stateMachine.ChangeState(AiStateId.Attack);

            } else if (agent.knockback){
                enemyPathfinding.chasing = false;
                if(agent.playerLookingRight) agent.rb.velocity = new Vector2(agent.knockbackDistance * Time.deltaTime, 0f);
                else agent.rb.velocity = new Vector2(-agent.knockbackDistance * Time.deltaTime, 0f);
            } else if(agent.IsHittingWall(agent) || agent.IsNearEdge(agent)){
                agent.animator.SetBool("isMoving", false);
                enemyPathfinding.chasing = false;
                agent.stateMachine.ChangeState(AiStateId.Patrol);
            } else{
                enemyPathfinding.chasing = true;
            } 
        }
        
            
    }


    public void Exit(AiAgent agent){
        enemyPathfinding = agent.GetComponent<PathfindingA>();
        enemyPathfinding.chasing = false;
        agent.animator.SetBool("isMoving", false);
    }


    

}

