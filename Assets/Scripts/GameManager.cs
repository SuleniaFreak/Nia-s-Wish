using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    #region Public_Variables

    [Header("Colliders Gameobjects Events")]
    [SerializeField] private GameObject bellSoundBox;
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

    [Header("SFX Files")]
    [SerializeField] private AudioClip bell1;
    [SerializeField] private AudioClip bell2;

    [Header("Panels")]
    [SerializeField] private GameObject tutoPanel;
    [SerializeField] private GameObject fishPanel;
   // [SerializeField] private GameObject thanksPanel; pendiente de quitar

  


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
    [SerializeField] private TextAsset inkJSONFishCathched; //sin usar aún
    [SerializeField] private TextAsset inkJSONChaseTheCat; //sin usar aún

    #endregion

    #endregion

    #region Private_Variables

    [Header("Characters Components")]
    Animator playerAnim;
    int playerSpeed = 5;//en pruebas
    PlayerImagination playerImaginationScript;
    PlayerMovement playerMovementScript;
    Animator nyaffyAnim; //sin usar aún
    AlphaManager nyaffyAlphaManager;
    NyaffyMovement nyaffyMovementScript;

    [Header("Audio")]
    AudioSource audioManager;
    AudioSource bellFull;

    [Header("Flowers Counter")]
    int numFlowers = 0;

    [Header("Bools")]
    bool cinematicCheck;
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

        //CollectedFlowersEvent
        audioManager = GetComponent<AudioSource>();
        bellFull = bellSoundBox.GetComponent<AudioSource>();

        //BackHouseEvent
        playerImaginationScript = player.gameObject.GetComponentInChildren<PlayerImagination>();
        playerMovementScript = player.GetComponent<PlayerMovement>();

        //NyaffyFirstAppearance
        nyaffyAnim = nyaffy.GetComponent<Animator>();
        nyaffyAlphaManager = nyaffy.GetComponent<AlphaManager>();
        nyaffyMovementScript = nyaffy.GetComponent<NyaffyMovement>();//en proceso

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
        numFlowers++;

        if (numFlowers == 3) //6 será el número final
        {
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
        else if (isFoundNyaffyEventReady) //en revisión
        {
            StartCoroutine(FoundNyaffyEvent());
        }
        else if(isFinalEventReady) //en revisión
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
            tutoPanel.SetActive(false); //añadir sonidito al cerrar el panel
            isTutoPanelShowing = false;
            underHouseCam.SetActive(true);
        }
    }


    #endregion

    #region Events_Corrutines
    //Evento que se ejecuta al recoger las flores (90% completado)
    IEnumerator CollectedFlowersEvent()
    {
        StartDialogue(inkJSONFlowers);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        audioManager.PlayOneShot(bell1);
        yield return new WaitForSeconds(1f);
        StartDialogue(inkJSONSurprise);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        audioManager.PlayOneShot(bell2);
        yield return new WaitForSeconds(1f);
        StartDialogue(inkJSONSurprise2);
        //giro muy brusco, pendiente de mejora (pero funciona)
        //posible cambio de cámara con cinemachine sino se encuentra algo más fluido
        player.gameObject.transform.LookAt(bellSoundBox.transform);
        bellSoundBox.SetActive(true);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        //mejorar la recepción y distancia del sonido (pendiente)
        bellFull.Play();
        isHouseEventReady = true; //en proceso
        StopAllCoroutines();
    }

    //Evento que se ejecutará al interactuar con el collider de detrás de la casa (completado, falta pulir)
    IEnumerator BackHouseEvent()
    {
        bellFull.Stop();
        player.transform.LookAt(house.transform);
        bellSoundBox.SetActive(false);
        playerMovementScript.enabled = false;
        StartDialogue(inkJSONBackHouse);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        playerAnim.Play("StandToCrouch");
        StartDialogue(inkJSONBackHouse2);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        tutoPanel.SetActive(true); //añadir sonidito al aparecer el panel al activarse
        isTutoPanelShowing = true;
        yield return new WaitUntil(RButtonPressedStatus);
        yield return new WaitUntil(nyaffyAlphaManager.isAphaMax);
        StartCoroutine(NyaffyFirstAppearence());
    }

    //Evento que se ejecutará al encontrar al Nyaffy por primera vez (en proceso)
    IEnumerator NyaffyFirstAppearence() //(90% completado) pendiente de revisión
    {
        isHouseEventReady = false;
        nyaffy.transform.LookAt(underHouseCam.transform);
        StartDialogue(inkJSONCouldBeACat); //mensaje de "es un gato"
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        StartCoroutine(nyaffyMovementScript.FirstTranslation()); //traslado al Nyaffy
        yield return new WaitForSeconds(1f);
        StartDialogue(inkJSONWhereTheCatGoes);//mensaje de "oye a donde vas?
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        underHouseCam.SetActive(false);
        yield return new WaitForSeconds(2f);
        playerAnim.Play("CrouchedToStand");
        StartDialogue(inkJSONFollowTheCat); //mensaje de "ha ido hacia los cultivos"
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        playerMovementScript.enabled = true;
        playerImaginationScript.enabled = true;//script de imaginación de Nia Activado
        isCropsEventReady = true; //pendiente de quitar?
        cropsColliderBox.SetActive(true);
        yield return new WaitForSeconds(2f);

    }
    //Evento que se activará al entrar en un collider cerca de los cultivos (en proceso)
    public IEnumerator CropsEvent() //(90% completado)
    {
        nyaffy.transform.LookAt(player.transform);
        StartDialogue(inkJSONDoNotEatCrops); //mensaje del player "eso no es para ti"
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        StartCoroutine(nyaffyMovementScript.SecondTranslation()); //iniciamos el siguiente movimiento del nyaffy
        yield return new WaitForSeconds(1f);
        //animación de confusión?
        StartDialogue(inkJSONMissedNyaffy); //mensaje del player "a buscar al gato"
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        yield return new WaitForSeconds(3f);
        StartDialogue(inkJSONExtraInfoFindNyaffy);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        nyaffyAreaColliderBox.SetActive(true); //activamos el area para encontrar el Nyaffy
        isCropsEventReady = false; //el evento ha terminado
        isFoundNyaffyEventReady = true;
        Debug.Log("Se ha activado el bool y esperamos hasta encontrar al gato de nuevo");
        yield return new WaitUntil(nyaffyAlphaManager.isAphaMax);
        Debug.Log("El nyaffy está con alfa al máximo");
        EventStatus();

        Debug.Log("He terminado la corrutina Crops");
    }
    //Evento que se ejecutará cuando encuentres al nyaffy por segunda vez (pendiente)
    IEnumerator FoundNyaffyEvent()
    {

        Debug.Log("He encontrado al gato por segunda vez");
        nyaffyAreaColliderBox.SetActive(false);
        finalEventColliderBox.SetActive(true); //evento de salida de la escena
        nyaffy.transform.LookAt(player.transform); //el nyaffy mira al player
        //el nyaffy ejecuta una animación (pendiente)
        StartDialogue(inkJSONFoundNyaffy); //dialogo "gato encontrado"
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        StartCoroutine(nyaffyMovementScript.FinalTranslation()); //corrutina movimiento final del nyaffy
        StartDialogue(inkJSONWhatIsThis); //mensaje del player de "a donde vas?"
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        isFoundNyaffyEventReady = false;
        isFinalEventReady = true;


    }
    //Evento que se ejecutará cuando el player recoja el pez
    IEnumerator FinalEvent()
    {
        fishfigure.SetActive(false);
        fishPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        fishPanel.SetActive(false);
        StartDialogue(inkJSONFishCathched);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        StartDialogue(inkJSONChaseTheCat);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        StartCoroutine(AutoPlayerMove());
        //Si llego hasta aquí.......
        Debug.Log("Ruta de personaje terminada!!!!!!!!");
        yield return null;
    }
    #endregion

    //corrutina que llevará al player automáticamente al final de la demo (podria cambiar a cambio de escena)
    IEnumerator AutoPlayerMove()
    {
        Debug.Log("He hecho que el player se mueva solo hacia el collider final");
        player.transform.LookAt(finalEventColliderBox.transform);
        
        while (player.transform.position != finalEventColliderBox.transform.position)
        { 
            playerAnim.Play("Run");
            player.transform.position = Vector3.MoveTowards(player.transform.position, finalEventColliderBox.transform.position, playerSpeed * Time.deltaTime);
            yield return null; //espera al siguiente frame
        }
        //if (player.transform.position == finalEventColliderBox.transform.position)
        //{
        //    player.SetActive(false);
        //    thanksPanel.SetActive(true);

        //}
    }


}
