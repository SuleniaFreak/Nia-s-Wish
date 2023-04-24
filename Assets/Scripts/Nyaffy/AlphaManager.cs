using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaManager : MonoBehaviour
{
    [Header("Changeable Materials")]
    [SerializeField] private Material nyaffyBase;
    [SerializeField] private Material nyaffyFace;

    private Color colorBase;
    private Color colorFace;
    bool isShowing;
    bool isAlphaComplete;

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

    void AlphaManagement()
    {
        if (isShowing)
        {
            colorBase.a += 0.2f * Time.deltaTime;
            colorFace.a += 0.2f * Time.deltaTime;
        }
        else if (!isShowing && colorBase.a < 1f)
        {
            colorBase.a -= 0.2f * Time.deltaTime;
            colorFace.a -= 0.2f * Time.deltaTime;
        }

        if (colorBase.a >= 1f)
        {
            colorBase.a = 1f;
            colorFace.a = 1f;
            isAlphaComplete = true;
        }
        else if (colorBase.a <= 0f)
        {
            colorBase.a = 0;
            colorFace.a = 0;
        }
        nyaffyBase.color = colorBase;
        nyaffyFace.color = colorFace;
    }

    //método que será llamado desde una animación del Nyaffy
    public void ResetTransparency()
    {
        colorBase.a = 0;
        colorFace.a = 0;
        isAlphaComplete = false;
    }
}
