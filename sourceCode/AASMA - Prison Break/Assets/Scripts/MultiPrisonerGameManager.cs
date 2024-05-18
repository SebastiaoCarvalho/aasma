using System.Collections.Generic;
using UnityEngine;

public class MultiPrisonerGameManager : GameManager {
    List<int> timesNumberOfEscaped = new List<int>(); // COunt of number of prisoners escaped
    int prisonersPerRound = 0;
    List<Prisoner> remainingPrisoners = new List<Prisoner>();
    private int averageInciteAmount;
    private int inciteTimes;

    private new void Start() {
        base.Start();
        timesNumberOfEscaped = new List<int>(); // acount for 0 escapee
        for (int i = 0; i < prisoners.Count + 1; i++) {
            timesNumberOfEscaped.Add(0);
        }
        remainingPrisoners = new List<Prisoner>(prisoners);
    }

    public void AddInciteAmount(int amount) {
        averageInciteAmount = averageInciteAmount * inciteTimes + amount;
        inciteTimes++;
        averageInciteAmount /= inciteTimes;
    }

    public override void PrisonerArrested(Prisoner prisoner) {
        remainingPrisoners.Remove(prisoner);
        if (remainingPrisoners.Count == 0) {
            runs++;
            ResetGame();
        }
    }

    public override void PrisonerEscaped(Prisoner prisoner) {
        Debug.LogFormat("Previous prisoners: {0}", remainingPrisoners.Count);
        remainingPrisoners.Remove(prisoner);
        Debug.LogFormat("Remaining prisoners: {0}", remainingPrisoners.Count);
        prisonersPerRound++;
        if (remainingPrisoners.Count == 0) {
            runs++;
            ResetGame();
        }
    }

    protected override void ResetGame() {
        Debug.Log("Prisoners escaped: " + prisonersPerRound);
        Debug.Log(timesNumberOfEscaped.Count);
        timesNumberOfEscaped[prisonersPerRound]++;
        prisonersPerRound = 0;
        base.ResetGame();
        remainingPrisoners = new List<Prisoner>(prisoners);
    }

    protected override void PrintStats()
    {
        Debug.LogFormat("Total simulations: {0}", runs);
        for(int i = 0; i < timesNumberOfEscaped.Count; i++) {
            Debug.LogFormat("Rounds with {0} escapees : {1}", i , timesNumberOfEscaped[i]);
        }
        Debug.LogFormat("Average bribery amount: {0}", averageBriberyAmount);
    }

}