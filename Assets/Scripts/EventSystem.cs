using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EventSystem : MonoBehaviour
{
    #region Singleton
    public static EventSystem instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
    }
    #endregion

    public static Action<Transform, int> OnTakeDamage;
    public static void OnTakeDamageEvent(Transform source, int amount) 
    {
        OnTakeDamage?.Invoke(source, amount);
    }

    public static Action<Entity, float> OnDamageEnemy;
    public static void OnDamageEnemyEvent(Entity source, float amount)
    {
        OnDamageEnemy?.Invoke(source, amount);
    }

    public static Action<Entity> OnKillEnemy;
    public static void OnKillEnemyEvent(Entity source)
    {
        OnKillEnemy?.Invoke(source);
    }

    public static Action<ICollectable> OnCollectItem;
    public static void OnCollectItemEvent(ICollectable c)
    {
        OnCollectItem?.Invoke(c);
    }

    public static Action<PlayerStats, PlayerStats> OnUpdateStats;
    public static void OnUpdateStatsEvent(PlayerStats p, PlayerStats n)
    {
        OnUpdateStats?.Invoke(p, n);
    }

    public static Action<Spell, Focus> OnFireSpell;
    public static void OnFireSpellEvent(Spell cast, Focus f)
    {
        OnFireSpell?.Invoke(cast, f);
    }

    public static Action<Hunter, int> OnUseMelee;
    public static void OnUseMeleeEvent(Hunter source, int atkIndex)
    {
        OnUseMelee?.Invoke(source, atkIndex);
    }

    public static Action<Spell, IDamageable> OnProjectileLand;
    public static void OnProjectileLandEvent(Spell source, IDamageable hit)
    {
        OnProjectileLand?.Invoke(source, hit);
    }

    public static Action<Hunter, int, IDamageable> OnMeleeLand;
    public static void OnMeleeLandEvent(Hunter source, int atkIndex, IDamageable hit)
    {
        OnMeleeLand?.Invoke(source, atkIndex, hit);
    }

    public static Action<Weapon, Weapon> OnWeaponSwapped;
    public static void OnWeaponSwappedEvent(Weapon from, Weapon to)
    {
        OnWeaponSwapped?.Invoke(from, to);
    }

    public static Action OnPlayerJump;
    public static void OnPlayerJumpEvent()
    {
        OnPlayerJump?.Invoke();
    }

    public static Action OnPlayerLand;
    public static void OnPlayerLandEvent()
    {
        OnPlayerLand?.Invoke();
    }

    public static Action OnPlayerOverflow;
    public static void OnPlayerOverflowEvent()
    {
        OnPlayerOverflow?.Invoke();
    }

    public static Action<int> OnSpawnerDestroyed;
    public static void OnSpawnerDestroyedEvent(int id)
    {
        OnSpawnerDestroyed?.Invoke(id);
    }
}
