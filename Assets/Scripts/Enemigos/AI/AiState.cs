using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AiStateId
{ 
    ChasePlayer,
    Death,
    Idle,
    Patrol,
    Attack,
    Decoy,
    HighHealth,
    LowHealth,
    ChargeAttack,
    RangedAttacks
}
public interface AiState
{
    AiStateId GetId();
    void Enter(AiAgent agent);
    void Update(AiAgent agent);
    void Exit(AiAgent agent);
}
