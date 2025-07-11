using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ResultadosDatos
{
    public int C_Confianza;
    public int C_ID;
    public string C_Nombre;
}

[System.Serializable]
public class ListaDeResultados
{
    public List<ResultadosDatos> resultados = new List<ResultadosDatos>();
}

public class ResultadosManager : MonoBehaviour
{
    public ListaDeResultados resultados = new ListaDeResultados();

    public void ReiniciarResultados()
    {
        resultados.resultados.Clear(); //aquí se borran todos los datos
        Debug.Log("Resultados reiniciados");
    }

    public void GuardarResultado(int id, string nombreCaso, int confianza)
    {
        ResultadosDatos nuevoResultado = new ResultadosDatos
        {
            C_ID = id,
            C_Nombre = nombreCaso,
            C_Confianza = confianza
        };

        resultados.resultados.Add(nuevoResultado);
    }

    public void ExportarResultadosAJson()
    {
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string fileName = "resultados_" + timestamp + ".json";
        string json = JsonUtility.ToJson(resultados, true);
        string path = Path.Combine(Application.persistentDataPath, "resultados.json");

        File.WriteAllText(path, json);
        Debug.Log("Resultados guardados en: " + path);
    }
}
