using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class SliderController : MonoBehaviour
{
    public XRSlider slider; //Slider físico
    public UnityEngine.UI.Slider sliderUI; //Slider dentro de la UI
    [SerializeField] Transform handle;
    [SerializeField] Vector3 minLimite; // valores locales
    [SerializeField] Vector3 maxLimite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SincronizarSlider(float value)
    {
        if (sliderUI != null)
        {
            sliderUI.value = Mathf.Lerp(sliderUI.minValue, sliderUI.maxValue, value);
        }
    }

    public void GuardarValoracion(CVItem cv)
    {
        if (cv != null && sliderUI != null)
        {
            cv.CVData.C_Confianza = Mathf.RoundToInt(sliderUI.value);
            Debug.Log($"Valor guardado en {cv.CVData.name}: {cv.CVData.C_Confianza}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        SincronizarSlider(slider.value);
    }

}
