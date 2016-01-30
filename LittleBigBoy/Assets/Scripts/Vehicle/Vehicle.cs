using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vehicle
{
    private GameObject obj;
    public GameObject Obj { get { return obj; } }

    public Rigidbody rig;

    [HideInInspector]
    public  Vector3 velocity;
    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Vector3 heading;
    [HideInInspector]
    public  Transform target;
    [HideInInspector]
    public Vector3 steeringTarget;
    [HideInInspector]
    // holds vehicels near this one
    public List<GameObject> neighbors;
    // holds a target other than the player that leads the vehical 
    public GameObject leader;
    // hold where you want to look
    private Vector3 lookPos;

    // public inspector //
    // forces
    public float maxSpeed = 0.5f;
    public float acceleration = 100.0f;
    public float turnRate = 3.0f;
    
    // follower
    public Vector3 followOffset = new Vector3(0,-1,0);
    
    // SteeringBehavior fields //
    // wander
    public float wanderRadius   = 1.2f;
    public float wanderDistance = 1.0f;
    public float wanderJitter   = 40.0f;
    
    // steering weights
    public float seekWeight       = 1.0f;
    public float fleeWeight       = 1.0f;
    public float arriveWeight     = 1.0f;
    public float pursuitWeight    = 1.0f;
    public float evadeWeight      = 1.0f;
    public float wanderWeight     = 1.0f;
    public float hideWeight       = 1.0f;
    public float separationWeight = 1.0f;
    public float cohesionWeight   = 1.0f;
    public float alignmentWeight  = 1.0f;
    public float followWeight     = 1.0f;
    // ...
    // ...
    // ...
    
    //the steering behavior class
    public SteeringBehavior steeringBehavior;
    
    public Vehicle(GameObject _thisObj)
    {
        obj = _thisObj;
        rig = obj.GetComponent<Rigidbody>();
        steeringBehavior = new SteeringBehavior(this);
    }
    
    public void Update () 
    {
        // hold current velocity, postion, and heading for use in steeringBehavior
        velocity = rig.velocity;
        position = Obj.transform.position;
        heading = Obj.transform.forward;
        // debug facing
        Debug.DrawLine(Obj.transform.position, Obj.transform.position + heading * 2);
        // turn to face movment vector
        FaceHeading();
        // accelerate to target
        Vector2 steeringForce = steeringBehavior.GetSteeringForce();
        if (steeringForce.magnitude > 0.1f)
        {
            steeringTarget = new Vector2(steeringForce.x, steeringForce.y).normalized;
            rig.AddForce(steeringTarget * acceleration * Time.deltaTime);
            // clamp speed
            rig.velocity = Vector3.ClampMagnitude(rig.velocity, maxSpeed);
        }
	 }
     
     void SetTargetToPlayer()
     {
         target = (GameObject.Find("player") as GameObject).transform;
     }
     
     // rotate to face target
     void FaceHeading()
     {
        //Vector3 newLookPos = (new Vector3(rig.velocity.x, rig.velocity.y, 0) + Obj.transform.position) - Obj.transform.position;
        //lookPos = Vector3.Lerp(lookPos, newLookPos, Time.deltaTime * turnRate);
        //float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
        //Obj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        Obj.transform.rotation = Quaternion.Lerp(Obj.transform.rotation, Quaternion.LookRotation(rig.velocity), Time.deltaTime * turnRate);
     }
      
}
