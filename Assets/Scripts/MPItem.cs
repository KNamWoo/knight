using UnityEngine;

public class MPItem : ItemBase
{
    public override void Use() {
        Debug.Log("MP 회복!");
        itemCount--;
        
        if (itemCount <= 0) {
            Destroy(gameObject);
        }
    }
}