using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TikiEngine.Elements.Graphics;

namespace TikiEngine.Elements.Effects
{
    public class ShaderEffect : NameObjectTextured
    {
        #region Vars
        private bool _isAlive = false;

        public readonly Effect EffectXna;
        #endregion

        #region Init
        public ShaderEffect(string effectFile)
        {
            this.EffectXna = GI.Content.Load<Effect>(effectFile);

            this.UpdateMatrices();
        }

        public ShaderEffect(Effect effect)
        {
            this.EffectXna = effect;

            this.UpdateMatrices();
        }
        #endregion

        #region Member
        public Effect SetMatrices(Matrix worldMatrix)
        {
            if (this.EffectXna is BasicEffect)
            {
                ((BasicEffect)this.EffectXna).World = worldMatrix;
            }
            else
            {
                this.EffectXna.Parameters["World"].SetValue(worldMatrix);
            }

            this.UpdateMatrices();

            return this.EffectXna;
        }

        public void UpdateMatrices()
        {
            if (this.EffectXna is BasicEffect)
            {
                BasicEffect effect = (BasicEffect)this.EffectXna;

                effect.View = GI.Camera.ViewMatrix;
                effect.Projection = GI.Camera.ProjectionMatrix;
            }
            else
            {
                this.EffectXna.Parameters["View"].SetValue(GI.Camera.ViewMatrix);
                this.EffectXna.Parameters["Projection"].SetValue(GI.Camera.ProjectionMatrix);
            }
        }

        public sealed override void Draw(GameTime gameTime)
        {
            throw new NotSupportedException();
        }

        public sealed override void Update(GameTime gameTime)
        {
            if (_isAlive)
            {
                this.UpdateInternal(gameTime);
            }
        }

        public override void Dispose()
        {
            if (this.EffectXna != null)
            {
                this.EffectXna.Dispose();
            }
        }
        #endregion

        #region Member - Protected
        protected override void SetTexture(Texture2D texture)
        {
            base.SetTexture(texture);

            if (this.EffectXna.Parameters["Texture"] != null)
            {
                this.EffectXna.Parameters["Texture"].SetValue(texture);

                if (this.EffectXna.Parameters["TextureSize"] != null)
                {
                    this.EffectXna.Parameters["TextureSize"].SetValue(this.TextureSize);
                }
            }
        }

        protected override void ApplyChanges()
        {
        }

        protected virtual void UpdateInternal(GameTime gameTime)
        {
        }
        #endregion

        #region Member - Get/Set - Protected
        protected object getValue(string key)
        {
            if (this.EffectXna == null) return null;

            var p = this.EffectXna.Parameters[key];

            switch (p.ParameterType)
            {
                case EffectParameterType.Bool:
                    return p.GetValueBoolean();
                case EffectParameterType.Int32:
                    return p.GetValueInt32();
                case EffectParameterType.Single:
                    switch (p.ColumnCount)
                    {
                        case 1:
                            return p.GetValueSingle();
                        case 2:
                            return p.GetValueVector2();
                        case 3:
                            return p.GetValueVector3();
                        case 4:
                            return p.GetValueVector4();
                    }
                    break;
                case EffectParameterType.String:
                    return p.GetValueString();
                case EffectParameterType.Texture:
                case EffectParameterType.Texture1D:
                case EffectParameterType.Texture2D:
                    return p.GetValueTexture2D();
                case EffectParameterType.Texture3D:
                    return p.GetValueTexture3D();
                case EffectParameterType.TextureCube:
                    return p.GetValueTextureCube();
            }

            return null;
        }

        protected void setValue(string key, object value)
        {
            if (this.EffectXna == null) return;

            var p = this.EffectXna.Parameters[key];

            switch (p.ParameterType)
            {
                case EffectParameterType.Bool:
                    p.SetValue((bool)value);
                    break;
                case EffectParameterType.Int32:
                    p.SetValue((int)value);
                    break;
                case EffectParameterType.Single:
                    switch (p.ColumnCount)
                    {
                        case 1:
                            p.SetValue((float)value);
                            break;
                        case 2:
                            p.SetValue((Vector2)value);
                            break;
                        case 3:
                            p.SetValue((Vector3)value);
                            break;
                        case 4:
                            p.SetValue((Vector4)value);
                            break;
                    }
                    break;
                case EffectParameterType.String:
                    p.SetValue((string)value);
                    break;
                case EffectParameterType.Texture:
                    p.SetValue((Texture2D)value);
                    break;
                case EffectParameterType.Texture1D:
                    p.SetValue((Texture2D)value);
                    break;
                case EffectParameterType.Texture2D:
                    p.SetValue((Texture2D)value);
                    break;
                case EffectParameterType.Texture3D:
                    p.SetValue((Texture3D)value);
                    break;
                case EffectParameterType.TextureCube:
                    p.SetValue((TextureCube)value);
                    break;
            }
        }
        #endregion

        #region Properties
        public object this[string key]
        {
            get { return getValue(key); }
            set { setValue(key, value); }
        }

        public bool IsAlive
        {
            get { return _isAlive; }
            set { _isAlive = value; }
        }

        new public Vector2 TextureSize
        {
            get { return (Vector2)getValue("TextureSize"); }
            set { setValue("TextureSize", value); }
        }

        public override bool Ready
        {
            get { return this.EffectXna != null; }
        }
        #endregion
    }
}
