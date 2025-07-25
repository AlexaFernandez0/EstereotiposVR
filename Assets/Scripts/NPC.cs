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

    [SerializeField] public Transform SitPoint;
    [SerializeField] public Transform StartPoint;
    [SerializeField] Animator animNPC;
    [SerializeField] private Transform jugador;

    private bool isWalking;
    [SerializeField] public bool StartWalking = false;
    private bool isSitting = false;
    public NPCState currentState = NPCState.Idle;

    [SerializeField] private GameObject preguntasPanel;
    //[SerializeField] private Transform CanvasTransform;
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

        jugador = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (StartWalking && !isWalking)
        {
            IniciarMovimiento(SitPoint);
        }
        if (TerminePreguntas && currentState == NPCState.ReadyForQuestions)
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
        Debug.Log("Entregando CV: " + miCV.C_Nombre);
        GameObject hoja = Instantiate(prefabHojaCV, puntoEntrega.position, puntoEntrega.rotation);
        hoja.GetComponent<CVItem>().CVData = this.miCV;
        hoja.transform.SetParent(null); // Desanclar del mundo
        currentState = NPCState.GivingCV;
        StartCoroutine(EntregaCV());
    }

    IEnumerator EntregaCV()
    {
        Debug.Log("Ya dejé el CV en el punto de entrega");
        //Posible animación de dar CV
        yield return new WaitForSeconds(0.5f);
        LevantarNPC();
        yield return new WaitForSeconds(5f);

    }

    public void IniciarMovimiento(Transform destino)
    {
        agente.isStopped = false;
        agente.updatePosition = true;
        agente.updateRotation = true;
        isWalking = true;
        agente.SetDestination(destino.position);
        currentState = NPCState.Walking;

        animNPC.SetBool("IsWalking", true);
    }

    private void LlegarAlDestino()
    {
        isWalking = false;
        StartWalking = false;
        if (TerminePreguntas == false)
        {
            MirarAlJugador();
            SentarNPC();
        }
    }

    void SentarNPC()
    {
        agente.isStopped = true;
        agente.updatePosition = false;
        agente.updateRotation = false;
        currentState = NPCState.Sitting;

        animNPC.SetBool("IsWalking", false);
        StartCoroutine(Sentarse());
    }

    public void LevantarNPC()
    {
        StartCoroutine(Levantarse());

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
        yield return new WaitForSeconds(0.5f);
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
        Debug.Log("NPC Clicked: " + gameObject.name);
        if (currentState == NPCState.ReadyForQuestions)
        {
            TogglePreguntas();
        }
    }

    IEnumerator MostrarPreguntas()
    {
        Debug.Log("Mostrando preguntas");
        preguntasPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        //preguntasAnimator.SetTrigger("Open");
    }
    IEnumerator OcultarPreguntas()
    {
        //preguntasAnimator.SetTrigger("Close");
        Debug.Log("Ocultando preguntas");
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
            //Agrega interfaz de diálogo aquí
            if (pregunta == 1)
            {
                currentState = NPCState.Talking;
                StartCoroutine(EsperarYHablar(miCV.C_AudioRespuestaPregunta1));
            }
            if (pregunta == 2)
            {
                currentState = NPCState.Talking;
                StartCoroutine(EsperarYHablar(miCV.C_AudioRespuestaPregunta2));
            }
            if (pregunta == 3)
            {
                currentState = NPCState.Talking;
                TerminePreguntas = true;
                TogglePreguntas();
            }
        }
    }

    IEnumerator EsperarYHablar(AudioClip respuesta)
    {
        TogglePreguntas();
        animNPC.SetTrigger("Talk");
        //Debug.Log("Diálogo:" + respuesta);
        AudioSource audioSource = GetComponent<AudioSource>();
        if (respuesta != null && audioSource != null)
        {
            audioSource.clip = respuesta;
            audioSource.Play();
            yield return new WaitForSeconds(respuesta.length);
        }
        else
        {
            Debug.LogWarning("No hay clip de audio o AudioSource asignado.");
        }

        currentState = NPCState.ReadyForQuestions;
    }
}
