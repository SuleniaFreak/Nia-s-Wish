using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalEventCollider : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject player;

    [Header("Thanks Panel")]
    [SerializeField] private GameObject thanksPanel;

    [Header("Game Manager")]
    [SerializeField] private GameObject gameManager;
    GameManager gameManagerScript;

    private void Awake()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameManagerScript.SetPlayerArrived();
            thanksPanel.SetActive(true);
        }
    }


}
