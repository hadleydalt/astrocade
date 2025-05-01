using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Actions/DamageAction")]
public class DamageAction : ActionBase
{
    public int power;

    public override void Execute(Agent user, Agent target)
    {
        int damage = Mathf.Max(0, Mathf.RoundToInt(user.Attack - target.Defense + power));
        target.TakeDamage(damage);
        Debug.Log($"{user.getName()} attacks {target.getName()} for {damage} damage!");
    }
}
