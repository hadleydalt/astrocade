Development Process: 
1. Create Agent class (Player and Enemy) with attributes like IsAlive, HP, Name, etc. [5 minutes]
2. Create BattleManager class: initializes battle, actions, calls on agents to queue actions, and check for battle end [30 minutes]
3. Create Action subclasses [20 minutes]
4. Further flesh out BattleManager: Add Target delegation system [30 minutes]
5. Add co-routines to actions happening over time [10 minutes]
6. Debug and test BattleManager and Agent queuing actions [~2 hours]
7. Add Prefabs for UI (including creating original UI assets) [30 minutes]

Design Decisions: 
1. Modular Action System: Actions are stored as subclasses of ActionBase rather than as methods of the Agent class. This makes them easier to queue and reuse since they are not hard-coded into the Agent. It also makes the system more scalable and easier for designers to add new abilities without changing core logic. There is a clean separation of data and behavior.
2. Dynamic Agent Initialization: Agents are created at runtime so the team sizes can be configurable. 
3. Omit visualization of Buff/Debuff Actions on screen: Time did ot permit me to add this visualization, however, within the given time I was able to add HP visualization and Debug logs that explain what is happening, which can be translated into UI in the future. 
4. Spatial Separation of Teams: While sticking to a 2D layout, Player and Enemy teams are clearly separated to improve visual heirarchy and readability of the battlefield. 
5. Battle Lifecycle Management: REgularly checks for end-of-battle conditions to prevent the game from running on forever. 
6. Debug Logs to explain battle process: Beacsue of time limitations, the battle process is explained in debug logs. With more time, these can be translated into dynamic UI. 