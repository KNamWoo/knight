using UnityEngine;
using UnityEngine.UI;

public abstract class ItemBase : MonoBehaviour
{
    public string ItemName;
    public int itemCount;
    public Sprite ItemImage;

    public abstract void Use();
}
