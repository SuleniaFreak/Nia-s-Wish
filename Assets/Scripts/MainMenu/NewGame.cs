using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class NewGame : MonoBehaviour
{
    [Header("Main Menu Settings")]
    [SerializeField] private GameObject menuPanel;
    private AudioSource startSound;  
    Animator anim;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkHouse;
    [SerializeField] private TextAsset inkNia;

    [Header("Characters")]
    [SerializeField] private GameObject player;
    Animator playerAnim;

    [Header("Cinemachine (VCameras)")]
    [SerializeField] private GameObject menuCam;
    [SerializeField] private GameObject introCam;
    private CinemachineVirtualCamera camPriority;

    public bool isIntroPlaying;

    private void Start()
    {
        StopAllCoroutines();
        CatcherReferences();
    }

    private void CatcherReferences()
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
        //gestión de sistemas de particulas pétalos y viento
        //música y sfx (si hay)
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
