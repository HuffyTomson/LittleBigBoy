using UnityEngine;
using System.Collections;
using Huffy.Utilities;
using System.Collections.Generic;
using System;
using System.IO;

public class XMLManager : SingletonBehaviour<XMLManager>
{
    public List<FishData> fish = new List<FishData>();

    DateTime dateTime;
    DateTime lastDateTime;

    float timeElapsed = 0;

    // Use this for initialization
    void Start ()
    {
        Load();
        // update fish value
        foreach(FishData f in fish)
        {
            f.value *= UnityEngine.Random.Range(0.99f, 1.02f);
            f.value += timeElapsed * UnityEngine.Random.Range(0.9f, 1.15f);
        }
        Save();
        Debug.Log(Application.persistentDataPath + "Save.txt");
        DontDestroy();
    }

    public void Load()
    {
        fish.Clear();

        if (File.Exists(Application.persistentDataPath + "Save.txt"))
        {
            string[] lines = File.ReadAllLines(Application.persistentDataPath + "Save.txt");
            foreach (string s in lines)
            {
                Debug.Log(s);
            }

            if(lines.Length > 0)
            {
                string[] dateArray = lines[0].Split(',');
                lastDateTime = new DateTime(int.Parse(dateArray[0]), // yyyy
                                            int.Parse(dateArray[1]), // MM
                                            int.Parse(dateArray[2]), // dd
                                            int.Parse(dateArray[3]), // hh
                                            int.Parse(dateArray[4]), // mm
                                            int.Parse(dateArray[5]));// ss

                timeElapsed = (float)DateTime.Now.Subtract(lastDateTime).TotalHours;

                Debug.Log(lastDateTime);
                Debug.Log(DateTime.Now);
                Debug.Log("timeElapsed: " + timeElapsed);
                
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] line = lines[i].Split(',');
                    fish.Add(new FishData(line[0], float.Parse(line[1])));
                }
            }
        }
    }

    public void Save()
    {
        List<string> lines = new List<string>();
        lines.Add(DateTime.Now.ToString("yyyy,MM,dd,hh,mm,ss"));
        foreach(FishData f in fish)
        {
            lines.Add(f.name + "," + f.value.ToString());
        }
        File.WriteAllLines(Application.persistentDataPath + "Save.txt", lines.ToArray());
    }

    public void DeleteSave()
    {
        if (File.Exists(Application.persistentDataPath + "Save.txt"))
        {
            File.Delete(Application.persistentDataPath + "Save.txt");
        }
    }

    public float GetTimeElapsed()
    {
        return timeElapsed;
    }

    public List<FishData> GetFish()
    {
        if(fish.Count == 0)
        {
            Load();
        }

        return fish;
    }

    public void UpdateFish(List<FishData> _fish)
    {
        fish = _fish;
    }
}
