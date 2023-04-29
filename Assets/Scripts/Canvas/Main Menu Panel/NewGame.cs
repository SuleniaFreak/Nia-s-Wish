using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using KrillAudio.Krilloud;

public class NewGame : MonoBehaviour
{
    #region Public_Variables
    [Header("Main Menu Settings")]
    [SerializeField] private GameObject menuPanel;
    public bool isIntroPlaying; //si lo pongo con serialize no funciona en dialogue Manager (por revisar)

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkHouse;
    [SerializeField] private TextAsset inkNia;

    [Header("Characters")]
    [SerializeField] private GameObject player;

    [Header("Cinemachine (VCameras)")]
    [SerializeField] private GameObject menuCam;
    [SerializeField] private GameObject introCam;

    [Header("Particle Systems")]
    [SerializeField] private ParticleSystem SakuraGeneral;

    [Header("Other Buttons")]
    [SerializeField] private GameObject controlButton;
    [SerializeField] private GameObject exitButton;
    Button controlButtoninteract;
    Button exitButtonInteract;
    #endregion

    #region Private_Variables

    [Header("Animators")]
    Animator playerAnim;
    Animator anim;

    [Header("Scripts")]
    PlayerMovement playerMovementScript;

    [Header("Cinemachine")]
    CinemachineVirtualCamera camPriority;

    [Header("Audio")]
    KLAudioSource source;
    #endregion

    private void Start()
    {
        StopAllCoroutines();
        CatcherReferences();
    }

    private void CatcherReferences()
    {
        source = GetComponent<KLAudioSource>();
        anim = menuPanel.GetComponent<Animator>();
        playerAnim = player.GetComponent<Animator>();
        camPriority = menuCam.gameObject.GetComponent<CinemachineVirtualCamera>();
        playerMovementScript = player.gameObject.GetComponent<PlayerMovement>();
        controlButtoninteract = controlButton.GetComponent<Button>();
        exitButtonInteract = exitButton.GetComponent<Button>();
    }

    #region IntroMethods
    public void NewGamePressed()
    {
        StartCoroutine("IntroSetting");
        isIntroPlaying = true;
        GetComponent<Button>().interactable = false;
        controlButtoninteract.interactable = false;
        exitButtonInteract.interactable = false;
    }

    IEnumerator IntroSetting() //completado, pendiente de pulir, añadir sfx, etc
    {
        source.SetFloatVar(KL.Variables.buttonstatus, 0);
        source.Play(KL.Tags.button); //en pruebas
        playerMovementScript.enabled = false;
        anim.enabled = true;
        camPriority.Priority = 2;
        yield return new WaitForSeconds(1f);
        //gestión de sistemas de particulas pétalos y viento
        SakuraGeneral.Play();
        playerAnim.Play("StandUp");
        yield return new WaitForSeconds(2f);
        StartConversation(inkHouse);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        isIntroPlaying = false;
        SetPlayerCam();
        yield return new WaitForSeconds(2f);
        StartConversation(inkNia);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        playerMovementScript.enabled = true;
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
