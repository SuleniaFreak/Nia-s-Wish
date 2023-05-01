using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannotLeave : MonoBehaviour
{
    [Header("inkJSON")]
    [SerializeField] private TextAsset inkJSONCannotLeave;

    private void OnCollisionEnter(Collision collision)
    {
          if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("entro en el if");
          DialogueManager.GetInstance().EnterDialogueMode(inkJSONCannotLeave);
        }  
    }
   

}
