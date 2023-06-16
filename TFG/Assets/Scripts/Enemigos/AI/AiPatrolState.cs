using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPatrolState : AiState
{
    //private bool facingRight;
    public AiStateId GetId()
    {
        return AiStateId.Patrol;
    }
    public void Enter(AiAgent agent){
        //facingRight = true;
        agent.animator.SetBool("isMoving", true);
    }

    public void Exit(AiAgent agent){
        agent.animator.SetBool("isMoving", false);
    }


    public void Update(AiAgent agent)
    {
        if (DialogueManager.GetInstance().DialogueIsPlaying) {
            agent.animator.SetBool("isMoving", false);
            return;
        }

        agent.animator.SetBool("isMoving", true);

        if (agent.IsHittingWall(agent) || agent.IsNearEdge(agent)){
                agent.ChangeFacingDirection();
        }

        if(agent.PlayerOnFOV(AiStateId.Patrol)){
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);  
        }

        if(agent.OnRangeOfDecoy()) agent.stateMachine.ChangeState(AiStateId.Decoy);

        if (agent.knockback){
            agent.ChangeWithDelay(AiStateId.ChasePlayer, 0.1f);
        }
        else {
            agent.rb.velocity = new Vector2((agent.facingRight ? 1: -1) * agent.patrolSpeed * Time.deltaTime, agent.rb.velocity.y);
        }
        
    }
   
}
