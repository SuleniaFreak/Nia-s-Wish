using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;

    [Header("Audio")]
    [SerializeField] private AudioSource AudioS; // en puebas
    [SerializeField] private AudioClip GrassSteps; //en pruebas

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
        AudioS = GetComponent<AudioSource>();
    }

    void Update()
    {
        //inhabilita el movimiento si hay una conversaci�n
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
        AudioS.Play();
    }

    #region IntroMethods

    //m�todo que ir� como evento dentro de la animaci�n de standUp
    public void EnableRbAndCollider()
    {
        //activar el collider y la gravedad del rigidbody del player
        rb.useGravity = true;
        col.enabled = true;
        anim.applyRootMotion = true;
    }

    #endregion


}
