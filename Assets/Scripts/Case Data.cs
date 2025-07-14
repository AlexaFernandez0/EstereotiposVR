using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Case Data", menuName = "Case Data", order = 1)]
public class CaseData : ScriptableObject
{
    [SerializeField] private string c_ID;
    [SerializeField] private string c_Nombre;
    [SerializeField] private string c_Profesion;
    [SerializeField] private string c_Sexo;
    [SerializeField] private string c_Experiencia;
    [SerializeField] private string c_Formacion;
    [SerializeField] private string c_Puesto;
    [SerializeField] private string c_TipoProfesion;

    [Range(1, 10)]
    [SerializeField] private int c_Confianza;

    public string C_Nombre { get { return c_Nombre; } }
    public string C_Profesion { get { return c_Profesion; } }
    public string C_Sexo { get { return c_Sexo; } }
    public string C_Formacion { get { return c_Formacion; } }
    public string C_Puesto { get { return c_Puesto; } }
    public string C_Experiencia { get { return c_Experiencia; } }
    public int C_Confianza { get; set; }
    public string C_TipoProfesion { get { return c_TipoProfesion; } }
}



