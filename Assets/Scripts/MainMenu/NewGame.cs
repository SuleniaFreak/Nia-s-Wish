using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class NewGame : MonoBehaviour
{
    [Header("Main Menu Settings")]
    [SerializeField] private GameObject menuPanel;
    private AudioSource startSound; //sonido al apretar el botón "Nueva Partida" 
    Animator anim;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkHouse; //primer dialogo
    [SerializeField] private TextAsset inkNia;

    //en pruebas
    [Header("Intro Settings (player)")]
    [SerializeField] private GameObject player;
    Animator playerAnim;

    [Header("Intro Settings (Cameras)")]
    [SerializeField] private GameObject menuCam;
    [SerializeField] private GameObject introCam;
    private CinemachineVirtualCamera camPriority;

    public bool isIntroPlaying;

    private void Start()
    {
        StopAllCoroutines();
        ComponentsCollector();
    }

    private void ComponentsCollector()
    {
        startSound = GetComponent<AudioSource>();
        anim = menuPanel.GetComponent<Animator>();
        playerAnim = player.GetComponent<Animator>();
        camPriority = menuCam.gameObject.GetComponent<CinemachineVirtualCamera>();
    }

    #region IntroMethods
    public void NewGamePressed()
    {
        StartCoroutine("IntroSetting");
        isIntroPlaying = true;
        
    }

    IEnumerator IntroSetting() //en proceso
    {
        startSound.Play();
        anim.enabled = true;
        camPriority.Priority = 2;
        yield return new WaitForSeconds(1f);
        // gestión de sistemas de particulas
        playerAnim.Play("StandUp");
        yield return new WaitForSeconds(2f);
        StartConversation(inkHouse);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        isIntroPlaying = false;
        SetPlayerCam();
        yield return new WaitForSeconds(2f);
        
        StartConversation(inkNia);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);

        
        StopAllCoroutines();

    }

    void StartConversation(TextAsset inkJSON)
    {
        
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
    }


    public void SetPlayerCam()
    {
        camPriority = introCam.gameObject.GetComponent<CinemachineVirtualCamera>();
        camPriority.Priority = 2;
    }

    #endregion
}
