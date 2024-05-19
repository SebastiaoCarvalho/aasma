using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrestZone : MonoBehaviour
{
    private Guard holder;

    private void Awake() {
        holder = transform.parent.GetComponent<Guard>();
    }

    public void OnTriggerEnter(Collider collider) {
        if (holder.alerted && !holder.arresting && collider.GetComponent<Prisoner>() != null && !collider.GetComponent<Prisoner>().arrested && !holder.prisonersToIgnore.Contains(collider.GetComponent<Prisoner>())) {
            holder.ArrestPrisoner(collider.GetComponent<Prisoner>());
            Debug.Log("help");
        }
    }
}
