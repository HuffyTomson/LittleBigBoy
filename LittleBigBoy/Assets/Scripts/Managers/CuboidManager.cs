using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Huffy.Utilities;

[System.Serializable]
public class Bounds
{
    public float maxX = 0;
    public float minX = 0;

    public float maxY = 0;
    public float minY = 0;

    public float maxZ = 0;
    public float minZ = 0;

    public Vector3 RandomBetweenBounds()
    {
        return new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
    }
}

public class CuboidManager : SingletonBehaviour<CuboidManager>
{
    public Cuboid fishPrefab;
    public Bounds startBounds = new Bounds();
    
    public List<Cuboid> cubeoidList = new List<Cuboid>();

    void Awake()
    {
        List<FishData> fishList = XMLManager.Instance.GetFish();
        foreach (FishData f in fishList)
        {
            Cuboid fish = GameObject.Instantiate(fishPrefab);
            fish.transform.position = startBounds.RandomBetweenBounds();
            fish.data = f;
            AddToList(fish);
        }
        
    }

    public static void AddToList(Cuboid _cuboid)
    {
        Instance.cubeoidList.Add(_cuboid);
    }

    public static void RemoveFromList(Cuboid _cuboid)
    {
        Instance.cubeoidList.Remove(_cuboid);
    }
}
