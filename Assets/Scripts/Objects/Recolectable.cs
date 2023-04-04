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
            Debug.Log("primer condicional");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("segundo condicional");
                gameObject.SetActive(false);
                gManagerScript.AddFlower();
            }

        }
    }
}
