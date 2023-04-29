using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using KrillAudio.Krilloud;

public class BackToMainMenu : MonoBehaviour
{

    [Header("Main Menu Settings")]
    [SerializeField] private GameObject newGameButton;

    KLAudioSource source;

    Button newGameButtonMode;

    private void Awake()
    {
        newGameButtonMode = newGameButton.GetComponent<Button>();
        source = GetComponent<KLAudioSource>();
    }


    public void LoadScene()
    {
        source.Play(); //en pruebas
        SceneManager.LoadScene("Scene1");

        newGameButtonMode.interactable = true;
    }




}
