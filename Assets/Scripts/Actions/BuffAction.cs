using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType { Attack, Defense, Speed }

[CreateAssetMenu(menuName = "RPG Actions/BuffAction")]
public class BuffAction : ActionBase
{
    public StatType statToBuff;
    public int amount;
    public float duration;

    public override void Execute(Agent user, Agent target)
    {
        target.StartCoroutine(ApplyBuff(target));
    }

    private IEnumerator ApplyBuff(Agent target)
    {
        target.ModifyStat(statToBuff, amount);
        yield return new WaitForSeconds(duration);
        target.ModifyStat(statToBuff, -amount);
    }
}
