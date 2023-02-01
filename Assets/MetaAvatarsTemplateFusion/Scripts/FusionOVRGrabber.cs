using System;
using UnityEngine;

namespace Chiligames.MetaAvatarsFusion
{
    public class FusionOVRGrabber : OVRGrabber
    {
        private OVRHand trackingHand;
        private float pinchThreshold = 0.7f;

        protected override void Awake()
        {
            base.Awake();
            //Try to find a OVRHand (handtracking) component
            if(GetComponent<OVRHand>() != null)
            {
                trackingHand = GetComponent<OVRHand>();
            }
        }

        public override void Update()
        {
            base.Update();
            //If hand tracking component detected, check pinch for grabbing mechanism.
            if (trackingHand != null)
            {
                CheckPinch();
            }
        }

        //If the pinch strenght is bigger than the threshold, call GrabBegin(), if smaller, call GrabEnd().
        private void CheckPinch()
        {
            float pinchStrenght = trackingHand.GetFingerPinchStrength(OVRHand.HandFinger.Index);

            if(!m_grabbedObj && pinchStrenght > pinchThreshold && m_grabCandidates.Count > 0)
            {
                GrabBegin();
            }
            else if (pinchStrenght < pinchThreshold)
            {
                GrabEnd();
            }
        }
    }
}
