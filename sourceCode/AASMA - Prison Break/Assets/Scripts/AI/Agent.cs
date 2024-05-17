

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System;

public class Agent : MonoBehaviour {

    protected Vector3 target;
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

    public void MoveTo(Vector3 target) {
        agent.SetDestination(target);
    }

    public void PopUp(String message) {
        text.transform.position = new Vector3(this.transform.position.x, 4, this.transform.position.z);
        text.GetComponent<TextMeshPro>().text = message;
        var animation = Instantiate(text);
        Destroy(animation, 1.5f);
    }
}