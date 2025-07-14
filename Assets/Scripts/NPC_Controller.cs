using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Controller : MonoBehaviour
{
    public NPC npcMasculino;
    public NPC npcFemenino;
    public NPC currentNPC;

    void Update()
    {
        if(currentNPC != null && currentNPC.currentState == NPC.NPCState.Talking)
        {
            if(currentNPC.TerminePreguntas)
            {
                TerminarPreguntasNPC();
            }
        }
    }
    public void SpawnNPC(CaseData caso)
    {
        bool usarMasculino = caso.C_Sexo == "Hombre";
        NPC npc = Instantiate(usarMasculino ? npcMasculino : npcFemenino);
        npc.SetCV(caso);
        currentNPC = npc;
    }
    public void DestroyNPC()
    {
        if (currentNPC != null)
        {
            Destroy(currentNPC.gameObject);
            currentNPC = null;
        }
    }

    public void IniciarMovimientoNPC()
    {
        currentNPC.StartWalking = true;
    }

    public void TerminarPreguntasNPC()
    {
        currentNPC.TogglePreguntas();
        currentNPC.EntregarCV();
        DestroyNPC();
    }

}
