using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    #region Public_Variables

    [Header("Colliders Gameobjects Events")]
    [SerializeField] private GameObject bellSoundBox;
    [SerializeField] private GameObject cropsColliderBox; //sin usar aún

    [Header("Characters")]
    [SerializeField] private GameObject player;
    //[SerializeField] private GameObject nyaffy;//sin usar aún    

    [Header("Cinemachine")]
    [SerializeField] private GameObject underHouseCam;//sin usar aún

    [Header("SFX Files")]
    [SerializeField] private AudioClip bell1;
    [SerializeField] private AudioClip bell2;



    #region JSON_Files
    [Header("Collected flowers Event")]
    [SerializeField] private TextAsset inkJSONFlowers;
    [SerializeField] private TextAsset inkJSONSurprise;
    [SerializeField] private TextAsset inkJSONSurprise2;

    [Header("Back House Event")]
    [SerializeField] private TextAsset inkJSONBackHouse; //sin usar aún /sin crear el archivo
    [SerializeField] private TextAsset inkJSONBackHouse2; //sin usar aún /sin crear el archivo

    [Header("Crops Event")]
    [SerializeField] private TextAsset inkJSONCrops; //sin usar aún /sin crear el archivo
    [SerializeField] private TextAsset inkJSONMissedNyaffy; //sin usar aún /sin crear el archivo

    [Header("Final Event")]
    [SerializeField] private TextAsset inkJSONFoundNyaffy; //sin usar aún /sin crear el archivo
    [SerializeField] private TextAsset inkJSONFinalDialogue; //sin usar aún /sin crear el archivo

    #endregion

    #endregion

    #region Private_Variables

    [Header("Characters")]
    Animator playerAnim; //sin usar aún
    PlayerImagination playerImaginationScript;//sin usar aún 
    PlayerMovement playerMovementScript;//sin usar aún
    Animator nyaffyAnim; //sin usar aún

    [Header("Cinemachine")]
    CinemachineVirtualCamera camPriority; //sin usar aún

    [Header("Audio")]
    AudioSource audioManager;
    AudioSource bellFull;

    int numFlowers = 0;

    bool cinematicCheck;

    #endregion


    private void Awake()
    {
        CatcherReferences();
    }

    #region General_Methods
    void CatcherReferences()
    {
        playerAnim = player.GetComponent<Animator>();//sin usar aún
       // nyaffyAnim = nyaffy.GetComponent<Animator>();//sin usar aún

        //CollectedFlowersEvent
        audioManager = GetComponent<AudioSource>();
        bellFull = bellSoundBox.GetComponent<AudioSource>();

        //BackHouseEvent
        playerImaginationScript = player.GetComponent<PlayerImagination>();
        playerMovementScript = player.GetComponent<PlayerMovement>();

    }
    void StartDialogue(TextAsset inkJSON)
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
        StopAllCoroutines();

    }

    //Evento que se ejecutará al interactuar con el collider de detrás de la casa (en proceso)
    IEnumerator BackHouseEvent()
    {
        Debug.Log("He iniciado la corrutina");
        bellFull.Stop();
        playerMovementScript.enabled = false;
        StartDialogue(inkJSONBackHouse);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        //animación de agacharse
        StartDialogue(inkJSONBackHouse2);
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);

        //cuadro pequeño de pulsa el botón derecho del ratón setactive true 
        if (Input.GetMouseButtonDown(0))
        {
            // se cierra el panel y la cámara cambia a debajo de la casa
            //prioridad de la cámara
            
            playerImaginationScript.enabled = true;//script de imaginación de Nia Activado
        }
        yield return new WaitForSeconds(2f);
    }

    //Evento que se ejecutará al encontrar al Nyaffy por primera vez (pendiente)
    IEnumerator NyaffyFirstAppearence()
    {

        //mensaje de "es un gato y se dirige a los cultivos"
        //ver al gato irse hacia la siguiente posición de la array
        //cambiar la cámara de prioridad
        //animación de levantarse
        //activar de nuevo el script de movimiento del player
        playerMovementScript.enabled = true;
        //desactivar el collider que activó el evento de la casa (en proceso)
        yield return new WaitForSeconds(2f);

    }
    //Evento que se activará al entrar en un collider cerca de los cultivos (pendiente)
    IEnumerator CropsEvent()
    {

        //el nyaffy mira al jugador
        //mensaje del player "eso no es para ti"
        //el nyaffy hace una animación y desaparece
        //mensaje del player "a buscar al gato"
        //desactivar el collider del evento


        yield return new WaitForSeconds(2f);
    }
    //Evento que se ejecutará cuando encuentres al nyaffy por segunda vez
    IEnumerator FoundNyaffyEvent()
    {
        //habilitar el collider del final del evento
        //el nyaffy mira al player y ejecuta una animación
        //mensaje del player "te he pillado"
        //El nyaffy gira hacia la salida
        //suelta un objeto en el suelo
        //se marcha por los arbustos (setactive false al contacto con el collider)
        //mensaje del player de "a donde vas?"
        //mensaje de que es eso que hay en el suelo


        yield return new WaitForSeconds(2f);
    }
    //Evento que se ejecutará cuando el player recoja el pez
    IEnumerator FinalEvent()
    {

        //panel pequeño mostrando el pez
        //mensaje del player "oh esto es...."
        //el player va hacia el collider mientras la pantalla hace fundido a negro
        //se muestra el panel de gracias por jugar con un botón de volver al main menú
        //Si llego hasta aquí.......Ruta de personaje terminada!!!!!!!!
        yield return new WaitForSeconds(2f);
    }


    #endregion


}
