using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerStat
{
    public float BaseValue;
    
    public int Value { get { return CalculateFinalValue(); } }
 
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
 
    private int CalculateFinalValue()
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
        
        return (int)Math.Round(finalValue, 4);
    }
}