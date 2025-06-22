using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public GameObject slotItem;
    private bool itemBeing;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Inventory inven = collision.GetComponent<Inventory>();
            for (int i = 0; i < inven.slots.Count; i++)
            {
                if (inven.slots[i].itemName == this.tag)
                {
                    inven.slots[i].itemCount++;
                    Destroy(this.gameObject);
                    itemBeing = true;
                    break;
                }
                itemBeing = false;
            }

            for (int i = 0; i < inven.slots.Count; i++)
            {
                if (itemBeing == true)
                {
                    break;
                }
                if (inven.slots[i].isEmpty)
                {
                    Instantiate(slotItem, inven.slots[i].slotObj.transform, false);
                    inven.slots[i].isEmpty = false;
                    inven.slots[i].itemName = this.tag;
                    inven.slots[i].itemCount++;
                    Destroy(this.gameObject);
                    break;
                }
            }
        }
    }
}
