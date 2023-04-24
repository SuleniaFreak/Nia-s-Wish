using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NyaffyAreaColliderTrigger : MonoBehaviour
{
    [Header("Game Manager")]
    [SerializeField] private GameObject gameManager;


    [Header("Ink JSON Files")]
    [SerializeField] private TextAsset inkJSONCanHearTheBell;
    [SerializeField] private TextAsset inkJSONCannotHearTheBell;

    GameManager gManager;
    AudioSource audioS;

    private void Awake()
    {
        gManager = gameManager.GetComponent<GameManager>();
        audioS = GetComponent<AudioSource>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            gManager.StartDialogue(inkJSONCanHearTheBell);
            audioS.Play();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            gManager.StartDialogue(inkJSONCannotHearTheBell);
            audioS.Stop();

        }
    }


}
