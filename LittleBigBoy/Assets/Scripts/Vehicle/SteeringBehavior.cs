using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SteeringBehavior
{

    // calling Vehicle
    private Vehicle vehicle;
    
    // hold wander target
    Vector3 wanderTarget;
    
    // steering types
    public bool seek;
    public bool flee;
    public bool arrive;
    public bool follow;
    public bool pursuit;
    public bool evade;
    public bool wander;
    public bool hide;
    public bool separation;
    public bool cohesion;
    public bool alignment;
    // ...
    // ...
    // ...
    
    public void ClearSteeringBehavior()
    {
        seek = false;
        flee = false;
        arrive = false;
        follow = false;
        pursuit = false;
        evade = false;
        wander = false;
        hide = false;
        separation = false;
        cohesion = false;
        alignment = false;
        // ...
        // ...
        // ...
    }
    
    public SteeringBehavior(Vehicle v)
    {
        vehicle = v;
       
        // get random postion on wander circle
        float theta = Random.Range(0.0f,1.0f) * (Mathf.PI*2);
        wanderTarget = new Vector3(vehicle.wanderRadius * Mathf.Cos(theta), vehicle.wanderRadius * Mathf.Sign(theta));
    }
    
    // seek //
    public Vector3 Seek(Vector3 target)
    {
        return ((Vector3.Normalize(target - vehicle.position) * vehicle.maxSpeed) - vehicle.velocity);
    }
    
    // flee //
    public Vector3 Flee(Vector3 target)
    {
        return ((Vector3.Normalize(vehicle.position - target) * vehicle.maxSpeed) - vehicle.velocity);
    }
    
    // arrive //
    public Vector3 Arrive(Vector3 target)
    {
        Vector3 toTarget = target - vehicle.position;
        float dist = Vector3.Distance(target, vehicle.position);
        
        if (dist > 0.5f)
        {
            float speed = dist / vehicle.rig.drag;
            
            if (speed > vehicle.maxSpeed)
               speed = vehicle.maxSpeed;
            
            Vector3 desiredVelocity = toTarget * speed / dist;
            
            return desiredVelocity - vehicle.velocity;
        }
        
        return Vector3.zero;
    }
    
    // Follow //
    public Vector3 Follow( Transform leader, Vector3 offset)
    {
        //return Arrive(leader.position + leader.worldToLocalMatrix.MultiplyVector(offset));
        return Arrive(leader.position + leader.right * offset.x + leader.up * offset.y);
    }
    
    // pursuit //
    public Vector3 Pursuit(Transform evader)
    {
        float distToTarget = Vector3.Distance(evader.position, vehicle.position);
        float lookAhead = distToTarget / (vehicle.maxSpeed + evader.GetComponent<Rigidbody>().velocity.magnitude);
        Vector3 targetVelocity = new Vector3(evader.GetComponent<Rigidbody2D>().velocity.x, evader.GetComponent<Rigidbody>().velocity.y, 0);
        return Seek(evader.position + targetVelocity * lookAhead);
    }
    
    // evade //
    public Vector3 Evade(Transform pursuer)
    {
        float distToTarget = Vector3.Distance(pursuer.position, vehicle.position);
        float lookAhead = distToTarget / (vehicle.maxSpeed + pursuer.GetComponent<Rigidbody>().velocity.magnitude);
        Vector3 targetVelocity = new Vector3(pursuer.GetComponent<Rigidbody>().velocity.x, pursuer.GetComponent<Rigidbody>().velocity.y, 0);
        return Flee(pursuer.position + targetVelocity * lookAhead);
    }
    
    // wander //
    public Vector3 Wander()
    {
        float jitterOverTime = vehicle.wanderJitter * Time.deltaTime;
        
        wanderTarget += new Vector3(Random.Range(-jitterOverTime, jitterOverTime),
                                    Random.Range(-jitterOverTime, jitterOverTime), 
                                    0);
        
        wanderTarget = Vector3.Normalize(wanderTarget);
        wanderTarget *= vehicle.wanderRadius;
        
        Vector3 target = wanderTarget + vehicle.Obj.transform.forward * vehicle.wanderDistance;
        target += vehicle.position;
        
        return Vector3.Normalize(target - vehicle.position);
    }
    
    // get hiding spot //
    private Vector3 GetHidingSpot(Vector3 obstaclePos, float radius, Vector3 hunterPos)
    {
        float distAway = radius + 2;
        Vector3 toObstacle = Vector3.Normalize(obstaclePos - hunterPos);
        return toObstacle * distAway + obstaclePos;
    }
    // hide //
    public Vector3 Hide(Transform hunter)
    {
        // need to remove hide or put a list of obstacles in a game manager
        return Vector3.zero;
        /*
        float distToClosest = float.MaxValue;
        Vector3 bestHidingSpot = Vector3.zero;
        
        for (int i = 0; i < GameManager.Instance().obstacles.Count; ++i)
        {
           float radius = 0;
        
           if (GameManager.Instance().obstacles[i].GetComponent<CircleCollider2D>())
              radius = GameManager.Instance().obstacles[i].GetComponent<CircleCollider2D>().transform.localScale.magnitude;
           else if (GameManager.Instance().obstacles[i].GetComponent<BoxCollider2D>())
              radius = GameManager.Instance().obstacles[i].GetComponent<BoxCollider2D>().transform.localScale.magnitude;
        
           Vector3 hisingSpot = GetHidingSpot(GameManager.Instance().obstacles[i].transform.position, radius, hunter.position);
        
           float dist = Vector3.SqrMagnitude(hisingSpot - vehicle.position);
        
           if (dist < distToClosest)
           {
              distToClosest = dist;
              bestHidingSpot = hisingSpot;
           }
        }
        
        if (distToClosest == float.MaxValue)
        {
           return Evade(hunter);
        }
        else
        {
           return Arrive(bestHidingSpot);
        }
        //*/
        
    }
    
    // Separation //
    public Vector3 Separation(List<GameObject> neighbors)
    {
        Vector3 steeringForce = Vector3.zero;
        
        for (int i = 0; i < neighbors.Count; ++i)
        {
            Vector3 toAgent = vehicle.position - neighbors[i].transform.position;
            steeringForce += toAgent.normalized / toAgent.magnitude;
        }
        
        return steeringForce;
    }
    
    // Cohesion //
    public Vector3 Cohesion(List<GameObject> neighbors)
    {
        Vector3 steeringForce = Vector3.zero;
        Vector3 centerMass = Vector3.zero;
        
        for (int i = 0; i < neighbors.Count; ++i)
        {
            centerMass += neighbors[i].transform.position;
        }
        
        if (neighbors.Count != 0)
        {
            centerMass /= neighbors.Count;
            steeringForce = Seek(centerMass).normalized;
        }
        
        return steeringForce;
    }
    
    // alignment //
    public Vector3 Alignment(List<GameObject> neighbors)
    {
        Vector3 averageHeading = Vector3.zero;
        
        for (int i = 0; i < neighbors.Count; ++i)
        {
            averageHeading += neighbors[i].transform.right;
        }
        
        if (neighbors.Count != 0)
        {
            averageHeading /= neighbors.Count;
            averageHeading -= vehicle.heading;
        }
        
        return averageHeading;
    }
    // ...
    // ...
    // ...
    
    // tag neighbors
    public void TagNeghbors(GameObject thisObj, List<GameObject> listOfObjects, float radius)
    {
        vehicle.neighbors.Clear();
        
        radius *= radius;
        
        for (int i = 0; i < listOfObjects.Count; ++i)
        {
            Vector3 to = listOfObjects[i].transform.position - thisObj.transform.position;
        
            if (to.magnitude < radius && listOfObjects[i] != thisObj)
            {
                vehicle.neighbors.Add(listOfObjects[i]);
            }
        }
    }
    public void AddNeghbor(GameObject neghbor)
    {
        if (neghbor != null)
            vehicle.neighbors.Add(neghbor);
        else
            Debug.Log("Failed to add leader to list of neghbor in: " + vehicle.Obj.name);
    }
    public void RemoveNeghbor(GameObject neghbor)
    {
        if(neghbor != null)
            vehicle.neighbors.Remove(neghbor);
        else
            Debug.Log("Failed to remove leader to list of neghbor in: " + vehicle.Obj.name);
    }
    
    // returns a normlised vector will all active steering behaviors added together by there weight
    public Vector3 GetSteeringForce()
    {
        Vector3 steeringTarget = Vector3.zero;
        
        if (seek)
            steeringTarget += Seek(vehicle.target.position) * vehicle.seekWeight;
        
        if (flee)
            steeringTarget += Flee(vehicle.target.position) * vehicle.fleeWeight;
        
        if (arrive)
            steeringTarget += Arrive(vehicle.target.position) * vehicle.arriveWeight;
        
        if (follow)
            steeringTarget += Follow(vehicle.leader.transform, vehicle.followOffset);
        
        if (pursuit)
            steeringTarget += Pursuit(vehicle.target) * vehicle.pursuitWeight;
        
        if (evade)
            steeringTarget += Evade(vehicle.target) * vehicle.evadeWeight;
        
        if (wander)
            steeringTarget += Wander() * vehicle.wanderWeight;
        
        if (hide)
            steeringTarget += Hide(vehicle.target) * vehicle.hideWeight;
        
        if (separation)
            steeringTarget += Separation(vehicle.neighbors) * vehicle.separationWeight;
        
        if (cohesion)
            steeringTarget += Cohesion(vehicle.neighbors) * vehicle.cohesionWeight;
        
        if (alignment)
            steeringTarget += Alignment(vehicle.neighbors) * vehicle.alignmentWeight;
        // ...
        // ...
        // ...
        
        if (steeringTarget.magnitude < 0.0001f)
            return Vector3.zero;
        else
            return Vector3.ClampMagnitude(steeringTarget, 1);
            //return steeringTarget.normalized;
    }
}
