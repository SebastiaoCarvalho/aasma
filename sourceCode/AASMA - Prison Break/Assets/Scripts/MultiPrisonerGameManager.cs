using System.Collections.Generic;
using UnityEngine;

public class MultiPrisonerGameManager : GameManager {
    List<int> escapesOfPrisoner = new List<int>(); // Count times each prisoner escaped
    List<float> averageTimeToEscape = new List<float>(); // Average time to escape for each prisoner
    int prisonersPerRound = 0;
    List<Prisoner> remainingPrisoners = new List<Prisoner>();

    private new void Start() {
        base.Start();
        escapesOfPrisoner = new List<int>(); // acount for 0 escapee
        for (int i = 0; i < prisoners.Count; i++) {
            escapesOfPrisoner.Add(0);
            averageTimeToEscape.Add(0);
        }
        remainingPrisoners = new List<Prisoner>(prisoners);
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
        int index = prisoner.name[prisoner.name.Length - 1] - '0';
        escapesOfPrisoner[index]++;
        averageTimeToEscape[index] = averageTimeToEscape[index] * (escapesOfPrisoner[index] - 1) + currentTimeToEscape;
        averageTimeToEscape[index] /= escapesOfPrisoner[index];
        if (remainingPrisoners.Count == 0) {
            runs++;
            ResetGame();
        }
    }

    protected override void ResetGame() {
        Debug.Log("Prisoners escaped: " + prisonersPerRound);
        prisonersPerRound = 0;
        base.ResetGame();
        remainingPrisoners = new List<Prisoner>(prisoners);
    }

    protected override void PrintStats()
    {
        Debug.LogFormat("Total simulations: {0}", runs);
        for(int i = 0; i < escapesOfPrisoner.Count; i++) {
            Debug.LogFormat("Prisoner {0} escaped {1} times ", i , escapesOfPrisoner[i]);
            Debug.LogFormat("Average time to escape for Prisoner {0}: {1}", i, averageTimeToEscape[i]);
        }
        Debug.LogFormat("Average bribery amount: {0}", averageBriberyAmount);
        Debug.LogFormat("Number of bribes: {0}", briberyTimes);
        Debug.LogFormat("Average incite amount: {0}", averageInciteAmount);
        Debug.LogFormat("Number of incites: {0}", inciteTimes);
    }

}