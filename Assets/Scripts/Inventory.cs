using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject[] invens;
    private List<GameObject> inventory = new List<GameObject>();
    public List<GameObject> Inven => inventory;
    public static Inventory instance;

    public List<SlotData> slots = new List<SlotData>();
    public int maxSlot = 3;
    public GameObject slotPrefab;

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

    private void Start()
    {
        invens = GameObject.FindGameObjectsWithTag("Player");
        inventory.AddRange(invens);

        GameObject slotPanel = GameObject.Find("Panel");

        for (int i = 0; i < maxSlot; i++)
        {
            GameObject go = Instantiate(slotPrefab, slotPanel.transform, false);
            go.name = "Slot_" + i;
            SlotData slot = new SlotData();
            slot.isEmpty = true;
            slot.itemCount = 0;
            slot.itemName = "";
            slot.slotObj = go;
            slots.Add(slot);
        }

        InvenDataLoad.instance.LoadInven();
    }
}
