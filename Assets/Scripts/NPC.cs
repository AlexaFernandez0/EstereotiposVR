using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public CaseData miCV;
    public GameObject prefabHojaCV;
    public Transform puntoEntrega;
    private NavMeshAgent agente;

    [SerializeField] Transform SitPoint;
    [SerializeField] Transform StartPoint;
    [SerializeField] Transform SittingPlace;
    [SerializeField] Animator animNPC;
    [SerializeField] private Transform jugador;

    private bool isWalking;
    [SerializeField] bool StartWalking = false;
    private bool isSitting = false;
    public NPCState currentState = NPCState.Idle;

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
            IniciarMovimiento();
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
    }

    private void IniciarMovimiento()
    {
        isWalking = true;
        agente.isStopped = false;
        agente.updatePosition = true;
        agente.updateRotation = true;
        agente.SetDestination(SitPoint.position);
        animNPC.SetBool("IsWalking", true);
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

        animNPC.SetBool("IsWalking", false);
        StartCoroutine(Sentarse());
    }

    IEnumerator Sentarse()
    {
        yield return new WaitForSeconds(0.5f);
        animNPC.SetTrigger("Sentarse");

        yield return new WaitForSeconds(1f);
        currentState = NPCState.ReadyForQuestions;
    }
    private void MirarAlJugador()
    {
        Vector3 lookPosition = jugador.position;
        lookPosition.y = transform.position.y;
        transform.LookAt(lookPosition);
    }

    
}
