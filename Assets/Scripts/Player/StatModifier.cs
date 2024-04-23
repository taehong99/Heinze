using System;

public class StatModifier
{
    public readonly float Value;
    public readonly IncreaseRate Type;
 
    public StatModifier(float value, IncreaseRate type)
    {
        Value = value;
        Type = type;
    }
}