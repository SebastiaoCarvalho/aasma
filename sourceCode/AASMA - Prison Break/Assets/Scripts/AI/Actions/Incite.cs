using UnityEngine;

public class Incite : Action
{
    // Action used when a prisoner incites other to go to be caught by a guard, allowing the inciter to escape

    private Prisoner inciter;
    private Prisoner incited;
    private Guard guard;

    public Incite(Prisoner inciter, Prisoner incited, Guard guard)
    {
        this.inciter = inciter;
        this.incited = incited;
        this.guard = guard;
    }

    public override void Execute()
    {
        if (incited.KnowsGuard(guard)) return;
        float currentUtilty = incited.Utility();
        float neededUtility = currentUtilty * 1.1f;
        if (inciter.HasEnoughCash(neededUtility)) {
            inciter.Incite(incited, neededUtility, guard);
            MultiPrisonerGameManager.Instance.AddInciteAmount(neededUtility);
        }
    }

    public override bool IsDone()
    {
        return true;
    }

    public override float Utility()
    {
        Debug.Log("Incite utility");
        float currentUtilty = incited.Utility();
        float neededUtility = currentUtilty * 1.1f;
        if (! inciter.HasEnoughCash(neededUtility) || incited.arrested) {
            return -10;
        }
        Debug.LogFormat("Needed utility: {0}, min bribe {1}", neededUtility, guard.MinAmountBribery);
        return guard.MinAmountBribery - neededUtility;
    }
}