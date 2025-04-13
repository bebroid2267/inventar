using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUi : MonoBehaviour
{
    public GameObject ParentPanel;

    public Image SwitchToResourcesImage;
    public Image SwitchToAnimalsImage;
    public Image SwitchToConsumablesImage;

    public Button AddItemButton;
    public Button RemoveItemButton;
    public Button ChangeStatusButton;
    public Button SaveInventoryButton;

    public Button AddAnimalButton;
    public Button AddConsumableButton;
    public Button AddResourceButton;

    public ItemType currentTypeInventory = ItemType.Resource;

    public void Start()
    {
        AddItemButton.onClick.AddListener(AddItem);
        RemoveItemButton.onClick.AddListener(DeleteRandomItem);
        SaveInventoryButton.onClick.AddListener(SaveInventory);
        ChangeStatusButton.onClick.AddListener(ChangeStatusAnimal);

        AddAnimalButton.onClick.AddListener(AddAnimalItem);
        AddConsumableButton.onClick.AddListener(AddConsumableItem);
        AddResourceButton.onClick.AddListener(AddResourceItem);
    }

    private void ChangeStatusAnimal()
    {
        if (GameManager.Instance.InventoryController.SelectedSlot.itemData.Type == ItemType.Animal)
        {
            GameManager.Instance.InventoryController.ChangeAnimalStatus(GameManager.Instance.InventoryController.SelectedSlot);
        }
    }

    private void AddItem()
    {
        GameManager.Instance.InventoryController.AddItem(GameManager.Instance.InventoryController.SelectedSlot?.itemData, 1);

        SwitchType(currentTypeInventory);
    }

    private void AddResourceItem()
    {
        var items = GameManager.Instance.InventoryController.ItemsResources;
        GameManager.Instance.InventoryController.AddItem(items[new System.Random().Next(0, items.Count)], 1);

        SwitchType(ItemType.Resource);
    }

    private void AddConsumableItem()
    {
        var items = GameManager.Instance.InventoryController.ItemsConsumables;
        GameManager.Instance.InventoryController.AddItem(items[new System.Random().Next(0, items.Count)], 1);

        SwitchType(ItemType.Consumable);
    }

    private void AddAnimalItem()
    {
        var items = GameManager.Instance.InventoryController.ItemsAnimals;
        GameManager.Instance.InventoryController.AddItem(items[new System.Random().Next(0, items.Count)], 1);

        SwitchType(ItemType.Animal);
    }

    private void SaveInventory()
    {
        GameManager.Instance.InventoryController.SaveInventory();
    }

    private void DeleteRandomItem()
    {
        GameManager.Instance.InventoryController.DeleteItem(GameManager.Instance.InventoryController.SelectedSlot.Number);
    }

    public void SwitchToAnimals()
    {
        SwitchType(ItemType.Animal);
    }

    public void SwitchToResources()
    {
        SwitchType(ItemType.Resource);
    }

    public void SwitchToConsumables()
    {
        SwitchType(ItemType.Consumable);
    }


    private void SwitchType(ItemType type)
    {
        currentTypeInventory = type;
        foreach (var item in GameManager.Instance.InventoryController.Slots)
        {
            item.gameObject.SetActive(item.itemData?.Type == type);

            item.GetComponent<RectTransform>().localScale = Vector3.zero;
            item.GetComponent<RectTransform>().DOScale(1f, 0.3f)
                .SetEase(Ease.OutBack);


            if (type == ItemType.Animal)
            {
                item.SetStateAnimal(item.GetStateAnimal());
            }
            else
            {
                item.DisableAnimalImages();
            }
        }
    }
}
