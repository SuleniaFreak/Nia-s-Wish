using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCueLookAt : MonoBehaviour
{
    [Header("Look At Settings")]
    [SerializeField] private Camera main;
    void Update()
    {
        transform.LookAt(main.transform);
    }
}
