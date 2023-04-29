using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KrillAudio.Krilloud;

public class AlphaManager : MonoBehaviour
{
    #region Public_Variables
    [Header("Changeable Materials")]
    [SerializeField] private Material nyaffyBase;
    [SerializeField] private Material nyaffyFace;
    [SerializeField] private Material nyaffyAlphaMax; //en pruebas

    [Header("Particle Systems")]
    [SerializeField] private GameObject TransparentParticleSystem;
    #endregion

    #region Private_Particles
    [Header("Materials")]
    private Color colorBase;
    private Color colorFace;

    [Header("Bools")]
    bool isShowing;
    bool isAlphaComplete;

    [Header("Audio")]
    KLAudioSource source;
    #endregion

    private void Awake()
    {
        colorBase = nyaffyBase.color;
        isAlphaComplete = false;
        source = GetComponent<KLAudioSource>();
    }

   
 
    private void Start()
    {
        SettingColors();
    }

    void Update()
    {
        AlphaManagement();
    }

    void SettingColors()
    {
        colorBase.a = 0f;
        nyaffyBase.color = colorBase;
        colorFace.a = 0f;
        colorFace = Color.white;
        nyaffyFace.color = colorFace;
    }

    public void TransparencyState(bool showing)
    {
        isShowing = showing;
    }

    public bool isAphaMax()
    {
        return isAlphaComplete;
    }

    //pendiente de mejorar el material para que se vea más claro y medir el tiempo de aparición
    void AlphaManagement()
    {
        if (isShowing)
        {
            colorBase.a += 0.2f * Time.deltaTime;
            colorFace.a += 0.2f * Time.deltaTime;
            TransparentParticleSystem.SetActive(true);
        }
        else if (!isShowing && colorBase.a < 1f)
        {
            colorBase.a -= 0.2f * Time.deltaTime;
            colorFace.a -= 0.2f * Time.deltaTime;
            TransparentParticleSystem.SetActive(false);
        }

        if (colorBase.a >= 1f)
        {
            colorBase.a = 1f;
            colorFace.a = 1f;
            isAlphaComplete = true;
            TransparentParticleSystem.SetActive(false);
        }
        else if (colorBase.a <= 0f)
        {
            colorBase.a = 0;
            colorFace.a = 0;
        }
        nyaffyBase.color = colorBase;
        nyaffyFace.color = colorFace;
    }


    #region on_animation_methods
    //método que será llamado desde una animación del Nyaffy
    public void ResetTransparency()
    {
        source.SetFloatVar(KL.Variables.nyaffymode, 2);//en pruebas
        source.Play(KL.Tags.nyaffymovement); // en pruebas
        colorBase.a = 0;
        colorFace.a = 0;
        isAlphaComplete = false;

        //pendiente de un sistema de particulas (por determinar si aquí o en la animación)
    }

    //método en pruebas
    public void Jumping()
    {
        source.SetFloatVar(KL.Variables.nyaffymode, 3);
        source.Play(KL.Tags.nyaffymovement);
    }

    #endregion
}
