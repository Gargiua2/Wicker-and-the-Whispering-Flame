using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    List<BaseItem> itemInventory = new List<BaseItem>();
    List<Spell> spellInventory = new List<Spell>();
    Weapon w;

    #region Singleton
    public static Inventory instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
    }
    #endregion

    void Update() 
    {
        foreach(BaseItem i in itemInventory) 
        {
            i.OnUpdateItem();
        }
    }

    public float GetItemDamageBonuses(IDamageable target, Weapon attackingWeapon, Spell activeSpell = null) 
    {
        float bonus = 0;

        foreach(BaseItem i in itemInventory) 
        {
            bonus += i.GetDamageBonus(target, attackingWeapon, activeSpell);
        }

        return bonus;
    }

    public float GetItemDamageMultipliers(IDamageable target, Weapon attackingWeapon, Spell activeSpell = null) 
    {

        float mult = 1;

        foreach (BaseItem i in itemInventory)
        {
            Debug.Log("Checking for damage multipliers on " + i.GetName());
            mult += i.GetDamageMultiplier(target, attackingWeapon, activeSpell);
        }

        return mult;
    }

    public void CollectItem(ICollectable c) 
    {
        EventSystem.OnCollectItemEvent(c);

        c.OnEquip();
        
        if(c as Weapon != null) 
        {
            Weapon f = (Weapon)c;

            w = f;

            CombatController.instance.CollectWeapon(f);
        } 
        else if (c as Spell != null) 
        {
            Spell s = (Spell)c;

            spellInventory.Add(s);

            CombatController.instance.CollectSpell(s);
        } 
        else if (c as BaseItem != null) 
        {
            BaseItem i = (BaseItem)c;

            itemInventory.Add(i);
        }
    }
}
