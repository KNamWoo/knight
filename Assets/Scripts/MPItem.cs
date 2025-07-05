using UnityEngine;

public class MPItem : MonoBehaviour
{
    Inventory inven;
    public int num;
    public bool unAbleUse;
    
    public static MPItem instance;

    GameManager gameManager;
    
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

        gameManager = GameManager.instance;
    }

    void Start()
    {
        unAbleUse = false;
        inven = GameObject.Find("Player").GetComponent<Inventory>();
        num = transform.parent.GetComponent<Slot>().num;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!gameManager.currentPause) {
            if (Input.inputString == (num + 1).ToString()) {
                Debug.Log("MP 아이템 사용");
                inven.slots[num].itemCount--;
                
                if (inven.slots[num].itemCount <= 0) {
                    inven.slots[num].itemName = "";
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
