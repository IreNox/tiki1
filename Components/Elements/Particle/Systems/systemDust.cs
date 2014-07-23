using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Particle
{
    public class systemDust : ParticleSystem
    {
        #region Vars
        private float _width;
        private float _force;

        private bool _effectStarted = true;
        private double _effectLifeTime = 0;
        private double _effectStartTime = 0;
        #endregion
        
        #region Init
        public systemDust()
        {
            this.AddEffect<effectCloud>().LayerDepth = 0.20f;
            this.AddEffect<effectParticle>().LayerDepth = 0.21f;
        }
        #endregion

        #region Private Member
        private void _calcForce()
        {
            this.GetEffect<effectCloud>().Width = _width;
            this.GetEffect<effectParticle>().Width = _width;

            this.GetEffect<effectCloud>().ReleaseQuantity = (int)((_width / 5) + (_force / 4));
            this.GetEffect<effectParticle>().ReleaseQuantity = (int)((_width / 5) + (_force / 4));

            _effectLifeTime = (_force / 9);
        }
        #endregion

        #region Member
        public override void Update(GameTime gameTime)
        {
            if (_effectStarted && _effectStartTime == 0)
            {
                _effectStartTime = gameTime.TotalGameTime.TotalSeconds;
            }
            else if (_effectStarted)
            {
                if (_effectStartTime + (_force / 9) < gameTime.TotalGameTime.TotalSeconds)
                {
                    this.IsAlive = false;
                }
            }

            base.Update(gameTime);
        }
        #endregion

        #region Properties
        public float Width
        {
            get { return _width; }
            set
            {
                _width = value;
                _calcForce();
            }
        }

        public float Force
        {
            get { return _force; }
            set
            {
                _force = value;
                _calcForce();
            }
        }

        public override bool IsAlive
        {
            get { return base.IsAlive; }
            set
            {
                base.IsAlive = value;
                _effectStarted = value;

                if (!_effectStarted)
                {
                    _effectStartTime = 0;
                }
            }
        }

        public double LifeTime
        {
            get { return _effectLifeTime; }
            set { _effectLifeTime = value; }
        }
        #endregion
    }
}
