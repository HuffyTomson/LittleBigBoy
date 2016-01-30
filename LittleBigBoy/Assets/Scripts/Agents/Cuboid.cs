using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Cuboid : MonoBehaviour
{  
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


//public class Vehicle
//{
//    private SteeringBehaviors steering;
//
//    public Stats stats;
//
//    private Vector3 forward;
//    private Vector3 right;
//    private Vector3 velocity;
//    private Vector3 position;
//
//    public Vector3 Forward { get { return forward; } }
//    public Vector3 Right { get { return right; } }
//    public Vector3 Velocity { get { return velocity; } }
//    public Vector3 Position { get { return position; } }
//
//    public Vehicle(Stats _stats, Vector3 _position)
//    {
//        stats = _stats;
//        position = _position;
//        steering = new SteeringBehaviors(this);
//    }
//
//    public void Update(float _timeElapsed)
//    {
//        // add all forces together
//        Vector3 steeringForce = steering.Calculate();
//
//        //Acceleration = Force/Mass
//        Vector3 acceleration = steeringForce / stats.Mass;
//
//        //update velocity
//        velocity += acceleration * _timeElapsed;
//
//        //make sure vehicle does not exceed maximum velocity
//        velocity = Vector3.ClampMagnitude(velocity, stats.Speed);
//
//        //update the position
//        position += velocity * _timeElapsed;
//
//        //update the heading if the vehicle has a velocity greater than a very small
//        //value
//        if (velocity.sqrMagnitude > 0.0000001)
//        {
//            forward = Vector3.Normalize(velocity);
//            right = Vector3.Cross(Vector3.up, forward).normalized;
//        }
//    }
//}
//
//public class SteeringBehaviors
//{
//    // calling Vehicle
//    private Vehicle vehicle;
//    
//    private float maxForce = 10;
//    
//    public SteeringBehaviors(Vehicle _vehicle)
//    {
//        vehicle = _vehicle;
//    }
//
//    public Vector3 Calculate()
//    {
//        Vector3 force = Vector3.zero;
//
//        //force += 
//        force += Wander();
//        force = Vector3.ClampMagnitude(force, maxForce);
//        return force;
//    }
//
//    Vector3 Seek(Vector3 _targetPos)
//    {
//        Vector3 desiredVelocity = Vector3.Normalize(_targetPos - vehicle.Position) * vehicle.stats.Speed;
//        return desiredVelocity - vehicle.Velocity;
//    }
//
//    Vector3 Flee(Vector3 _targetPos)
//    {
//        float minFleeDistance = 15;
//        if (Vector3.Distance(vehicle.Position, _targetPos) > minFleeDistance)
//        {
//            return Vector3.zero;
//        }
//
//        Vector3 desiredVelocity = Vector3.Normalize(vehicle.Position - _targetPos) * vehicle.stats.Speed;
//        return desiredVelocity - vehicle.Velocity;
//    }
//
//    enum DECELERATION
//    {
//        fast = 1,
//        normal = 2,
//        slow = 3
//    }
//
//    Vector3 Arrive(Vector3 _targetPos, DECELERATION _arriveSpeed)
//    {
//        Vector3 toTarget = _targetPos - vehicle.Position;
//        //calculate the distance to the target position
//        float dist = toTarget.magnitude;
//        if (dist > 0)
//        {
//            //because Deceleration is enumerated as an int, this value is required
//            //to provide fine tweaking of the deceleration.
//            float decelerationTweaker = 0.3f;
//            //calculate the speed required to reach the target given the desired
//            //deceleration
//            float speed = dist / ((float)_arriveSpeed * decelerationTweaker);
//            //make sure the velocity does not exceed the max
//            speed = Mathf.Min(speed, vehicle.stats.Speed);
//            //from here proceed just like Seek except we don't need to normalize
//            //the ToTarget vector because we have already gone to the trouble
//            //of calculating its length: dist.
//            Vector3 desiredVelocity = toTarget * speed / dist;
//            return (desiredVelocity - vehicle.Velocity);
//        }
//        return Vector3.zero;
//    }
//
//    Vector3 Pursuit(Vehicle _evader)
//    {
//        //if the evader is ahead and facing the agent then we can just seek
//        //for the evader's current position.
//        Vector3 toEvader = _evader.Position - vehicle.Position;
//
//        float relativeHeading = Vector3.Dot(vehicle.Forward, _evader.Forward);
//        if ((Vector3.Dot(toEvader, vehicle.Forward) > 0) &&
//        (relativeHeading < -0.95)) //acos(0.95)=18 degs
//        {
//            return Seek(_evader.Position);
//        }
//
//        //Not considered ahead so we predict where the evader will be.
//        //the look-ahead time is proportional to the distance between the evader
//        //and the pursuer; and is inversely proportional to the sum of the
//        //agents' velocities
//        float lookAheadTime = toEvader.magnitude / (vehicle.stats.Speed + _evader.Velocity.magnitude);
//
//        // used when we want to have a max turning speed
//        //lookAheadTime += TurnAroundTime(vehicle, evader.Position);
//
//        //now seek to the predicted future position of the evader
//        return Seek(_evader.Position + _evader.Velocity * lookAheadTime);
//    }
//
//    Vector3 Evade(Vehicle _pursuer)
//    {
//        /* Not necessary to include the check for facing direction this time */
//        Vector3 toPursuer = _pursuer.Position -vehicle.Position;
//        //the look-ahead time is proportional to the distance between the pursuer
//        //and the evader; and is inversely proportional to the sum of the
//        //agents' velocities
//        float lookAheadTime = toPursuer.magnitude / (vehicle.stats.Speed + _pursuer.Velocity.magnitude);
//        //now flee away from predicted future position of the pursuer
//        return Flee(_pursuer.Position + _pursuer.Velocity * lookAheadTime);
//    }
//
//
//    float wanderAmt = 10;
//    Vector3 wanderVec = Vector3.zero;
//
//    Vector3 Wander()
//    {
//        wanderVec *= 0.75f;
//        wanderVec += Random.onUnitSphere * 0.5f;
//        wanderVec = wanderVec.normalized;
//        Vector3 wanderForce = wanderVec;
//
//        return wanderForce.normalized * vehicle.stats.Speed;
//    }
//
//    //float wanderRadius;
//    //float wanderDistance;
//    //float wanderJitter;
//    //Vector3 wanderTarget;
//    //
//    //Vector3 Wander()
//    //{
//    //    //first, add a small random vector to the target’s position (RandomClamped
//    //    //returns a value between -1 and 1)
//    //    wanderTarget += new Vector3(Random.value * wanderJitter, Random.value * wanderJitter, Random.value * wanderJitter);
//    //    wanderTarget = wanderTarget.normalized;
//    //
//    //    //increase the length of the vector to the same as the radius
//    //    //of the wander circle
//    //    wanderTarget *= wanderRadius;
//    //
//    //    //move the target into a position WanderDist in front of the agent
//    //    Vector3 targetLocal = wanderTarget + new Vector3(wanderDistance, 0,0);
//    //    //project the target into world space
//    //    Vector3 targetWorld = PointToWorldSpace(targetLocal,
//    //    vehicle.Forward,
//    //    vehicle.Right,
//    //    vehicle.Position);
//    //    //and steer toward it
//    //    return targetWorld - vehicle.Position;
//    //}
//
//    // used when we want to have a max turning speed
//    // used in Persuit
//    //float TurnaroundTime(Vehicle _vehicle, Vector3 _targetPos)
//    //{
//    //    //determine the normalized vector to the target
//    //    Vector3 toTarget = Vector3.Normalize(_targetPos - _vehicle.Position);
//    //    float dot = Vector3.Dot(_vehicle.Forward,toTarget);
//    //    //change this value to get the desired behavior. The higher the max turn
//    //    //rate of the vehicle, the higher this value should be. If the vehicle is
//    //    //heading in the opposite direction to its target position then a value
//    //    //of 0.5 means that this function will return a time of 1 second for the
//    //    //vehicle to turn around.
//    //    float coefficient = 0.5f;
//    //    //the dot product gives a value of 1 if the target is directly ahead and -1
//    //    //if it is directly behind. Subtracting 1 and multiplying by the negative of
//    //    //the coefficient gives a positive value proportional to the rotational
//    //    //displacement of the vehicle and target.
//    //    return (dot - 1.0f) * -coefficient;
//    //}
//}