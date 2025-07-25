using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
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
    [SerializeField] public GameObject[] Instrucciones;
    int instruccionActual = -1;

    [SerializeField] public GameObject PanelAjustes;
    private bool isPaused = false;

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
        PanelAjustes = GameObject.Find("PanelAjustes");
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
                StartCoroutine(EsperarTiempoPrimerasInstrucciones(0));
                currentState = GameState.EsperandoSombrero;
                break;
            case GameState.EsperandoSombrero:
                MostrarInstrucciones(1);
                if (playerHasHat) IrAFaseTelefono();
                break;
            case GameState.Telefono:
                StartCoroutine(EsperarTiempoPrimerasInstrucciones(2));
                MostrarInstrucciones(3);
                if (playerHasPhone) IrAFaseSentarse();
                break;
            case GameState.EsperandoSentarse:
                MostrarInstrucciones(4);
                if (playerIsSitting) IniciarRondaNPC();
                break;
            case GameState.Preguntas:
                MostrarInstrucciones(5);
                if (npcController.GetComponent<NPC_Controller>().currentNPC.GetComponent<NPC>().TerminePreguntas) IrAEvaluarCV();
                break;
            case GameState.EvaluacionCV:
                StartCoroutine(EsperarInstruccionesEvaluacionCV());
                if (TermineDeEvaluar) ContinuarRondas();
                break;
        }

          if (Input.GetKeyDown(KeyCode.X))
        {
            TogglePauseMenu();
        }
    }

    private void TogglePauseMenu()
    {
        isPaused = !isPaused;
        PanelAjustes.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f; // Pausa el tiempo
        }
        else
        {
            Time.timeScale = 1f; // Reanuda el tiempo
        }
    }
    public void ReanudarJuego()
    {
        isPaused = false;
        PanelAjustes.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GuardarResultado(int id, string nombreCaso, int confianza)
    {
        resultadosManager.GuardarResultado(id, nombreCaso, confianza);
    }

    public void TerminarJuego()
    {
        resultadosManager.ExportarResultadosAJson();
    }

    public void MostrarInstrucciones(int index)
    {
        for (int i = 0; i < Instrucciones.Length; i++)
        {
            Instrucciones[i].gameObject.SetActive(i == index);
        }
    }
    void OcultarInstruccionActual()
    {
        if (instruccionActual >= 0 && instruccionActual < Instrucciones.Length)
        {
            Instrucciones[instruccionActual].gameObject.SetActive(false);
            instruccionActual = -1;
        }
    }

    public void IrAFaseTelefono()
    {
        OcultarInstruccionActual();
        audioSourcePhone.Play();
        Debug.Log("Cambiando a la fase de teléfono.");
        currentState = GameState.Telefono;
    }

    public void IrAFaseSentarse()
    {
        OcultarInstruccionActual();
        Debug.Log("Cambiando a la fase de sentarse.");
        currentState = GameState.EsperandoSentarse;

    }

    public void IniciarRondaNPC()
    {
        OcultarInstruccionActual();
        Debug.Log("Iniciando ronda con NPC.");
        SiguienteCaso();
        npcController.IniciarMovimientoNPC();
        currentState = GameState.Preguntas;
    }

    public void IrAEvaluarCV()
    {
        OcultarInstruccionActual();
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

    public void FinalizarEvaluacion()
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
        OcultarInstruccionActual();
        playerHasHat = true;
        Debug.Log("El jugador tiene el sombrero: " + playerHasHat);
    }
    public void YaTengoTelefono()
    {
        OcultarInstruccionActual();
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

    IEnumerator EsperarTiempoPrimerasInstrucciones(int index)
    {
        MostrarInstrucciones(index);
        yield return new WaitForSeconds(5f);
        OcultarInstruccionActual();
    }
    
    IEnumerator EsperarInstruccionesEvaluacionCV()
    {
        MostrarInstrucciones(6);
        yield return new WaitForSeconds(5f);
        OcultarInstruccionActual();

        MostrarInstrucciones(7);
        yield return new WaitForSeconds(5f);
        OcultarInstruccionActual();

        MostrarInstrucciones(8);
        yield return new WaitForSeconds(5f);
        OcultarInstruccionActual();
    }
    

}


