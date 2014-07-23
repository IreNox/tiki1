using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikiEngine.Elements.Effects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TikiEngine
{
    public class PostProcessingBatch : BasicBatch<PostProcessingBatch.PostProcessingDrawInfo>
    {
        #region Vars
        private RenderTarget2D _renderTarget;
        #endregion

        #region Init
        public PostProcessingBatch(GraphicsDevice device)
            : base(device)
        {
            this.Reset();
        }
        #endregion

        #region Member
        public override void Reset()
        {
            if (_renderTarget != null) _renderTarget.Dispose();
            _renderTarget = new RenderTarget2D(device, device.Viewport.Width, device.Viewport.Height);

            base.Reset();
        }

        public void Draw(Action<SpriteBatch> drawCall, ShaderEffect postProcessing, float layerDepth = 0.5f, SpriteBatchType batchType = SpriteBatchType.Default)
        {
            drawQueue.Add(
                new PostProcessingDrawInfo(drawCall, postProcessing, layerDepth, batchType)
            );
        }
        #endregion

        #region Member - Protected
        protected override void EndInit()
        {
            device.SamplerStates[0] = SamplerState.AnisotropicClamp;
        }

        protected override void SpriteBatchBegin(PostProcessingDrawInfo info)
        {
            if (info.BatchType == SpriteBatchType.Default)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, GI.Camera.ViewMatrix2D);
            }
            else
            {
                spriteBatch.Begin();
            }
        }

        protected override void DrawRenderTarget(PostProcessingDrawInfo info, RenderTarget2D target)
        {
            Effect effect = (info.Shader != null ? info.Shader.EffectXna : null);

            if (effect != null)
            {
                device.SetRenderTarget(_renderTarget);
                device.Clear(Color.Transparent);

                effect.Parameters["World"].SetValue(Matrix.Identity);
                effect.Parameters["View"].SetValue(Matrix.Identity);
                effect.Parameters["Projection"].SetValue(
                    Matrix.CreateTranslation(-0.5f, -0.5f, 0) * Matrix.CreateOrthographicOffCenter(0, device.Viewport.Width, device.Viewport.Height, 0, 0, 1)
                );

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, null, null, null, effect);
                spriteBatch.Draw(target, Vector2.Zero, Color.White);
                spriteBatch.End();

                target = _renderTarget;
            }

            base.DrawRenderTarget(info, target);
        }
        #endregion

        #region Class - PostProcessingDrawInfo
        public class PostProcessingDrawInfo : BasicBatch<PostProcessingDrawInfo>.BasicDrawInfo
        {
            #region Vars
            public ShaderEffect Shader;

            public Action<SpriteBatch> DrawCall;
            #endregion

            #region Init
            public PostProcessingDrawInfo(Action<SpriteBatch> drawCall, ShaderEffect shader, float layerDepth, SpriteBatchType batchType)
                : base(layerDepth, batchType)
            {
                this.Shader = shader;
                this.DrawCall = drawCall;
            }
            #endregion

            #region Member
            public override void Draw(SpriteBatch batch)
            {
                this.DrawCall(batch);
            }
            #endregion

            #region Operators
            public override bool Equals(object obj)
            {
                if (obj is PostProcessingDrawInfo)
                {
                    return this == (PostProcessingDrawInfo)obj;
                }

                return false;
            }

            public override int GetHashCode()
            {
                return (int)(100 * this.LayerDepth) * ((int)this.BatchType) * (this.Shader != null ? this.Shader.GetHashCode() : 1);
            }

            public static bool operator ==(PostProcessingDrawInfo o1, PostProcessingDrawInfo o2)
            {
                return (o1.LayerDepth == o2.LayerDepth) && (o1.BatchType == o2.BatchType) && (o1.Shader == o2.Shader);
            }

            public static bool operator !=(PostProcessingDrawInfo o1, PostProcessingDrawInfo o2)
            {
                return !(o1 == o2);
            }
            #endregion
        }
        #endregion
    }
}
