using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricPlayerMovement : MonoBehaviour
{   
    CharacterController characterController;
    public Vector3 respawnPosition;
    [SerializeField]
    public float speed = 12;
    [SerializeField]
    public float jumpSpeed = 16.0f;
    [SerializeField]
    public float gravity = 40.0f;
    ProtagonistaVida vida;
    private Vector3 moveDirection = Vector3.zero, aux;
    private Vector3 forward, right, point, moveVector;
    private Animator _animator;

    [SerializeField]
    private bool boostVelAct;

    [SerializeField] private GameObject inventoryObject;
    [SerializeField] private GameObject velocityIcon;
    [SerializeField] private GameObject music; 
    [SerializeField] private GameObject noItem;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        _animator = gameObject.GetComponent<Animator>();
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
        boostVelAct = false;
    }

    void Update()
    {

        if(DialogueManager.GetInstance().dialogueIsPlaying){
            return;
        }
        if (characterController.isGrounded)
        {
            Vector3 rightMovement = right * speed * Time.deltaTime * Input.GetAxis("HorizontalStreet");
            Vector3 upMovement = forward * speed * Time.deltaTime * Input.GetAxis("VerticalStreet");

            aux = Vector3.Normalize(rightMovement + upMovement);
            moveDirection = Vector3.Normalize(rightMovement + upMovement);
            moveDirection *= speed;

            if(moveDirection != Vector3.zero){ _animator.SetBool("moving", true);}
            else{ _animator.SetBool("moving", false);}

            if (Input.GetButton("Jump"))
            {
                _animator.SetBool("jumping", true);
                moveDirection.y = jumpSpeed;
            }

            if(Input.GetButton("item2") && !boostVelAct ){
                beastPower();
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;



        characterController.Move(moveDirection * Time.deltaTime);

        if(aux != Vector3.zero){
        transform.rotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
        }

        if(_animator.GetCurrentAnimatorStateInfo(0).IsName("jump")){
            StartCoroutine(rutinaSalto());
        }

    }

    private IEnumerator rutinaSalto(){
        yield return new WaitForSeconds(0.1f);
        _animator.SetBool("jumping", false);
    }
    public void respawn(){
        vida = gameObject.GetComponent<ProtagonistaVida>();
        point = new Vector3(respawnPosition.x, respawnPosition.y + 5, respawnPosition.z);
        moveVector = transform.TransformDirection(point - transform.position);
        characterController.Move(point-transform.position);
        vida.LoseHealth();
    }

    private void beastPower(){
        if(inventoryObject.GetComponent<InventorySystem>().beast()){
            print("AAAAAAAAAAAAAAAAAA");
            StartCoroutine(esperar(10));
            print("uff");
        }else{
            StartCoroutine(noIconShow());
        }   
    }
    
    private IEnumerator esperar(int segundos){
        boostVelAct = true;
        speed = 30;
        velocityIcon.SetActive(true);
        music.GetComponent<AudioSource>().pitch = 2.0f;
        yield return new WaitForSeconds(segundos);
        speed = 12;
        boostVelAct = false;
        music.GetComponent<AudioSource>().pitch= 1.0f;
        velocityIcon.SetActive(false);
    }

    private IEnumerator noIconShow(){
        noItem.SetActive(true);
        yield return new WaitForSeconds(1);
        noItem.SetActive(false);
    }
   
}





















