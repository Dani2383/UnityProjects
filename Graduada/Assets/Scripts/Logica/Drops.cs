using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drops : MonoBehaviour
{
    public List<GameObject> dropeableItems;

    private int idCalcetin;
    private int idRefresco;
    private int idVodka;
    private int idManifiesto;
    private int idMovil;

    private double chanceRefresco;
    private double chanceVodka;
    private double chanceManifiesto;
    private double chanceMovil;
    private double dropChance;

    private int numDrops;

    void Start()
    {
        idCalcetin = 0;
        idRefresco = 1;
        idVodka = 2;
        idManifiesto = 3;
        idMovil = 4;
        
        chanceRefresco = 0.35f; //0.35
        chanceVodka = 0.60f; //0.60
        chanceManifiesto = 0.80f; //0.80
        chanceMovil = 0.95f; //0.95
    }

    public void Drop(Vector3 dropPosition){
        numDrops = Random.Range(1,3);
        print("Numero de drops = " + numDrops);

        for( int j = 0; j< numDrops; j++){
            dropChance = Random.Range(0.0f,1f);
            print("dropChance= " + dropChance);

            if(dropChance >= chanceMovil){
                print("Dropeado un " + "Movil");
                Instantiate(dropeableItems[idMovil], dropPosition  , Quaternion.identity);
            }
            else if(dropChance >= chanceManifiesto){
                print("Dropeado un " + "Manifiesto");
                Instantiate(dropeableItems[idManifiesto], dropPosition  , Quaternion.identity);
            }
            else if(dropChance >= chanceVodka){
                print("Dropeado un " + "Vodka");
                Instantiate(dropeableItems[idVodka], dropPosition  , Quaternion.identity);
            }
            else if(dropChance >= chanceRefresco){
                print("Dropeado un " + "Refresco");
                Instantiate(dropeableItems[idRefresco], dropPosition  , Quaternion.identity);
            }
            else{
                print("Dropeado un " + "Calcetin");
                Instantiate(dropeableItems[idCalcetin], dropPosition  , Quaternion.identity);
            }
        }
    }
}
