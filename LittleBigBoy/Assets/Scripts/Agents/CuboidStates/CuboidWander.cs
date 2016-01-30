using UnityEngine;
using System.Collections;


public class CuboidWander : State<Cuboid>
{

    public CuboidWander(Cuboid _owner) : base(_owner)
    {
    }

    public override void Enter()
    {
        owner.vehicle.maxSpeed = 10;

        GameObject t = new GameObject();
        t.transform.position = new Vector3(5, 5, 5);
        owner.vehicle.target = t.transform;

        owner.vehicle.steeringBehavior.arrive = true;
        owner.vehicle.arriveWeight = 0.25f;

        owner.vehicle.steeringBehavior.wander = true;
        owner.vehicle.wanderWeight = 1f;
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            sm.ChangeState(new CuboidIdle(owner));
        }
    }
    
    void OnDrawGizmosSelected()
    {
    }
}

