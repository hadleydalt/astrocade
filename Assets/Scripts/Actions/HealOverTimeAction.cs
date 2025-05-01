using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Actions/HealOverTimeAction")]
public class HealOverTimeAction : ActionBase
{
    public int healPerTick;
    public float duration;
    public float tickInterval;

    public override void Execute(Agent user, Agent target)
    {
        // Prevent starting a new coroutine if one is already running
        if (!target.IsHealing)
        {
            target.StartCoroutine(ApplyHOT(target));
        }
    }

    private IEnumerator ApplyHOT(Agent target)
    {
        target.IsHealing = true;  // Flag that healing is in progress

        float elapsed = 0f;
        while (elapsed < duration && target.IsAlive)
        {
            target.Heal(healPerTick);  // Heal the target
            elapsed += tickInterval;
            yield return new WaitForSeconds(tickInterval);  // Wait for the next tick
        }

        target.IsHealing = false;  // Flag that healing is done
    }
}
