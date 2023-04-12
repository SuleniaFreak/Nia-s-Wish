using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ImaginationCam : MonoBehaviour
{
    #region Public_Variables

    [Header("Screen Limit Angles")]
    [SerializeField] private float bottonAngle; //eje x
    [SerializeField] private float topAngle;

    [Header("Camera Rotation Settings")]
    [SerializeField] private float yRotationSpeed;
    [SerializeField] private float xCamSpeed;

    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity;
    #endregion

    #region Private_Variables

    [Header("Desired rotation Variables")]
    float desiredYRotation; 
    float desiredXRotation;

    [Header("Current Rotation Variables")]
    float currentYRotation; 
    float currentXRotation;

    [Header("Velocity Management")]
    float rotationYVelocity;
    float camXVelocity;

    [Header("Mouse and Camera")]
    float mouseX;
    float mouseY;

    #endregion

    private void Awake()
    {
        
    }

    private void Update()
    { 
        if (!Input.GetMouseButton(1))
        {
            return;
        }
        MouseInputMovement();
    }

    private void FixedUpdate()
    {
        if (!Input.GetMouseButton(1))
        {
            //restablecer la rotación inicial cuando se deje de pulsar el botón (en proceso)
            return;
        }
        ApplyRotation();
    }

    void MouseInputMovement()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        //definimos la rotación
        desiredYRotation = desiredYRotation + (mouseX * mouseSensitivity);
        desiredXRotation = desiredXRotation - (mouseY * mouseSensitivity);
        //establecemos los límites de giro
        desiredXRotation = Mathf.Clamp(desiredXRotation, bottonAngle, topAngle);
        desiredYRotation = Mathf.Clamp(desiredYRotation, bottonAngle, topAngle);

    }

    void ApplyRotation()
    {
        //suavidad de rotación
        currentYRotation = Mathf.SmoothDamp(currentYRotation, desiredYRotation, ref rotationYVelocity, yRotationSpeed);
        currentXRotation = Mathf.SmoothDamp(currentXRotation, desiredXRotation, ref camXVelocity, xCamSpeed);
        
        transform.rotation = Quaternion.Euler(currentXRotation, currentYRotation, 0);
    }
}
