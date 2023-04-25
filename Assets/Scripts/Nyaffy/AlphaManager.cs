using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaManager : MonoBehaviour
{
    #region Public_Variables
    [Header("Changeable Materials")]
    [SerializeField] private Material nyaffyBase;
    [SerializeField] private Material nyaffyFace;

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
    #endregion

    private void Awake()
    {
        colorBase = nyaffyBase.color;
        isAlphaComplete = false;
    }

    private void Start()
    {
        colorBase.a = 0f;
        nyaffyBase.color = colorBase;
        colorFace.a = 0f;
        colorFace = Color.white;
        nyaffyFace.color = colorFace;
    }

    void Update()
    {
        AlphaManagement();
    }

    public void TransparencyState(bool showing)
    {
        isShowing = showing;
    }

    public bool isAphaMax()
    {
        return isAlphaComplete;
    }

    //pendiente de mejorar el material para que se vea m�s claro y medir el tiempo de aparici�n
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
            //sistema de particulas adicional o sonido de que se ha completado (o ambos)
        }
        else if (colorBase.a <= 0f)
        {
            colorBase.a = 0;
            colorFace.a = 0;
        }
        nyaffyBase.color = colorBase;
        nyaffyFace.color = colorFace;
    }

    //m�todo que ser� llamado desde una animaci�n del Nyaffy
    public void ResetTransparency()
    {
        colorBase.a = 0;
        colorFace.a = 0;
        isAlphaComplete = false;
        //pendiente de un sistema de particulas (por determinar si aqu� o en la animaci�n)
    }
}
