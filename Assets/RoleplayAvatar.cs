using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using Chiligames.MetaAvatarsFusion;

public class RoleplayAvatar : MyComponent
{
    // Start is called before the first frame update
    [SerializeField]
    FusionMetaAvatar avatar;

    [SerializeField]
    GameObject[] hats;

    GameObject hat;

    bool dressed = false;

    protected override void _set(Dictionary<string, object> args = null)
    {
        base._set(args);

        avatar.OnSkeletonLoadedEvent.AddListener((_) =>
        {
            Dress();

            if(avatar.characterId == NetworkRunner.GetRunnerForGameObject(gameObject).LocalPlayer.PlayerId)
            {
                Object.FindObjectOfType<Locomotion>().StartMove();
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (dressed)
        {
            //Debug.Log(avatar.GetSkeletonTransformByType(Oculus.Avatar2.CAPI.ovrAvatar2JointType.Head));
        }
    }

    public void Dress()
    {
        dressed = true;

        Transform headTm = avatar.GetSkeletonTransformByType(Oculus.Avatar2.CAPI.ovrAvatar2JointType.Head);

        hat = Instantiate(hats[avatar.characterId], headTm);
        hat.transform.localPosition = new Vector3(0.12f, 0, 0);
        hat.transform.localEulerAngles = new Vector3(-90, 0, -90);

        hat.layer = LayerMask.NameToLayer("MyIgnore"); // I will ignore my own hat


    }
}
