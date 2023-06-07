using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerobj;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float rotationSpeed;


    private void Start()
    {
       // Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    private void Update()
    {
        //calculamos la orientación del player para saber donde es "mirar hacia adelante"
        //rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        //rotamos el player
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");
        //definimos la orientación en base a los inputs
        Vector3 inputDir = orientation.forward * vInput + orientation.right * hInput;

        if(inputDir != Vector3.zero)
        {
            playerobj.forward = Vector3.Slerp(playerobj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }

    }
}
