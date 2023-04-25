using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToMainMenu : MonoBehaviour
{

    [Header("Main Menu Settings")]
    [SerializeField] private GameObject newGameButton;

    Button newGameButtonMode;

    private void Awake()
    {
        newGameButtonMode = newGameButton.GetComponent<Button>();
    }


    public void LoadScene()
    {
        SceneManager.LoadScene("Scene1");

        newGameButtonMode.interactable = true;
    }




}
