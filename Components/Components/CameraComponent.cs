using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TikiEngine.Components
{
    public class CameraComponent : GameComponent
    {
        #region Vars
        private Matrix _view;
        private Matrix _view2D;
        private Matrix _projection;

        private Vector2 offsetSpeed;
        private Vector2 trackingSpeed;
        private Vector2 targetOffset;
        private Vector2 trackingOffset;

        private Body _trackingBody;
        private Vector2 _trackingPosition;

        private Vector2 _currentPosition;

        private Vector2 _screenSize;
        private Vector2 _screenCenter;

        private float _zoom = 1.0f;
        private float targetZoom = 1.0f;

        #endregion

        #region Init
        internal CameraComponent()
            : base(GI.Game)
        {
            _view = Matrix.Identity;
            _view2D = Matrix.Identity;

            this.ResetConfig();
            this.ResetScreen();
            this.ResetPosition();
        }
        #endregion

        #region Member - Reset
        public void ResetConfig()
        {
            offsetSpeed = new Vector2(TikiConfig.CameraOffsetSpeedX, TikiConfig.CameraOffsetSpeedY);
            trackingSpeed = new Vector2(TikiConfig.CameraTrackingSpeedX, TikiConfig.CameraTrackingSpeedY);

            targetOffset = new Vector2(TikiConfig.CameraTrackingOffsetX, TikiConfig.CameraTrackingOffsetY);
            trackingOffset = new Vector2(TikiConfig.CameraTrackingOffsetX, TikiConfig.CameraTrackingOffsetY);

            targetOffset = Vector2.Zero;
            trackingOffset = Vector2.Zero;
        }

        public void ResetScreen()
        {
            _projection = Matrix.CreateOrthographicOffCenter(
                0,
                ConvertUnits.ToSimUnits(GI.Device.Viewport.Width) / _zoom,
                ConvertUnits.ToSimUnits(GI.Device.Viewport.Height) / _zoom,
                0,
                0f,
                1f
            );

            _screenSize = new Vector2(GI.Device.Viewport.Width, GI.Device.Viewport.Height);
            _screenCenter = _screenSize / 2;
        }

        public void ResetPosition()
        {
            this.CurrentPosition = new Vector2(
                GI.Device.Viewport.Width / 2,
                GI.Device.Viewport.Height / 2
            );
        }
        #endregion

        #region Member - Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_zoom != targetZoom)
            {
                _zoom += (targetZoom - _zoom) * 0.05f;
                this.ResetScreen();
            }

            if (_trackingBody != null)
            {
                trackingOffset += new Vector2(
                    (this.targetOffset.X - this.trackingOffset.X) * offsetSpeed.X,
                    (this.targetOffset.Y - this.trackingOffset.Y) * offsetSpeed.Y
                );
                
                Vector2 tmp = _screenSize / _zoom;
                tmp *= trackingOffset / -100;

                _trackingPosition += new Vector2(
                    (TrackingBody.Position.X - _trackingPosition.X) * trackingSpeed.X,
                    (TrackingBody.Position.Y - _trackingPosition.Y) * trackingSpeed.Y
                );

                _currentPosition = (ConvertUnits.ToDisplayUnits(_trackingPosition) * -1) + _screenCenter / _zoom + tmp;
            }

            _view2D = Matrix.CreateTranslation(
                _currentPosition.X,
                _currentPosition.Y,
                0
            ) * Matrix.CreateScale(_zoom);

            Vector2 pos = ConvertUnits.ToSimUnits(_currentPosition) * -1;

            _view = Matrix.CreateLookAt(
                new Vector3(pos, 1.0f),
                new Vector3(pos, 0.0f),
                Vector3.Up
            );

            pos = -_currentPosition;
        }
        #endregion

        #region Member - Calculate
        public Point GetPoint(Vector2 pos)
        {
            pos = ConvertUnits.ToDisplayUnits(pos);

            return new Point(
                (int)pos.X,
                (int)pos.Y
            );
        }

        public Point GetScreenPoint(Vector2 pos)
        {
            pos = ConvertUnits.ToDisplayUnits(pos);

            return new Point(
                (int)(pos.X + this.CurrentPositionNagativ.X),
                (int)(pos.Y + this.CurrentPositionNagativ.Y)
            );
        }
        #endregion

        #region Properties - Tracking
        public Body TrackingBody
        {
            get { return _trackingBody; }
            set
            {
                _trackingBody = value;
                if (_trackingBody != null) _trackingPosition = _trackingBody.Position;
            }
        }

        public Vector2 TrackingPosition
        {
            get { return this._trackingPosition; }
        }

        public Vector2 TrackingSpeed
        {
            get { return trackingSpeed; }
            set { trackingSpeed = value; }
        }

        public Vector2 Offset
        {
            get { return this.trackingOffset; }
            set { this.targetOffset = value; }
        }

        public Vector2 TargetOffset
        {
            get { return this.targetOffset; }

        }

        public Vector2 OffsetSpeed
        {
            get { return this.offsetSpeed; }
            set { this.offsetSpeed = value; }
        }
        #endregion

        #region Properties - Matrix
        public Matrix ViewMatrix
        {
            get { return _view; }
        }

        public Matrix ViewMatrix2D
        {
            get { return _view2D; }
        }

        public Matrix ProjectionMatrix
        {
            get
            {
                return _projection; 
            }
        }
        #endregion

        #region Properties - Position
        public Vector2 CurrentPosition
        {
            get { return _currentPosition; }
            set { _currentPosition = value; }
        }

        public Vector2 CurrentPositionNagativ
        {
            get { return _currentPosition * -1; }
        }

        public Vector2 CurrentPositionCenter
        {
            get { return this.CurrentPositionNagativ + (_screenSize / _zoom) - this.ScreenCenter; }
        }
        #endregion

        #region Properties - View
        public float Zoom
        {
            get { return targetZoom; }
            set { targetZoom = value; }
        }

        public float RealZoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                targetZoom = value;
            }
        }

        public Vector2 ScreenSize
        {
            get { return _screenSize; }
        }

        public Vector2 ScreenCenter
        {
            get { return _screenCenter / _zoom; }
        }

        public Rectangle ViewDisplayRectangle
        {
            get
            {
                return new Rectangle(
                    (int)(_currentPosition.X),
                    (int)(_currentPosition.Y),
                    GI.Device.Viewport.Width,
                    GI.Device.Viewport.Height
                );
            }
        }

        public Vector4 ViewSimVector
        {
            get
            {
                return new Vector4(
                    -_currentPosition.X,
                    -_currentPosition.Y,
                    GI.Device.Viewport.Width / _zoom,
                    GI.Device.Viewport.Height / _zoom
                ) / ConvertUnits.DisplayUnitsToSimUnitsRatio;
            }
        }
        #endregion
    }
}
