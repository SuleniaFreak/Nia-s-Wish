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
    KLAudioSource source; //en pruebas
    float valueMaterial; //en pruebas

    [Header("Intro Settings")]
    Rigidbody rb;
    Collider col;

    Animator anim;
    float h, v;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.Play("Sitting");
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        source = GetComponent<KLAudioSource>();
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

    private void Movement()//por transform, se pone en update (si es por rb se pone en fixed)
    {
        h = Input.GetAxis("Horizontal");
        transform.Rotate(h * turnSpeed * Vector3.up * Time.deltaTime);
        v = Input.GetAxis("Vertical");
        transform.Translate(v * moveSpeed * Vector3.forward * Time.deltaTime);
       
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
        switch (collision.gameObject.tag)
        {
            case "grass":
                valueMaterial = 0;
                break;
            case "wood":
                valueMaterial = 1;
                break;
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


}
