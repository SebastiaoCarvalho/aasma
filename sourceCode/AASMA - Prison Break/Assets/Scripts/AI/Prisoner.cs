using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Prisoner : Agent
{
    
    [SerializeField] List<GameObject> roomWaypoints = new List<GameObject>();
    private Action currentAction;
    private Dictionary<Guard, int> guardInfo = new Dictionary<Guard, int>();

    public Vector3 startingPosition;
    int currentRoom = 2;
    public int targetRoom = 0;
    [SerializeField] float cash = 100;
    bool escaped = false;
    public bool arrested = false;

    Guard guardPerformingTheArrest;
    float persuasion = 1;
    public float Persuasion { get => persuasion; }

    new void Start()
    {
        base.Start();
        startingPosition = transform.position;
        roomWaypoints = GameObject.FindGameObjectsWithTag("RoomWaypoint").OrderBy(waypoint => waypoint.name).ToList();
        ChooseAction();
    }

    public void ChooseAction() {
        if (arrested == true) return;
        List<Action> actions = GetAvailableActions();
        foreach (Guard guard in guardInfo.Keys.ToList().Where(guard => guardInfo[guard] == currentRoom)) {
            actions.Add(new Bribe(this, guard));
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

        if (currentAction != null && currentAction.IsDone()) {
            if (arrested == true) {
                guardPerformingTheArrest.FinishArrest();
                gameObject.SetActive(false);
                return;
            }
            ChooseAction();
        }
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

    public void Spend(float amount) {
        cash -= amount;
    }

    public float ProposeBribe(float min, float max) {
        return Random.Range(min, max);
    }

    public void Incite(Prisoner prisoner) {
        int guardRoom = guardInfo.Values.Last();
        prisoner.ForceMove(guardRoom);
    }

    public void ForceMove(int room) {
        currentAction = new MoveTo(
            roomWaypoints[room - 1].transform.position, 
            roomWaypoints[room - 1].transform.parent.GetComponent<Room>(), 
            this
        );
        currentAction.Execute();
    }

    public bool AcceptAmmount(float amount) {
        return cash >= amount && Random.Range(0, 1) > 0.5; // TODO : fix this later
    }

    public void AddGuardInfo(Guard guard) {
        Debug.Log("Adding guard info");
        guardInfo[guard] = targetRoom;
    }

    public void RemoveGuardInfo(Guard guard) {
        guardInfo.Remove(guard);
    }

    public void Arrested(Guard guard) {
        Debug.Log("-----------------------------");
        arrested = true;
        guardPerformingTheArrest = guard;
        currentAction = new MoveTo(startingPosition, roomWaypoints[1].transform.parent.GetComponent<Room>(), this);
        currentAction.Execute();
    }

}
