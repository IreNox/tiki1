using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Particle
{
    public class systemDestroy : ParticleSystem
    {
        #region Vars
        private float _angle;
        private Vector2 _angleVector;

        private float _velocity = 1;

        private float _distance;
        #endregion

        #region Init
        public systemDestroy()
        {
            this.AddEffect<effectStar>().LayerDepth = 0.99f;
            this.AddEffect<effectTail2>().LayerDepth = 0.96f;
            this.AddEffect<effectAbsorb>().LayerDepth = 0.98f;
            this.AddEffect<effectCenterPoint>().LayerDepth = 0.97f;
        }
        #endregion

        #region Member
        public void Start(Vector2 pos, float angle)
        {
            this.StartPosition = pos;
            this.GetEffect<effectTail2>().LastPosition = pos;

            this.Angle = angle;
            _distance = 0;

            this.IsAlive = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (this.IsAlive)
            {
                _distance += (float)(_velocity * gameTime.ElapsedGameTime.TotalSeconds);
                this.CurrentPosition = positionStart + (_angleVector * _distance);

                if (_distance > 20)
                {
                    this.IsAlive = false;
                }
            }

            base.Update(gameTime);
        }
        #endregion

        #region Properties
        public float Angle
        {
            get { return _angle; }
            set
            {
                _angle = value;
                _angleVector = Functions.AngleToVector(value);
            }
        }

        public float Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }
        
        public float Distance
        {
            get { return _distance; }
        }
        #endregion
    }
}
