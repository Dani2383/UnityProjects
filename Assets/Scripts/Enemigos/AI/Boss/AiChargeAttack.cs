using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiChargeAttack : AiState {
    Vector3 playerDirection;
    float speed;
    public AiStateId GetId() {
        return AiStateId.ChargeAttack;
    }
    public void Enter(AiAgent agent) {
        agent.animator.SetTrigger("Attack");
        AudioManager.instance.PlaySound("Boss Charge");
        speed = 150f;
        agent.StopCharging();
        playerDirection = agent.player.transform.position - agent.transform.position;
    }

    public void Update(AiAgent agent) {
        agent.GetComponent<PathfindingA>().chasing = false;
        if (agent.chargeAttackReloaded) {
            agent.isCharging = true;
            agent.rb.velocity = playerDirection.normalized * speed * Time.deltaTime;
        } else agent.ChangeToCurrentPhase();

        
    }

    public void Exit(AiAgent agent) {
        agent.GetComponent<PathfindingA>().chasing = true;
        
    }


}
