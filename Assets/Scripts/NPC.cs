using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public CaseData miCV;
    public GameObject prefabHojaCV;
    public Transform puntoEntrega;
    private NavMeshAgent agente;

    [SerializeField] Transform SitPoint;
    [SerializeField] Transform StartPoint;
    [SerializeField] Animator animNPC;
    [SerializeField] private Transform jugador;

    private bool isWalking;
    [SerializeField] public bool StartWalking = false;
    private bool isSitting = false;
    public NPCState currentState = NPCState.Idle;

    [SerializeField] private GameObject preguntasPanel;
    [SerializeField] private Transform CanvasTransform;
    [SerializeField] private Animator preguntasAnimator;
    private bool preguntasMostradas = false;
    public bool TerminePreguntas = false;
    public enum NPCState
    {
        Idle,
        Walking,
        Sitting,
        ReadyForQuestions,
        Talking,
        GivingCV
    }

    void Awake()
    {
        agente = GetComponent<NavMeshAgent>();
        agente.Warp(StartPoint.position);
    }

    void Update()
    {
        if (StartWalking && !isWalking)
        {
            IniciarMovimiento(SitPoint);
        }
        if (TerminePreguntas && currentState == NPCState.Talking)
        {
            LevantarNPC();
        }

        if (isWalking && !agente.pathPending && agente.remainingDistance <= agente.stoppingDistance && !isSitting)
        {
            LlegarAlDestino();
        }
    }

    public void SetCV(CaseData cv)
    {
        this.miCV = cv;
    }

    public void EntregarCV()
    {
        GameObject hoja = Instantiate(prefabHojaCV, puntoEntrega.position, puntoEntrega.rotation);
        hoja.GetComponent<CVItem>().CVData = this.miCV;
        hoja.transform.SetParent(null); // Desanclar del mundo
        currentState = NPCState.GivingCV;
        StartCoroutine(EntregaCV());
    }

    IEnumerator EntregaCV()
    {
        //Posible animación de dar CV
        yield return new WaitForSeconds(0.5f);
        LevantarNPC();
        yield return new WaitForSeconds(5f);

    }

    public void IniciarMovimiento(Transform destino)
    {
        isWalking = true;
        agente.isStopped = false;
        agente.updatePosition = true;
        agente.updateRotation = true;
        agente.SetDestination(destino.position);
        animNPC.SetBool("IsWalking", true);
        currentState = NPCState.Walking;
    }

    private void LlegarAlDestino()
    {
        isWalking = false;
        StartWalking = false;
        MirarAlJugador();
        SentarNPC();
    }

    void SentarNPC()
    {
        agente.isStopped = true;
        agente.updatePosition = false;
        agente.updateRotation = false;
        agente.enabled = false;
        currentState = NPCState.Sitting;

        animNPC.SetBool("IsWalking", false);
        StartCoroutine(Sentarse());
    }

    public void LevantarNPC()
    {
        Levantarse();

    }

    IEnumerator Sentarse()
    {
        yield return new WaitForSeconds(0.5f);
        animNPC.SetTrigger("Sentarse");

        yield return new WaitForSeconds(1f);
        //CanvasTransform.position = new Vector3(-0.391f, 1.226f, 0.152f);
        currentState = NPCState.ReadyForQuestions;
    }
    IEnumerator Levantarse()
    {
        animNPC.SetTrigger("Levantarse");
        yield return new WaitForSeconds(1f);
        IniciarMovimiento(StartPoint);
    }
    private void MirarAlJugador()
    {
        Vector3 lookPosition = jugador.position;
        lookPosition.y = transform.position.y;
        transform.LookAt(lookPosition);
    }
    public void OnClickNPC() // Llamas este método cuando haces click en tu NPC
    {
        if (currentState == NPCState.ReadyForQuestions)
        {
            TogglePreguntas();
        }
    }

    IEnumerator MostrarPreguntas()
    {
        preguntasPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        preguntasAnimator.SetTrigger("Open");
    }
    IEnumerator OcultarPreguntas()
    {
        preguntasAnimator.SetTrigger("Close");
        yield return new WaitForSeconds(0.5f);
        preguntasPanel.SetActive(false);
    }


    public void TogglePreguntas()
    {
        preguntasMostradas = !preguntasMostradas;
        if (preguntasMostradas)
        {
            StartCoroutine(MostrarPreguntas());
        }
        else
        {
            StartCoroutine(OcultarPreguntas());
        }
    }

    public void HablarNPC(int pregunta)
    {
        if (currentState == NPCState.ReadyForQuestions)
        {
            currentState = NPCState.Talking;
            TogglePreguntas();
            //Agrega interfaz de diálogo aquí
            if (pregunta == 1)
            {
                StartCoroutine(EsperarYHablar(miCV.C_RespuestaPregunta1));
            }
            if (pregunta == 2)
            {
                StartCoroutine(EsperarYHablar(miCV.C_RespuestaPregunta2));
            }
            if (pregunta == 3)
            {
                TerminePreguntas = true;
            }
        }
    }

    IEnumerator EsperarYHablar(String respuesta)
    {
        yield return new WaitForSeconds(0.5f);
        animNPC.SetTrigger("Talk");
        Debug.Log("Diálogo:" + respuesta);
        yield return new WaitForSeconds(1f);
        currentState = NPCState.ReadyForQuestions;

    }



    
}
