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
    float worldProgress = 0;

    // Use this for initialization
    void Awake ()
    {
        Load();
        // TODO:
        // update fish stats
        // 
        //
        Save();
        Debug.Log(Application.persistentDataPath + "\\Save.txt");
        DontDestroy();
    }

    public void Load()
    {
        fish.Clear();

        if (File.Exists(Application.persistentDataPath + "\\Save.txt"))
        {
            string[] lines = File.ReadAllLines(Application.persistentDataPath + "\\Save.txt");
            //foreach (string s in lines)
            //{
            //    Debug.Log(s);
            //}

            if(lines.Length > 2)
            {
                string[] globalData = lines[0].Split('|');

                // date //
                string[] dateArray = globalData[0].Split(',');
                lastDateTime = new DateTime(int.Parse(dateArray[0]), // yyyy
                                            int.Parse(dateArray[1]), // MM
                                            int.Parse(dateArray[2]), // dd
                                            int.Parse(dateArray[3]), // HH
                                            int.Parse(dateArray[4]), // mm
                                            int.Parse(dateArray[5]));// ss
                
                timeElapsed = (float)DateTime.Now.Subtract(lastDateTime).TotalMinutes;
                worldProgress = float.Parse(lines[1]);
                //worldProgress += timeElapsed;

                Debug.Log("Last Time: " + lastDateTime);
                Debug.Log("Now: " + DateTime.Now);
                Debug.Log("timeElapsed: " + timeElapsed);

                // next fish id
                if (globalData.Length > 1)
                {
                    int nextID = int.Parse(globalData[1]);
                }

                for (int i = 2; i < lines.Length; i++)
                {
                    fish.Add(new FishData(lines[i]));
                }
            }
        }
        else
        {
            for (int i = 0; i < 50; i++)
            {
                fish.Add(new FishData("Fish" + i.ToString(),0));
            }
        }
    }

    public void Save()
    {
        List<string> lines = new List<string>();
        lines.Add(DateTime.Now.ToString("yyyy,MM,dd,HH,mm,ss"));
        lines.Add(worldProgress.ToString());
        foreach(FishData f in fish)
        {
            lines.Add(f.name + "," + f.value.ToString());
        }
        File.WriteAllLines(Application.persistentDataPath + "\\Save.txt", lines.ToArray());
    }

    public void DeleteSave()
    {
        if (File.Exists(Application.persistentDataPath + "\\Save.txt"))
        {
            File.Delete(Application.persistentDataPath + "\\Save.txt");
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
