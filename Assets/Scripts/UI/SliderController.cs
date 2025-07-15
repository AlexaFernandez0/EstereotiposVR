using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    //public XRSlider slider; //Slider físico
    //public UnityEngine.UI.Slider sliderUI; //Slider dentro de la UI
    //[SerializeField] Transform handle;
    [SerializeField] int minLimite;
    [SerializeField] int maxLimite;
    [SerializeField] Sprite FillAmount;
    [SerializeField] public int currentConfianza = 1;
    [SerializeField] List<GameObject> fillImage;
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        for (int i = 0; i < fillImage.Count; i++)
        {
            if (i < currentConfianza)
            {
                fillImage[i].SetActive(true);
            }
            else
            {
                fillImage[i].SetActive(false);
            }
        }
    }

    public void MoreButtton()
    {
        currentConfianza++;
        if (currentConfianza > maxLimite)
        {
            currentConfianza = maxLimite;
        }
    }
    
    public void LessButton()
    {
        currentConfianza--;
        if (currentConfianza < minLimite)
        {
            currentConfianza = minLimite;
        }
    }
    public void GuardarValoracion(CVItem cv)
    {
        if (cv != null)
        {
            cv.CVData.C_Confianza = currentConfianza;
            Debug.Log($"Valor guardado en {cv.CVData.name}: {cv.CVData.C_Confianza}");
        }
    }


}
