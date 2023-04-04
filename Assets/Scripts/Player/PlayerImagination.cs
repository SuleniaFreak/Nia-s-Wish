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


    CinemachineVirtualCamera virtualCamera;
    PlayerMovement playerMovement;

    private Color colorAlpha; //gestiona el alfa del color del nyaffy
    private Material objectColor; //gestiona el material del nyaffy
    void Start()
    {
        virtualCamera = transform.GetComponent<CinemachineVirtualCamera>();
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    void Update()
    {
        ray.origin = transform.position;
        ray.direction = transform.forward;
        ActivateImagination();
    }

    //m�todo que se activar� cuando estemos detr�s de la casa
    void ActivateImagination()
    {
        if (Input.GetMouseButton(1))
        {
            playerMovement.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            virtualCamera.Priority = 25;

            Debug.DrawRay(ray.origin, ray.direction * 100, Color.blue);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                ObjectDetected = hit.collider.gameObject;

                if (ObjectDetected.tag == "Nyaffy")
                {
                    //creo la variable para obtener el script del cubo
                    AlphaManager alphaManager = ObjectDetected.GetComponent<AlphaManager>();

                    alphaManager.TransparencyState(true);

                    Debug.Log("He comprobado que la etiqueta sea Nyaffy");
                }

                Debug.Log("He chocado con alguien con layer Hidden");
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
            virtualCamera.Priority = 8;
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