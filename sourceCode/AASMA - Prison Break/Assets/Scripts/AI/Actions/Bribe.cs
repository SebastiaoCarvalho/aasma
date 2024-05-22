using UnityEngine;
using System; 

public class Bribe : Action
{

    private Guard guard;
    private Prisoner prisoner;
    private Room currentRoom;

    public Bribe(Prisoner prisoner, Guard guard, Room currentRoom) {
        this.prisoner = prisoner;
        this.guard = guard;
        this.currentRoom = currentRoom;
    }

    public override float Utility()
    {
        /*
        double escapeVal = currentRoom.Utility;
        float bribeVal = (prisoner.cash - guard.MinAmountBribery) / 100.0f;

        Debug.LogFormat("Bribe utility: {0}", (float) Math.Pow(1.4, escapeVal) + bribeVal);
        if (guard.prisonersToIgnore.Contains(prisoner)) return 0; // add * prisioner to ignore
        return (float) Math.Pow(1.4, escapeVal) + bribeVal;
        */
        
        float escapeVal = currentRoom.Utility / 10;
        float bribeVal = (100 - guard.MinAmountBribery) / 100;

        return (bribeVal * 0.8f + escapeVal * .5f) * 10;
    }

    public override void Execute()
    {
        Debug.Log("Attempting to bribe guard");
        int step = 1;
        float briberyStartAmount = 10;
        float amount =  briberyStartAmount * UnityEngine.Random.Range(1f, 1.1f);
        float proposal = briberyStartAmount;

        // prisoner proposes amount and guard counters
        // if guard countes with -1, prisoner is arrested
        // if guard counters with same amount, prisoner pays and guard sleeps

        while (amount != -1 && amount != proposal) { 
            proposal = prisoner.ProposeBribe(proposal, amount);
            amount = guard.NegotiateBribe(step, proposal);
            Debug.LogFormat("Prisoner proposed {0}, guard countered with {1}", proposal, amount);
            step++;
        }
        if (amount < 0 || ! prisoner.HasEnoughCash(amount)) {
            guard.ArrestPrisoner(prisoner);
        }
        else {
            prisoner.Spend(amount);
            guard.Ignore(prisoner);
            prisoner.RemoveGuardInfo(guard);
            prisoner.PopUp("Bribe");
            Debug.Log("Bribe");
            GameManager.Instance.AddBriberyAmount(amount);
        }
    }

    public override bool IsDone()
    {
        return true;
    }
}