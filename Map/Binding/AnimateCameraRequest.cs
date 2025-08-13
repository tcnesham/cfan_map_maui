﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CFAN.SchoolMap.Maui.GoogleMaps.Bindings
{
    public sealed class AnimateCameraRequest
    {
        internal AnimateCameraBehavior AnimateCameraBehavior { get; set; }
        public Task<AnimationStatus> AnimateCamera(CameraUpdate cameraUpdate, TimeSpan? duration = null)
        {
            if (AnimateCameraBehavior == null) throw new InvalidOperationException("Not binding to AnimateCameraBehavior.");

            return AnimateCameraBehavior.AnimateCamera(cameraUpdate, duration);
        }
    }
}
