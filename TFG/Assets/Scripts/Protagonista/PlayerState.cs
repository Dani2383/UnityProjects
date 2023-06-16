using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerState : MonoBehaviour, IDataPersistence
{
    public bool canAttack, invencible;
    private Movement movement;
    public Animator animator;
    public Transform attackPosition;
    [NonSerialized] public float attackCircle;
    public LayerMask enemyLayerMask;

    public int health;
    public int numberOfHearts;
    private int currentLife;

    public GameObject[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    private Vector2 mousePos, rotation;
    private string[] damageAudio = new string[] { "Damage1", "Damage2", "Damage3"};

    public DeathMenu deathMenu;

    void Awake(){
        health = 6;
        numberOfHearts = 3;
        invencible = false;
        hearts = GameObject.FindGameObjectsWithTag("Heart");
        Cursor.visible = false;

    }
    void Start()
    {
        IEnumerable<GameObject> heartsOrdered = hearts.OrderBy(hearts => hearts.transform.name);
        hearts = heartsOrdered.ToArray();
        hearts.OrderBy(heart => heart.transform.name);
        animator = GameObject.FindGameObjectWithTag("Body").GetComponent<Animator>(); 
        deathMenu = GameObject.FindGameObjectWithTag("UI").GetComponent<DeathMenu>();
        canAttack = true;
        attackCircle = 0.25f;
        movement = GetComponent<Movement>();
    }

    void Update()
    {
        int aux = 0;
        currentLife = -1;
        if (health != 0) {
            currentLife = health / 2;
            for (int i = 0; i < currentLife; i++) {
                hearts[i].GetComponent<Image>().sprite = fullHeart;
            }
            if (health % 2 == 1) {
                hearts[currentLife].GetComponent<Image>().sprite = halfHeart;
                aux = currentLife + 1;
            } else aux = currentLife;

        }

        for (int i = aux; i < numberOfHearts; i++) {
            hearts[i].GetComponent<Image>().sprite = emptyHeart;
        }

        for (int i = 0; i < hearts.Length; i++) {
            if (i < numberOfHearts) {
                hearts[i].GetComponent<Image>().enabled = true;
            } else {
                hearts[i].GetComponent<Image>().enabled = false;
            }
        }

        if (health <= 0) {
            deathMenu.GetComponent<DeathMenu>().ImDead();
        }

        
    }

    void GetMousePos(){
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rotation = mousePos - (Vector2)transform.position;
        transform.right = -rotation;
    }

    public void StartAttack(){
        canAttack = false;
		animator.SetTrigger("Attack");
        AudioManager.instance.PlaySound("SwordAttack");
        StartCoroutine("AttackCooldown");
    }
    private void Attack(){
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition.position, attackCircle, enemyLayerMask);
        if(hitEnemies.Length > 0) {
            GameObject.FindWithTag("MainCamera").GetComponent<CameraMovement>().ShakeCamera();
            foreach (Collider2D enemy in hitEnemies){
                Debug.Log(enemy.name);
                enemy.gameObject.GetComponent<AiAgent>().Hitted(movement.lookingRight);
            }
        }
        
    }

    //Utilizado para saber como de grande deberia ser el circulo
    void OnDrawGizmosSelected(){
        if(attackPosition == null) return;
        Gizmos.DrawWireSphere(attackPosition.position, attackCircle);
    }
    public void TakeDamage(){
        if(!invencible){
            health -= 1;
            GameObject.FindWithTag("MainCamera").GetComponent<CameraMovement>().ShakeCamera();
            string audioFile = damageAudio[UnityEngine.Random.Range(0, damageAudio.Length)];
            AudioManager.instance.PlaySound(audioFile);
            animator.SetTrigger("Hitted");
            StartCoroutine("Hitted");
        } 

    }
    private IEnumerator AttackCooldown(){
        yield return new WaitForSeconds(0.25f);
        Attack();
        yield return new WaitForSeconds(0.25f);
        canAttack = true;
    }
    
    private IEnumerator Hitted(){
        invencible = true;
        if(health <= 0) animator.SetTrigger("Dead");
        yield return new WaitForSeconds(1f);
        if(health > 0) invencible = false;
    }

    public void LoadData(GameData data) {
        this.transform.position = data.playerPosition;
        this.health = data.life;
        Physics.SyncTransforms();
    }

    public void SaveData(GameData data) {
        data.playerPosition = transform.position;
        data.sceneIndex = Math.Min(SceneManager.GetActiveScene().buildIndex + 1, 3);
        data.life = health;
    }

    public void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Enemy") || col.CompareTag("Boss")) TakeDamage();
        if (col.CompareTag("EnergyBall") || col.CompareTag("Rock")) TakeDamage();
    }
}
