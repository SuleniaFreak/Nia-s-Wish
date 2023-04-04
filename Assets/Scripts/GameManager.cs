using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Flowers Counter")]
    private int numFlowers = 0;


    public void AddFlower()
    {
        numFlowers++;

        //llamar al método que activará el evento cuando recojas 6 flores
        Debug.Log("He recogido: " + numFlowers.ToString());
    }

}
