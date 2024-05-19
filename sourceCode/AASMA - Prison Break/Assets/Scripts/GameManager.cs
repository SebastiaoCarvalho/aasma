using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    const int ENDRUNS = 100;

    public bool canBribe = true;
    public bool canIncite = true;

    // Statistics
    int prisonerWins = 0;
    protected float averageBriberyAmount = 0;
    private float averageTimeToEscape = 0;
    private float currentTimeToEscape = 0;
    int briberyTimes = 0;
    protected int runs = 0;


    // Prisoners and Guards to respawn
    protected List<Prisoner> prisoners = new List<Prisoner>();
    protected List<Guard> guards = new List<Guard>();

    private static GameManager instance = null;
    public static GameManager Instance { get => instance; }

    private void Awake() {
        if (instance == null) {
            instance = this;
            Debug.Log("GameManager instance created");
        }
    }

    protected void Start() {
        prisoners = new List<Prisoner>(FindObjectsOfType<Prisoner>());
        guards = new List<Guard>(FindObjectsOfType<Guard>());
    }

    protected void Update() {
        currentTimeToEscape += Time.deltaTime;
    }

    virtual public void PrisonerEscaped(Prisoner prisoner) {
        prisonerWins++;
        runs++;
        averageTimeToEscape = averageTimeToEscape * (runs - 1) + currentTimeToEscape;
        averageTimeToEscape /= runs;
        ResetGame();
    }

    public void AddBriberyAmount(float amount) {
        averageBriberyAmount = averageBriberyAmount * briberyTimes + amount;
        briberyTimes++;
        averageBriberyAmount /= briberyTimes;
    }

    virtual protected void ResetGame() {
        if (runs == ENDRUNS) {
            PrintStats();
            UnityEditor.EditorApplication.isPlaying = false; // Stop the game
        }
        prisoners.ForEach(prisoner => prisoner.Reset());
        guards.ForEach(guard => guard.Reset());
        currentTimeToEscape = 0;
    }

    virtual public void PrisonerArrested(Prisoner prisoner) {
        runs++;
        ResetGame();
    }

    private void OnApplicationQuit() {
        PrintStats();
    }

    protected virtual void PrintStats() {
        Debug.LogFormat("Total simulations: {0}", runs);
        Debug.LogFormat("Prisoner wins: {0}", prisonerWins);
        Debug.LogFormat("Average time to escape: {0}", averageTimeToEscape);
        if (canBribe) Debug.LogFormat("Average bribery amount: {0}", averageBriberyAmount);
    }

}