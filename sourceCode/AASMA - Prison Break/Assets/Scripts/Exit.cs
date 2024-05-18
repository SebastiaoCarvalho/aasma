using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Prisoner")) {
            Debug.Log("Prisoner escaped!");
            GameManager.Instance.PrisonerEscaped(other.GetComponent<Prisoner>());
        }
    }
}
