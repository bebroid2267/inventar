using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventory
{
    public void ShowInventory();
    public void HideInventory();
    public void SwitchCategory();

    public void LoadItems();

}
