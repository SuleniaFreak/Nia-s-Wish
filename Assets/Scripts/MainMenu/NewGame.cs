using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGame : MonoBehaviour
{
    [Header("Main Menu Settings")]
    [SerializeField] private GameObject menuPanel;
    private AudioSource startSound; //sonido al apretar el botón "Nueva Partida" 
    Animator anim;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON; //primer dialogo


    private void Start()
    {
        startSound = GetComponent<AudioSource>();
        anim = menuPanel.GetComponent<Animator>();
    }

    public void StartGame() //pendiente de sopesar cambiar a corrutina
    {
        startSound.Play();
        anim.enabled = true;
        StartConversation();
    }

    void StartConversation()
    {
        //inicia el dialogo
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
    }
}
