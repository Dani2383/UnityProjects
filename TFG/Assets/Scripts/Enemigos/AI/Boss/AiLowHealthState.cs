using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AiLowHealthState : AiState {
    public PathfindingA enemyPathfinding;
    public GameObject[] throwAttackSpawners;
    public AiStateId GetId() {
        return AiStateId.LowHealth;
    }

    public void Enter(AiAgent agent) {
        enemyPathfinding = agent.GetComponent<PathfindingA>();
        throwAttackSpawners = agent.throwAttackSpawners;
        enemyPathfinding.chasing = true;
        Debug.Log("Estoy en low health");

    }

    public void Update(AiAgent agent) {

        if (DialogueManager.GetInstance().DialogueIsPlaying) {
            enemyPathfinding.chasing = false;
            return;
        }

        if (UnityEngine.Random.Range(0.0f, 1000.0f) <= 15.0f) {
            if (UnityEngine.Random.Range(0.0f, 10.0f) <= 5.0f) {
                if (agent.chargeAttackReloaded) {
                    agent.stateMachine.ChangeState(AiStateId.ChargeAttack);
                }
            } else {
                if (agent.rangedAttackReloaded) {
                    agent.attackSelected = AiAgent.RangedAttacks.LHRangedAttack;
                    agent.stateMachine.ChangeState(AiStateId.RangedAttacks);
                }
            }
        } else if (UnityEngine.Random.Range(0.0f, 1000.0f) <= 5.0f && agent.throwAttackReloaded) {
            agent.attackSelected = AiAgent.RangedAttacks.ThrowAttack;
            agent.stateMachine.ChangeState(AiStateId.RangedAttacks);
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
        enemyPathfinding = agent.GetComponent<PathfindingA>(); ;
        enemyPathfinding.chasing = false;
    }
}