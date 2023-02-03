using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Can only pull of z-axis
/// </summary>
public class PullContraption : MyComponent
{
    [SerializeField]
    Takable takable;

    [SerializeField]
    Transform anchor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (takable.Taking)
        {
            //takable.CurrentGrabbableToTake

            anchor.up = (takable.CurrentGrabbableToTake.transform.position - anchor.transform.position).normalized;
            anchor.localEulerAngles = new Vector3(anchor.localEulerAngles.x, 0, 0);

        }
    }
}
