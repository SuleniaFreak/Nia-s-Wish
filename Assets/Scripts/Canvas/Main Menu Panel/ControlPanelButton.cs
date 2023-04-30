using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KrillAudio.Krilloud;

public class ControlPanelButton : MonoBehaviour
{

    #region public_variables
    [Header("Control Panel")]
    [SerializeField] private GameObject ControllersPanel;

    [Header("Other Button")]
    [SerializeField] private GameObject NewGameButton;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject howToPlayButton;
    #endregion

    #region private_variables
    [Header("Button interactions")]
    Button newGameButtonInteract;
    Button exitButtonInteract;
    Button howToPlayButtonInteract;

    [Header("Krilloud")]
    KLAudioSource source;
    #endregion

    private void Awake()
    {
        newGameButtonInteract = NewGameButton.GetComponent<Button>();
        exitButtonInteract = exitButton.GetComponent<Button>();
        howToPlayButtonInteract = howToPlayButton.GetComponent<Button>();
        source = GetComponent<KLAudioSource>();
    }

    public void TurnOffControlPanel()
    {
        ControllersPanel.SetActive(false);
        newGameButtonInteract.interactable = true;
        exitButtonInteract.interactable = true;
        howToPlayButtonInteract.interactable = true;
        source.SetFloatVar(KL.Variables.panelmode, 1);
        source.Play(KL.Tags.panel);
    }

    public void EventTriggerMouse()
    {
        source.SetFloatVar(KL.Variables.buttonstatus, 2);
        source.Play(KL.Tags.button);
    }
}
