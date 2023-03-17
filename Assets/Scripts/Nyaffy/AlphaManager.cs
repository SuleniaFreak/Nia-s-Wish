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
    private void Awake()
    {
        colorBase = nyaffyBase.color;

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
        }
        else if (colorBase.a <= 0f)
        {
            colorBase.a = 0;
            colorFace.a = 0;
        }
        nyaffyBase.color = colorBase;
        nyaffyFace.color = colorFace;
    }
}
