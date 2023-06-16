using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiIdleState : AiState
{
    public AiStateId GetId()
    {
        return AiStateId.Idle;
    }
    public void Enter(AiAgent agent){}



    public void Update(AiAgent agent)
    {
        if (agent.transform.CompareTag("Boss")) agent.rb.isKinematic = true;
        if(agent.PlayerOnFOV(AiStateId.Idle)) agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
        if(agent.OnRangeOfDecoy()) agent.stateMachine.ChangeState(AiStateId.Decoy);


        if (agent.knockback){
            if(agent.playerLookingRight) agent.rb.velocity = new Vector2(agent.knockbackDistance * Time.deltaTime, 0f);
            else agent.rb.velocity = new Vector2(-agent.knockbackDistance * Time.deltaTime, 0f);
            agent.ChangeWithDelay(AiStateId.ChasePlayer, 0.1f);
        }         

    }
    public void Exit(AiAgent agent){
        if (agent.transform.CompareTag("Boss")) agent.rb.isKinematic = false;
    }
}
