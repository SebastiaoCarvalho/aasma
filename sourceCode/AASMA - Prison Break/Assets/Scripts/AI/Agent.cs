

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour {

    protected Vector3 target;
    protected NavMeshAgent agent;

    [Header("Agent Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float angularSpeed = 120f;

    protected void Start() {
        /* List<GameObject> rooms = GameObject.FindGameObjectsWithTag("Rooms").ToList();
        target = rooms[Random.Range(0, rooms.Count)].transform.position; */
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.angularSpeed = angularSpeed;
        /* agent.SetDestination(target); */
    }

    

}