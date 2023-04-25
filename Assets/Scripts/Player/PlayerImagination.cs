using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerImagination : MonoBehaviour
{



    [Header("Raycast Settings")]
    Ray ray;
    RaycastHit hit;
    public LayerMask layerMask;
    GameObject ObjectDetected;

    [Header("Scripts")]
    CinemachineVirtualCamera virtualCamera;
    PlayerMovement playerMovement;

    [Header("Animator")]
    Animator playerAnim; //en pruebas

   
    void Start()
    {
        virtualCamera = transform.GetComponent<CinemachineVirtualCamera>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerAnim = GetComponentInParent<Animator>();// en pruebas
    }

    void Update()
    {
        ray.origin = transform.position;
        ray.direction = transform.forward;
        ActivateImagination();
    }



    //método que se activará cuando estemos detrás de la casa
    void ActivateImagination()
    {
        if (Input.GetMouseButton(1))
        {
            playerMovement.enabled = false;
            playerAnim.SetBool("IsRunning", false); //en pruebas
            Cursor.lockState = CursorLockMode.Locked;
            virtualCamera.Priority = 25;

            Debug.DrawRay(ray.origin, ray.direction * 100, Color.blue);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                ObjectDetected = hit.collider.gameObject;

                if (ObjectDetected.tag == "Nyaffy")
                {
                    //creo la variable para obtener el script
                    AlphaManager alphaManager = ObjectDetected.GetComponent<AlphaManager>();

                    alphaManager.TransparencyState(true);

                    Debug.Log("He comprobado que la etiqueta sea Nyaffy");
                }

                Debug.Log(hit.collider.gameObject);

            }
            else
            {
                if (ObjectDetected != null)
                {
                    AlphaManager alphaManager = ObjectDetected.GetComponent<AlphaManager>();
                    alphaManager.TransparencyState(false);
                    ObjectDetected = null;
                }

            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            virtualCamera.Priority = 6;
            playerMovement.enabled = true;

            if (ObjectDetected != null)
            {
                AlphaManager alphaManager = ObjectDetected.GetComponent<AlphaManager>();
                alphaManager.TransparencyState(false);
                ObjectDetected = null;
            }

        }
    }

}
