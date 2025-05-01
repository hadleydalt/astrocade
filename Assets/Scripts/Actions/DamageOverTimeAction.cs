using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Actions/DamageOverTimeAction")]
public class DamageOverTimeAction : ActionBase
{
    public int damagePerTick;
    public float duration;
    public float tickInterval;

    public override void Execute(Agent user, Agent target)
    {
        // Prevent starting a new coroutine if one is already running
        if (!target.IsTakingDamage)
        {
            target.StartCoroutine(ApplyDOT(target));
        }
    }

    private IEnumerator ApplyDOT(Agent target)
    {
        target.IsTakingDamage = true;  // Flag that damage over time is in progress

        float elapsed = 0f;
        while (elapsed < duration && target.IsAlive)
        {
            target.TakeDamage(damagePerTick);  // Apply damage
            elapsed += tickInterval;
            yield return new WaitForSeconds(tickInterval);  // Wait for the next tick
        }

        target.IsTakingDamage = false;  // Flag that damage over time is done
    }
}
