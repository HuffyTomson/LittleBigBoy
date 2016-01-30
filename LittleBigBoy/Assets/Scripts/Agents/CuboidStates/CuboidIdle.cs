using UnityEngine;
using System.Collections;

public class CuboidIdle : State<Cuboid>
{
    public CuboidIdle(Cuboid _owner) : base(_owner)
    {
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            sm.ChangeState(new CuboidWander(owner));
        }
    }
}

