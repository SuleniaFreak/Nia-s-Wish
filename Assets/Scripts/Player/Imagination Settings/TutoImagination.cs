using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoImagination : MonoBehaviour
{
    [Header("Raycast Settings")]
    Ray ray;
    RaycastHit hit;
    public LayerMask layerMask;
    GameObject ObjectDetected;

    [Header("Scripts")]
    PlayerMovement playerMovement;

    [Header("Nyaffy material settings")]
    Color colorAlpha; //gestiona el alfa del color del nyaffy
    Material objectColor; //gestiona el material del nyaffy
    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
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
            Cursor.lockState = CursorLockMode.Locked;
          
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

            if (ObjectDetected != null)
            {
                AlphaManager alphaManager = ObjectDetected.GetComponent<AlphaManager>();
                alphaManager.TransparencyState(false);
                ObjectDetected = null;
            }

        }
    }
}
