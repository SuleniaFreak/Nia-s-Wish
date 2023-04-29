using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KrillAudio.Krilloud;

public class Controllers : MonoBehaviour
{
    #region public_variables
    [Header("Control Panel")]
    [SerializeField] private GameObject ControllersPanel;

    [Header("Other Button")]
    [SerializeField] private GameObject NewGameButton;
    [SerializeField] private GameObject exitButton;
    #endregion

    #region private_variables
    [Header("Button interactions")]
    Button newGameButtonInteract;
    Button exitButtonInteract;

    [Header("Krilloud")]
    KLAudioSource source;
    #endregion

    private void Awake()
    {
        newGameButtonInteract = NewGameButton.GetComponent<Button>();
        exitButtonInteract = exitButton.GetComponent<Button>();
        source = GetComponent<KLAudioSource>();
    }


    public void TurnOnControlPanel()
    {
        ControllersPanel.SetActive(true);
        newGameButtonInteract.interactable = false;
        exitButtonInteract.interactable = false;
        GetComponent<Button>().interactable = false;
        source.SetFloatVar(KL.Variables.panelmode, 0);
        source.Play(KL.Tags.panel);
    }

    public void TurnOffControlPanel()
    {
        ControllersPanel.SetActive(false);
        newGameButtonInteract.interactable = true;
        exitButtonInteract.interactable = true;
        GetComponent<Button>().interactable = true;
        source.SetFloatVar(KL.Variables.panelmode, 1);
        source.Play(KL.Tags.panel);
    }

    public void AboveTheButton()
    {
        if(newGameButtonInteract.interactable || exitButtonInteract.interactable || GetComponent<Button>().interactable)
        {
        source.SetFloatVar(KL.Variables.buttonstatus, 2);
        source.Play(KL.Tags.button);
        }
        
    }

}
