using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using KrillAudio.Krilloud;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;

    #region Public_Variables
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText; //texto de quien habla
    private Animator dialogueBoxAnimator; //cambia de animación del panel de dialogo

    [Header("Choices UI")]// en pruebas
    [SerializeField] private GameObject[] choicesButtons; //en pruebas
    private TextMeshProUGUI[] choicesButtonsTexts; //en pruebas
    private bool waitingForChoice;


    [Header("New Game Button")]
    [SerializeField] private NewGame newGameButton;
    #endregion

    #region Private_Variables
    private Story currentStory;//Variable para usar el archivo JSON del dialogo
    public bool dialogueIsPlaying;

    [Header("Tags Management")]
    private const string SPEAKER_TAG = "speaker";
    private const string LAYOUT_TAG = "layout";

    [Header("Krilloud")]
    KLAudioSource source;
    #endregion

    private void Awake()
    {
        //Comprobación de si instance es distinto de null en awake
        if (instance != null)
        {
            Debug.LogWarning("Hay más de un DialogueManager en la escena");
        }
        instance = this;

        source = GetComponent<KLAudioSource>();
    }


    //método de acceso que devuelve el singleton
    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {

        dialogueIsPlaying = false;
        dialogueBox.SetActive(false);
        CatchingDialogueReferences();

    }

    private void Update()
    {
        //si no hay ningún dialogo, salte de aqui
        if (!dialogueIsPlaying)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !waitingForChoice)
        {
            ContinueStory();
            source.Play();
        }
    }

    public void CatchingDialogueReferences()
    {
        dialogueBoxAnimator = dialogueBox.GetComponent<Animator>();
        waitingForChoice = false;
        //inicializamos la array de los textos de los botones para que cuadre
        choicesButtonsTexts = new TextMeshProUGUI[choicesButtons.Length];
        int index = 0; 
        foreach (GameObject choice in choicesButtons)
        {
            choicesButtonsTexts[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    #region Story_Methods

    //método que iniciará el dialogo
    public void EnterDialogueMode(TextAsset inkJSON)
    {
        //Inicializamos la variable asignandole el valor del parámetro de entrada
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialogueBox.SetActive(true);
        source.SetFloatVar(KL.Variables.panelmode, 0);
        source.Play(); 

        //Resetamos los cuadros de texto y nombre para que no guarde nada de otras conversaciones
        displayNameText.text = "???";
        dialogueBoxAnimator.Play("idleBox");

        ContinueStory();

    }

    //método para cerrar el dialogo si ha terminado el texto
    private IEnumerator ExitDialogueMode()
    {
        source.SetFloatVar(KL.Variables.panelmode, 1);
        source.Play(); 
        if (newGameButton.isIntroPlaying)
        {
            dialogueBoxAnimator.Play("introClose");
        }
        else
        {
            dialogueBoxAnimator.Play("idleClose"); //ejecuta la animación para ocultar el panel antes de apagarlo
            yield return new WaitForSeconds(0.5f);
        }
        dialogueIsPlaying = false;
        dialogueBox.SetActive(false);
        dialogueText.text = "";

    }

    //método llamado para comprobar si quedan lineas de dialogo
    private void ContinueStory()
    {
        //Si aún no ha finalizado el texto del dialogo, continua
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();

            //llamada al método que gestionará los tags de inky 
            HandleTags(currentStory.currentTags);

            //Despliega las opciones de botones de ink en caso de que haya
            DisplayChoices(); // en pruebas
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

    //método que convertirá los tags de inky en c# para gestionar animaciones del dialogueBox
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

            //creamos un switch para determinar que hacer en cada caso según el resultado
            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue; //Muestra el nombre del hablante segun el tag en ink
                    //Debug.Log("speaker=" + tagValue);
                    break;
                case LAYOUT_TAG:
                    dialogueBoxAnimator.Play(tagValue); //ejecutará las animaciones según el tag en ink
                    // Debug.Log("layout=" + tagValue);
                    break;
            }


        }
    }
    #endregion

    #region choice_Methods

    private void DisplayChoices()
    {
        //creamos una lista para almacenar las diferentes opciones (si las tiene)
        List<Choice> currentChoices = currentStory.currentChoices;
        //comprobamos que nuestra UI pueda asumir el número de opciones que estamos seleccionando.
        if(currentChoices.Count > choicesButtons.Length)
        {
            Debug.LogError("Se dieron más opciones de las que UI puede asumir. Nº de opciones dadas:" + currentChoices.Count);
        }

        //Nos aseguramos que los botones estén desactivados al iniciar el juego.
        for (int i = 0; i < choicesButtons.Length; i++)
        {
            choicesButtons[i].gameObject.SetActive(false);
        }

        //habilitamos e inicializamos las opciones disponibles en el diálogo.
        int index = 0;
        foreach  (Choice choice in currentChoices)
        {
            waitingForChoice = true;
            //activa los botones
            choicesButtons[index].gameObject.SetActive(true);
            //sobreescribe el texto de los botones con los de las opciones del inkJSON a través de la posicion index
            choicesButtonsTexts[index].text = choice.text;
            index++; //añadimos valor a index para que siga recorriendo el bucle
        }

    }

    //método que irá en el onclick de los botones con el parámetro de entrada definido según la opción elegida
    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
        waitingForChoice = false;
        source.Play(); //en pruebas
    }

    #endregion
}

