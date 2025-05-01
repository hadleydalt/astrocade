using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType
{
    Enemy,
    Ally,
    Self
}

public abstract class ActionBase : ScriptableObject
{
    public string ActionName;
    public TargetType TargetType; // Enum for Ally, Enemy, or Self
    public float Power;  // Power, like attack or healing amount

    public abstract void Execute(Agent user, Agent target);

    public string getName()
    {
        return ActionName;
    }
}