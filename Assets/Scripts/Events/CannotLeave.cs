using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannotLeave : MonoBehaviour
{
    [Header("inkJSON")]
    [SerializeField] private TextAsset inkJSONCannotLeave;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DialogueManager.GetInstance().EnterDialogueMode(inkJSONCannotLeave);
        }
    }

}
