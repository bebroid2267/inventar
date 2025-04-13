using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public List<InventorySlot> Slots = new List<InventorySlot>();
    public GameObject ItemPrefab;

    public List<Item> Items;

    public List<ItemInventory> ItemsResources => ItemDatas.Where(x => x.Type == ItemType.Resource).ToList();
    public List<ItemInventory> ItemsConsumables => ItemDatas.Where(x => x.Type == ItemType.Consumable).ToList();
    public List<ItemInventory> ItemsAnimals => ItemDatas.Where(x => x.Type == ItemType.Animal).ToList();

    public List<ItemInventory> ItemDatas;

    public Action<InventorySlot> OnDestroyedItem;
    public Action<InventorySlot, int> OnStackOverflow;
    public Action<InventorySlot> OnSelectedSlot;

    public InventorySlot SelectedSlot => Slots.FirstOrDefault(x => x.isSelected);

    public int CountCells;

    private void Start()
    {
        CreateSlots();

        LoadInventory();

        OnSelectedSlot += SelectSlot;
        OnDestroyedItem += DeleteSlot;
        OnStackOverflow += OnStackItemOverflow;

        GameManager.Instance.InventoryUi.SwitchToResources();
    }

    private void CreateSlots()
    {
        for (int i = 0; i < CountCells; i++)
        {
            var item = Instantiate(ItemPrefab, GameManager.Instance.InventoryUi.ParentPanel.transform);
            item.GetComponent<InventorySlot>().Number = i;
            Slots.Add(item.GetComponent<InventorySlot>());
        }
    }

    private void DeleteSlot(InventorySlot slot)
    {
        slot.itemData = null;
        slot.ImageSpriteItem.gameObject.SetActive(false);
        slot.IsEmpty = true;
        slot.CountItemText.text = string.Empty;
        slot.gameObject.SetActive(false);
    }

    private void SelectSlot(InventorySlot slot)
    {
        foreach (var item in Slots)
        {
            item.SelectOrDeSelect(false);
        }

        slot.SelectOrDeSelect(!slot.isSelected);
    }

    public void AddItem(ItemInventory item, int amount, bool needNewSlot = false)
    {
        foreach (var slot in Slots)
        {
            if (slot.itemData == item)
            {
                slot.ImageSpriteItem.gameObject.SetActive(true);

                if (slot.Amount > slot.itemData.Stack)
                {
                    continue;
                }

                slot.Amount += amount;

                slot.Initialize();
                slot.UpdateUi();

                return;
            }
        }

        foreach (var slot in Slots)
        {
            if (slot.IsEmpty)
            {
                slot.ImageSpriteItem.gameObject.SetActive(true);
                slot.itemData = item;
                slot.Amount = amount;
                slot.IsEmpty = false;
                slot.CountItemText.text = amount.ToString();

                slot.Initialize();
                slot.UpdateUi();

                break;
            }
        }
    }

    public void OnStackItemOverflow(InventorySlot slot, int amount)
    {
        AddItem(slot.itemData, amount, true);
    }

    public void DeleteItem(int numberSlot)
    {
        var slot = Slots.First(x => x.Number == numberSlot);

        slot.Amount--;

        slot.UpdateUi();
    }
    public void ChangeAnimalStatus(InventorySlot slot)
    {
        if (slot.GetStateAnimal() == StateAnimal.Wounded)
        {
            slot.SetStateAnimal(StateAnimal.Healthy);
        }
        else
        {
            slot.SetStateAnimal(StateAnimal.Wounded);
        }
    }

    public void LoadInventory()
    {
        if (!PlayerPrefs.HasKey("Inventory")) return;

        string json = PlayerPrefs.GetString("Inventory");
        ItemDataList dataList = JsonUtility.FromJson<ItemDataList>(json);

        var items = new List<Item>();
        foreach (var saveData in dataList.items)
        {
            var baseItem = ItemDatas.FirstOrDefault(u => u.Id == saveData.IdItem);
            if (baseItem != null)
            {
                var slot = Slots.FirstOrDefault(x => x.Number == saveData.NumberSlot);

                if (slot == null)
                {
                    continue;
                }

                slot.itemData = baseItem;
                slot.Amount = saveData.Count;
                slot.IsEmpty = false;

                slot.Initialize();
                slot.UpdateUi();
            }
        }
    }
    public void SaveInventory()
    {
        ItemDataList dataList = new ItemDataList { items = Slots
            .Where(x => !x.IsEmpty)
            .Select(x => new ItemSaveData() 
        {
            Count = x.Amount,
            NumberSlot = x.Number,
            IdItem = x.itemData.Id,
        }).ToList()};

        string json = JsonUtility.ToJson(dataList);
        PlayerPrefs.SetString("Inventory", json);
        PlayerPrefs.Save();
    }
}   