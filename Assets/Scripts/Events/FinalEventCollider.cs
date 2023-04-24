using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalEventCollider : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject player;

    [Header("Thanks Panel")]
    [SerializeField] private GameObject thanksPanel;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.SetActive(false);
            thanksPanel.SetActive(true);
        }
    }


}
