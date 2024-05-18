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
        }
    }

    public override bool IsDone()
    {
        return true;
    }

    public override float Utility()
    {
        if (! inciter.HasGuardInfo()) return -10;
        return 10; 
    }
}