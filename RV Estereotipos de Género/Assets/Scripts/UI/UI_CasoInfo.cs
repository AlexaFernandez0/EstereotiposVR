using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class UI_CasoInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Nombre;
    [SerializeField] TextMeshProUGUI Profesion;
    [SerializeField] TextMeshProUGUI Genero;
    [SerializeField] TextMeshProUGUI Puesto;
    [SerializeField] int confianza;
    [SerializeField] SliderController sliderController;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateUI(CVItem cvItem)
    {
        CaseData caseData = cvItem.CVData;
        Nombre.text = caseData.C_Nombre;
        Profesion.text = caseData.C_Profesion;
        Genero.text = caseData.C_Sexo;
        Puesto.text = caseData.C_Puesto;
        sliderController.slider.value = caseData.C_Confianza;
        sliderController.sliderUI.value = caseData.C_Confianza;
    }

    

}
