using Fusion;
using System.Collections;
using UnityEngine;

namespace Chiligames.MetaAvatarsFusion
{
    public class GrabbableNetworkBehaviour : NetworkBehaviour
    {
        private OVRGrabbableWithEvents _grabbable;
        private Rigidbody _rigidBody;
        //This is a Networked value which is synchronized in the network, when the value changes, OnKinematicChanged is called on every client.
        [Networked(OnChanged = nameof(OnKinematicChanged))]
        private bool Kinematic { get; set; }
        private bool _wasKinematic;
        private bool _spawned;

        //When the Kinematic value changes (i.e when an user grabs the object), it is updated for everyone in the network
        public static void OnKinematicChanged(Changed<GrabbableNetworkBehaviour> changed)
        {
            changed.Behaviour._rigidBody.isKinematic = changed.Behaviour.Kinematic;
        }

        private void Awake()
        {
            _grabbable = GetComponent<OVRGrabbableWithEvents>();
            _rigidBody = GetComponent<Rigidbody>();
            _wasKinematic = _rigidBody.isKinematic;
        }

        private void Start()
        {
            _grabbable.OnGrabBegin += Grabbable_OnGrabBegin;
            _grabbable.OnGrabEnd += Grabbable_OnGrabEnd;
        }

        private void Grabbable_OnGrabBegin()
        {
            //If we don't have State Authority over grabbed object, request it.
            if (!Object.HasStateAuthority)
            {
                Object.RequestStateAuthority();
                StartCoroutine(WaitForAuthority());
            }
            //Else, we can directly set the Kinematic State
            else
            {
                Kinematic = true;
            }
        }

        //We wait for the State authority to be ours (not instant) and then set the [Networked] Kinematic value
        IEnumerator WaitForAuthority()
        {
            while (!Object.HasStateAuthority)
            {
                yield return null;
            }
            Kinematic = true;
        }

        public override void Spawned()
        {
            _spawned = true;
        }

        //Also change State Authority when a grabbable is touched by our grabbed object to be able to push
        private void OnCollisionEnter(Collision collision)
        {
            if (!_spawned) return;
            if (Kinematic) return;
            var collidingNetBehaviour = collision.gameObject.GetComponent<GrabbableNetworkBehaviour>();
            if (collidingNetBehaviour)
            {
                if (collidingNetBehaviour.Object.HasStateAuthority && collidingNetBehaviour.Kinematic)
                {
                    Object.RequestStateAuthority();
                }
            }
        }

        private void Grabbable_OnGrabEnd()
        {
            Kinematic = _wasKinematic;
        }

        private void OnDestroy()
        {
            _grabbable.OnGrabBegin -= Grabbable_OnGrabBegin;
            _grabbable.OnGrabEnd -= Grabbable_OnGrabEnd;
        }
    }
}
