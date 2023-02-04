using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class AvatarWalker : MyComponent
{
    [SerializeField]
    protected Transform[] pathPoints;

    [SerializeField]
    protected bool pingPongPath = true;

    [SerializeField]
    float walkSpeed = 2.5f;

    [SerializeField]
    float walkVerticalMotion = 0.2f;

    int targetPathIdx = 0;

    [SerializeField]
    Transform avatarHolder;

    [SerializeField]
    float walkVerticalMotionSpeed = 0.8f;

    protected override void _set(Dictionary<string, object> args = null)
    {
        base._set(args);

        if (pingPongPath) // duplicate the path at the end but reverse to get back to the starting point
        {
            List<Transform> reversePathPoints = new List<Transform>(pathPoints);
            reversePathPoints.RemoveAt(0);
            reversePathPoints.RemoveAt(reversePathPoints.Count-1);
            reversePathPoints.Reverse();
            pathPoints = pathPoints.Concat(reversePathPoints).ToArray();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        targetPathIdx += 1;
        transform.position = pathPoints[0].position;
        transform.rotation = pathPoints[0].rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // walking
        Vector3 targetPt = pathPoints[targetPathIdx].position;

        float eps = 0.1f;

        Vector3 targetDir = (targetPt - transform.position).normalized;

        if(Vector3.Dot(targetDir, transform.forward) < -0.9f)
        {
            transform.forward = targetDir;
        }

        transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(transform.forward), Quaternion.LookRotation(targetDir), 1f * Time.deltaTime);
        transform.position += transform.forward * Time.deltaTime * walkSpeed;

        if((targetPt - transform.position).sqrMagnitude < eps * eps)
        {
            targetPathIdx += 1;
            if(targetPathIdx >= pathPoints.Length)
            {
                targetPathIdx = 0;
            }
        }

        // walking vertical motion
        float verticalMotion = Mathf.Sin(Time.time * walkVerticalMotionSpeed);
        verticalMotion = verticalMotion * verticalMotion * walkVerticalMotion;
        avatarHolder.localPosition = new Vector3(avatarHolder.localPosition.x, verticalMotion, avatarHolder.localPosition.z);

    }
}
