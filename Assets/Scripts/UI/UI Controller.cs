using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class UIController : MonoBehaviour
{
    public XRRayInteractor leftHandRay;
    public XRRayInteractor rightHandRay;

    private GameObject lastLeftOutlined;
    private GameObject lastRightOutlined;

    void Update()
    {
        HandleOutline(leftHandRay, ref lastLeftOutlined);
        HandleOutline(rightHandRay, ref lastRightOutlined);
    }

    void HandleOutline(XRRayInteractor rayInteractor, ref GameObject lastOutlined)
    {
        if (rayInteractor.interactablesHovered.Count > 0)
        {
            var interactable = rayInteractor.interactablesHovered[0];
            var outline = interactable.transform.GetComponent<Outline>();
            var npc = interactable.transform.GetComponent<NPC>();

            if (outline != null)
            {
                bool puedeResaltar = true;

                // Si es un NPC, solo permite resaltar si está listo para preguntas
                if (npc != null)
                {
                    puedeResaltar = npc.currentState == NPC.NPCState.ReadyForQuestions;
                }

                if (puedeResaltar)
                {
                    if (lastOutlined != null && lastOutlined != interactable.transform.gameObject)
                    {
                        lastOutlined.GetComponent<Outline>().enabled = false;
                    }

                    outline.enabled = true;
                    lastOutlined = interactable.transform.gameObject;
                    return;
                }
            }
        }

        if (lastOutlined != null)
        {
            lastOutlined.GetComponent<Outline>().enabled = false;
            lastOutlined = null;
        }
    }
}
