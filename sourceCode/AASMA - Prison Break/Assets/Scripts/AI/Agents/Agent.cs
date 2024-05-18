

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System;

public abstract class Agent : MonoBehaviour {

    protected Vector3 target;
    public Vector3 startingPosition;
    protected NavMeshAgent agent;

    public GameObject text;

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

    public abstract void Reset();

    public void MoveTo(Vector3 target) {
        agent.SetDestination(target);
    }

    public void PopUp(String message) {
        var animation = Instantiate(text);
        animation.transform.position = new Vector3(this.transform.position.x, 4, this.transform.position.z);
        animation.GetComponent<TextMeshPro>().text = message;
        Destroy(animation, 1.5f);
    }
}