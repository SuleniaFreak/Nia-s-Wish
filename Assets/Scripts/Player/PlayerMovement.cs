using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;

    [Header("Intro Settings")]
    Rigidbody rb;
    Collider col;

    Animator anim;
    Vector3 movement;
    float h, v;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.Play("Sitting");
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    void Update()
    {
        //inhabilita el movimiento si hay una conversación y pone animación idle
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        { 
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
