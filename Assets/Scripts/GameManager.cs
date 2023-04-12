using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Flowers Counter")]
    private int numFlowers = 0;

    [Header("JSON Files")]
    [SerializeField] private TextAsset inkJSONFlowers; //sin usar aún / sin crear el archivo
    [SerializeField] private TextAsset inkJSONBackHouse; //sin usar aún /sin crear el archivo
    [SerializeField] private TextAsset inkJSONCrops; //sin usar aún /sin crear el archivo
    [SerializeField] private TextAsset inkJSONFinalDialogue; //sin usar aún /sin crear el archivo

    bool cinematicCheck;


    public void AddFlower()
    {
        numFlowers++;

        //llamar al método que activará el evento cuando recojas 6 flores
        if (numFlowers == 3)
        {
            StartCoroutine(CollectedFlowersEvent());
        }


        Debug.Log("He recogido: " + numFlowers.ToString());
    }

    void StartDialogue(TextAsset inkJSON)
    {
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
    }

    #region Events
    //Evento que se ejecuta al recoger las flores
    IEnumerator CollectedFlowersEvent()
    {
        Debug.Log("He iniciado la corrutina");
        StartDialogue(inkJSONFlowers);//mensaje del player "he recogido las flores"
        yield return new WaitUntil(DialogueManager.GetInstance().IsNotDialoguePlaying);
        //sonido de cascabel
        //giro del personaje hacia la fuente del sonido
        //mensaje de" he escuchado algo, parece que viene de detrás de la casa"
        yield return new WaitForSeconds(2f);
    }

    //Evento que se ejecutará al encontrar al Nyaffy por primera vez
    IEnumerator NyaffyFirstAppearance()
    {

        //animación de levantarse
        //mensaje de "es un gato y se dirige a los cultivos"
        //desactivar el collider que activó el evento de la casa (en proceso)
        yield return new WaitForSeconds(2f);

    }

    IEnumerator Event3()
    {
        //evento que se activará al entrar en un collider cerca de los cultivos
        //el nyaffy mira al jugador
        //mensaje del player "eso no es para ti"
        //el nyaffy hace una animación y desaparece
        //mensaje del player "a buscar al gato"
        //desactivar el collider del evento


        yield return new WaitForSeconds(2f);
    }

    IEnumerator Event4()
    {
        //evento que se ejecutará cuando encuentres al nyaffy por segunda vez
        //el nyaffy mira al player y ejecuta una animación
        //mensaje del player "te he pillado"
        //El nyaffy gira hacia la salida
        //suelta un objeto en el suelo
        //se marcha por los arbustos (setactive false al contacto con el collider)
        //mensaje del player de "a donde vas?"
        //mensaje de que es eso que hay en el suelo


        yield return new WaitForSeconds(2f);
    }

    IEnumerator FinalEvent()
    {
        //Evento que se ejecutará cuando el player recoja el pez
        //panel pequeño mostrando el pez
        //mensaje del player "oh esto es...."
        //el player va hacia el collider mientras la pantalla hace fundido a negro
        //se muestra el panel de gracias por jugar con un botón de volver al main menú
        yield return new WaitForSeconds(2f);
    }


    #endregion


}
