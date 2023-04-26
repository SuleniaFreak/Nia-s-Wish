using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controllers : MonoBehaviour
{
    [Header("Control Panel")]
    [SerializeField] private GameObject ControllersPanel;

    [Header("Other Button")]
    [SerializeField] private GameObject NewGameButton;
    Button newGameButtonInteract;


    void Start()
    {
        newGameButtonInteract = NewGameButton.GetComponent<Button>();
    }

   
    public void TurnOnControlPanel()
    {
        ControllersPanel.SetActive(true);
        newGameButtonInteract.interactable = false;
        GetComponent<Button>().interactable = false;
    }

    public void TurnOffControlPanel()
    {
        ControllersPanel.SetActive(false);
        newGameButtonInteract.interactable = true;
        GetComponent<Button>().interactable = true;
    }
   
}
