using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiRangedAttacksState : AiState {
    public PathfindingA enemyPathfinding;
    Vector2[] ballPos, ballSpeed;
    GameObject[] ballObjects;

    public AiStateId GetId() {
        return AiStateId.RangedAttacks;
    }
    public void Enter(AiAgent agent) {
        agent.rb.isKinematic = true;
        enemyPathfinding = agent.GetComponent<PathfindingA>();
        enemyPathfinding.chasing = false;
        switch (agent.attackSelected) {
            case AiAgent.RangedAttacks.HHRangedAttack:
                AudioManager.instance.PlaySound("Boss Magic");
                ballPos = new Vector2[4];
                ballSpeed = new Vector2[4];
                ballObjects = new GameObject[4];
                HHRangedAttack(agent);
                break;
            case AiAgent.RangedAttacks.LHRangedAttack:
                AudioManager.instance.PlaySound("Boss Magic");
                ballPos = new Vector2[8];
                ballSpeed = new Vector2[8];
                ballObjects = new GameObject[8];
                LHRangedAttack(agent);
                break;
            case AiAgent.RangedAttacks.ThrowAttack:
                agent.ThrowAttack();
                break;
            default:
                break;
        }
        agent.ChangeToCurrentPhase();
    }

    public void Exit(AiAgent agent) {
        enemyPathfinding.chasing = true;
        agent.rb.isKinematic = false;
    }


    public void Update(AiAgent agent) {
        
    }


    void HHRangedAttack(AiAgent agent) {
        agent.rangedAttackReloaded = false;
        ballPos[0] = new Vector2(agent.transform.position.x - 0.1f, agent.transform.position.y);
        ballPos[1] = new Vector2(agent.transform.position.x + 0.1f, agent.transform.position.y);
        ballPos[2] = new Vector2(agent.transform.position.x, agent.transform.position.y - 0.1f);
        ballPos[3] = new Vector2(agent.transform.position.x, agent.transform.position.y + 0.1f);

        ballSpeed[0] = new Vector2(-agent.launchSpeed, 0);
        ballSpeed[1] = new Vector2(agent.launchSpeed, 0);
        ballSpeed[2] = new Vector2(0, -agent.launchSpeed);
        ballSpeed[3] = new Vector2(0, agent.launchSpeed);

        for (int i = 0; i < ballPos.Length; i++) {
            ballObjects[i] = GameObject.Instantiate(agent.energyBall, ballPos[i], Quaternion.identity) as GameObject;
            ballObjects[i].GetComponent<Rigidbody2D>().AddRelativeForce(ballSpeed[i]);
        }

        agent.ReloadRangedAttack();
    }

    void LHRangedAttack(AiAgent agent) {
        agent.rangedAttackReloaded = false;
        ballPos[0] = new Vector2(agent.transform.position.x - 0.1f, agent.transform.position.y);
        ballPos[1] = new Vector2(agent.transform.position.x + 0.1f, agent.transform.position.y);
        ballPos[2] = new Vector2(agent.transform.position.x, agent.transform.position.y - 0.1f);
        ballPos[3] = new Vector2(agent.transform.position.x, agent.transform.position.y + 0.1f);

        ballPos[4] = new Vector2(agent.transform.position.x - 0.1f, agent.transform.position.y - 0.1f);
        ballPos[5] = new Vector2(agent.transform.position.x + 0.1f, agent.transform.position.y - 0.1f);
        ballPos[6] = new Vector2(agent.transform.position.x + 0.1f, agent.transform.position.y + 0.1f);
        ballPos[7] = new Vector2(agent.transform.position.x - 0.1f, agent.transform.position.y + 0.1f);

        ballSpeed[0] = new Vector2(-agent.launchSpeed, 0);
        ballSpeed[1] = new Vector2(agent.launchSpeed, 0);
        ballSpeed[2] = new Vector2(0, -agent.launchSpeed);
        ballSpeed[3] = new Vector2(0, agent.launchSpeed);

        ballSpeed[4] = new Vector2(-agent.launchSpeed, -agent.launchSpeed);
        ballSpeed[5] = new Vector2(agent.launchSpeed, -agent.launchSpeed);
        ballSpeed[6] = new Vector2(agent.launchSpeed, agent.launchSpeed);
        ballSpeed[7] = new Vector2(-agent.launchSpeed, agent.launchSpeed);

        for (int i = 0; i < ballPos.Length; i++) {
            ballObjects[i] = GameObject.Instantiate(agent.energyBall, ballPos[i], Quaternion.identity) as GameObject;
            ballObjects[i].GetComponent<Rigidbody2D>().AddRelativeForce(ballSpeed[i]);
        }

        agent.ReloadChargeAttack();
    }
}
