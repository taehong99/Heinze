using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerStat
{
    public float BaseValue;
    
    public float Value { get { return CalculateFinalValue(); } }
 
    private readonly List<StatModifier> statModifiers;
 
    public PlayerStat(float baseValue)
    {
        BaseValue = baseValue;
        statModifiers = new List<StatModifier>();
    }
    
    public void AddModifier(StatModifier mod)
    {
        statModifiers.Add(mod);
    }
 
    public bool RemoveModifier(StatModifier mod)
    {
        return statModifiers.Remove(mod);
    }
 
    private float CalculateFinalValue()
    {
        float finalValue = BaseValue;
 
        for (int i = 0; i < statModifiers.Count; i++)
        {
            StatModifier mod = statModifiers[i];
 
            if (mod.Type == IncreaseRate.Flat)
            {
                finalValue += mod.Value;
            }
            else if (mod.Type == IncreaseRate.Percent)
            {
                finalValue *= 1 + mod.Value;
            }
        }
        
        return (float)Math.Round(finalValue, 4);
    }
}