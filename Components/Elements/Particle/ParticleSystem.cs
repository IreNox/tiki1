using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using TikiEngine.Elements.Graphics;

namespace TikiEngine.Elements.Particle
{
    [Serializable]
    public abstract class ParticleSystem : NameObjectGraphics
    {
        #region Vars
        private bool _isAlive = true;

        private List<ParticleEffect> _listEffects = new List<ParticleEffect>();
        #endregion

        #region Init
        public ParticleSystem()
        {
            this.LayerDepth = 0.2f;
        }

        public ParticleSystem(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { 
        }
        #endregion

        #region Member
        public T AddEffect<T>()
            where T : ParticleEffect, new()
        {
            T effect = new T();
            effect.IsAlive = true;
            effect.LayerDepth = layerDepth;

            _listEffects.Add(effect);

            return effect;
        }

        public T GetEffect<T>()
            where T : ParticleEffect
        {
            return _listEffects.OfType<T>().FirstOrDefault();
        }

        protected override void ApplyChanges()
        {
        }

        public override void Dispose()
        {
            foreach (ParticleEffect effect in _listEffects)
            {
                effect.IsAlive = false;
                effect.Dispose();
            }
            _listEffects.Clear();
        }
        #endregion

        #region Member - Xna
        public override void Draw(GameTime gameTime)
        {
            foreach (ParticleEffect effect in _listEffects)
            {
                effect.Draw(gameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (ParticleEffect effect in _listEffects)
            {
                effect.Update(gameTime);
            }
        }
        #endregion

        #region Properties
        public virtual bool IsAlive
        {
            get { return _isAlive; }
            set
            {
                _isAlive = value;

                foreach (ParticleEffect effect in _listEffects)
                {
                    effect.IsAlive = value;
                }
            }
        }

        public override bool Ready
        {
            get { return true; }
        }

        protected List<ParticleEffect> Effects
        {
            get { return _listEffects; }
        }

        public override Vector2 CurrentPosition
        {
            get { return base.CurrentPosition; }
            set
            {
                base.CurrentPosition = value;

                foreach (ParticleEffect effect in _listEffects)
                {
                    effect.CurrentPosition = value;
                }
            }
        }

        public override SpriteBatchType SpriteBatchType
        {
            get { return base.SpriteBatchType; }
            set
            {
                base.SpriteBatchType = value;

                foreach (ParticleEffect effect in _listEffects)
                {
                    effect.SpriteBatchType = value;
                }
            }
        }

        public override float LayerDepth
        {
            get { return base.LayerDepth; }
            set
            {
                base.LayerDepth = value;

                foreach (ParticleEffect e in _listEffects)
                {
                    e.LayerDepth = value;
                }
            }
        }
        #endregion
    }
}
