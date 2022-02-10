using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ScriptableObject, ICollectable
{
    [Header("General Information")]
    public string weaponName = "Weapon";
    public string description = "It's a weapon.";
    [TextArea(1, 6)] public string lengthyDescription = "It's a weapon.";

    [Space, Header("Animation & Visual Config")]
    public Sprite sprite;
    public Sprite icon;
    public bool freeStanding = false;
    public Vector3 offsetFromCamera;
    public Vector3 rotationOffset;
    public Vector3 scale;
    public AnimatorOverrideController overrideController;
    public WeaponAnimatorSettings animationSettings;

    public virtual void OnEquip()
    {
        NotificationPanel.instance.SendNewNotification(weaponName, description, GetDescriptions());
    }

    public virtual void OnUpdate()
    {

    }

    public virtual UIPanelContent GetDescriptions() 
    {
        UIPanelContent newContent = new UIPanelContent();
        newContent.UIPanelTitle = weaponName;
        newContent.UIPanelBody = lengthyDescription;
        newContent.UIPanelHeaderImage = icon;
        newContent.UIPanelLabel = "Weapons";

        return newContent;
    }

    public virtual void OnUnequip()
    {

    }

    public virtual Sprite GetDisplaySprite()
    {
        return sprite;
    }

    public virtual Sprite GetIcon()
    {
        return icon;
    }

    public virtual string GetName()
    {
        return weaponName;
    }
}

[System.Serializable]
public struct DamageTypeModifier
{
    public MagicDamageType damageType;
    public float typeModfier;
}
