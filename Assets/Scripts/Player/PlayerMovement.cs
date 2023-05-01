using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KrillAudio.Krilloud;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;

    [Header("Audio")]
    KLAudioSource source; 
    float valueMaterial;

    [Header("Character controller")]
    CharacterController playerController;

    [Header("Intro Settings")]
    Rigidbody rb;
    Collider col;

    Animator anim;
    float h, v;
    int finalEventSpeed = 10;
    void Start()
    {
        CatherReferences();
        anim.Play("Sitting");
    }

    void Update()
    {
        //inhabilita el movimiento si hay una conversación
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            anim.SetBool("IsRunning", false);
            return;
        }

        Movement();
        Animating();
    }

    void CatherReferences()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        source = GetComponent<KLAudioSource>();
        playerController = GetComponent<CharacterController>(); // en pruebas

    }

    private void Movement()//por transform, se pone en update (si es por rb se pone en fixed)
    {
        h = Input.GetAxis("Horizontal");
        transform.Rotate(h * turnSpeed * Vector3.up * Time.deltaTime);
        v = Input.GetAxis("Vertical");
        Vector3 deltaPos = transform.forward * v * moveSpeed * Time.deltaTime; // en pruebas
        playerController.Move(deltaPos);// en pruebas
         
    }

    #region sound_and_animation_methods
    void Animating()
    {
        if (h != 0 || v != 0)
        {
            anim.SetBool("IsRunning", true);
        }
        else
        {
            anim.SetBool("IsRunning", false);
        }
    }

    public void soundStep() //en pruebas
    {
        source.Stop();
        source.SetFloatVar(KL.Variables.steps, valueMaterial);
        source.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "wood")
        {
            valueMaterial = 1;
        }
        else
        {
            valueMaterial = 0;
        }

    }

    #endregion

    #region IntroMethods

    //método que irá como evento dentro de la animación de standUp
    public void EnableRbAndCollider()
    {
        //activar el collider y la gravedad del rigidbody del player
        rb.useGravity = true;
        col.enabled = true;
        anim.applyRootMotion = true;
    }

    #endregion

    #region Final_event_Method

    public void MoveForward()
    {
        Vector3 direction = transform.forward * finalEventSpeed * Time.deltaTime;
        playerController.Move(direction);
    }

    #endregion


}
