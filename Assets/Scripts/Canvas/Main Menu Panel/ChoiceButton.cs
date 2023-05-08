using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KrillAudio.Krilloud;

public class ChoiceButton : MonoBehaviour
{
    [Header("Krilloud")]
    KLAudioSource source;

    private void Awake()
    {
        source = GetComponent<KLAudioSource>();
    }
    public void EventTriggerMouse()
    {
        source.SetFloatVar(KL.Variables.buttonstatus, 2);
        source.Play(KL.Tags.button);
    }
}
