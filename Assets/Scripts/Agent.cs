using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Agent : MonoBehaviour
{
    public string Name;
    public float HP;
    public float MaxHP;
    public float Attack;
    public float Defense;
    public float Speed;
    public bool IsPlayer;

    public GameObject labelPrefab;
    private GameObject labelInstance;
    private TMP_Text nameText;
    private TMP_Text hpText;

    public float currentActionTimer = 0f;
    public float actionThreshold = 100f;

    public List<ActionBase> Actions;

    // Action queue
    private Queue<(ActionBase action, Agent target)> actionQueue = new Queue<(ActionBase, Agent)>();

    // Updated CanAct: only true if timer is full AND there's an action queued
    public bool CanAct => currentActionTimer >= actionThreshold && actionQueue.Count > 0;

    private void Start()
    {
        if (labelPrefab == null)
        {
            labelPrefab = Resources.Load<GameObject>("AgentLabel");
        }

        /*

        labelInstance = Instantiate(labelPrefab, transform); // child of Agent GameObject
        labelInstance.transform.localPosition = new Vector3(0, -1.5f, 0); // adjust as needed

        // Update text with name and HP
        TMP_Text labelText = labelInstance.GetComponentInChildren<TMP_Text>();
        if (labelText != null)
        {
            labelText.text = $"{Name}\nHP: {HP}";
        }
        */

        if (labelPrefab != null && labelInstance == null)
        {
            // Instantiate as a child of the Agent
            labelInstance = Instantiate(labelPrefab, transform);

            // Move the label below the agent (adjust as needed)
            labelInstance.transform.localPosition = new Vector3(0f, -1f, 0f);

            // Scale down if needed (adjust as needed)
            labelInstance.transform.localScale = Vector3.one * 0.01f;

            // Ensure the canvas is in World Space
            foreach (Canvas canvas in labelInstance.GetComponentsInChildren<Canvas>())
            {
                canvas.renderMode = RenderMode.WorldSpace;
            }

            UpdateLabel();
        }
    }

    public void Initialize(string name, float hp, float atk, float def, float spd, bool isPlayer, List<ActionBase> actions)
    {
        Name = name;
        MaxHP = hp;
        HP = hp;
        Attack = atk;
        Defense = def;
        Speed = spd;
        IsPlayer = isPlayer;
        Actions = new List<ActionBase>(actions);
    }

    public void UpdateLabel()
    {
        if (labelInstance == null) return;

        TMP_Text[] texts = labelInstance.GetComponentsInChildren<TMP_Text>();
        foreach (TMP_Text text in texts)
        {
            if (text.name.ToLower().Contains("name"))
                text.text = Name;
            else if (text.name.ToLower().Contains("hp"))
                text.text = $"HP: {HP}";
        }
    }

    public void UpdateTimer(float deltaTime)
    {
        currentActionTimer += Speed * deltaTime;
    }

    public void ResetActionTimer()
    {
        currentActionTimer = 0f;
    }

    public void QueueAction(ActionBase action, Agent target)
    {
        actionQueue.Enqueue((action, target));
    }

    public void ExecuteQueuedAction()
    {
        if (actionQueue.Count > 0)
        {
            var (action, target) = actionQueue.Dequeue();
            if (action != null && target != null && IsAlive && target.IsAlive)
            {
                action.Execute(this, target);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;
        HP = Mathf.Max(HP, 0); // clamp to zero
        Debug.Log($"{Name} took {amount} damage. Remaining HP: {HP}");

        UpdateLabel();

        if (HP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{Name} has been defeated!");
        // Trigger death animation or logic here

        TMP_Text[] texts = labelInstance?.GetComponentsInChildren<TMP_Text>();
        if (texts != null)
        {
            foreach (var text in texts)
            {
                if (text.name.ToLower().Contains("hp"))
                    text.text = "DEFEATED";
            }
        }

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Sprite deadSprite;
        if (IsPlayer)
        {
            deadSprite = Resources.Load<Sprite>("DefeatedPlayer");
        } else
        {
            deadSprite = Resources.Load<Sprite>("DefeatedEnemy");
        }
        if (renderer != null && deadSprite != null)
        {
            renderer.sprite = deadSprite;
        }
    }

    public bool IsAlive => HP > 0;

    public void Heal(int amount)
    {
        HP += amount;
        HP = Mathf.Min(HP, MaxHP);
        Debug.Log($"{Name} healed for {amount}. Current HP: {HP}");
        UpdateLabel();
    }

    public void ModifyStat(StatType stat, int amount)
    {
        switch (stat)
        {
            case StatType.Attack:
                Attack += amount;
                break;
            case StatType.Defense:
                Defense += amount;
                break;
            case StatType.Speed:
                Speed += amount;
                break;
        }
        Debug.Log($"{Name}'s {stat} modified by {amount}");
    }

    public bool IsPlayerAgent()
    {
        return IsPlayer;
    }

    public string getName()
    {
        return Name;
    }

    public bool IsHealing { get; set; }
    public bool IsTakingDamage { get; set; }
    public bool HasActionsQueued => actionQueue.Count > 0;
}