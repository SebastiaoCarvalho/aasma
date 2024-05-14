using UnityEngine;

public class Bribe : Action
{

    private Guard guard;
    private Prisoner prisoner;

    public Bribe(Prisoner prisoner, Guard guard) {
        this.prisoner = prisoner;
        this.guard = guard;
    }

    public override float Utility() // FIXME : what should the utility be?
    {
        //return 1 / (guard.MinAmountBribery + 1) * prisoner.Persuasion * 100;
        return /* Random.Range(0f, 1f) * */ 10;
    }

    public override void Execute()
    {
        int step = 1;
        float amount = guard.MinAmountBribery;
        float proposal = guard.MinAmountBribery * Random.Range(1f, 1.1f);
        while (amount != -1 && amount != proposal) {
            if (step > 1 && prisoner.AcceptAmmount(amount))
                break;
            proposal = prisoner.ProposeBribe(proposal, amount);
            amount = guard.NegotiateBribe(step, proposal);
            Debug.LogFormat("Prisoner proposed {0}, guard countered with {1}", proposal, amount);
            step++;
        }
        if (amount < 0) {
            guard.ArrestPrisoner(prisoner);
        }
        else {
            prisoner.Spend(amount);
            guard.Sleep();
            prisoner.RemoveGuardInfo(guard);
        }
    }

    public override bool IsDone()
    {
        return true;
    }
}