using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    // Referencias a los objetos del juego
    public GameState currentState = GameState.Instrucciones;
    public NPC_Controller npcController;
    [SerializeField] public bool playerHasHat = false;
    [SerializeField] public bool playerIsSitting = false;
    [SerializeField] bool TermineDeEvaluar = false;
    public GameObject telefonoInput;
    [SerializeField] bool playerHasPhone = false;
    public List<CaseData> todosLosCasos; // Llena en el inspector
    private List<CaseData> casosRestantes;
    private int casosCompletados = 0;
    public ResultadosManager resultadosManager;

    //Colliders y controladores de las manos
    public GameObject leftHandCollider;
    public GameObject rightHandCollider;
    public GameObject leftHandController;
    public GameObject rightHandController;

    [SerializeField] Vector3 positionOffsetRight;
    [SerializeField] Vector3 positionOffsetLeft;

    public enum GameState
    {
        Instrucciones,
        EsperandoSombrero,
        Telefono,
        EsperandoSentarse,
        Preguntas,
        EvaluacionCV,
        Finalizado
    }



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        leftHandCollider = GameObject.FindGameObjectWithTag("LeftHand");
        rightHandCollider = GameObject.FindGameObjectWithTag("RightHand");
        rightHandController = GameObject.FindGameObjectWithTag("RightHandTarget");
        leftHandController = GameObject.FindGameObjectWithTag("LeftHandTarget");
        npcController = GetComponent<NPC_Controller>();
    }

    void Start()
    {
        // Verificar que los objetos de mano y controladores están asignados correctamente
        if (leftHandCollider == null || rightHandCollider == null)
        {
            Debug.LogError("No encuentro los colliders de las manos");
        }
        if (leftHandController == null || rightHandController == null)
        {
            Debug.Log("No encuentro los controladores de las manos");
        }

        // Inicializar el ResultadosManager
        resultadosManager = FindObjectOfType<ResultadosManager>();
        casosRestantes = new List<CaseData>(todosLosCasos);
        SiguienteCaso();

        if (resultadosManager != null)
        {
            resultadosManager.ReiniciarResultados();
        }


    }

    void Update()
    {
        switch (currentState)
        {
            case GameState.Instrucciones:
                MostrarInstrucciones();
                currentState = GameState.EsperandoSombrero;
                break;
            case GameState.EsperandoSombrero:
                if (playerHasHat) IrAFaseTelefono();
                break;
            case GameState.Telefono:
                if (playerHasPhone) IrAFaseSentarse();
                break;
            case GameState.EsperandoSentarse:
                if (playerIsSitting) IniciarRondaNPC();
                break;
            case GameState.Preguntas:
                if (npcController.GetComponent<NPC>().TerminePreguntas) IrAEvaluarCV();
                break;
            case GameState.EvaluacionCV:
                if (TermineDeEvaluar) ContinuarRondas();
                break;
        }
    }

    public void GuardarResultado(int id, string nombreCaso, int confianza)
    {
        resultadosManager.GuardarResultado(id, nombreCaso, confianza);
    }

    public void TerminarJuego()
    {
        resultadosManager.ExportarResultadosAJson();
    }

    void LateUpdate()
    {

        // Actualizar las posiciones de los colliders de las manos
        leftHandCollider.transform.position = leftHandController.transform.position + leftHandController.transform.TransformDirection(positionOffsetLeft);
        rightHandCollider.transform.position = rightHandController.transform.position + rightHandController.transform.TransformDirection(positionOffsetRight);
    }

    public void MostrarInstrucciones()
    {
        // Aquí puedes implementar la lógica para mostrar las instrucciones al jugador
        Debug.Log("Mostrando instrucciones al jugador.");
        // Por ejemplo, activar un panel de UI con las instrucciones
    }

    public void IrAFaseTelefono()
    {
        Debug.Log("Cambiando a la fase de teléfono.");
        currentState = GameState.Telefono;

    }

    public void IrAFaseSentarse()
    {
        Debug.Log("Cambiando a la fase de sentarse.");
        currentState = GameState.EsperandoSentarse;

    }

    public void IniciarRondaNPC()
    {
        Debug.Log("Iniciando ronda con NPC.");
        //currentState = GameState.Preguntas;

    }

    public void IrAEvaluarCV()
    {
        Debug.Log("Cambiando a la fase de evaluación de CV.");
        currentState = GameState.EvaluacionCV;
    }

    public void ContinuarRondas()
    {
        Debug.Log("Continuando con la siguiente ronda.");
        casosCompletados++;
        if (casosCompletados >= 12)
        {
            currentState = GameState.Finalizado;
            FinalizarEvaluacion();
        }
        else
        {
            currentState = GameState.EsperandoSentarse;
            playerIsSitting = false;
            TermineDeEvaluar = false;
            casosCompletados++;
        }

        // Aquí podrías reiniciar el NPC o cargar uno nuevo
        npcController.DestroyNPC();
    }

    void FinalizarEvaluacion()
    {
        Debug.Log("Finalizando evaluación.");
        // Aquí puedes implementar la lógica para finalizar la evaluación, como mostrar resultados finales
        TerminarJuego();
    }

    void TerminarEvaluacionActual()
    {
        npcController.TerminarPreguntasNPC();
        SiguienteCaso();
    }

    void SiguienteCaso()
    {
        CaseData casoSeleccionado = casosRestantes[0]; // O usa Random si prefieres aleatorio
        casosRestantes.Remove(casoSeleccionado);
        npcController.SpawnNPC(casoSeleccionado);
    }

    public void YaTengoSombrero()
    {
        playerHasHat = true;
        Debug.Log("El jugador tiene el sombrero: " + playerHasHat);
    }
    public void YaTengoTelefono()
    {
        playerHasPhone = true;
        Debug.Log("El jugador tiene el telefono: " + playerHasPhone);
    }

}


