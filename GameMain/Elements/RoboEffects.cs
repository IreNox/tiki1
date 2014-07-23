using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using TikiEngine.Elements.Particle;
using TikiEngine.Elements.Physic;

namespace TikiEngine.Elements
{
    internal partial class Robo
    {
        #region Vars
        private ParticleSystem _currentSystem;

        private systemRunDust _systemRunDust;
        private systemRunSpark _systemRunSpark;

        private Dictionary<Material, ParticleSystem> _userdataSystem;

        private Vector2 _offset = new Vector2(0, 1.3f);
        #endregion

        #region Init
        private void _initEffects()
        {
            _systemRunDust = new systemRunDust()
            { 
                IsAlive = false,
                LayerDepth = 0.59f
            };

            _systemRunSpark = new systemRunSpark()
            { 
                IsAlive = false,
                LayerDepth = 0.59f
            };
            
            _userdataSystem = new Dictionary<Material, ParticleSystem>()
            {
                { Material.island, _systemRunDust },
                { Material.wood, _systemRunDust },
                { Material.metal, _systemRunDust },
                { Material.block, _systemRunDust },
                { Material.slide, _systemRunSpark }
            };

            //circle.Body.OnCollision += new OnCollisionEventHandler(RoboRun_OnCollision);
            //circle.Body.OnSeparation += new OnSeparationEventHandler(RoboRun_OnSeparation);
        }
        #endregion

        #region Member
        private void _drawEffects(GameTime gameTime)
        {
            _systemRunDust.Draw(gameTime);
            _systemRunSpark.Draw(gameTime);
        }

        private void _updateEffects(GameTime gameTime)
        {
            float speed = Math.Abs(body.LinearVelocity.X);

            if (!this.Left() && !this.Right() && this.currMaterial != Material.slide) speed = 0;

            if (_currentSystem != null)
            {
                _currentSystem.CurrentPosition = this.CurrentPosition + _offset;
                _currentSystem.Update(gameTime);
            }

            _systemRunDust.Speed = speed;
            _systemRunSpark.Speed = speed;

            _systemRunDust.Update(gameTime);
            _systemRunSpark.Update(gameTime);

            Material material = this.currMaterial;

            if (_userdataSystem.ContainsKey(material))
            {
                if (this.onGround)
                {
                    _currentSystem = _userdataSystem[material];

                    _systemRunDust.IsAlive = (_systemRunDust == _currentSystem);
                    _systemRunSpark.IsAlive = (_systemRunSpark == _currentSystem);
                }
                else
                {
                    if (_currentSystem != null)
                    {
                        _currentSystem.IsAlive = false;
                    }

                    _currentSystem = null;
                }
            }


            
        }
        #endregion

        #region Member - EventHandler
        private bool RoboRun_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            Material material = ((NameObjectPhysic)fixtureB.Body.UserData).Material;

            if (_userdataSystem.ContainsKey(material))
            {
                _currentSystem = _userdataSystem[material];

                _systemRunDust.IsAlive = (_systemRunDust == _currentSystem);
                _systemRunSpark.IsAlive = (_systemRunSpark == _currentSystem);
            }

            return true;
        }

        private void RoboRun_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            Material material = ((NameObjectPhysic)fixtureB.Body.UserData).Material;

            if (_userdataSystem.ContainsKey(material))
            {
                if (_currentSystem != null)
                {
                    _currentSystem.IsAlive = false;
                }

                _currentSystem = null;
            }
        }
        #endregion
    }
}
