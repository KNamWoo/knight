using UnityEngine;

public class InventorySys : MonoBehaviour
{
    public static InventorySys instance;
    
    public GameObject[] slotUIArray;
    public GameObject HPPrefab;
    public GameObject MPPrefab;

    public QuickSlot[] slots;

    InventorySys inventory;
    
    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
        }

        slots = new QuickSlot[slotUIArray.Length];
        inventory = InventorySys.instance;
        
        for (int i = 0; i < slots.Length; i++) {
            slots[i] = slotUIArray[i].GetComponent<QuickSlot>();
        }
    }
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            inventory.UseSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            inventory.UseSlot(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            inventory.UseSlot(2);
        }
    }
    
    public void AddItem(string itemName) {
        Debug.Log("AddItem 실행");
        GameObject prefab = null;
        
        switch (itemName) {
            case "HPPotion":
                prefab = HPPrefab;
                break;
            case "MPPotion":
                prefab = MPPrefab;
                break;
            default:
                Debug.LogError("알 수 없는 아이템");
                return;
        }
        
        Debug.Log("인벤토리 채우기");
        
        for (int i = 0; i < slots.Length; i++) {
            if (!slots[i].IsEmpty) {
                if (slots[i].currentItem.ItemName == itemName) {
                    Debug.Log("기존 공간에 채우기");
                    slots[i].currentItem.itemCount++;
                    return;
                }
            }
        }

        Debug.Log("빈 공간에 채우기");
        for (int i = 0; i < slots.Length; i++) {
            if (slots[i].IsEmpty) {
                GameObject go = Instantiate(prefab);
                ItemBase item = go.GetComponent<ItemBase>();
                item.itemCount++;
                slots[i].IsEmpty = false;
                slots[i].SetItem(item);
                return;
            }
        }
        Debug.Log("빈 슬롯 없음");
    }
    
    public void UseSlot(int index) {
        if (index >= 0 && index < slots.Length) {
            slots[index].UseItem();
        }
    }
    
    public void LoadQuickSlotData(QuickSlotData data) {
        for (int i = 0; i < slots.Length; i++) {
            if (data.itemNames[i] == "" || data.itemCounts[i] <= 0) {
                Debug.Log(i+1+"번째 슬롯 아이템 정보가 없음");
                continue;
            }

            string itemName = data.itemNames[i];
            int count = data.itemCounts[i];
            GameObject prefab = null;
            
            switch (itemName) {
                case "HPPotion":
                    prefab = HPPrefab;
                    break;
                case "MPPotion":
                    prefab = MPPrefab;
                    break;
                default:
                    Debug.LogWarning("알 수 없는 아이템 이름 : "+itemName);
                    continue;
            }

            GameObject go = Instantiate(prefab);
            ItemBase item = go.GetComponent<ItemBase>();
            item.itemCount = count;
            slots[i].SetItem(item);
        }
    }
    
    public void ResetQuickSlots() {
        foreach (var slot in slots) {
            slot.ClearSlot();
        }
    }
}
