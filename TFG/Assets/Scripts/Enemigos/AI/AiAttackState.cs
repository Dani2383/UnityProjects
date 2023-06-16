using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAttackState : AiState {

    public AiStateId GetId(){
        return AiStateId.Attack;     
    }

    public void Enter(AiAgent agent){

        agent.rb.velocity = new Vector2(0f, 0f);
        agent.animator.SetTrigger("Attack");
        Collider2D player = Physics2D.OverlapCircle(agent.attackPos.position, 0.15f, agent.playerLayerMask);
        if(agent.isDead) agent.stateMachine.ChangeState(AiStateId.Death);
        AudioManager.instance.PlaySound("Enemy Attack");
        if (player != null) {
            agent.Attack();
        }
        agent.ChangeWithDelay(AiStateId.ChasePlayer, 0.75f);

    }
    public void Update(AiAgent agent){

         if (agent.knockback){
            if(agent.playerLookingRight) agent.rb.velocity = new Vector2(agent.knockbackDistance * Time.deltaTime, 0f);
            else agent.rb.velocity = new Vector2(-agent.knockbackDistance * Time.deltaTime, 0f);
        }
    }
    public void Exit(AiAgent agent){

    }
}
