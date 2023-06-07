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
        StartCoroutine(FinishedDemo());
    }

    IEnumerator FinishedDemo()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        source.SetFloatVar(KL.Variables.buttonstatus, 0);
        source.Play(KL.Tags.button);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("EnglishScene");
        newGameButtonMode.interactable = true;
    }




}
