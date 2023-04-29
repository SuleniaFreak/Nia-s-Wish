using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KrillAudio.Krilloud;
using UnityEngine.UI;

public class Exit : MonoBehaviour
{
    [Header("Black panel")]
    [SerializeField] private GameObject exitPanel;

    [Header("Buttons")]
    [SerializeField] private GameObject newGameButton;
    [SerializeField] private GameObject controlButton;
    Button newButtonInteract;
    Button controlButtonInteract;


    KLAudioSource source;

    private void Awake()
    {
        source = GetComponent<KLAudioSource>();
        newButtonInteract = newGameButton.GetComponent<Button>();
        controlButtonInteract = controlButton.GetComponent<Button>();
    }
    public void TurnOffButtons()
    {
        GetComponent<Button>().interactable = false;
        newButtonInteract.interactable = false;
        controlButtonInteract.interactable = false;
        StartCoroutine(CloseTheGame());
        source.SetFloatVar(KL.Variables.buttonstatus, 0);
        source.Play(KL.Tags.button);
        exitPanel.SetActive(true);
    }


    public IEnumerator CloseTheGame()
    {
        yield return new WaitForSeconds(1f);

        //directivas de pre-procesador (cierra el editor o el juego al pulsar el botón)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }







}
