using UnityEngine;
using System.Collections;

public class FishData
{
    public int id;
    public string name;
    public float value;

    public FishData(string _name, float _value)
    {
        name = _name;
        value = _value;
    }

    public FishData(string _data)
    {
        string[] line = _data.Split(',');
        if(line.Length > 1)
        {
            name = line[0];
            value = float.Parse(line[1]);
        }
    }

}
