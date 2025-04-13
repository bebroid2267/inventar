using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Image ImageItem;
    public Image ImageSpriteItem;
    public Image ImageHealthAnimal;
    public Image ImageWoundedAnimal;


    public TextMeshProUGUI CountItemText;
    public ItemInventory itemData;

    public bool IsEmpty = true;

    public bool isSelected = false;

    private StateAnimal _stateAnimal = StateAnimal.Healthy;

    public bool IsSelected;

    public Color ItemColorSelected = new Color(0.5188f, 0.4234f, 0.4234f);
    public Color SpriteItemColorSelected = new Color(0.6226f, 0.4611f, 0.4611f);
    public Color CountItemTextColorSelected = new Color(0.5943f, 0.5748f, 0.5242f);

    public Color ItemColor = new Color(1f, 1f, 1f);
    public Color SpriteItemColor = new Color(1f, 1f, 1f);
    public Color CountItemTextColor = new Color(1f, 1f, 1f);


    private int _amount;

    public int Amount
    {
        get { return _amount; }
        set
        {
            _amount = value;
            if (_amount <= 0)
            {
                GameManager.Instance.InventoryController.OnDestroyedItem.Invoke(this);
            }
            else if (_amount > itemData.Stack)
            {
                GameManager.Instance.InventoryController.OnStackOverflow.Invoke(this, _amount - itemData.Stack);
                _amount = itemData.Stack;
            }
        }
    }

    public int Number;

    public void DisableAnimalImages()
    {
        ImageWoundedAnimal.gameObject.SetActive(false);
        ImageHealthAnimal.gameObject.SetActive(false);
    }
    public void SetStateAnimal(StateAnimal state)
    {
        if (state == StateAnimal.Healthy)
        {
            ImageWoundedAnimal.gameObject.SetActive(false);
            ImageHealthAnimal.gameObject.SetActive(true);
        }
        else
        {
            ImageWoundedAnimal.gameObject.SetActive(true);
            ImageHealthAnimal.gameObject.SetActive(false);
        }

        _stateAnimal = state;
    }

    public StateAnimal GetStateAnimal() => _stateAnimal;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.InventoryController.OnSelectedSlot?.Invoke(this);
    }
    public void UpdateUi()
    {
        if (Amount > 0)
        {
            CountItemText.text = Amount.ToString();
        }
    }

    public void Initialize()
    {
        ImageSpriteItem.gameObject.SetActive(true);
        ImageSpriteItem.sprite = itemData.Icon;

        ImageHealthAnimal.gameObject?.SetActive(false);
        ImageWoundedAnimal.gameObject?.SetActive(false);
    }

    public void SelectOrDeSelect(bool neededSelect)
    {
        isSelected = neededSelect;

        ImageItem.DOKill();
        ImageSpriteItem.DOKill();
        CountItemText.DOKill();
        transform.DOKill();


        if (isSelected)
        {
            ImageItem.DOColor(ItemColorSelected, 0.25f);
            ImageSpriteItem.DOColor(SpriteItemColorSelected, 0.25f);
            CountItemText.DOColor(CountItemTextColorSelected, 0.25f);

            transform.DOScale(1.1f, 0.2f).SetEase(Ease.OutBack)
                     .OnComplete(() =>
                        transform.DOScale(1f, 0.15f).SetEase(Ease.InOutSine));
        }
        else
        {
            ImageItem.DOColor(ItemColor, 0.25f);
            ImageSpriteItem.DOColor(SpriteItemColor, 0.25f);
            CountItemText.DOColor(CountItemTextColor, 0.25f);

            transform.DOScale(0.95f, 0.15f).SetEase(Ease.OutQuad)
                     .OnComplete(() =>
                        transform.DOScale(1f, 0.15f).SetEase(Ease.OutSine));
        }
    }
}
