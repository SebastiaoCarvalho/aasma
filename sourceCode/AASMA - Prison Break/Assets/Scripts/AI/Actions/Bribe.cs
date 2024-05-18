using UnityEngine;

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

    public override float Utility() // FIXME : what should the utility be?
    {
        float escapeVal = currentRoom.Utility / 10;
        float bribeVal = (100 - guard.MinAmountBribery) / 100;

        Debug.LogFormat("Bribe utility: {0}", (bribeVal * 1.3f - escapeVal * .5f) * 10);
        if (guard.prisonersToIgnore.Contains(prisoner)) return 0; // add * prisioner to ignore
        return (bribeVal * 1.3f - escapeVal * .5f) * 10;
    }

    public override void Execute()
    {
        int step = 1;
        float briberyStartAmount = 10;
        float amount =  briberyStartAmount * Random.Range(1f, 1.1f);
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