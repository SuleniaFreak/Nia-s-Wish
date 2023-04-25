using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NyaffyAreaColliderTrigger : MonoBehaviour
{
    #region Public_Variables
    [Header("Game Manager")]
    [SerializeField] private GameObject gameManager;

    [Header("Ink JSON Files")]
    [SerializeField] private TextAsset inkJSONCanHearTheBell;
    [SerializeField] private TextAsset inkJSONCannotHearTheBell;
    #endregion

    #region Private_Variables
    GameManager gManager;
    AudioSource audioS;
    #endregion

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
