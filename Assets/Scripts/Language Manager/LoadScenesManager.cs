using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using KrillAudio.Krilloud;

public class LoadScenesManager : MonoBehaviour
{
    KLAudioSource source;

    private void Awake()
    {
        source = GetComponent<KLAudioSource>();
    }


    public void LoadSpanishScene()
    {
        StartCoroutine(SpanishCorrutine());
    }

    IEnumerator SpanishCorrutine()
    {
        source.SetFloatVar(KL.Variables.buttonstatus, 0);
        source.Play(KL.Tags.button);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("SpanishScene");
    }

    public void LoadEnglishScene()
    {
        StartCoroutine(EnglishCorrutine());
    }

    IEnumerator EnglishCorrutine()
    {
        source.SetFloatVar(KL.Variables.buttonstatus, 0);
        source.Play(KL.Tags.button);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("EnglishScene");
    }






}
