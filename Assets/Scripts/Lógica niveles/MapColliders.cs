using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;


public class MapColliders : MonoBehaviour
{
    [SerializeField] Tilemap torchesTilemap;
    [SerializeField] Tile torchOffTile;
    [SerializeField] Tile torchOnTile;
    UnityEngine.Rendering.Universal.Light2D light;

    [SerializeField] VictoryMenu victoryMenu;
    [SerializeField] GameObject bossDoor;

    [SerializeField] GameObject deathMenu;
    [SerializeField] GameObject puzzleDoor;
    private int counter;


    void Start(){
        if(gameObject.tag == "Torch") light = gameObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        counter = 0;
    }


    private void OnTriggerEnter2D(Collider2D col){
        // Si flecha le da a antorcha (Light) --> Se apaga
        if(gameObject.tag == "Torch" && col.transform.tag == "Flecha"){
            StartCoroutine(PowerOff());
            return;
        }
        else if (gameObject.tag == "PuzzleTorch" && col.transform.tag == "FireBall") {
            gameObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 2;
            return;
        }

        // si jugador entra en zona de escondite --> Se esconde o sale depende de a lo que apriete
        if (gameObject.tag == "Hidding Place" && col.transform.tag == "Player"){
            col.GetComponent<Movement>().inHiddingPlace = true;
            return;
        }
        // Si jugador entra a la luz --> Se marca como isLighted
        if(gameObject.tag == "Light" && col.transform.tag == "Player"){
            col.GetComponent<Movement>().isLighted = true;
            return;
        }
        
        if (gameObject.tag == "Goal" && col.transform.tag == "Player") {
            victoryMenu.Victory();
        }

        if(gameObject.tag == "Boss Zone" && col.transform.tag == "Player") {
            StartCoroutine("CameraRoutine");
            GameObject.FindGameObjectWithTag("Boss").GetComponent<AiAgent>().stateMachine.ChangeState(AiStateId.HighHealth);
            AudioManager.instance.PlaySound("Door Close");
            bossDoor.SetActive(true);
        }

        if (gameObject.tag == "DeathPit" && col.transform.tag == "Player") {
            col.GetComponent<PlayerState>().animator.SetTrigger("Dead");
            deathMenu.GetComponent<DeathMenu>().ImDead();
        }


        // Si flecha le da a antorcha (sprite) --> Se transforma en antorcha apagada
        try {
           if(gameObject.tag == "Torches" && col.transform.tag == "Flecha"){
            Vector3Int clickCell = torchesTilemap.WorldToCell(col.transform.position);
            if( torchesTilemap.GetTile(clickCell) != null){
                torchesTilemap.SetTile(clickCell, torchOffTile);
            }
        } 
        } catch(Exception e){ Debug.Log(e);}

        try {
            if (gameObject.tag == "PuzzleTorches" && col.transform.tag == "FireBall") {
                Vector3Int clickCell = torchesTilemap.WorldToCell(col.transform.position);
                if (torchesTilemap.GetTile(clickCell) != null) {
                    torchesTilemap.SetTile(clickCell, torchOnTile);
                    counter += 1;
                    Debug.Log(counter);
                    if (counter == 6) {
                        puzzleDoor.SetActive(false);
                    }
                }
            }
        } catch (Exception e) { Debug.Log(e); }

    }

    private void OnTriggerExit2D(Collider2D col){
        if(gameObject.tag == "Light" && col.transform.tag == "Player"){
            col.GetComponent<Movement>().isLighted = false;
        }

        if(gameObject.tag == "Hidding Place" && col.transform.tag == "Player"){
            col.GetComponent<Movement>().inHiddingPlace = false;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Boss") && gameObject.CompareTag("Terrain")) {
            if (collision.gameObject.GetComponent<AiAgent>().isCharging) {
                collision.gameObject.GetComponent<AiAgent>().chargeAttackReloaded = false;
                collision.gameObject.GetComponent<AiAgent>().ReloadChargeAttack();
            }
            

        }
        if(collision.gameObject.CompareTag("EnergyBall") && gameObject.CompareTag("Terrain")) {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Rock") && gameObject.CompareTag("Terrain")) {
            Destroy(collision.gameObject);
        }
    }
    private IEnumerator PowerOff(){
        yield return new WaitForSeconds(0.1f);
        if(light.intensity >= 0.4) {
            light.intensity = light.intensity - 0.4f;
            StartCoroutine(PowerOff());
        } else {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator CameraRoutine() {
        Camera MainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        while(MainCamera.orthographicSize < 1.6f) {
            yield return new WaitForSeconds(0.02f);
            MainCamera.orthographicSize += 0.02f;
        }
    }

}
