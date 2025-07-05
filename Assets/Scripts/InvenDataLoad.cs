using System.Threading;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

public class InvenDataLoad : MonoBehaviour
{
    public static InvenDataLoad instance;

    Inventory inventory;
    ItemManager itemMan;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //
    }
}
