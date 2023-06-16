using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Actions : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tile tile;
    [SerializeField] private LayerMask terrainLayerMask;
    [SerializeField] private GameObject arrow, decoy;
    [SerializeField] private Transform arrowTransform;
    private bool canFire;
    public bool decoyEquipped;
    private float timer, launchForce;

    private Vector2 mousePos, rotation;
    private bool reloaded = true;

    // Linea de puntos para el apuntado de los señuelos -->
    public GameObject point;
    GameObject[] points;
    public int numberOfPoints;
    public float spaceBetweenPoints;


    void Start(){
        if(decoyEquipped){
            points = new GameObject[numberOfPoints];
            for(int i = 0; i < numberOfPoints; i++){
                points[i] = Instantiate(point, arrowTransform.position, Quaternion.identity);
            }
        }
        
    }
    void Update()
    {
        GetMousePos();
        launchForce = Vector2.Distance(arrowTransform.position,  Camera.main.ScreenToWorldPoint(Input.mousePosition)) * 3;
        if(Input.GetMouseButtonDown(0) && reloaded && !GameObject.FindWithTag("Player").GetComponent<Movement>().isHidded){
            SpawnArrow();
            //ThrowDecoy();
            StartCoroutine("Reload");
        }

        for(int i = 0; i < numberOfPoints; i++){
            points[i].transform.position = PointPosition(i * spaceBetweenPoints);
        }
    }


    void GetMousePos(){
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rotation = mousePos - (Vector2)transform.position;
        transform.right = -rotation;
    }
    private void SpawnArrow(){
        GameObject firedArrow = Instantiate(arrow, arrowTransform.position, Quaternion.identity);
        reloaded = false;              
    }

    private void ThrowDecoy(){
        GameObject thrownDecoy = Instantiate(decoy, arrowTransform.position, Quaternion.identity);
        thrownDecoy.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;
        reloaded = false;
    }

    Vector2 PointPosition(float time){
       return (Vector2)arrowTransform.position + (rotation.normalized * launchForce * time) + 0.5f * Physics2D.gravity * (time * time);
    }
    private IEnumerator Reload(){
        yield return new WaitForSeconds(0.6f);
        reloaded = true;
    }
}



