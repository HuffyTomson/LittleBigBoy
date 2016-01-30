using UnityEngine;
using System.Collections;
using Huffy.Utilities;

public class GameManager : SingletonBehaviour<GameManager>
{
    public int id = 0;
    private StateMachine<GameManager> sm;

    public Transform hand;

    IEnumerator Start ()
    {
        DontDestroy();
        yield return null;
        sm = new StateMachine<GameManager>(new TestStateOne(this));
    }

    void Update()
    {
        if (sm != null)
            sm.Update();
    }
    
}
