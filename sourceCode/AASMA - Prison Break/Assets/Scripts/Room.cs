using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] public int roomNumber = 0;
    [SerializeField] private float roomUtility = 0;

    public float Utility { get { return roomUtility; } }

    private void OnTriggerEnter(Collider other) {
        Prisoner prisoner = other.GetComponent<Prisoner>();

        if (prisoner != null)
            prisoner.SetRoom(roomNumber);
    }
}
