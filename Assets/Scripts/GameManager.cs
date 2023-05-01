using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using KrillAudio.Krilloud;

public class GameManager : MonoBehaviour
{
    #region Public_Variables

    [Header("Colliders Gameobjects Events")]
    [SerializeField] private GameObject backHouseBox;
    [SerializeField] private GameObject cropsColliderBox;
    [SerializeField] private GameObject nyaffyAreaColliderBox;
    [SerializeField] private GameObject finalEventColliderBox;
    [SerializeField] private GameObject fishfigure;

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
    [SerializeField] private GameObject houseLookAtSpot; // en pruebas

    [Header("Particle System")]
    [SerializeField] private ParticleSystem vanishedNyaffyParticle;

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
    int playerSpeed = 10;
    PlayerImagination playerImaginationScript;
    PlayerMovement playerMovementScript;

    [Header("Nyaffy Components")]
    Animator nyaffyAnim; //sin usar aún
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
        nyaffyAnim = nyaffy.GetComponent<Animator>();
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
        source.SetFloatVar(KL.Variables.pickup, 0);
        source.Play(KL.Tags.recolectable);
        numFlowers++;

        if (numFlowers == 6)
        {
            source.Stop();
            source.SetFloatVar(KL.Variables.pickup, 1);
            source.Play(KL.Tags.recolectable);
            StartCoroutine(CollectedFlowersEvent());
        }
    }

    //método que llamará a las corrutinas a través de los colliders
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
    //booleano que hará que continue la corrutina cuando se cumplan las condiciones del método siguiente
    bool RButtonPressedStatus()
    {
        return isRButtonPressed;
    }

    //método que cerrará el panel de tutorial y activará la imaginación
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
            underHouseCam.transform.LookAt(house.transform); //en pruebas
        }
    }


    #endregion

    #region Events_Corrutines
    //Evento que se ejecuta al recoger las flores (90% completado)
    IEnumerator CollectedFlowersEvent()
    {
        StartDialogue(inkJSONFlowers);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        source.SetFloatVar(KL.Variables.nyaffymode, 2);
        source.Play(KL.Tags.nyaffymovement);
        yield return new WaitForSeconds(1f);
        StartDialogue(inkJSONSurprise);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        yield return new WaitForSeconds(2f);
        source.Play(KL.Tags.nyaffymovement);
        yield return new WaitForSeconds(1f);
        //funciona pero el giro es muy brusco, pendiente de mejora
        //posible cambio de cámara con cinemachine sino se encuentra algo más fluido
        player.gameObject.transform.LookAt(nyaffy.transform);
        StartDialogue(inkJSONSurprise2);
        backHouseBox.SetActive(true);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        nyaffy.SetActive(true);
        isHouseEventReady = true;
    }

    //Evento que se ejecutará al interactuar con el collider de detrás de la casa (completado, falta pulir)
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
        source.Play(KL.Tags.panel);//en pruebas
        yield return new WaitUntil(nyaffyAlphaManager.isAphaMax);
        StartCoroutine(NyaffyFirstAppearence());
    }

    //Evento que se ejecutará al encontrar al Nyaffy por primera vez
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
        isHouseEventReady = false; // en pruebas
        StartDialogue(inkJSONFollowTheCat);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        playerMovementScript.enabled = true;
        playerImaginationScript.enabled = true;
        isCropsEventReady = true;
        cropsColliderBox.SetActive(true);
        yield return new WaitForSeconds(2f);

    }
    //Evento que se activará al entrar en un collider cerca de los cultivos
    public IEnumerator CropsEvent() //(Completado, falta pulir y añadir sonidos, particulas, etc)
    {
        nyaffy.transform.LookAt(player.transform);
        StartDialogue(inkJSONDoNotEatCrops);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        vanishedNyaffyParticle.Play(); // en pruebas
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(nyaffyMovementScript.SecondTranslation());
        yield return new WaitForSeconds(1f);
        //animación de confusión?
        StartDialogue(inkJSONMissedNyaffy);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        //nyaffy.SetActive(false); //en pruebas
        yield return new WaitForSeconds(3f);
        StartDialogue(inkJSONExtraInfoFindNyaffy);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        nyaffyAreaColliderBox.SetActive(true); //pendiente de comprobar si sigue necesitándose
        //nyaffy.SetActive(true); //en pruebas
        nyaffySource.Play();//en pruebas
        isCropsEventReady = false;
        isFoundNyaffyEventReady = true;
        yield return new WaitUntil(nyaffyAlphaManager.isAphaMax);
        EventStatus();
    }
    //Evento que se ejecutará cuando encuentres al nyaffy por segunda vez
    IEnumerator FoundNyaffyEvent() //Completado, pendiente de pulir y meter particulas, sonidos, etc
    {
        nyaffyAreaColliderBox.SetActive(false);
        nyaffySource.Stop();//en pruebas
        nyaffy.transform.LookAt(player.transform);
        //el nyaffy ejecuta una animación al ser descubierto??
        StartDialogue(inkJSONFoundNyaffy);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        nyaffySource.Play();
        StartCoroutine(nyaffyMovementScript.FinalTranslation());
        yield return new WaitForSeconds(1f); //en pruebas
        nyaffySource.Stop();// en pruebas
        StartDialogue(inkJSONWhatIsThis);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        isFoundNyaffyEventReady = false;
        isFinalEventReady = true;


    }
    //Evento que se ejecutará cuando el player recoja el pez
    IEnumerator FinalEvent()
    {
        finalEventColliderBox.SetActive(true);
        playerMovementScript.enabled = false;
        fishfigure.SetActive(false);
        fishPanel.SetActive(true);
        source.SetFloatVar(KL.Variables.pickup, 2); //en pruebas
        source.Play(KL.Tags.recolectable);//en pruebas
        yield return new WaitForSeconds(2f);
        fishPanel.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        playerMovementScript.enabled = true;
        player.transform.LookAt(nyaffy.transform); //en pruebas
        StartDialogue(inkJSONFishCathched);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        StartDialogue(inkJSONChaseTheCat);
        StartCoroutine(AutoPlayerMove());
        //Si llego hasta aquí.......
        Debug.Log("Ruta de personaje terminada!!!!!!!!");
        yield return null;
    }
    #endregion

    //corrutina que llevará al player automáticamente al final de la demo (podria cambiar a cambio de escena)
    IEnumerator AutoPlayerMove()
    {
        playerMovementScript.enabled = false;

        while (player.transform.position != finalEventColliderBox.transform.position)
        {
            playerAnim.SetBool("IsRunning", true);
            player.transform.position = Vector3.MoveTowards(player.transform.position, finalEventColliderBox.transform.position, playerSpeed * Time.deltaTime);
            yield return null;
        }

        playerAnim.SetBool("IsRunning", false);
    }


}
