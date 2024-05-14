using UnityEngine;

public class MoveTo : Action // Action used to move between rooms
{
    private Vector3 target;
    private Prisoner agent;
    private Room room;

    public MoveTo(Vector3 target, Room room, Prisoner agent)
    {
        this.target = target;
        this.room = room;
        this.agent = agent;
    }

    public override void Execute()
    {
        agent.MoveTo(target);
        agent.targetRoom = room.roomNumber;
    }

    public override bool IsDone()
    {
        return Vector3.Distance(agent.transform.position, target) < 1;
    }

    public override float Utility()
    {
        if (agent.GuardInRoom(room.roomNumber))
            return -10;
        return room.Utility;
    }

    public override string ToString()
    {
        return base.ToString() + " to " + room.name;
    }
}