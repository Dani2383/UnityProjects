using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
//redecillas
using Unity.Netcode;
using Random = UnityEngine.Random;

public class Movement : NetworkBehaviour
{

    // Parametros -->
    float jumpSpeed = 160f; 
    float movementSpeed = 60f;
    float midAirControl = 40f;
    float duckedSpeed = 10f;
    float ladderSpeed = 60f;
    float distance = 0.15f;
    float dashSpeed = 350f;

    // Variables -->
    private Rigidbody2D rb;
    private CapsuleCollider2D capsCol;
    private BoxCollider2D boxCol;
    private BoxCollider2D colliderCabeza;
    [SerializeField] GameObject cabeza;
    private GameObject[] stairs, oneWayPlatforms;
    [SerializeField] private LayerMask groundLayerMask, terrainLayerMask, ladderLayerMask, stairLayerMask;
    public bool isLighted, inHiddingPlace, isHidded, climb, dashing, canDash, jump;
    public float waitTime;
    private Animator animator;
    [NonSerialized] public bool lookingRight;
    private GameObject shooter;
    private CameraMovement cameraMovement;
    private string[] jumpAudio = new string[] { "Jump1", "Jump2", "Jump3",""};

    private bool isMoving, isCrouched;

    //redecillas
    
    private NetworkVariable<float> _xMovement = new();
    private NetworkVariable<float> _yMovement = new();

    float xMovement = 0;
    private float _yPrev;
    private float _xPrev;
/*
    public Vector2 defaultPositionRange = new(-0.3f, 0.3f);
*/

    void Awake(){
        rb = transform.GetComponent<Rigidbody2D>();
        capsCol = transform.GetComponent<CapsuleCollider2D>();
        boxCol = transform.GetComponent<BoxCollider2D>();
        colliderCabeza = cabeza.GetComponent<BoxCollider2D>();
        animator = GameObject.FindGameObjectWithTag("Body").GetComponent<Animator>();
        lookingRight = true;
        shooter = GameObject.FindGameObjectWithTag("Shooter");
        canDash = true;
        stairs = GameObject.FindGameObjectsWithTag("Stairs").Concat(GameObject.FindGameObjectsWithTag("StairsCollider")).ToArray();
        oneWayPlatforms = GameObject.FindGameObjectsWithTag("OneWayPlatform");
    }

    void Start(){
        dashing = false;
        cameraMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
        isMoving = false;
        StartCoroutine(WalkingSounds());

        //redecillas
        /*
        if (IsClient && IsOwner)
        {
            transform.position = new Vector2(
                Random.Range(defaultPositionRange.x, defaultPositionRange.y),
                Random.Range(defaultPositionRange.x, defaultPositionRange.y));
        }
        */
    }
    void Update()
    {
        if(!IsGrounded()) colliderCabeza.enabled = false;
        else if(IsGrounded() && !isCrouched) colliderCabeza.enabled = true;

        if(jump && !Input.GetKey(KeyCode.Space) && IsGrounded()) jump = false;

        if(IsGrounded() && !OnStairs() && !inHiddingPlace && Input.GetKey(KeyCode.S) && !(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))) cameraMovement.lookingDown = true;
        else cameraMovement.lookingDown = false;
        DuckHandler();
        CoverHandler();
        OneWayPlatform();

