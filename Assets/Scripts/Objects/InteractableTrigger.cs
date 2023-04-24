using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue; //signo para saber que se puede interactuar

    [Header("Game Manager")]
    [SerializeField] private GameObject gameManager;
    GameManager gameManagerScript;

    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
        gameManagerScript = gameManager.GetComponent<GameManager>();
    }

    private void Update()
    {
        //si el player est� en rango y no hay una conversaci�n pasando
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                gameManagerScript.EventStatus();
                visualCue.SetActive(false);
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
