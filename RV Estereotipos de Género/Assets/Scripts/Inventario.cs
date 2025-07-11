using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    public static Inventario Instance;
    public List<CVItem> cvItems = new List<CVItem>();
    public int ActualCase = 0;

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
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void AddCVItem(CVItem item)
    {
        if (!cvItems.Contains(item))
        {
            cvItems.Add(item);
            Debug.Log("CV Item added: " + item.name);
        }
        else
        {
            Debug.Log("CV Item already exists in inventory: " + item.name);
        }
    }
}
