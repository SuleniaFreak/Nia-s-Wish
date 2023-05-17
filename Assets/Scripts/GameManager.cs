using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using KrillAudio.Krilloud;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Public_Variables

    [Header("Colliders Gameobjects Events")]
    [SerializeField] private GameObject backHouseBox;
    [SerializeField] private GameObject cropsColliderBox;
    [SerializeField] private GameObject nyaffyAreaColliderBox;
    [SerializeField] private GameObject finalEventColliderBox;
    [SerializeField] private GameObject fishfigure;

    [Header("Flowers Counter")]
    [SerializeField] private GameObject flowerPanel;
    [SerializeField] private TextMeshProUGUI flowerText; 

    [Header("Characters")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject nyaffy;
    [SerializeField] private GameObject house;

    [Header("Cinemachine")]
    [SerializeField] private GameObject underHouseCam;

    [Header("Panels")]
    [SerializeField] private GameObject tutoPanel;
    [SerializeField] private GameObject fishPanel;

    [Header("House Look At")]
    [SerializeField] private GameObject houseLookAtSpot;

    [Header("Particle System")]
    [SerializeField] private ParticleSystem vanishedNyaffyParticle;

    [Header("Player Event Bool")]
    [SerializeField] private bool isPlayerArrived = false;

    #region JSON_Files
    [Header("Collected flowers Event")]
    [SerializeField] private TextAsset inkJSONFlowers;
    [SerializeField] private TextAsset inkJSONSurprise;
    [SerializeField] private TextAsset inkJSONSurprise2;

    [Header("Back House Event")]
    [SerializeField] private TextAsset inkJSONBackHouse;
    [SerializeField] private TextAsset inkJSONBackHouse2;

    [Header("Nyaffy First Appearance")]
    [SerializeField] private TextAsset inkJSONCouldBeACat;
    [SerializeField] private TextAsset inkJSONWhereTheCatGoes;
    [SerializeField] private TextAsset inkJSONFollowTheCat;

    [Header("Crops Event")]
    [SerializeField] private TextAsset inkJSONDoNotEatCrops;
    [SerializeField] private TextAsset inkJSONMissedNyaffy;
    [SerializeField] private TextAsset inkJSONExtraInfoFindNyaffy;

    [Header("Found Nyaffy Event")]
    [SerializeField] private TextAsset inkJSONFoundNyaffy;
    [SerializeField] private TextAsset inkJSONWhatIsThis;

    [Header("Final Event")]
    [SerializeField] private TextAsset inkJSONFishCathched;
    [SerializeField] private TextAsset inkJSONChaseTheCat;

    #endregion

    #endregion

    #region Private_Variables

    [Header("Player Components")]
    Animator playerAnim;
    PlayerImagination playerImaginationScript;
    PlayerMovement playerMovementScript;

    [Header("Nyaffy Components")]
    AlphaManager nyaffyAlphaManager;
    NyaffyMovement nyaffyMovementScript;

    [Header("Audio")]
    KLAudioSource source;
    KLAudioSource nyaffySource;

    [Header("Flowers Counter")]
    int numFlowers = 0;

    [Header("Bools")]
    bool isRButtonPressed;
    bool isTutoPanelShowing;

    [Header("Event Bools")]
    bool isHouseEventReady;
    bool isCropsEventReady;
    bool isFoundNyaffyEventReady;
    bool isFinalEventReady;

    #endregion


    private void Awake()
    {
        CatcherReferences();
        SettingBoolValues();
    }

    private void Update()
    {
        RMouseButtonPress();
    }

    #region General_Methods
    void CatcherReferences()
    {
        playerAnim = player.GetComponent<Animator>();
        source = GetComponent<KLAudioSource>();

        //BackHouseEvent
        playerImaginationScript = player.gameObject.GetComponentInChildren<PlayerImagination>();
        playerMovementScript = player.GetComponent<PlayerMovement>();

        //NyaffyFirstAppearance
        nyaffyAlphaManager = nyaffy.GetComponent<AlphaManager>();
        nyaffyMovementScript = nyaffy.GetComponent<NyaffyMovement>();
        nyaffySource = nyaffy.GetComponent<KLAudioSource>();

    }

    void SettingBoolValues()
    {
        isHouseEventReady = false;
        isCropsEventReady = false;
        isRButtonPressed = false;
        isTutoPanelShowing = false;
        isFinalEventReady = false;
    }
    public void StartDialogue(TextAsset inkJSON)
    {
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
    }

    #endregion

    #region Trigger_Methods
    public void AddFlower()
    {
        flowerPanel.SetActive(true);
        source.SetFloatVar(KL.Variables.pickup, 0);
        source.Play(KL.Tags.recolectable);
        numFlowers++;
        flowerText.text = numFlowers.ToString() + "/6";

        if (numFlowers == 6)
        {
            source.Stop();
            source.SetFloatVar(KL.Variables.pickup, 1);
            source.Play(KL.Tags.recolectable);
            StartCoroutine(CollectedFlowersEvent());
        }
    }

    //m�todo que llamar� a las corrutinas a trav�s de los colliders
    public void EventStatus()
    {
        if (isHouseEventReady)
        {
            StartCoroutine(BackHouseEvent());
        }
        else if (isCropsEventReady)
        {
            StartCoroutine(CropsEvent());
        }
        else if (isFoundNyaffyEventReady)
        {
            StartCoroutine(FoundNyaffyEvent());
        }
        else if (isFinalEventReady)
        {
            StartCoroutine(FinalEvent());
        }

    }
    //booleano que har� que continue la corrutina cuando se cumplan las condiciones del m�todo siguiente
    bool RButtonPressedStatus()
    {
        return isRButtonPressed;
    }

    //m�todo que cerrar� el panel de tutorial y activar� la imaginaci�n
    void RMouseButtonPress()
    {
        if (Input.GetMouseButtonDown(1) && isTutoPanelShowing)
        {
            isRButtonPressed = true;
            tutoPanel.SetActive(false);
            source.SetFloatVar(KL.Variables.panelmode, 1);
            source.Play(KL.Tags.panel);
            isTutoPanelShowing = false;
            underHouseCam.SetActive(true);
            underHouseCam.transform.LookAt(house.transform);
        }
    }
    #endregion

    #region Events_Corrutines
    //Evento que se ejecuta al recoger las flores (completado, pendiente de pulir)
    IEnumerator CollectedFlowersEvent()
    {
        StartDialogue(inkJSONFlowers);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        source.SetFloatVar(KL.Variables.nyaffymode, 2);
        source.Play(KL.Tags.nyaffymovement);
        yield return new WaitForSeconds(1f);
        StartDialogue(inkJSONSurprise);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        yield return new WaitForSeconds(1f);
        source.Play(KL.Tags.nyaffymovement);
        yield return new WaitForSeconds(1f);
        player.gameObject.transform.LookAt(nyaffy.transform);
        StartDialogue(inkJSONSurprise2);
        backHouseBox.SetActive(true);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        nyaffy.SetActive(true);
        isHouseEventReady = true;
        flowerPanel.SetActive(false);
    }

    //Evento que se ejecutar� al interactuar con el collider de detr�s de la casa (completado, falta pulir)
    IEnumerator BackHouseEvent()
    {
        nyaffySource.Stop();
        player.transform.LookAt(nyaffy.transform);
        backHouseBox.SetActive(false);
        playerMovementScript.enabled = false;
        StartDialogue(inkJSONBackHouse);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        playerAnim.Play("StandToCrouch");
        StartDialogue(inkJSONBackHouse2);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        yield return new WaitForSeconds(0.1f);
        source.SetFloatVar(KL.Variables.panelmode, 0);
        source.Play(KL.Tags.panel);
        tutoPanel.SetActive(true); 
        isTutoPanelShowing = true;
        yield return new WaitUntil(RButtonPressedStatus);
        source.SetFloatVar(KL.Variables.panelmode, 1);
        source.Play(KL.Tags.panel);
        yield return new WaitUntil(nyaffyAlphaManager.isAphaMax);
        StartCoroutine(NyaffyFirstAppearence());
    }

    //Evento que se ejecutar� al encontrar al Nyaffy por primera vez
    IEnumerator NyaffyFirstAppearence()
    {
        nyaffy.transform.LookAt(underHouseCam.transform);
        StartDialogue(inkJSONCouldBeACat);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        StartCoroutine(nyaffyMovementScript.FirstTranslation());
        yield return new WaitForSeconds(1f);
        StartDialogue(inkJSONWhereTheCatGoes);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        underHouseCam.SetActive(false);
        yield return new WaitForSeconds(2f);
        playerAnim.Play("CrouchedToStand");
        isHouseEventReady = false; 
        StartDialogue(inkJSONFollowTheCat);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        playerMovementScript.enabled = true;
        playerImaginationScript.enabled = true;
        isCropsEventReady = true;
        cropsColliderBox.SetActive(true);
        yield return new WaitForSeconds(2f);

    }
    //Evento que se activar� al entrar en un collider cerca de los cultivos
    public IEnumerator CropsEvent() //(Completado, falta pulir)
    {
        nyaffy.transform.LookAt(player.transform);
        StartDialogue(inkJSONDoNotEatCrops);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        vanishedNyaffyParticle.Play(); // en pruebas
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(nyaffyMovementScript.SecondTranslation());
        yield return new WaitForSeconds(1f);
        //animaci�n de confusi�n?
        StartDialogue(inkJSONMissedNyaffy);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        yield return new WaitForSeconds(3f);
        StartDialogue(inkJSONExtraInfoFindNyaffy);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        nyaffyAreaColliderBox.SetActive(true); 
        nyaffySource.Play();
        isCropsEventReady = false;
        isFoundNyaffyEventReady = true;
        yield return new WaitUntil(nyaffyAlphaManager.isAphaMax);
        EventStatus();
    }
    //Evento que se ejecutar� cuando encuentres al nyaffy por segunda vez
    IEnumerator FoundNyaffyEvent() //Completado, pendiente de pulir y meter particulas, sonidos, etc
    {
        nyaffyAreaColliderBox.SetActive(false);
        nyaffySource.Stop();
        nyaffy.transform.LookAt(player.transform);
        //el nyaffy ejecuta una animaci�n al ser descubierto??
        StartDialogue(inkJSONFoundNyaffy);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        nyaffySource.Play();
        StartCoroutine(nyaffyMovementScript.FinalTranslation());
        yield return new WaitForSeconds(1f); 
        nyaffySource.Stop();
        StartDialogue(inkJSONWhatIsThis);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        isFoundNyaffyEventReady = false;
        isFinalEventReady = true;


    }
    //Evento que se ejecutar� cuando el player recoja el pez
    IEnumerator FinalEvent()
    {
        finalEventColliderBox.SetActive(true);
        playerMovementScript.enabled = false;
        fishfigure.SetActive(false);
        fishPanel.SetActive(true);
        source.SetFloatVar(KL.Variables.pickup, 2); 
        source.Play(KL.Tags.recolectable);
        yield return new WaitForSeconds(2f);
        fishPanel.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        playerMovementScript.enabled = true;
        player.transform.LookAt(nyaffy.transform); 
        StartDialogue(inkJSONFishCathched);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        StartDialogue(inkJSONChaseTheCat);
        StartCoroutine(AutoPlayerMove());
        //Si llego hasta aqu�.......
        Debug.Log("Ruta de personaje terminada!!!!!!!!");
        yield return null;
    }
    #endregion

    #region auto_player_movement_methods
    //corrutina que llevar� al player autom�ticamente al final de la demo
    IEnumerator AutoPlayerMove()
    {
        playerMovementScript.enabled = false;

        while (!isPlayerArrived)
        {
            playerAnim.SetBool("IsRunning", true);
            playerMovementScript.MoveForward();
            yield return null;
        }

        playerAnim.SetBool("IsRunning", false);
    }

    public void SetPlayerArrived()
    {
        isPlayerArrived = true;
    }
    #endregion
}
