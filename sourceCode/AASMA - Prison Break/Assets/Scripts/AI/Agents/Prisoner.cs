using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Prisoner : Agent
{
    
    public bool canBribe = true;
    public bool canIncite = true;
    [SerializeField] List<GameObject> roomWaypoints = new List<GameObject>();
    private Action currentAction;
    private Dictionary<Guard, int> guardInfo = new Dictionary<Guard, int>();
    private Dictionary<Guard, float> timeSinceLastSeen = new Dictionary<Guard, float>();
    private Prisoner lastSeen;
    public int currentRoom = 2;
    public int targetRoom = 0;
    [SerializeField] public float cash = 67;
    float startingCash;
    bool escaped = false;
    public bool arrested = false;
    Guard guardPerformingTheArrest;

    new void Start()
    {
        base.Start();
        startingCash = cash;
        startingPosition = transform.position;
        roomWaypoints = GameObject.FindGameObjectsWithTag("RoomWaypoint").OrderBy(waypoint => waypoint.name).ToList();
        Reset();
    }

    public override void Reset()
    {
        currentAction = null;
        guardInfo.Clear();
        timeSinceLastSeen.Clear();
        currentRoom = 2;
        targetRoom = 0;
        cash = startingCash;
        escaped = false;
        arrested = false;
        gameObject.SetActive(true);
        agent.Warp(startingPosition);
        ChooseAction();
    }

    public void ChooseAction() {
        if (arrested) return;
        List<Action> actions = GetAvailableActions();
        if (canBribe)
            foreach (Guard guard in guardInfo.Keys.ToList().Where(guard => guardInfo[guard] == targetRoom)) {
                if (guard.prisonersToIgnore.Contains(this) || guard.arresting) continue;
                Room room = roomWaypoints[guardInfo[guard] - 1].transform.parent.GetComponent<Room>();
                actions.Add(new Bribe(this, guard, room));
            }
        if (lastSeen != null && canIncite && guardInfo.Count > 0) {
            actions.Add(new Incite(this, lastSeen, guardInfo.Last().Key));
        }
        Action best = null;
        float bestScore = Mathf.NegativeInfinity;
        foreach (Action action in actions) {
            float score = action.Utility();
            if (score > bestScore) {
                best = action;
                bestScore = score;
            }

            else if (score == bestScore && Random.Range(0f, 1f) > 0.5) { // Don't always choose the same action
                best = action;
            }
        }
        Debug.LogFormat("Best action: {0} with utility {1}", best, bestScore);
        currentAction = best;
        currentAction.Execute();
    }

    private List<Action> GetAvailableActions() {
        List<Action> actions = new List<Action>();
        foreach (int room in RoomMap.GetRoomConnections(currentRoom)) {
            GameObject roomWaypoint = roomWaypoints[room - 1];
            actions.Add(new MoveTo(roomWaypoint.transform.position, roomWaypoint.transform.parent.GetComponent<Room>(), this));
        }
        return actions;
    }

    // Update is called once per frame
    void Update()
    {
        if (escaped) return;
        UpdateLastSeenGuards();
        if (currentAction != null && currentAction.IsDone()) {
            if (arrested) {
                guardPerformingTheArrest.FinishArrest();
                gameObject.SetActive(false);
                GameManager.Instance.PrisonerArrested(this);
                return;
            }
            ChooseAction();
        }
    }

    private void UpdateLastSeenGuards() {
        timeSinceLastSeen = timeSinceLastSeen.ToDictionary(pair => pair.Key, pair => pair.Value + Time.deltaTime);
        foreach (Guard guard in timeSinceLastSeen.Keys.ToList().Where(guard => timeSinceLastSeen[guard] > 5)) {
            RemoveGuardInfo(guard);
            Debug.Log("Removing guard info");
        }
    }

    public float Utility() {
        if (escaped) return 100;
        if (arrested) return -100;
        return currentAction.Utility();
    }

    //place holder
    public void SetRoom(int room) {
        if (room != currentRoom) {
            currentRoom = room;
            if (currentRoom == 18) {
                escaped = true;
                return;
            }
            //ChooseAction();
        }
    }

    public bool GuardInRoom(int room) {
        return guardInfo.ContainsValue(room);
    }

    public bool HasGuardInfo() {
        return guardInfo.Count > 0;
    }

    public bool HasEnoughCash(float amount) {
        return cash >= amount;
    }
    public void Spend(float amount) {
        cash -= amount;
    }

    public void ReceiveCash(float amount) {
        cash += amount;
    }

    public float ProposeBribe(float min, float max) { // always return max since Prisoner value more the freedom than the money
        return max;
    }

    public void Incite(Prisoner prisoner, float amount, Guard guard) {
        Spend(amount);
        prisoner.ReceiveCash(amount);
        int guardRoom = guardInfo.Values.Last();
        prisoner.ForceMove(guardRoom);
        RemovePrisonerInfo(prisoner);
        PopUp("Incite");
    }

    public void ForceMove(int room) {
        currentAction = new MoveTo(
            roomWaypoints[room - 1].transform.position, 
            roomWaypoints[room - 1].transform.parent.GetComponent<Room>(), 
            this
        );
        currentAction.Execute();
    }

    public void AddGuardInfo(Guard guard) {
        Debug.Log("Adding guard info");
        guardInfo[guard] = targetRoom;
        timeSinceLastSeen[guard] = 0;
    }

    public void RemoveGuardInfo(Guard guard) {
        guardInfo.Remove(guard);
        timeSinceLastSeen.Remove(guard);
    }

    public bool KnowsGuard(Guard guard) {
        return guardInfo.ContainsKey(guard);
    }

    public void AddPrisonerInfo(Prisoner prisoner) {
        Debug.LogWarning("Adding prisoner info");
        lastSeen = prisoner;
    }

    public void RemovePrisonerInfo(Prisoner prisoner) {
        lastSeen = null;
    }

    public void Arrested(Guard guard) {
        Debug.Log("-----------------------------");
        arrested = true;
        guardPerformingTheArrest = guard;
        currentAction = new MoveTo(startingPosition, roomWaypoints[1].transform.parent.GetComponent<Room>(), this);
        currentAction.Execute();
    }

}
