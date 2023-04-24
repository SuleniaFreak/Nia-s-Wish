using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropsColliderEvent : MonoBehaviour
{
    [Header("Game Manager")]
    [SerializeField] private GameObject gameManager;
    GameManager gManager;

    private void Awake()
    {
        gManager = gameManager.GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gManager.EventStatus();
            gameObject.SetActive(false);
        }
    }
}
