using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Actions/HealAction")]
public class HealAction : ActionBase
{
    public int healAmount;

    public override void Execute(Agent user, Agent target)
    {
        target.Heal(healAmount);
    }
}
