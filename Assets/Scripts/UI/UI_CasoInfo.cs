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
    [SerializeField] public int confianza = 1;
    [SerializeField] SliderController sliderController;
    // Start is called before the first frame update
    void Start()
    {
        // Inicializar los valores de confianza
        confianza = 1; // Valor inicial
        sliderController.currentConfianza = confianza; // Sincronizar con el slider
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
        confianza = caseData.C_Confianza;
        sliderController.currentConfianza = confianza; // Actualizar el slider con el valor de confianza
        Debug.Log("Curriculum de" + caseData.C_Nombre + " actualizado en UI con confianza: " + confianza);
    }

    

}
