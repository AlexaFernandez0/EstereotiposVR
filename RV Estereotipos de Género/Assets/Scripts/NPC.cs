using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public CaseData miCV;
    public GameObject prefabHojaCV; // Asignas el prefab del papel
    public Transform puntoEntrega; // Lugar donde aparecerá la hoja
    private NavMeshAgent agente;
    [SerializeField] Transform SitPoint; // Punto de sentado del NPC
    [SerializeField] Transform StartPoint; // Punto de inicio del NPC
    //private Animator anim; // Referencia al Animator del NPC


    // Start is called before the first frame update
    void Awake()
    {
        agente = GetComponent<NavMeshAgent>();
        agente.Warp(StartPoint.position); // Coloca al NPC en el punto de inicio
    }
    void Start()
    {
        agente.SetDestination(SitPoint.position); // Mueve al NPC al sillon
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EntregarCV()
    {
        GameObject hoja = Instantiate(prefabHojaCV, puntoEntrega.position, puntoEntrega.rotation); //Instanciamos el CV
        hoja.GetComponent<CVItem>().CVData = this.miCV; //Le asignamos la información al CV del NPC
    }
}
