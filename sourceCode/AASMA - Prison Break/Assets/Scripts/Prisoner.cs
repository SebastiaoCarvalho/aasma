using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Prisoner : Agent
{
    List<List<int>> roomConnections = new List<List<int>>();
    [SerializeField] List<GameObject> roomWaypoints = new List<GameObject>();

    int currentRoom = 2;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        roomConnections.Add(new List<int> {2, 5, 6});
        roomConnections.Add(new List<int> {1, 3});
        roomConnections.Add(new List<int> {4, 7, 17});
        roomConnections.Add(new List<int> {3, 5, 14});
        roomConnections.Add(new List<int> {1, 4});
        roomConnections.Add(new List<int> {1, 10, 14});
        roomConnections.Add(new List<int> {3, 8, 15, 16});
        roomConnections.Add(new List<int> {7, 9, 15, 17});
        roomConnections.Add(new List<int> {8, 11, 13, 16});
        roomConnections.Add(new List<int> {6, 11, 12});
        roomConnections.Add(new List<int> {9, 10});
        roomConnections.Add(new List<int> {10, 13});
        roomConnections.Add(new List<int> {9, 12});
        roomConnections.Add(new List<int> {4, 6});
        roomConnections.Add(new List<int> {7, 8, 16});
        roomConnections.Add(new List<int> {7, 9, 15});
        roomConnections.Add(new List<int> {3, 8});
        roomWaypoints = GameObject.FindGameObjectsWithTag("RoomWaypoint").OrderBy(waypoint => waypoint.name).ToList();
        Debug.Log(roomWaypoints);
        chooseAction();
    }

    void chooseAction() {
        moveTo(roomConnections[currentRoom - 1][Random.Range(0, roomConnections[currentRoom - 1].Count)]);
    }

    void moveTo(int room) {
        Debug.Log("Going to room " + room);
        agent.SetDestination(roomWaypoints[room - 1].transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //place holder
    public void setRoom(int room) {
        if (room != currentRoom) {
            currentRoom = room;
            chooseAction();
        }
    }
}
