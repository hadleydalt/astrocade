using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Actions/DebuffAction")]
public class DebuffAction : ActionBase
{
    public StatType statToDebuff;
    public int amount;
    public float duration;

    public override void Execute(Agent user, Agent target)
    {
        target.StartCoroutine(ApplyDebuff(target));
    }

    private IEnumerator ApplyDebuff(Agent target)
    {
        target.ModifyStat(statToDebuff, -amount);
        yield return new WaitForSeconds(duration);
        target.ModifyStat(statToDebuff, amount);
    }
}
