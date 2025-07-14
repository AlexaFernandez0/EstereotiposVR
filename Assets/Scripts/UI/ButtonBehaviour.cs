using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour
{
    [SerializeField] bool SaveButton;
    [SerializeField] bool DeleteButton;
    [SerializeField] bool NextLeftButton;
    [SerializeField] bool NextRightButton;
    [SerializeField] SliderController slideInfo;
    [SerializeField] UI_CasoInfo uI_CasoInfo;

    private bool botonDisponible = true;
    public float cooldown = 0.5f; // medio segundo para evitar doble entrada

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other){
    if (!botonDisponible) return;

    if (other.CompareTag("RightHand") || other.CompareTag("LeftHand"))
    {
        // Validaciones antes de ejecutar la lógica
        if (slideInfo == null){
            Debug.LogError("SliderController no está asignado");
            return;
        }
        if (Inventario.Instance == null){
            Debug.LogError("Inventario no está asignado");
            return;
        }
        if(Inventario.Instance.cvItems == null){
            Debug.LogError("CvItems no están asignados correctamente");
            return;
        }
        if (Inventario.Instance.ActualCase < 0 || Inventario.Instance.ActualCase >= Inventario.Instance.cvItems.Count)
        {
            Debug.LogWarning("ActualCase está fuera de rango");
            return;
        }
                

        // Ejecuta la lógica del botón
            botonDisponible = false;
        Invoke(nameof(ReactivarBoton), cooldown);

        Debug.Log("Botón presionado: " + gameObject.name);

        if (SaveButton)
        {
            slideInfo.GuardarValoracion(Inventario.Instance.cvItems[Inventario.Instance.ActualCase]);
        }

        if (NextRightButton)
        {
            Inventario.Instance.ActualCase++;
            if (Inventario.Instance.ActualCase >= Inventario.Instance.cvItems.Count)
            {
                Inventario.Instance.ActualCase = 0;
            }
            uI_CasoInfo.UpdateUI(Inventario.Instance.cvItems[Inventario.Instance.ActualCase]);
        }

        if (NextLeftButton)
        {
            Inventario.Instance.ActualCase--;
            if (Inventario.Instance.ActualCase < 0)
            {
                Inventario.Instance.ActualCase = Inventario.Instance.cvItems.Count - 1;
            }
            uI_CasoInfo.UpdateUI(Inventario.Instance.cvItems[Inventario.Instance.ActualCase]);
        }
    }
}

void ReactivarBoton()
{
    botonDisponible = true;
}
   
}

    