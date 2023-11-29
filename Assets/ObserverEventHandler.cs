/*==============================================================================
Copyright (c) 2021 PTC Inc. All Rights Reserved.

Confidential and Proprietary - Protected under copyright and other laws.
Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.
==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Vuforia;

public class ObserverEventHandler : DefaultObserverEventHandler
{

    int t = 0;
    protected override void HandleTargetStatusChanged(Status previousStatus, Status newStatus)
    {
        var shouldBeRendererBefore = ShouldBeRendered(previousStatus);
        var shouldBeRendererNow = ShouldBeRendered(newStatus);
        if (shouldBeRendererBefore != shouldBeRendererNow)
        {
            if (shouldBeRendererNow)
            {
                OnTrackingFound();


            }
            else
            {
                OnTrackingLost();
            }
        }
        else
        {
            if (!mCallbackReceivedOnce && !shouldBeRendererNow)
            {
                // This is the first time we are receiving this callback, and the target is not visible yet.
                // --> Hide the augmentation.
                OnTrackingLost();
            }
        }

        mCallbackReceivedOnce = true;
    }

    protected override void OnTrackingFound()
    {

        if (mObserverBehaviour)
        {
            var rendererComponents = VuforiaRuntimeUtilities.GetComponentsInChildrenExcluding<Renderer, DefaultObserverEventHandler>(mObserverBehaviour.gameObject);
            var colliderComponents = VuforiaRuntimeUtilities.GetComponentsInChildrenExcluding<Collider, DefaultObserverEventHandler>(mObserverBehaviour.gameObject);
            var canvasComponents = VuforiaRuntimeUtilities.GetComponentsInChildrenExcluding<Canvas, DefaultObserverEventHandler>(mObserverBehaviour.gameObject);
            var item = 0;

            // Enable colliders:
            foreach (var component in colliderComponents)
                component.enabled = true;

            // Enable canvas':
            foreach (var component in canvasComponents)
                component.enabled = true;

            // Enable rendering:
            foreach (var component in rendererComponents)
            {

                if (item == t)
                {
                    component.enabled = true;
  
                }
                else
                {
                    component.enabled = false;
                }
                item++;
                if (item == 3){
                    item = 0;
                }
         
            }
            t++;
            if (t == 3)
            {
                t = 0;
            }
            

            
        }

        OnTargetFound?.Invoke();
    }

    protected override void OnTrackingLost()
    {
        if (mObserverBehaviour)
        {
            var rendererComponents = VuforiaRuntimeUtilities.GetComponentsInChildrenExcluding<Renderer, DefaultObserverEventHandler>(mObserverBehaviour.gameObject);
            var colliderComponents = VuforiaRuntimeUtilities.GetComponentsInChildrenExcluding<Collider, DefaultObserverEventHandler>(mObserverBehaviour.gameObject);
            var canvasComponents = VuforiaRuntimeUtilities.GetComponentsInChildrenExcluding<Canvas, DefaultObserverEventHandler>(mObserverBehaviour.gameObject);

            // Disable rendering:
            foreach (var component in rendererComponents)
                component.enabled = false;

            // Disable colliders:
            foreach (var component in colliderComponents)
                component.enabled = false;

            // Disable canvas':
            foreach (var component in canvasComponents)
                component.enabled = false;
        }

        OnTargetLost?.Invoke();
    }

}