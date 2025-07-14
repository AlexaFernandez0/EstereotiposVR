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
            var interactable = rayInteractor.interactablesHovered[0] as MonoBehaviour;
            Debug.Log("Detectando: " + interactable.gameObject.name);
            var outline = interactable.GetComponent<Outline>();
            var npc = interactable.GetComponent<NPC>();

            if (outline != null)
            {
                bool puedeResaltar = (npc == null) || (npc.currentState == NPC.NPCState.ReadyForQuestions);
                outline.enabled = puedeResaltar;

                if (puedeResaltar)
                {
                    lastOutlined = interactable.gameObject;
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
