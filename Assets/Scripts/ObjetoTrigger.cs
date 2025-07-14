using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoTrigger : MonoBehaviour
{
    private bool yaSeSentó = false;
    private void OnTriggerEnter(Collider other)
    {
        if (yaSeSentó) return;
        if (other.CompareTag("Player") && gameObject.CompareTag("Sillon"))
        {
            yaSeSentó = true;
            Debug.Log("El jugador ha entrado en el trigger.");
            GameController.Instance.playerIsSitting = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("Sillon"))
        {
            yaSeSentó = false;
            Debug.Log("El jugador se levantó.");
        }
    }


}
