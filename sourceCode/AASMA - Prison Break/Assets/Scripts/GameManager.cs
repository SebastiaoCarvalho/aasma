using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    const int ENDRUNS = 30;

    // Statistics
    int prisonerWins = 0;
    float averageBriberyAmount = 0;
    int briberyTimes = 0;
    int runs = 0;

    // TODO : add stats for multiple prisoners

    // Prisoners and Guards to respawn
    List<Prisoner> prisoners = new List<Prisoner>();
    List<Guard> guards = new List<Guard>();

    private static GameManager instance = null;
    public static GameManager Instance { get => instance; }

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    private void Start() {
        prisoners = new List<Prisoner>(FindObjectsOfType<Prisoner>());
        guards = new List<Guard>(FindObjectsOfType<Guard>());
    }

    public void PrisonerEscaped() {
        prisonerWins++;
        runs++;
        ResetGame();
    }

    public void AddBriberyAmount(float amount) {
        averageBriberyAmount += amount;
        briberyTimes++;
        averageBriberyAmount /= briberyTimes;
    }

    private void ResetGame() {
        if (runs == ENDRUNS) {
            PrintStats();
            UnityEditor.EditorApplication.isPlaying = false; // Stop the game
        }
        prisoners.ForEach(prisoner => prisoner.Reset());
        guards.ForEach(guard => guard.Reset());
    }

    public void PrisonerArrested() {
        runs++;
        ResetGame();
    }

    private void OnApplicationQuit() {
        PrintStats();
    }

    private void PrintStats() {
        Debug.LogFormat("Total simulations: {0}", runs);
        Debug.LogFormat("Prisoner wins: {0}", prisonerWins);
        Debug.LogFormat("Average bribery amount: {0}", averageBriberyAmount);
    }

}