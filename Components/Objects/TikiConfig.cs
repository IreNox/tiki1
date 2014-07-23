using System;
using System.Runtime.Serialization;
using TikiEngine.Elements;

namespace TikiEngine
{
    public static class TikiConfig
    {
        #region Vars - Config
        public static float MoveMaxSpeed = 8.0f;

        public static float BuildBlockSize = 2.0f;
        public static float StoneCube = 2.5f;
        public static float BuildMaxDistance = 5.5f;
        #endregion

        #region Vars - Camera
        public static float CameraTrackingSpeedX = 1f;
        public static float CameraTrackingSpeedY = 0.1f;

        public static float CameraOffsetSpeedX = 0.1f;
        public static float CameraOffsetSpeedY = 0.1f;

        public static float CameraTrackingOffsetX = 3f;
        public static float CameraTrackingOffsetY = 1f; 
        #endregion
    }
}
