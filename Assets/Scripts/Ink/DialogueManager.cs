using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialogueBox;// panel de dialogo
    [SerializeField] private TextMeshProUGUI dialogueText; //texto de dialogo
    [SerializeField] private TextMeshProUGUI displayNameText; //texto de quien habla
    private Animator dialogueBoxAnimator; //cambia de animaci�n del panel de dialogo

    [Header("New Game Button")]
    [SerializeField] private NewGame newGameButton;


    //Variable para usar el archivo JSON del dialogo
    private Story currentStory;
    public bool dialogueIsPlaying; //chequeamos si el dialogo est� activo

    [Header("Tags Management")]
    private const string SPEAKER_TAG = "speaker";
    private const string LAYOUT_TAG = "layout";

    private void Awake()
    {
        //Comprobaci�n de si instance es distinto de null en awake
        if (instance != null)
        {
            Debug.LogWarning("Hay m�s de un DialogueManager en la escena");
        }
        instance = this;
    }

    //m�todo que devuelve el singleton
    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialogueBox.SetActive(false);
        dialogueBoxAnimator = dialogueBox.GetComponent<Animator>();


    }

    private void Update()
    {
        //si no hay ning�n dialogo, salte de aqui
        if (!dialogueIsPlaying)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            ContinueStory();
        }
    }

    #region Story_Methods

    //m�todo que iniciar� el dialogo
    public void EnterDialogueMode(TextAsset inkJSON)
    {
        //Inicializamos la variable asignandole el valor del par�metro de entrada
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialogueBox.SetActive(true);

        //Resetamos los cuadros de texto y nombre para que no guarde nada de otras conversaciones
        displayNameText.text = "???";
        dialogueBoxAnimator.Play("idleBox");

        ContinueStory();

    }

    //m�todo para cerrar el dialogo si ha terminado el texto
    private IEnumerator ExitDialogueMode()
    {
        if (newGameButton.isIntroPlaying)
        {
            dialogueBoxAnimator.Play("introClose");
        }
        else
        {
            dialogueBoxAnimator.Play("idleClose"); //ejecuta la animaci�n para ocultar el panel antes de apagarlo
            yield return new WaitForSeconds(0.5f);
        }
        dialogueIsPlaying = false;
        dialogueBox.SetActive(false); //pendiente de ver si con evento de animacion funciona igual
        dialogueText.text = "";

    }

    //m�todo llamado para comprobar si quedan lineas de dialogo
    private void ContinueStory()
    {
        //Si a�n no ha finalizado el texto del dialogo, continua
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();

            //llamada al m�todo que gestionar� los tags en inky 
            HandleTags(currentStory.currentTags);
        }
        else
        { //si has acabado, cierra el panel de texto
            StartCoroutine(ExitDialogueMode());
        }
    }

    public bool IsNotDialoguePlaying()
    {
        return dialogueIsPlaying == false;
    }
    #endregion

    #region Ink_Tags_Methods

    //m�todo que convertir� los tags de inky en c# para gestionar animaciones del dialogueBox
    private void HandleTags(List<string> currentTags)
    {
        //creamos un loop para que revise cada una de las tags en el texto de inky
        foreach (string tag in currentTags)
        {
            //divitir el tag en partes a traves de :   
            string[] splitTag = tag.Split(':');

            //la linea anterior devuelve 2 arrays tipo string
            //donde la primera es la palabra clave (speaker o layout)
            string tagKey = splitTag[0].Trim();
            //y la segunda el valor (padre/Nia o playerBox/npcBox)
            string tagValue = splitTag[1].Trim();
            //trim se usa para eliminar cualquier espacio en blanco tanto al principio
            //como al final del string

            //creamos un switch para determinar que hacer en cada caso seg�n el resultado
            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue; //Muestra el nombre del hablante segun el tag en ink
                    //Debug.Log("speaker=" + tagValue);
                    break;
                case LAYOUT_TAG:
                    dialogueBoxAnimator.Play(tagValue); //ejecutar� las animaciones seg�n el tag en ink
                                                        // Debug.Log("layout=" + tagValue);
                    break;
            }


        }
    }

    #endregion
}

