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
        t.transform.position = new Vector3(Random.Range(-5.0f, 5.0f), 0, Random.Range(-5.0f, 5.0f));
        owner.vehicle.target = t.transform;

        owner.vehicle.steeringBehavior.arrive = true;
        owner.vehicle.arriveWeight = 0.25f;

        owner.vehicle.steeringBehavior.wander = true;
        owner.vehicle.wanderWeight = 1f;

        owner.vehicle.evadeTransform = GameManager.Instance.hand;
        owner.vehicle.steeringBehavior.evade = true;
        owner.vehicle.evadeWeight = 1;
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

