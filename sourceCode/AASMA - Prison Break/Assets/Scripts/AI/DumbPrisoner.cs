using UnityEngine;

class DumbPrisoner : Agent {
    private void Start() {
        base.Start();
        GameObject exit = GameObject.Find("Exit");
        target = exit.transform.position;
        agent.SetDestination(target);
    }
}