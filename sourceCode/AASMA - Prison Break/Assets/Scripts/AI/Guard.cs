using System.Collections.Generic;
using UnityEngine;

class Guard : Agent {
    [SerializeField] List<GameObject> waypoints = new List<GameObject>();
    bool chasing = false;
    protected new void Start() {
        base.Start();
        if (waypoints == null || waypoints.Count == 0)
            return;
        ChangeTrajectory();
    }

    void Update() {
        if (waypoints == null || waypoints.Count == 0)
            return;
        if (chasing)
            return;
        if (Vector3.Distance(transform.position, target) < 1) 
            ChangeTrajectory();
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
        agent.SetDestination(other.transform.position);
        chasing = true;
    }

    public void LosePrisoner(Collider other) {
        chasing = false;
        ChangeTrajectory();
    }

}