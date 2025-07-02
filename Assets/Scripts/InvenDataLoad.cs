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

    public void LoadInven()
    {
        inventory = Inventory.instance;
        itemMan = ItemManager.instance;

        SaveData saveData = LoadSystem.LoadGameData();
        if (saveData != null)
        {
            Debug.Log("파일을 찾음");
            int i;

            for (i = 0; i<inventory.maxSlot; i++)
            {
                if (saveData.invenData.ItemNames[i] == "")
                {
                    continue;
                }
                itemMan.ItemAdd(saveData.invenData.ItemNames[i], i, saveData.invenData.ItemCounts[i]);
            }
        }
    }
}
