public class Incite : Action
{
    // Action used when a prisoner incites other to go to be caught by a guard, allowing the inciter to escape

    private Prisoner inciter;
    private Prisoner incited;

    public Incite(Prisoner inciter, Prisoner incited)
    {
        this.inciter = inciter;
        this.incited = incited;
    }

    public override void Execute()
    {
        inciter.Incite(incited);
    }

    public override bool IsDone()
    {
        return true;
    }

    public override float Utility()
    {
        if (inciter.HasGuardInfo()) return -10;
        return 10;
    }
}