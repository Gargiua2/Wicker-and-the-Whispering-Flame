using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Relic", menuName = "Scriptables/Items/Relics/Basic Relic")]
public class BaseItem : ScriptableObject, ICollectable
{
    public string itemName = "New Item";
    public string itemDescription = "An item!";
    [TextArea(1, 6)] public string lengthyDescription = "An item!";
    public UIPanelContent itemDescriptionPage;
    public Sprite sprite;
    [SerializeField] PlayerStats statsModifier;
    [SerializeField] int healing;

    public virtual void OnEquip() 
    {
        Player.instance.AddStats(statsModifier);
        Player.instance.Heal(healing);

        NotificationPanel.instance.SendNewNotification(itemName, itemDescription, GetDescriptions());
    }

    public virtual void OnUpdateItem() 
    {
        
    }

    public virtual void OnUnequip() 
    {
        
    }

    public virtual UIPanelContent GetDescriptions() 
    {
        UIPanelContent newContent = new UIPanelContent();
        newContent.UIPanelTitle = itemName;
        newContent.UIPanelBody = lengthyDescription;
        newContent.UIPanelHeaderImage = sprite;
        newContent.UIPanelLabel = "Relics";

        return newContent;
    }

    public virtual float GetDamageBonus(IDamageable target, Weapon attackingWeapon, Spell activeSpell = null) 
    {
        return 0;
    }

    public virtual float GetDamageMultiplier(IDamageable target, Weapon attackingWeapon, Spell activeSpell = null)
    {
        Debug.Log("BASE DAMAGE MULT CALLED");
        return 0;
    }

    public Sprite GetDisplaySprite()
    {
        return sprite;
    }

    public Sprite GetIcon() 
    {
        return sprite;
    }

    public string GetName() 
    {
        return itemName;
    }
}
