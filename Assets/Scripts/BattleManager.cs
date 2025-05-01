using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleManager : MonoBehaviour
{

    public static BattleManager Instance;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public int numberOfPlayers;
    public int numberOfEnemies;

    public Transform playerSpawnParent;
    public Transform enemySpawnParent;

    public List<Agent> players = new List<Agent>();
    public List<Agent> enemies = new List<Agent>();
    private List<Agent> allAgents = new List<Agent>();

    private List<ActionBase> actions;

    private bool battleInProgress = false;

    void Start()
    {
        Debug.Log("BattleManager intitialized!");
        CreateActions();
        InitializeBattle();
    }

    void CreateActions()
    {
        // Damage Action
        DamageAction slash = ScriptableObject.CreateInstance<DamageAction>();
        slash.ActionName = "Slash";
        slash.power = 10;

        // Heal Action
        HealAction heal = ScriptableObject.CreateInstance<HealAction>();
        heal.ActionName = "Heal";
        heal.healAmount = 8;

        // Buff Action
        BuffAction powerUp = ScriptableObject.CreateInstance<BuffAction>();
        powerUp.ActionName = "Power Up";
        powerUp.statToBuff = StatType.Attack;
        powerUp.amount = 5;
        powerUp.duration = 5f;

        // Damage Over Time Action
        DamageOverTimeAction burn = ScriptableObject.CreateInstance<DamageOverTimeAction>();
        burn.ActionName = "Burn";
        burn.damagePerTick = 3;
        burn.tickInterval = 1f;
        burn.duration = 5f;

        // Heal Over Time Action
        HealOverTimeAction regen = ScriptableObject.CreateInstance<HealOverTimeAction>();
        regen.ActionName = "Regeneration";
        regen.healPerTick = 2;
        regen.tickInterval = 1f;
        regen.duration = 5f;

        // Debuff Action
        DebuffAction weaken = ScriptableObject.CreateInstance<DebuffAction>();
        weaken.ActionName = "Weaken";
        weaken.statToDebuff = StatType.Attack;
        weaken.amount = 4;
        weaken.duration = 4f;

        // Assign to player/enemy
        actions = new List<ActionBase> { slash, heal, powerUp, burn, regen, weaken };
    }

    void Update()
    {
        if (battleInProgress)
        {
            float deltaTime = Time.deltaTime;
            //Debug.Log(deltaTime.ToString());

            float interval = 3f; // every 2 seconds
            if (Mathf.Abs(Time.time % interval) < 0.1f)
            {
                foreach (Agent agent in allAgents)
                {
                    if (agent.HP <= 0) continue;

                    agent.UpdateTimer(deltaTime);

                    if (agent.CanAct)
                        Debug.Log("Agent " + agent.getName() + " can act");
                    {
                        if (agent.HasActionsQueued)
                        {
                            Debug.Log("Agent " + agent.getName() + " has actions queued");
                            agent.ExecuteQueuedAction();
                            agent.ResetActionTimer();
                        }
                        else
                        {
                            Debug.Log("Agent " + agent.getName() + " did not have actions queued");
                            PerformAction(agent); // This will queue an action
                        }
                    }
                }
            }

            CheckForBattleEnd();
        }
    }

    public void InitializeBattle()
    {
        players.Clear();
        enemies.Clear();
        allAgents.Clear();

        // Spawn players on the left
        for (int i = 0; i < numberOfPlayers; i++)
        {
            GameObject player = Instantiate(playerPrefab, playerSpawnParent);

            // Ensure the player starts at the proper scale (0.5 of its original size)
            player.transform.localScale = Vector3.one * 0.2f;

            // Left side position
            Vector2 leftSideOffset = new Vector2(
                Random.Range(-6f, -3f),  // X between -6 and -3
                Random.Range(-3f, 3f)    // Y between -3 and 3
            );
            player.transform.localPosition = leftSideOffset;
            string name = NameGenerator.GenerateRandomName();

            Agent agent = player.GetComponent<Agent>();
            agent.Initialize(name, 100, Random.Range(10, 20), Random.Range(5, 10), Random.Range(1f, 3f), true, actions);
            players.Add(agent);
            allAgents.Add(agent);
            Debug.Log("Agent " + agent.getName() + " has been added to the game as a PLAYER.");
        }

        // Spawn enemies on the right
        for (int i = 0; i < numberOfEnemies; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, enemySpawnParent);

            // Ensure the enemy starts at the proper scale (0.5 of its original size)
            enemy.transform.localScale = Vector3.one * 0.2f;

            // Right side position
            Vector2 rightSideOffset = new Vector2(
                Random.Range(3f, 6f),    // X between 3 and 6
                Random.Range(-3f, 3f)    // Y between -3 and 3
            );
            enemy.transform.localPosition = rightSideOffset;
            string name = NameGenerator.GenerateRandomName();

            Agent agent = enemy.GetComponent<Agent>();
            agent.Initialize(name, 100, Random.Range(10, 20), Random.Range(5, 10), Random.Range(1f, 3f), false, actions);
            enemies.Add(agent);
            allAgents.Add(agent);
            Debug.Log("Agent " + agent.getName() + " has been added to the game as an ENEMY.");
        }

        battleInProgress = true;
        Debug.Log("Battle has started!");
    }

    void PerformAction(Agent agent)
    {
        if (agent.Actions.Count > 0)
        {
            ActionBase action = agent.Actions[Random.Range(0, agent.Actions.Count)];
            Agent target = GetRandomTarget(agent, action);
            Debug.Log(agent.getName() + " will perform " + action.getName() + " on " + target.getName());
            if (target != null)
            {
                agent.QueueAction(action, target); // Enqueue instead of executing right away
            }
        }
    }

    Agent GetRandomTarget(Agent user, ActionBase action)
    {
        List<Agent> targetCandidates = new List<Agent>();

        // Handle DamageAction
        if (action is DamageAction || action is DamageOverTimeAction || action is DebuffAction)
        {
            if (user.IsPlayerAgent())
            {
                targetCandidates.AddRange(enemies);
            } else
            {
                targetCandidates.AddRange(players);
            }
        }
        // Handle HealAction
        else if (action is HealAction || action is HealOverTimeAction || action is BuffAction)
        {
            if (user.IsPlayerAgent())
            {
                targetCandidates.AddRange(players);
            }
            else
            {
                targetCandidates.AddRange(enemies);
            }
        }

        // Filter out the user if they are in the list (they shouldn't target themselves in some cases)
        targetCandidates.Remove(user);

        // If there are valid targets, pick a random one
        if (targetCandidates.Count > 0)
        {
            return targetCandidates[Random.Range(0, targetCandidates.Count)];
        }

        return null; // Return null if no valid target is available
    }

    void CheckForBattleEnd()
    {
        bool allPlayersDead = players.TrueForAll(agent => agent.HP <= 0);
        bool allEnemiesDead = enemies.TrueForAll(agent => agent.HP <= 0);

        if (allPlayersDead || allEnemiesDead)
        {
            // Battle has ended, perform victory or defeat logic here
            battleInProgress = false;
            Debug.Log(allPlayersDead ? "Enemies Win!" : "Players Win!");

            foreach (Agent agent in allAgents)
            {
                agent.StopAllCoroutines();
                agent.enabled = false; // Prevents any remaining behavior
            }

            Time.timeScale = 0f; // freezes the game
            // ShowVictoryScreen(result);
        }
    }

    private void Awake()
    {
        // Make sure there's only one instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps it across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
