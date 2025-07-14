using UnityEngine;

public class HPItem : ItemBase
{
    public override void Use() {
        Debug.Log("HP 회복!");
        itemCount--;
        
        if (itemCount <= 0) {
            Destroy(gameObject);
        }
    }
}
