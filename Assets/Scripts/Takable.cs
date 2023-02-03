using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Chiligames.MetaAvatarsFusion;

/// <summary>
/// Unlike grabbable, takable is a place where you can keep taking grabbable away. Currently it only support tmp takable
/// </summary>
public class Takable : MonoBehaviour
{
    [SerializeField]
    OVRGrabbableWithEvents ipfb_grabbableToTake;

    OVRGrabbableWithEvents currentGrabbableToTake; 
    public OVRGrabbableWithEvents CurrentGrabbableToTake { get { return currentGrabbableToTake; } }

    [SerializeField]
    protected bool destroyWhenGrabFinished = true;

    [SerializeField]
    protected bool placeNewAfterGrabFinished = true;

    bool taking = false; public bool Taking { get { return taking; } }

    // Start is called before the first frame update
    void Start()
    {
        ipfb_grabbableToTake.gameObject.SetActive(false); // hide the internal prefab
        PlaceGrabbableToTake();
    }

    void PlaceGrabbableToTake()
    {
        currentGrabbableToTake = Instantiate(ipfb_grabbableToTake, ipfb_grabbableToTake.transform.parent);
        currentGrabbableToTake.gameObject.SetActive(true);
        currentGrabbableToTake.transform.position = ipfb_grabbableToTake.transform.position;
        currentGrabbableToTake.transform.rotation = ipfb_grabbableToTake.transform.rotation;

        currentGrabbableToTake.OnGrabBegin += () =>
        {
            taking = true;
            currentGrabbableToTake.transform.SetParent(null);
        };

        OVRGrabbableWithEvents _prevGrabbableToTake = currentGrabbableToTake;
        currentGrabbableToTake.OnGrabEnd += () =>
        {
            taking = false;
            if (placeNewAfterGrabFinished)
            {
                PlaceGrabbableToTake();
            }
            if (destroyWhenGrabFinished)
            {
                Destroy(_prevGrabbableToTake.gameObject);
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
