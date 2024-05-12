using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Guard : Agent {
    [SerializeField] List<GameObject> waypoints = new List<GameObject>();
    [SerializeField] List<GameObject> roomWaypoints = new List<GameObject>();
    [SerializeField] float minAmountBribery = 0;
    [SerializeField] private float alertedSpeed = 11f;
    List <GameObject> otherGuards = new List<GameObject>();
    bool sleep = false;
    bool chasing = false;
    
    // coop variables
    int currentRoom = 2;
    bool alerted = false;
    GameObject assisting = null;
    bool assisted = false;
    float timeWithoutSeeingPrisioner = 0.0F;

    protected new void Start() {
        base.Start();
        if (waypoints == null || waypoints.Count == 0)
            return;
        otherGuards = GameObject.FindGameObjectsWithTag("Guard").Where(gameObject => gameObject != this.gameObject).ToList();
        roomWaypoints = GameObject.FindGameObjectsWithTag("RoomWaypoint").OrderBy(waypoint => waypoint.name).ToList();
        ChangeTrajectory();
    }

    void Update() {
        if (alerted) { // if alerted
            timeWithoutSeeingPrisioner += Time.deltaTime;
        }

        if (waypoints == null || waypoints.Count == 0)
            return;
        if (chasing || sleep)
            return;
        if (Vector3.Distance(transform.position, target) < 1) {
            if (alerted && timeWithoutSeeingPrisioner < 5.0f) { // if alerted
                return;
            }
            else if (assisting != null) {
                assisting.GetComponent<Guard>().EndAssistance();
                assisting = null;
            }
            ChangeTrajectory();
        }
    }

    private void ChangeTrajectory() {
        float dist = Mathf.Infinity;
        Vector3 closest = Vector3.zero;
        foreach (GameObject waypoint in waypoints) {
            if ((transform.position - waypoint.transform.position).magnitude < 1) continue;
            float d = Vector3.Distance(transform.position, waypoint.transform.position);
            if (d < dist) {
                dist = d;
                closest = waypoint.transform.position;
            }
        }
        target = closest;
        agent.SetDestination(target);
    }

    public void DetectPrisoner(Collider other) {
        timeWithoutSeeingPrisioner = 0.0F;
        target = other.transform.position;
        agent.SetDestination(target);
        if (!assisted) {
            RequestAssistance();
        }
        alerted = true;
        agent.speed = alertedSpeed;
        chasing = true;
    }

    private void RequestAssistance() {
        int bestRoom = getInterseptionPoint(currentRoom);
        if (bestRoom == 0) { // or room 13
            return;
        }

        if (bestRoom != 13) {
            bestRoom = getInterseptionPoint(bestRoom);
        }

        if (bestRoom == 0) {
            return;
        }

        callClosestAvailableGuard(bestRoom);
    }

    private int getInterseptionPoint(int currentRoom) {
        int bestRoom = 0;
        float bestUtility = Mathf.NegativeInfinity;
        foreach (int roomNumber in RoomMap.GetRoomConnections(currentRoom)) {
            GameObject roomWaypoint = roomWaypoints[roomNumber - 1];
            Room room = roomWaypoint.transform.parent.GetComponent<Room>();
            if (room.Utility > bestUtility) {
                bestUtility = room.Utility;
                bestRoom = roomNumber;
            }
        }
        return bestRoom;
    }

    private void callClosestAvailableGuard(int room) {
        GameObject closestGuard = null;
        float distance = Mathf.Infinity;
        foreach (GameObject guardObject in otherGuards) {
            float d = guardObject.GetComponent<Guard>().AssistanceAvailability(room);
            if (d < distance) {
                distance = d;
                closestGuard = guardObject;
            }
        }

        if (closestGuard != null) {
            closestGuard.GetComponent<Guard>().InterseptPrisoner(room, this.gameObject);
            assisted = true;
        }
    }

    public void LosePrisoner(Collider other) {
        chasing = false;
        ChangeTrajectory();
    }

    public float NegotiateBribe(int negotiationStep, float proposalAmount) {
        float rejectionProb = negotiationStep * UnityEngine.Random.Range(0, 0.5f) + UnityEngine.Random.Range(0, 0.2f);
        if (rejectionProb > 1) return -1;
        return proposalAmount; // TODO : learn more about negotiation to improve this
    }

    public void ArrestPrisoner(Prisoner prisoner) {
        prisoner.gameObject.SetActive(false); // TODO : lead prisoner to cell
        return ;
    }

    public void Sleep() {
        transform.GetChild(0).gameObject.SetActive(false); // deactivate FOV
        agent.isStopped = true;
        sleep = true;
    }

    /*
    public bool InterseptPrisoner(int room) {
        if (chasing || assisting) {
            return false;
        }
        alerted = true;
        agent.speed = alertedSpeed;
        assisting = true;
        //d = Vector3.Distance(transform.position, waypoint.transform.position);
        GameObject roomWaypoint = roomWaypoints[room - 1];
        // se distancia dentro de certos parametros
        target = roomWaypoint.transform.position;
        agent.SetDestination(target); //position of the room
        return true;
    }
    */

    public float AssistanceAvailability(int room) {
        alerted = true;
        agent.speed = alertedSpeed;
        if (chasing || assisting) {
            return Mathf.Infinity;
        }
        GameObject roomWaypoint = roomWaypoints[room - 1];
        return Vector3.Distance(transform.position, roomWaypoint.transform.position);
    }

    public void InterseptPrisoner(int room, GameObject guard) {
        assisting = guard;
        GameObject roomWaypoint = roomWaypoints[room - 1];
        target = roomWaypoint.transform.position;
        agent.SetDestination(target);
    }

    public void EndAssistance() {
        assisted = false;
    }

    public void SetRoom(int room) {
        currentRoom = room;
    }

}