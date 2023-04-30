using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KrillAudio.Krilloud;

public class NyaffyMovement : MonoBehaviour
{
    #region Public_Variables
    [Header("Movement Settings")]
    [SerializeField] private Transform[] destinySpots;
    [SerializeField] private float walkSpeed;

    [Header("Characters and Objects")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject fishFigure;
    #endregion

    Animator anim;
    KLAudioSource source;
   
    private void Awake()
    {
        CatcherReferences();
    }


    void CatcherReferences()
    {
        anim = GetComponent<Animator>();
        source = GetComponent<KLAudioSource>();
    }

    #region Nyaffy_Translations
    //Corrutinas que gestionan el movimiento del Nyaffy por la escena
    public IEnumerator FirstTranslation() 
    {
        transform.LookAt(destinySpots[0].position);
        while (transform.position != destinySpots[0].position)
        {
            anim.Play("Walk");
            transform.position = Vector3.MoveTowards(transform.position, destinySpots[0].position, walkSpeed * Time.deltaTime);
            yield return null; //espera al siguiente frame
        }
        if (transform.position == destinySpots[0].position)
        {
            anim.Play("Idle");
        }

    }

    public IEnumerator SecondTranslation()
    {
        anim.Play("Jump");
        yield return new WaitForSeconds(2f);
        while (transform.position != destinySpots[1].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, destinySpots[1].position, walkSpeed * Time.deltaTime);
        }
        yield return null;
    }

    public IEnumerator FinalTranslation()
    {
        transform.LookAt(destinySpots[2].position);
        fishFigure.SetActive(true);
        while (transform.position != destinySpots[2].position) 
        {
            anim.Play("Walk");
            transform.position = Vector3.MoveTowards(transform.position, destinySpots[2].position, walkSpeed * Time.deltaTime);
            yield return null;
        }
    }
    #endregion

}
