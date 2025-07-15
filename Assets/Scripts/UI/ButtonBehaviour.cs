using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour
{
    [SerializeField] SliderController slideInfo;
    [SerializeField] UI_CasoInfo uI_CasoInfo;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveButton()
    {
        slideInfo.GuardarValoracion(Inventario.Instance.cvItems[Inventario.Instance.ActualCase]);
    }

    public void NextRightButton()
    {
        Inventario.Instance.ActualCase++;
        if (Inventario.Instance.ActualCase >= Inventario.Instance.cvItems.Count)
        {
            Inventario.Instance.ActualCase = 0;
        }
        uI_CasoInfo.UpdateUI(Inventario.Instance.cvItems[Inventario.Instance.ActualCase]);
    }

    public void NextLeftButton()
    {
        Inventario.Instance.ActualCase--;
        if (Inventario.Instance.ActualCase < 0)
        {
            Inventario.Instance.ActualCase = Inventario.Instance.cvItems.Count - 1;
        }
        uI_CasoInfo.UpdateUI(Inventario.Instance.cvItems[Inventario.Instance.ActualCase]);
    }
   
}

    