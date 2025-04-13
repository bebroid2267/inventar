using UnityEngine;

[CreateAssetMenu(fileName ="Item Data", menuName = "Inventory/Item")]
public class ItemInventory : ScriptableObject
{
    public int Id;
    public string Name;
    public ItemType Type;
    public Sprite Icon;
    public int Stack;
}

[System.Serializable]
public class ItemSaveData
{
    public int IdItem;
    public string IdLocal;
    public int Count;
    public int NumberSlot;
}

public enum ItemType
{
    Resource,
    Animal,
    Consumable,
}
