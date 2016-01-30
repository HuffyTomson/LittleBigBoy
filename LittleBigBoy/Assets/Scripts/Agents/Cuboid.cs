using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Cuboid : MonoBehaviour
{
    public FishData data;
    private StateMachine<Cuboid> sm;
    public Vehicle vehicle;
    
    void OnEnable()
    {
        CuboidManager.AddToList(this);
    }

    void OnDisable()
    {
        CuboidManager.RemoveFromList(this);
    }

    void Awake ()
    {
        vehicle = new Vehicle(this.gameObject);
        sm = new StateMachine<Cuboid>(new CuboidWander(this));
	}
	
	void FixedUpdate ()
    {
        if (sm != null)
        {
            sm.Update();
            vehicle.Update();
        }
    }
}
