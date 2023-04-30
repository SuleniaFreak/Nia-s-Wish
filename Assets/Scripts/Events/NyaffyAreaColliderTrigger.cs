using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KrillAudio.Krilloud;

public class NyaffyAreaColliderTrigger : MonoBehaviour
{
    #region Public_Variables
    [Header("Game Manager")]
    [SerializeField] private GameObject gameManager;

    [Header("Nyaffy")]
    [SerializeField] private GameObject nyaffy;

    [Header("Ink JSON Files")]
    [SerializeField] private TextAsset inkJSONCanHearTheBell;
    [SerializeField] private TextAsset inkJSONCannotHearTheBell;
    #endregion

    #region Private_Variables
    GameManager gManager;

    #endregion

    private void Awake()
    {
        gManager = gameManager.GetComponent<GameManager>();
       

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gManager.StartDialogue(inkJSONCanHearTheBell);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            gManager.StartDialogue(inkJSONCannotHearTheBell);
            nyaffy.SetActive(false);

        }
    }


}
