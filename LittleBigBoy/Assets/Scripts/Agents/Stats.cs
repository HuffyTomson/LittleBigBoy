
// stats of Cuboid

public class Stats
{
    public Stats(Stats _s)
    {
        health = _s.health;
        mass = _s.mass;
        speed = _s.speed;
        turnSpeed = _s.turnSpeed;
        strength = _s.strength;
        metabolism = _s.metabolism;
    }

    public Stats(float _health, float _mass, float _speed, float _turnSpeed, float _strength, float _metabolism)
    {
        health = _health;
        mass = _mass;
        speed = _speed;
        turnSpeed = _turnSpeed;
        strength = _strength;
        metabolism = _metabolism;
    }

    private float health;
    private float mass;
    private float speed;
    private float turnSpeed;
    private float strength;
    private float metabolism;

    public float Health { get { return health; } }
    public float Mass { get { return mass; } }
    public float Speed { get { return speed; } }
    public float TurnSpeed { get { return turnSpeed; } }
    public float Strength { get { return strength; } }
    public float Metabolism { get { return metabolism; } }
}
