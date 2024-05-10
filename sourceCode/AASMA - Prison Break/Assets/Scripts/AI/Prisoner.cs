using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Prisoner : Agent
{
    
    [SerializeField] List<GameObject> roomWaypoints = new List<GameObject>();
    private Action currentAction;

    int currentRoom = 2;

    new void Start()
    {
        base.Start();
        roomWaypoints = GameObject.FindGameObjectsWithTag("RoomWaypoint").OrderBy(waypoint => waypoint.name).ToList();
        Debug.Log(roomWaypoints);
        ChooseAction();
    }

    void ChooseAction() {
        List<Action> actions = GetAvailableActions();
        Action best = null;
        float bestScore = Mathf.NegativeInfinity;
        foreach (Action action in actions) {
            float score = action.Utility();
            if (score > bestScore) {
                best = action;
                bestScore = score;
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
        if (currentAction != null && currentAction.IsDone()) {
            ChooseAction();
        }
    }

    //place holder
    public void SetRoom(int room) {
        if (room != currentRoom) {
            currentRoom = room;
            ChooseAction();
        }
    }
}
