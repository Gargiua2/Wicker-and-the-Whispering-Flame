using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Magic
{
    const float minFireRate = 0.05f;
    const float maxFireRate = 1f;

    const float minFavorMultiplier = .2f;
    const float maxFavorMultiplier = 4f;

    const float minProwessMultiplier = .4f;
    const float maxProwessMultiplier = 2f;

    const float minResist = 1;
    const float maxResist = .5f;

    public static float GetResistanceMultiplier(float r) 
    {
        if(r <= 1) 
        {
            return minResist;
        }

        return Mathf.Lerp(maxResist, minResist, (20 - r) / 20);
    }

    public static float GetProwessDamageMultiplier(float prowess)
    {
        if (prowess <= 1)
        {
            return minProwessMultiplier;
        }

        return Mathf.Lerp(minProwessMultiplier, maxProwessMultiplier, prowess / 20f);
    }

    public static float GetFavorDamageMultiplier(float favor)
    {
        if (favor <= 1) 
        {
            return minFavorMultiplier;
        }

        return Mathf.Lerp(minFavorMultiplier, maxFavorMultiplier, favor/20f);
    }

    public static float CalculateFireRate(float fireValue) 
    {
        if (fireValue == 20)
            return minFireRate;

        return Mathf.Lerp(minFireRate, maxFireRate, (20 - fireValue)/20);
    }
}

public enum MagicDamageType 
{
    FORCE,
    FIRE,
    ICE,
    ELECTRIC
}

public enum FireType 
{
    SINGLE,
    AUTO,
    BURST,
    CHARGE
}