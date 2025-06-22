using UnityEngine;

public class Slot : MonoBehaviour
{
    Inventory inventory;
    public int num;

    private void Start()
    {
        inventory = GameObject.Find("Player").GetComponent<Inventory>();
        //int.TryParse(gameObject.name.Substring(gameObject.name.IndexOf("_" + 2)), out num);
        int underscoreIndex = gameObject.name.IndexOf("_");
        if (underscoreIndex != -1 && underscoreIndex + 1 < gameObject.name.Length)
        {
            string numberPart = gameObject.name.Substring(underscoreIndex + 1);
            int.TryParse(numberPart, out num);
        }
        else
        {
            Debug.LogError("Slot 이름에 '_'가 없거나 잘못된 형식입니다: " + gameObject.name);
        }
    }

    private void Update()
    {
        if (transform.childCount <= 0)
        {
            inventory.slots[num].isEmpty = true;
        }
    }
}
