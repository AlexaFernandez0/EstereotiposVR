using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private Transform cvHolder;
    [SerializeField] private XRSocketInteractor socket;
    [SerializeField] private UI_CasoInfo UICaseInfo;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CV"))
        {
            CVItem cv = other.GetComponent<CVItem>();
            if (cv != null)
            {
                Inventario.Instance.AddCVItem(cv);
                UICaseInfo.UpdateUI(cv);
            }
            else
            { 
                Debug.LogWarning("CVItem component not found on the object: " + other.name);
            }
        }
    }
    
    
}
