﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
#if IOS || MACCATALYST
using Foundation;
#endif
using Microsoft.Maui.ApplicationModel;

namespace CFAN.SchoolMap.Maui.GoogleMaps.Bindings
{
#if IOS || MACCATALYST
    [Preserve(AllMembers = true)]
#endif
    public sealed class MoveCameraBehavior : BehaviorBase<Map>
    {
        public static readonly BindableProperty RequestProperty = BindableProperty.Create("Request", typeof(MoveCameraRequest), typeof(MoveCameraBehavior), null, propertyChanged: OnRequestChanged);

        private static void OnRequestChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((MoveCameraBehavior)bindable).OnRequestChanged(oldValue as MoveCameraRequest, newValue as MoveCameraRequest);
        }

        private void OnRequestChanged(MoveCameraRequest? oldValue, MoveCameraRequest? newValue)
        {
            if (oldValue != null)
            {
                oldValue.MoveCameraBehavior = null;
            }
            if (newValue != null)
            {
                newValue.MoveCameraBehavior = this;
            }
        }

        public Task<AnimationStatus> MoveCamera(CameraUpdate cameraUpdate)
        {
            return AssociatedObject.MoveCamera(cameraUpdate);
        }
    }
}
