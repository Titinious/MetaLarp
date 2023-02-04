using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using Chiligames.MetaAvatarsFusion;
using Oculus.Avatar2;

public class RoleplayAvatar : MyComponent
{
    // Start is called before the first frame update
    [SerializeField]
    OvrAvatarEntity avatar;

    [SerializeField]
    GameObject[] hats; // for player, hat is the player order. For NPC, hat is always the first hat

    GameObject hat;

    bool dressed = false;

    protected override void _set(Dictionary<string, object> args = null)
    {
        base._set(args);

        avatar.OnSkeletonLoadedEvent.AddListener((_) =>
        {
            Dress();

            if (avatar is FusionMetaAvatar)
            {
                FusionMetaAvatar _avatar = (FusionMetaAvatar) avatar;
                if (_avatar.isMe)
                {
                    Object.FindObjectOfType<Locomotion>().StartMove();
                }
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

        if(avatar is FusionMetaAvatar)
        {
            FusionMetaAvatar _avatar = (FusionMetaAvatar)avatar;

            hat = Instantiate(hats[_avatar.characterId], headTm);
            hat.transform.localPosition = new Vector3(0.12f, 0, 0);
            hat.transform.localEulerAngles = new Vector3(-90, 0, -90);

            if (_avatar.isMe)
                hat.layer = LayerMask.NameToLayer("MyIgnore"); // I will ignore my own hat
        }
        else if(avatar is NPCAvatarEntity)
        {
            hat = Instantiate(hats[0], headTm); 
            //hat.transform.localPosition = new Vector3(0.12f, 0, 0);
            hat.transform.localEulerAngles = new Vector3(-90, 0, -90);
        }

    }
}
