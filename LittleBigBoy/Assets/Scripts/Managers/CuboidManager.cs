using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Huffy.Utilities;

public class CuboidManager : SingletonBehaviour<CuboidManager>
{
    public List<Cuboid> cubeoidList = new List<Cuboid>();

    public static void AddToList(Cuboid _cuboid)
    {
        Instance.cubeoidList.Add(_cuboid);
    }

    public static void RemoveFromList(Cuboid _cuboid)
    {
        Instance.cubeoidList.Remove(_cuboid);
    }
}