        //redecillas
        
        
            
    }
    void FixedUpdate(){
        if (DialogueManager.GetInstance().DialogueIsPlaying) {
            rb.velocity = new Vector2(0f,0f);
            animator.SetFloat("Speed", Math.Abs(rb.velocity.x));
            return;
        }
        if (gameObject.GetComponent<PlayerState>().health > 0) {
            if (IsGrounded() && !IsDucked() && !jump && !isHidded && Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.S)) {
                RaycastHit2D rc = Physics2D.BoxCast(colliderCabeza.bounds.center, colliderCabeza.bounds.size * 0.5f, 0f, Vector2.up, .15f, groundLayerMask);
                if (rc.collider == null) {
                    jump = true;
                    rb.velocity = new Vector2(rb.velocity.x, jumpSpeed * Time.deltaTime);
                    animator.SetTrigger("Jump");
                    string audioFile = jumpAudio[Random.Range(0, jumpAudio.Length)];
                    AudioManager.instance.PlaySound(audioFile);
                }
            }
            animator.SetFloat("Speed", Math.Abs(rb.velocity.x));
            animator.SetFloat("JumpSpeed", rb.velocity.y);
            animator.SetBool("IsGrounded", (IsGrounded() || OnStairs()));


            IsClimbing();
            MovementHandler();
            DashHandler();

            // if (IsServer)
            //     UpdateServer();
        
            // if(IsClient && IsOwner)
            //     UpdateClient();
        }   
    }

    // Comprueba si esta en el suelo --> 
    private bool IsGrounded(){
        RaycastHit2D terRaycast = Physics2D.BoxCast(capsCol.bounds.center - new Vector3(0f, 0.05f, 0f), capsCol.bounds.size * 0.5f, 0f, Vector2.down, .1f, terrainLayerMask);
        RaycastHit2D terRaycast1 = Physics2D.BoxCast(boxCol.bounds.center - new Vector3(0f, 0.05f, 0f), boxCol.bounds.size * 0.5f, 0f, Vector2.down, .15f, terrainLayerMask);
        // Devuelve null si esta en el aire
        return terRaycast.collider != null || terRaycast1.collider != null;
    }

    private bool OnStairs(){
        RaycastHit2D staRaycast = Physics2D.BoxCast(capsCol.bounds.center, capsCol.bounds.size * 0.5f, 0f, Vector2.down, .1f, stairLayerMask);
        RaycastHit2D staRaycast1 = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size * 0.5f, 0f, Vector2.down, .15f, stairLayerMask);
        // Devuelve null si esta en el aire
        return staRaycast.collider != null || staRaycast1.collider != null;
    }

    // Comprueba si esta en una escalera --> 
    private void IsClimbing(){
        RaycastHit2D onLadder = Physics2D.Raycast(transform.position - new Vector3(0f, 0.05f, 0f), Vector2.up, distance, ladderLayerMask);
        if (onLadder.collider != null && onLadder.collider.tag == "Ladder") {
            if (Input.GetKey(KeyCode.W)) {
                climb = true;
                animator.SetBool("IsClimbing", true);
            }

        } else {
            climb = false;
            animator.SetBool("IsClimbing", false);
        }
    }

    // Comprueba si esta agachado --> 
    public bool IsDucked(){
        return !colliderCabeza.enabled;
    }

    
    private void MovementHandler(){
        if(Input.GetKey(KeyCode.A) && !isHidded && !dashing){
            if (GameObject.FindWithTag("Stairs") != null) {
                GameObject.FindWithTag("Stairs").GetComponent<PolygonCollider2D>().enabled = true;
                GameObject.FindWithTag("StairsCollider").GetComponent<PolygonCollider2D>().enabled = false;
            }
            capsCol.enabled = true;
            boxCol.enabled = false;
            if (lookingRight) {
                this.transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                shooter.transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
            lookingRight = false;
            if(IsGrounded() && !IsDucked()){
                rb.velocity = new Vector2(-movementSpeed * Time.deltaTime, rb.velocity.y);
                isMoving = true;
            } else if(IsGrounded() && IsDucked()){
                isMoving = true;
                rb.velocity = new Vector2(-movementSpeed * duckedSpeed * Time.deltaTime , rb.velocity.y);
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -movementSpeed, +movementSpeed) * Time.deltaTime, rb.velocity.y );
            } else{
                isMoving = false;
                rb.velocity += new Vector2(-movementSpeed * midAirControl * Time.deltaTime , 0);
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -movementSpeed, +movementSpeed) * Time.deltaTime, rb.velocity.y);
            }
        }
        if(Input.GetKey(KeyCode.D) && !isHidded && !dashing){
            if(GameObject.FindWithTag("Stairs") != null) {
                GameObject.FindWithTag("Stairs").GetComponent<PolygonCollider2D>().enabled = true;
                GameObject.FindWithTag("StairsCollider").GetComponent<PolygonCollider2D>().enabled = false;
            }
            capsCol.enabled = true;
            boxCol.enabled = false;
            if (!lookingRight) {
                this.transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                shooter.transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
            lookingRight = true;
            if(IsGrounded() && !IsDucked()){
                isMoving = true;
                rb.velocity = new Vector2(movementSpeed * Time.deltaTime, rb.velocity.y);
            } else if(IsGrounded() && IsDucked()){
                isMoving = true;
                rb.velocity = new Vector2(movementSpeed * duckedSpeed * Time.deltaTime , rb.velocity.y);
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -movementSpeed, +movementSpeed) * Time.deltaTime, rb.velocity.y);
            } else {
                isMoving = false;
                rb.velocity += new Vector2(movementSpeed * midAirControl * Time.deltaTime , 0);
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -movementSpeed, +movementSpeed) * Time.deltaTime, rb.velocity.y);
            }
        }
        if( !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) {
            isMoving = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
            if(OnStairs()){
                GameObject.FindWithTag("StairsCollider").GetComponent<PolygonCollider2D>().enabled = true;
                GameObject.FindWithTag("Stairs").GetComponent<PolygonCollider2D>().enabled = false;
                boxCol.enabled = true;
                capsCol.enabled = false;

            }
        }
        // Escalado -->
        if(climb){
            rb.velocity = new Vector2(rb.velocity.x, Input.GetAxisRaw("Vertical") * ladderSpeed * Time.deltaTime);
            rb.gravityScale = -1;
        }else rb.gravityScale = 1;
        
    }
    

   
    private void DuckHandler(){
        if(IsGrounded() && Input.GetKeyDown(KeyCode.C)){
            // Para agacharse se elimina el collider de la parte superior del cuerpo, haciendo ademas animacion de agacharse
            // Mas adelante sustituir tecla C por ctrl
            // Funciona alternando entre agachado y de pie, se puede cambiar a mantener la tecla
            if (colliderCabeza.enabled) {
                colliderCabeza.enabled = false;
                isCrouched = true;
                animator.SetBool("IsCrouched", true);
            } else {
                RaycastHit2D rc = Physics2D.BoxCast(capsCol.bounds.center, capsCol.bounds.size * 0.5f, 0f, Vector2.up, .1f, terrainLayerMask);
                if (rc.collider == null) {
                    colliderCabeza.enabled = true;
                    isCrouched = false;
                    animator.SetBool("IsCrouched", false);
                }
            }

        }
    }

    private void CoverHandler(){
         if(inHiddingPlace){
            if(Input.GetKey(KeyCode.W) && !(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))){
                isHidded = true;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                capsCol.enabled = false;
                colliderCabeza.enabled = false;
                gameObject.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
                gameObject.transform.GetChild(4).GetComponent<BoxCollider2D>().enabled = false;
                isLighted = false;
            }
         }if(isHidded){
            if(Input.GetKey(KeyCode.S)){
                isHidded = false;
                //rb.constraints = RigidbodyConstraints2D.None;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                animator.SetBool("IsCrouched", false);
                capsCol.enabled = true;
                colliderCabeza.enabled = true;
                gameObject.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
                gameObject.transform.GetChild(4).GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }

    private void OneWayPlatform(){
        if(Input.GetKey(KeyCode.S)){
            if(waitTime <= 0){
                foreach(GameObject platform in oneWayPlatforms){
                    platform.GetComponent<PlatformEffector2D>().rotationalOffset = 180f;
                }
                foreach( GameObject stair in stairs){
                    if(stair.GetComponent<PlatformEffector2D>()){
                        if(stair.GetComponent<PlatformEffector2D>().rotationalOffset == -45f) stair.GetComponent<PlatformEffector2D>().rotationalOffset = 135f;
                        else if(stair.GetComponent<PlatformEffector2D>().rotationalOffset == 45f) stair.GetComponent<PlatformEffector2D>().rotationalOffset = 225f;
                    }
                    
                }

                StartCoroutine(Straighten());
                waitTime = 0.2f;
            } else {
               waitTime -= Time.deltaTime; 
            }
        }

        if(Input.GetKeyUp(KeyCode.S)){
            waitTime =  0.2f;
        }

        if(Input.GetKey(KeyCode.W)){
            foreach(GameObject platform in oneWayPlatforms){
                float offset = platform.GetComponent<PlatformEffector2D>().rotationalOffset;
                if(offset != 0f) platform.GetComponent<PlatformEffector2D>().rotationalOffset = 0f;
            }
            foreach( GameObject stair in stairs){
                if(stair.GetComponent<PlatformEffector2D>()){
                    if(stair.GetComponent<PlatformEffector2D>().rotationalOffset == 135f) stair.GetComponent<PlatformEffector2D>().rotationalOffset = -45;
                    else if(stair.GetComponent<PlatformEffector2D>().rotationalOffset == 225f) stair.GetComponent<PlatformEffector2D>().rotationalOffset = 45f;
                }
            }
        }
    }

    private void DashHandler(){
        if (canDash && !dashing && !isCrouched && !isHidded) {
            if (Input.GetAxisRaw("Horizontal") != 0 && Input.GetKey(KeyCode.LeftShift)) {
                animator.SetBool("IsDashing", true);
                canDash = false;
                dashing = true;
                AudioManager.instance.PlaySound("Dash");
                StartCoroutine("ActivateDash");
                StartCoroutine("StopDashing");
            }
        }
        if(dashing){
            if (Input.GetAxisRaw("Horizontal") == -1) rb.velocity = new Vector2(- dashSpeed * Time.deltaTime, 0f);
            else if (Input.GetAxisRaw("Horizontal") == 1) rb.velocity = new Vector2(dashSpeed * Time.deltaTime, 0f);
        }
        
    }

    private IEnumerator Straighten(){
        yield return new WaitForSeconds(0.2f);
        foreach(GameObject platform in oneWayPlatforms){
            platform.GetComponent<PlatformEffector2D>().rotationalOffset = 0f;
        }
        foreach( GameObject stair in stairs){
            if(stair.GetComponent<PlatformEffector2D>()){
                if(stair.GetComponent<PlatformEffector2D>().rotationalOffset == 135f) stair.GetComponent<PlatformEffector2D>().rotationalOffset = -45;
                else if(stair.GetComponent<PlatformEffector2D>().rotationalOffset == 225f) stair.GetComponent<PlatformEffector2D>().rotationalOffset = 45f;
            }
        }
    }

    private IEnumerator ActivateDash() {
        yield return new WaitForSeconds(2f);
        canDash = true;
    }
    private IEnumerator StopDashing() {
        yield return new WaitForSeconds(0.04f);
        animator.SetBool("IsDashing", false);
        dashing = false;

    }

    private IEnumerator WalkingSounds() {
        while (true) {
            yield return new WaitForSeconds(0.1f);
            if (Math.Abs(rb.velocity.x) > 0.01f && !dashing && IsGrounded() && !IsDucked()) {
                AudioManager.instance.PlayFootSteps("Footstep1");
                yield return new WaitForSeconds(0.3f);
            }
            yield return new WaitForSeconds(0.1f);
            if (Math.Abs(rb.velocity.x) > 0.01f & !dashing && IsGrounded() && !IsDucked()) {    
                AudioManager.instance.PlayFootSteps("Footstep2");
                yield return new WaitForSeconds(0.3f);
            }
        }
    }


    // private void UpdateServer()
    // {
    //     // The actual movement happens on the server (and the values get synchronized across the nodes)
    //     rb.velocity = new Vector2(
    //         _xMovement.Value,
    //         rb.velocity.y
    //     );
        
    // }

    // private void UpdateClient()
    // {
    //     /*
    //     // The client cannot directly modify neither _xMovement nor _zMovement
    //     // That's because the default permissions specify that the server is the only one who can modify them
        
    //     // We just read from the keyboard and ask the server to update the NetworkVariable
    //     float xMovement = 0;

    //     if (Input.GetKey(KeyCode.A))
    //     {
    //         xMovement -= movementSpeed * Time.deltaTime;
    //     }
    //     else if (Input.GetKey(KeyCode.D))
    //     {
    //         xMovement += movementSpeed * Time.deltaTime;
    //     }

    //     if (_xPrev != xMovement)
    //     {
    //         _xPrev = xMovement;
            
    //         // Tell the server to update the client's position
    //         // The method must end with the suffix "ServerRpc"
    //         UpdateClientPositionServerRpc(xMovement);
            
    //     }
    //     */
        
    //     MovementHandler1();
        
    // }

    // // The method must end with the suffix "ServerRpc" and be annotated as [ServerRpc]
    // [ServerRpc]
    // private void UpdateClientPositionServerRpc(float xMovement)
    // {
    //     // The annotation and the prefix are mandatory
    //     // if we want the CLIENT to tell the SERVER to execute something

    //     // Since we call this from the SERVER side, then we can modify this NetworkVariable
    //     _xMovement.Value = xMovement;
    // }

 //     private void MovementHandler1(){
        
    //     if(Input.GetKey(KeyCode.A) && !isHidded && !dashing){
    //         if (GameObject.FindWithTag("Stairs") != null) {
    //             GameObject.FindWithTag("Stairs").GetComponent<PolygonCollider2D>().enabled = true;
    //             GameObject.FindWithTag("StairsCollider").GetComponent<PolygonCollider2D>().enabled = false;
    //         }
    //         capsCol.enabled = true;
    //         boxCol.enabled = false;
    //         if (lookingRight) {
    //             this.transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    //             shooter.transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    //         }
    //         lookingRight = false;
    //         if(IsGrounded() && !IsDucked()){
    //             xMovement = -movementSpeed * Time.deltaTime;
    //             isMoving = true;
    //         } else if(IsDucked()){
    //             isMoving = true;
    //             xMovement = Mathf.Clamp(-movementSpeed * duckedSpeed * Time.deltaTime, -movementSpeed, +movementSpeed) * Time.deltaTime;
    //         } else{
    //             isMoving = false;
    //             xMovement += -movementSpeed * midAirControl * Time.deltaTime;
    //             xMovement = Mathf.Clamp(xMovement, -movementSpeed, +movementSpeed) * Time.deltaTime;
    //         }
    //     }
    //     if(Input.GetKey(KeyCode.D) && !isHidded && !dashing){
    //         if(GameObject.FindWithTag("Stairs") != null) {
    //             GameObject.FindWithTag("Stairs").GetComponent<PolygonCollider2D>().enabled = true;
    //             GameObject.FindWithTag("StairsCollider").GetComponent<PolygonCollider2D>().enabled = false;
    //         }
    //         capsCol.enabled = true;
    //         boxCol.enabled = false;
    //         if (!lookingRight) {
    //             this.transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    //             shooter.transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    //         }
    //         lookingRight = true;
    //         if(IsGrounded() && !IsDucked()){
    //             isMoving = true;
    //             xMovement = movementSpeed * Time.deltaTime;
    //         } else if(IsDucked()){
    //             isMoving = true;
    //             xMovement = Mathf.Clamp(movementSpeed * duckedSpeed * Time.deltaTime, -movementSpeed, +movementSpeed) * Time.deltaTime;
    //         } else {
    //             isMoving = false;
    //             xMovement += movementSpeed * midAirControl * Time.deltaTime;
    //             xMovement = Mathf.Clamp(xMovement, -movementSpeed, +movementSpeed) * Time.deltaTime;
    //         }
    //     }
    //     if( !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) {
    //         isMoving = false;
    //         rb.velocity = new Vector2(0, rb.velocity.y);
    //         if(OnStairs()){
    //             GameObject.FindWithTag("StairsCollider").GetComponent<PolygonCollider2D>().enabled = true;
    //             GameObject.FindWithTag("Stairs").GetComponent<PolygonCollider2D>().enabled = false;
    //             boxCol.enabled = true;
    //             capsCol.enabled = false;

    //         }
    //     }
    //     // Escalado -->
    //     if(climb){
    //         rb.gravityScale = -1;
    //         rb.velocity = new Vector2(rb.velocity.x, Input.GetAxisRaw("Vertical") * ladderSpeed * Time.deltaTime);
    //     }else rb.gravityScale = 1;

    //     UpdateClientPositionServerRpc(xMovement);
        
    // }

}
