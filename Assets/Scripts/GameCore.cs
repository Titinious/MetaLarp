using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore : MySingletonComponent<GameCore>
{
    protected override void _set(Dictionary<string, object> args = null)
    {
        base._set(args);

        UnityEngine.Random.InitState(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
