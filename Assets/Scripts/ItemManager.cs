using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject HPPotion;
    public GameObject MPPotion;
    public static ItemManager instance;

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

    Inventory inven;

    void Start()
    {
        //inven = Inventory.instance;
    }

    public void ItemAdd(string objectName, int slotNum, int add)
    {
        if (inven == null) {
            inven = Inventory.instance;
            
            if (inven == null) {
                Debug.LogError("Inventory.instance가 아직 초기화되지 않았습니다!");
                return;
            }
        }
        GameObject itemObject;
        if (objectName == "HPPotion")
        {
            itemObject = HPPotion;
        }
        else if (objectName == "MPPotion")
        {
            itemObject = MPPotion;
        }
        else
        {
            return;
        }
        Instantiate(itemObject, inven.slots[slotNum].slotObj.transform, false);
        inven.slots[slotNum].isEmpty = false;
        inven.slots[slotNum].itemName = objectName;
        inven.slots[slotNum].itemCount += add;
    }
    
    public void ItemReset(string objectName, int slotNum, int add)
    {
        if (inven == null) {
            inven = Inventory.instance;
            
            if (inven == null) {
                Debug.LogError("Inventory.instance가 아직 초기화되지 않았습니다!");
                return;
            }
        }
        GameObject itemObject;
        if (objectName == "HPPotion")
        {
            itemObject = HPPotion;
        }
        else if (objectName == "MPPotion")
        {
            itemObject = MPPotion;
        }
        else
        {
            return;
        }
        Instantiate(itemObject, inven.slots[slotNum].slotObj.transform, false);
        inven.slots[slotNum].isEmpty = false;
        inven.slots[slotNum].itemName = objectName;
        inven.slots[slotNum].itemCount = add;
    }
}
