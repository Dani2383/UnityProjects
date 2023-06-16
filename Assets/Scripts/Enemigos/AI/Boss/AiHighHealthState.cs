using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AihighHealthState : AiState {
    public PathfindingA enemyPathfinding;
    public AiStateId GetId() {
        return AiStateId.HighHealth;
    }

    public void Enter(AiAgent agent) {
        enemyPathfinding = agent.GetComponent<PathfindingA>();
        enemyPathfinding.chasing = true;
        Debug.Log("Estoy en High health");

    }

    public void Update(AiAgent agent) {

        if (DialogueManager.GetInstance().DialogueIsPlaying) {
            enemyPathfinding.chasing = false;
            return;
        }

        if(UnityEngine.Random.Range(0.0f, 1000.0f) <= 15.0f) {
            if(UnityEngine.Random.Range(0.0f, 10.0f) <= 5.0f) {
                if (agent.chargeAttackReloaded) {
                    agent.stateMachine.ChangeState(AiStateId.ChargeAttack);
                }
            }else {
                if (agent.rangedAttackReloaded) {
                    agent.attackSelected = AiAgent.RangedAttacks.HHRangedAttack;
                    agent.stateMachine.ChangeState(AiStateId.RangedAttacks);
                }
            }
        }


        if (agent.knockback) {
            enemyPathfinding.chasing = false;
            if (agent.playerLookingRight) agent.rb.velocity = new Vector2(agent.knockbackDistance * Time.deltaTime, 0f);
            else agent.rb.velocity = new Vector2(-agent.knockbackDistance * Time.deltaTime, 0f);
        } else {
            enemyPathfinding.chasing = true;
        }
        


    }


    public void Exit(AiAgent agent) {
        enemyPathfinding = agent.GetComponent<PathfindingA>();
        enemyPathfinding.chasing = false;
    }




}

