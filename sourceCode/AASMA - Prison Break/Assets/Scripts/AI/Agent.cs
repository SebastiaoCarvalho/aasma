

using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour {

    private Vector3 target;
    private NavMeshAgent agent;

    [Header("Agent Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float angularSpeed = 120f;

    private void Start() {
        target = GameObject.Find("Exit").transform.position;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.angularSpeed = angularSpeed;
        agent.SetDestination(target);
    }


}