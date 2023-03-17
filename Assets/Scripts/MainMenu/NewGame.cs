using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGame : MonoBehaviour
{
    [Header("Main Menu Settings")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private AudioSource startSound; //sonido al apretar el botón "Nueva Partida" 

    Animator anim;
    private void Start()
    {
        startSound = GetComponent<AudioSource>();
        anim = menuPanel.GetComponent<Animator>();
    }

    public void StartGame()
    {
        startSound.Play();
        anim.enabled = true;
    }
}
