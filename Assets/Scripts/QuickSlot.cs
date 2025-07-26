using System;
using UnityEngine;

public class QuickSlot : MonoBehaviour
{
    public GameObject slotUIObj;
    public ItemBase currentItem;
    
    public bool IsEmpty;
    private void Start() {
        IsEmpty = true;
    }
    public void SetItem(ItemBase item) {
        currentItem = item;
        IsEmpty = false;
        item.transform.SetParent(this.transform, false);
    }
    
    public void UseItem() {
        if (currentItem != null) {
            currentItem.Use();
            
            if (currentItem.itemCount <= 0) {
                IsEmpty = true;
            }
        }
    }
    
    public void ClearItem() {
        Destroy(currentItem.gameObject);
        currentItem = null;
    }
    
    public void ClearSlot() {
        if (currentItem != null) {
            Destroy(currentItem.gameObject);
            IsEmpty = true;
        }
        currentItem = null;
    }
}
