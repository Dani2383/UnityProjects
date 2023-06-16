using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AiDecoyState : AiState
{
    Transform decoyTransform;
    public AiStateId GetId()
    {
        return AiStateId.Decoy;
    }
    public void Enter(AiAgent agent){
        
        decoyTransform = GameObject.FindWithTag("Decoy").transform;
        if(decoyTransform == null) agent.stateMachine.ChangeState(agent.initialState);
        agent.animator.SetBool("isMoving", true);

    }
    public void Update(AiAgent agent){

        if (DialogueManager.GetInstance().DialogueIsPlaying) {
            agent.animator.SetBool("isMoving", false);
            return;
        }

        agent.animator.SetBool("isMoving", true);

        try {
            if(!agent.OnRangeOfDecoy() || decoyTransform == null) agent.stateMachine.ChangeState(agent.initialState);
            if(agent.PlayerOnFOV(AiStateId.Decoy)){
                agent.stateMachine.ChangeState(AiStateId.ChasePlayer);  
            } 
            if(agent.facingRight && decoyTransform.position.x < agent.transform.position.x) agent.ChangeFacingDirection();
            else if(!agent.facingRight && decoyTransform.position.x > agent.transform.position.x) agent.ChangeFacingDirection();

            agent.rb.velocity = new Vector2((decoyTransform.position - agent.transform.position).normalized.x * agent.patrolSpeed * Time.deltaTime, agent.rb.velocity.y);

        } catch(Exception e){ if(decoyTransform == null) agent.stateMachine.ChangeState(agent.initialState);}
        
        


    }

    public void Exit(AiAgent agent){
        agent.animator.SetBool("isMoving", false);
    }



}


