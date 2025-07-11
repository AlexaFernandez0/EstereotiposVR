using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public ResultadosManager resultadosManager;
    public GameObject leftHandCollider;
    public GameObject rightHandCollider;
    public GameObject leftHandController;
    public GameObject rightHandController;

    [SerializeField] Vector3 positionOffsetRight;
    [SerializeField] Vector3 positionOffsetLeft;


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

        if (resultadosManager != null)
        {
            resultadosManager.ReiniciarResultados();
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
        rightHandCollider.transform.position = rightHandController.transform.position+ rightHandController.transform.TransformDirection(positionOffsetRight);
    }




}

