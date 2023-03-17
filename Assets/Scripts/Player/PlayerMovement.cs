using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;

    Animator anim;
    Vector3 movement;
    float h, v;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //inhabilita el movimiento si hay una conversación

        //rehabilitar cuando se introduzca el dialogo
       // if (DialogueManager.GetInstance().dialogueIsPlaying)
       // {
         //   return;
       // }

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
}
