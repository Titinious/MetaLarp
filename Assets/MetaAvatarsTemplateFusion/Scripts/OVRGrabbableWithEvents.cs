using UnityEngine;
using System;

namespace Chiligames.MetaAvatarsFusion
{
    public class OVRGrabbableWithEvents : OVRGrabbable
    {
        public event Action OnGrabBegin;
        public event Action OnGrabEnd;

        public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
        {
            m_grabbedBy = hand;
            m_grabbedCollider = grabPoint;
            
            OnGrabBegin?.Invoke();
        }

        public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            rb.velocity = linearVelocity;
            rb.angularVelocity = angularVelocity;
            m_grabbedBy = null;
            m_grabbedCollider = null; 
            
            OnGrabEnd?.Invoke();
        }
    }
}

