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
    [SerializeField] bool playerHasPhone = false;

    //Audio Sources
    [SerializeField] AudioSource audioSourcePhone;
    [SerializeField] AudioSource audioSourcePhoneInstructions;

    // Lista de casos y control de rondas
    public List<CaseData> todosLosCasos; // Llena en el inspector
    private List<CaseData> casosRestantes;
    private int casosCompletados = 0;
    public ResultadosManager resultadosManager;

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

        npcController = GetComponent<NPC_Controller>();
    }

    void Start()
    {
        // Inicializar el ResultadosManager
        resultadosManager = FindObjectOfType<ResultadosManager>();
        casosRestantes = new List<CaseData>(todosLosCasos);

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
                if (npcController.GetComponent<NPC_Controller>().currentNPC.GetComponent<NPC>().TerminePreguntas) IrAEvaluarCV();
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

    }

    public void MostrarInstrucciones()
    {
        // Aquí puedes implementar la lógica para mostrar las instrucciones al jugador
        Debug.Log("Mostrando instrucciones al jugador.");
        // Por ejemplo, activar un panel de UI con las instrucciones
    }

    public void IrAFaseTelefono()
    {
        audioSourcePhone.Play();
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
        SiguienteCaso();
        npcController.IniciarMovimientoNPC();
        currentState = GameState.Preguntas;
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
            TerminarEvaluacionActual();
        }
    }

    void FinalizarEvaluacion()
    {
        Debug.Log("Finalizando evaluación.");
        // Aquí puedes implementar la lógica para finalizar la evaluación, como mostrar resultados finales
        TerminarJuego();
    }

    public void TerminarEvaluacionActual()
    {
        //npcController.TerminarPreguntasNPC();
        SiguienteCaso();
    }

    public void SiguienteCaso()
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
        StartCoroutine(ObtencionTelefono());
        Debug.Log("El jugador tiene el telefono: " + playerHasPhone);
    }
    IEnumerator ObtencionTelefono()
    {
        audioSourcePhone.Stop();
        audioSourcePhoneInstructions.Play();
        yield return new WaitForSeconds(5f);
        audioSourcePhoneInstructions.Stop();
        playerHasPhone = true;

    }

    public void YaMeSenté()
    {
        playerIsSitting = true;
        Debug.Log("El jugador se ha sentado: " + playerIsSitting);
    }

}


