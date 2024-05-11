using System.Collections.Generic;
using UnityEngine;

public class Guard : Agent {
    [SerializeField] List<GameObject> waypoints = new List<GameObject>();
    [SerializeField] float minAmountBribery = 0;
    bool sleep = false;
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
        if (chasing || sleep)
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

    public float NegotiateBribe(int negotiationStep, float proposalAmount) {
        float rejectionProb = negotiationStep * Random.Range(0, 0.5f) + Random.Range(0, 0.2f);
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

}