using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recolectable : MonoBehaviour
{
    [SerializeField] private GameManager gManagerScript;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {          
            if (Input.GetMouseButtonDown(0))
            {
                gameObject.SetActive(false);
                gManagerScript.AddFlower();
            }

        }
    }
}
