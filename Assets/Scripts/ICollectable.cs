using UnityEngine;


public interface ICollectable
{
    void OnEquip();
    void OnUnequip();
    Sprite GetDisplaySprite();
    Sprite GetIcon();
    string GetName();

    UIPanelContent GetDescriptions();
}
