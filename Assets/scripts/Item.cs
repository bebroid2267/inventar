using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class Item : MonoBehaviour
{
    public Image ImageItem;
    public Image ImageSpriteItem;
    public TextMeshProUGUI CountItemText;
    public ItemInventory itemData;
    //public string Id = Guid.NewGuid().ToString("N");

    //private int _count = 1;

    //public int Count
    //{
    //    get => _count;
    //    set
    //    {
    //        _count = value;
    //        if (_count <= 0)
    //        {
    //            GameManager.Instance.InventoryController.OnDestroyedItem.Invoke(this);
    //            Destroy(gameObject);
    //        }
    //    }
    //}
    //public void RefreshUI()
    //{
    //    CountItemText.text = Count.ToString();
    //}

    public StateAnimal StateAnimal;

    public bool IsSelected;

    public Color ItemColorSelected = new Color(0.5188f, 0.4234f, 0.4234f);
    public Color SpriteItemColorSelected = new Color(0.6226f, 0.4611f, 0.4611f);
    public Color CountItemTextColorSelected = new Color(0.5943f, 0.5748f, 0.5242f);

    public Color ItemColor = new Color(1f, 1f, 1f);
    public Color SpriteItemColor = new Color(1f, 1f, 1f);
    public Color CountItemTextColor = new Color(1f, 1f, 1f);

    public void Initialize()
    {
        ImageSpriteItem.sprite = itemData.Icon;
    }

    public void SelectOrDeSelect(bool needSelect)
    {
        IsSelected = needSelect;
        if (needSelect)
        {
            ImageItem.color = ItemColorSelected;
            ImageSpriteItem.color = SpriteItemColorSelected;
            CountItemText.color = CountItemTextColorSelected;
        }
        else
        {
            ImageItem.color = ItemColor;
            ImageSpriteItem.color = SpriteItemColor;
            CountItemText.color = CountItemTextColor;
        }
    }

}

public enum StateAnimal 
{
    Wounded,
    Healthy
}
