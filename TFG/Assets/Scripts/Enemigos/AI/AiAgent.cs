using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAgent : MonoBehaviour
{
    public AiStateMachine stateMachine;
    public Animator animator;
    public Movement movementVariables;
    public Rigidbody2D rb;
    public AiStateId initialState;
    public GameObject player, thrownObject, energyBall;
    public GameObject[] throwAttackSpawners;
    public Transform playerTransform;
    //public Vector2 agentDirection;
    public float life, knockbackDistance;
    [NonSerialized] public bool playerLookingRight, facingRight, playerOnTheRight, knockback, playerInZone;
    [NonSerialized] public bool chargeAttackReloaded, rangedAttackReloaded, throwAttackReloaded, isCharging;
    [SerializeField] public float patrolSpeed, maxSightDistance, maxDecoyDistance, baseCastDist, launchSpeed;
    [SerializeField] public Transform castPos, attackPos;
    [SerializeField] private bool flipped;
    [NonSerialized] public bool isDead;
    public LayerMask playerLayerMask, terrainLayerMask;
    public RangedAttacks attackSelected;
    void Start()
    {
        
        if (this.transform.name == "Skeleton") patrolSpeed = 30;
        else patrolSpeed = 40;
        facingRight = true;
        if (flipped) ChangeFacingDirection();
        baseCastDist = 0.1f;
        if (this.transform.name == "Boss") life = 6;
        else life = 4;
        isDead = false;
        knockback = false;
        playerOnTheRight = false;
        //agentDirection = new Vector2(flipped ? -1f: 1f, 0); 
        player = GameObject.FindGameObjectWithTag("Player");
        movementVariables = player.GetComponent<Movement>();
        maxSightDistance = 1.5f;
        maxDecoyDistance = 1f; //0.7
        knockbackDistance = 50;
        playerInZone = false;
        chargeAttackReloaded = true;
        rangedAttackReloaded = true;
        throwAttackReloaded = true;
        isCharging = false;
        launchSpeed = 80f;

        playerTransform = player.transform;
        rb = GetComponent<Rigidbody2D>();
        animator = this.transform.GetChild(0).GetComponent<Animator>();
        if (gameObject.transform.tag == "Boss") throwAttackSpawners = GameObject.FindGameObjectsWithTag("ThrowAttackSpawners");

        //Registro de la maquina de estados -->
        stateMachine = new AiStateMachine(this);
        if(gameObject.transform.tag != "Boss") {
            stateMachine.RegisterState(new AiIdleState());
            stateMachine.RegisterState(new AiChaseState());
            stateMachine.RegisterState(new AiPatrolState());
            stateMachine.RegisterState(new AiDeathState());
            stateMachine.RegisterState(new AiAttackState());
            stateMachine.RegisterState(new AiDecoyState());
        }else {
            stateMachine.RegisterState(new AiLowHealthState());
            stateMachine.RegisterState(new AihighHealthState());
            stateMachine.RegisterState(new AiChargeAttack());
            stateMachine.RegisterState(new AiRangedAttacksState());
            stateMachine.RegisterState(new AiDeathState());
            stateMachine.RegisterState(new AiIdleState());
        }
        stateMachine.ChangeState(initialState);
        if (this.transform.name != "Boss") StartCoroutine(WalkingSounds());
    }

    public enum RangedAttacks {
        HHRangedAttack,
        LHRangedAttack,
        ThrowAttack
    }

    void Update(){
        if(!movementVariables.isLighted || movementVariables.IsDucked()){
            if(!movementVariables.isLighted && movementVariables.IsDucked()) maxSightDistance = 0.9f;
            else maxSightDistance = 1.2f;
        } else maxSightDistance = 1.5f;

        if(life <= 0){
            stateMachine.ChangeState(AiStateId.Death);  
        } 
    }
    void FixedUpdate()
    {

        stateMachine.Update();
        if (stateMachine.currentState == AiStateId.ChasePlayer)
        {
            Vector2 direction = GetComponent<PathfindingA>().direction;
            direction = new Vector2((float)Math.Ceiling(direction.x), direction.y);
            //if (direction.x == 0) agentDirection = new Vector2(-1, direction.y);
            //else agentDirection = direction;
        }
    }

//Metodos usados en los diferentes estados --> 
    public void Hitted(bool lookingRight){
        Debug.Log("Llego a hacer hitted");
        playerLookingRight = lookingRight;
        animator.SetTrigger("Hurted");

        if (this.transform.name == "Skeleton") AudioManager.instance.PlaySound("Pain Skeleton");
        else if (this.transform.name == "Boss") AudioManager.instance.PlaySound("Pain Boss");
        else AudioManager.instance.PlaySound("Pain Mushroom");

        knockback = true;
        life -= 1;
        StartCoroutine("Knockback");
        Debug.Log("Llego a hacer hitted2");

    }
    public void ChangeFacingDirection(){

        this.transform.localScale = new Vector3(-1 * this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        facingRight = !facingRight;
        //if (facingRight) this.agentDirection = new Vector2(1f, 0f);
        //else this.agentDirection = new Vector2(-1f, 0f);
    }
    public bool PlayerOnFOV(AiStateId stateId){
        
        if(player.GetComponent<Movement>().isHidded) return false;
        playerOnTheRight = playerTransform.position.x > transform.position.x;
        if((facingRight && playerOnTheRight) || (!facingRight && !playerOnTheRight)){
            //Compruebo si hay alguien a rango de vision -->
            Vector3 playerDirection = playerTransform.position - transform.position;
            if (playerDirection.magnitude > maxSightDistance + (stateId == AiStateId.ChasePlayer ? 0.2 : 0)) return false;
            else{
                RaycastHit2D hit = Physics2D.Linecast(attackPos.position, playerTransform.position, terrainLayerMask);
                Debug.DrawLine(attackPos.position, playerTransform.position);
                if(hit.collider == null){
                    //No hay nada entre el enemigo y el jugador
                    return true;
                }
            }
        }
        return false;
    }   
    public bool IsHittingWall(AiAgent agent)
    {
        bool val = false;
        float castDist = agent.baseCastDist;
        if (!agent.facingRight) castDist = -agent.baseCastDist;
        Vector3 targetPos = agent.castPos.position;
        targetPos.x += castDist;
        Debug.DrawLine(agent.castPos.position, targetPos);

        // Detecta si tiene una pared delante suya -->
        if (Physics2D.Linecast(agent.castPos.position, targetPos, 1 << LayerMask.NameToLayer("Terrain"))) val = true;
        return val;
    }
    public bool IsNearEdge(AiAgent agent)
    {
        bool val = true;
        float castDist = agent.baseCastDist;
  
        Vector3 targetPos = agent.castPos.position;
        targetPos.y -= castDist;
        Debug.DrawLine(agent.castPos.position, targetPos);

        // Detecta si tiene un hueco delante suya -->
        if (Physics2D.Linecast(agent.castPos.position, targetPos, 1 << (LayerMask.NameToLayer("Terrain")))) val = false;
        return val;
    }
    
    public bool OnRangeOfDecoy(){
        GameObject decoy = GameObject.FindWithTag("Decoy");
        if(decoy == null) return false;
        else {
            float distance = (decoy.transform.position - transform.position).magnitude;
            if(distance > maxDecoyDistance) return false;
            else return true;
        }
    }

    //Boss --> 

    public void ChangeToCurrentPhase() {
        if (life <= 2) stateMachine.ChangeState(AiStateId.LowHealth); 
        else stateMachine.ChangeState(AiStateId.HighHealth);
    }

    public void ThrowAttack() {
        throwAttackReloaded = false;
        StartCoroutine(ThrowAttackRoutine(0));
    }

    public void StopCharging() {
        StartCoroutine("StopChargingRoutine");
    }


    // Lanzadores de corutinas --> 
    public void ChangeWithDelay(AiStateId state, float delay){
        StartCoroutine(ChangeWithDelayRoutine(state, delay));
    }
    public void Die(){
        isDead = true;
        StartCoroutine("DieRoutine");
    }

    public void Attack(){
        StartCoroutine("AttackRoutine");
    }

    public void ReloadRangedAttack() {
        StartCoroutine("ReloadRangedAttackRoutine");
    }

    public void ReloadChargeAttack() {
        StartCoroutine("ReloadChargeAttackRoutine");
    }



    
 // Corutinas --> 
    private IEnumerator Knockback(){
        yield return new WaitForSeconds(0.1f);
        knockback = false;
    }
    private IEnumerator DieRoutine(){
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(0.25f);
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    private IEnumerator ChangeWithDelayRoutine(AiStateId state, float delay){
        yield return new WaitForSeconds(delay);
        this.stateMachine.ChangeState(state);
    }

    private IEnumerator AttackRoutine(){
        yield return new WaitForSeconds(0.25f);
        Collider2D col = Physics2D.OverlapCircle(this.attackPos.position, 0.15f, this.playerLayerMask);
        if(col != null) player.GetComponent<PlayerState>().TakeDamage();
    }

    private IEnumerator ReloadRangedAttackRoutine() {
        yield return new WaitForSeconds(5f);
        rangedAttackReloaded = true;
    }

    private IEnumerator ReloadChargeAttackRoutine() {
        isCharging = false;
        yield return new WaitForSeconds(5f);
        chargeAttackReloaded = true;
    }
    private IEnumerator WalkingSounds() {
        while (true) {
            if((player.transform.position - transform.position).magnitude < 2){ 
                yield return new WaitForSeconds(0.1f);
                if (Math.Abs(rb.velocity.x) > 0.01f ) {
                    AudioManager.instance.PlayFootSteps("FootstepEnemy1");
                    yield return new WaitForSeconds(0.3f);
                }
                yield return new WaitForSeconds(0.1f);
                if (Math.Abs(rb.velocity.x) > 0.01f ) {
                    AudioManager.instance.PlayFootSteps("FootstepEnemy2");
                    yield return new WaitForSeconds(0.3f);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator ThrowAttackRoutine(int waveNum) {
        if(waveNum <= 2) {
            yield return new WaitForSeconds(1f);
            for(int i = waveNum; i< throwAttackSpawners.Length; i+= 2) {
                AudioManager.instance.PlaySound("Boss Rock");
                Instantiate(thrownObject, throwAttackSpawners[i].transform.position, Quaternion.identity);
            }
            StartCoroutine(ThrowAttackRoutine(++waveNum));
        } else {
            yield return new WaitForSeconds(10f);
            throwAttackReloaded = true;
            ChangeToCurrentPhase();
        }
    }

    private IEnumerator StopChargingRoutine() {
        yield return new WaitForSeconds(2f);
        chargeAttackReloaded = false;
        isCharging = false;
    }
}
