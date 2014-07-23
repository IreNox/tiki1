using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TikiEngine.Elements.Graphics;

namespace TikiEngine.Elements.Particle
{
    [Serializable]
    public class ParticleEffect : NameObjectTextured, IAttachable
    {
        #region Vars
        private bool _isAlive = true;
        private float _triggerTime;
        private float _triggerElapsed;

        private float _totalTime;

        private Vector2 _origin;
        private Vector2 _offset;

        private bool _offsetPosition;
        private float _lifeTime = 1.0f;        
        private int _releaseQuantity = 10;

        private int _count = 0;
        private int _budget = 1000;
        private Particle[] _particles = new Particle[1000];
        
        protected ParticleBatch particleBatch;

        public readonly BlendState BlendState;
        #endregion
        
        #region Init
        public ParticleEffect()
            : this(BlendState.NonPremultiplied)
        {
        }

        public ParticleEffect(BlendState blendState)
        {
            this.BlendState = blendState;

            _init();
        }

        public ParticleEffect(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _init();
        }

        private void _init()
        {
            particleBatch = GI.ParticleBatch;
        }
        #endregion

        #region Private Member
        private unsafe void _retireParticles(Particle* particleArray, int count)
        {
            int num = _count - count;
            Particle* src = (particleArray + count);
            Particle* dst = particleArray;

            for (int i = 0; i < num; i++)
            {
                *dst = *src;

                src++;
                dst++;
            }

            _count -= count;
        }
        #endregion

        #region Protected Member
        protected virtual unsafe void CreateParticle(Particle* particle)
        {
        }

        protected virtual unsafe void UpdateParticle(float elapsed, Particle* particle)
        {
            particle->Position += (particle->Velocity * elapsed);
        }
        #endregion

        #region Member
        protected override void ApplyChanges()
        {
        }

        public override void Dispose()
        {
            this.IsAlive = false;
            _particles = new Particle[0];
        }

        protected override void SetTexture(Texture2D texture)
        {
            base.SetTexture(texture);

            _origin = this.TextureSize / 2;
        }
        #endregion

        #region Member - Trigger
        public void Trigger()
        {
            unsafe
            {
                int oldCount = _count;

                for (int i = oldCount; i < oldCount + _releaseQuantity; i++)
                {
                    if (i >= _budget) break;

                    fixed (Particle* particle = &_particles[i])
                    {
                        particle->Scale = 1.0f;
                        particle->Color = Vector4.One;
                        particle->BirthTime = _totalTime;
                        particle->Velocity = Vector2.Zero;
                        particle->Position = positionCurrent;

                        this.CreateParticle(particle);

                        particle->PositionStart = particle->Position;
                    }

                    _count++;
                }
            }
        }
        #endregion

        #region Member - Xna - Draw
        public override void Draw(GameTime gameTime)
        {
            particleBatch.Draw(
                _particles,
                _count,
                _origin - ConvertUnits.ToDisplayUnits(_offset),
                this
            );
        }
        #endregion

        #region Member - Xna - Update
        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _totalTime += elapsed;
            _triggerElapsed += elapsed;
       
            if (_isAlive && _triggerElapsed > _triggerTime)
            {
                _triggerElapsed -= _triggerTime;

                this.Trigger();
            }

            unsafe
            {
                fixed (Particle* particles = _particles)
                {
                    int i = _count;
                    float minTime = _totalTime - _lifeTime;
                    Particle* particle = particles + (i - 1);

                    while (--i >= 0)
                    {
                        if (particle->BirthTime < minTime) break;

                        particle->Age = (_totalTime - particle->BirthTime) / _lifeTime;

                        this.UpdateParticle(elapsed, particle);

                        particle--;
                    }

                    if (i >= 0)
                    {
                        _retireParticles(particles, i + 1);
                    }
                }
            }
        }
        #endregion

        #region Properties
        public virtual bool IsAlive
        {
            get { return _isAlive; }
            set
            {
                if (_isAlive != value) _triggerElapsed = 0;

                _isAlive = value;                
            }
        }

        public bool OffsetPosition
        {
            get { return _offsetPosition; }
            set { _offsetPosition = value; }
        }

        public int Budget
        {
            get { return _budget; }
            set
            {
                _budget = value;
                _particles = new Particle[value];
            }
        }

        public float TotalTime
        {
            get { return _totalTime; }
        }

        public float Angle
        {
            get { return 0; }
            set { return; }
        }

        public Vector2 Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        public float LifeTime
        {
            get { return _lifeTime; }
            set { _lifeTime = value; }
        }

        public float TriggerTime
        {
            get { return _triggerTime; }
            set { _triggerTime = value; }
        }

        public int ReleaseQuantity
        {
            get { return _releaseQuantity; }
            set { _releaseQuantity = value; }
        }

        public override bool Ready
        {
            get { return this.Texture != null; }
        }
        #endregion
    }
}
