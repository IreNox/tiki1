using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TikiEngine
{
    public abstract class BasicBatch<T>
        where T : BasicBatch<T>.BasicDrawInfo
    {
        #region Vars
        protected bool isActive;        
        protected GraphicsDevice device;
        protected List<T> drawQueue = new List<T>();

        protected SpriteBatch spriteBatch;

        private Dictionary<BasicDrawInfo, RenderTarget2D> _renderTargets = new Dictionary<BasicDrawInfo, RenderTarget2D>();
        #endregion

        #region Init
        public BasicBatch(GraphicsDevice argDevice)
        {
            device = argDevice;
            spriteBatch = new SpriteBatch(device);
        }
        #endregion
        
        #region Private Member
        private RenderTarget2D _setRenderTarget(T info, Dictionary<T, RenderTarget2D> targets)
        {
            RenderTarget2D target;

            if (!_renderTargets.ContainsKey(info))
            {
                target = new RenderTarget2D(device, device.Viewport.Width, device.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
                _renderTargets[info] = target;
            }
            else
            {
                target = _renderTargets[info];
            }

            device.SetRenderTarget(target);
            if (!targets.ContainsKey(info))
            {
                device.Clear(Color.Transparent);
                targets[info] = _renderTargets[info];
            }

            return target;
        }
        #endregion

        #region Member
        public void Begin()
        {
            if (isActive)
            {
                throw new Exception("Batch already started");
            }

            isActive = true;
            drawQueue.Clear();
        }

        public void End()
        {
            if (!isActive)
            {
                throw new Exception("You must call Batch.Begin first");
            }

            this.EndInit();

            Dictionary<T, RenderTarget2D> drawTargets = new Dictionary<T, RenderTarget2D>();

            foreach (T info in drawQueue)
            {
                _setRenderTarget(info, drawTargets);

                this.SpriteBatchBegin(info);
                info.Draw(spriteBatch);
                this.SpriteBatchEnd();
            }
            drawQueue.Clear();

            foreach (var kvp in drawTargets)
            {
                this.DrawRenderTarget(kvp.Key, kvp.Value);
            }

            isActive = false;
        }

        public virtual void Reset()
        {
            foreach (RenderTarget2D target in _renderTargets.Values)
            {
                target.Dispose();
            }
            _renderTargets.Clear();
        }
        #endregion

        #region Member - Abstract/Virtual
        protected virtual void EndInit()
        {
        }

        protected abstract void SpriteBatchBegin(T info);
        
        protected virtual void SpriteBatchEnd()
        {
            spriteBatch.End();
        }

        protected virtual void DrawRenderTarget(T info, RenderTarget2D target)
        {
            if (info.BatchType == SpriteBatchType.Default)
            {
                info.BatchType.GetSpriteBatch().Draw(
                    target,
                    GI.Camera.CurrentPositionNagativ,
                    null,
                    Color.White,
                    0,
                    Vector2.Zero,
                    1 / GI.Camera.RealZoom,
                    SpriteEffects.None,
                    info.LayerDepth
                );
            }
            else
            {
                info.BatchType.GetSpriteBatch().Draw(
                    target,
                    Vector2.Zero,
                    null,
                    Color.White,
                    0,
                    Vector2.Zero,
                    1.0f,
                    SpriteEffects.None,
                    info.LayerDepth
                );
            }
        }
        #endregion

        #region Class - BasicDrawInfo
        public abstract class BasicDrawInfo
        {
            #region Vars
            public float LayerDepth;
            public SpriteBatchType BatchType;
            #endregion

            #region Init
            public BasicDrawInfo(float layerDepth, SpriteBatchType batchType)
            {
                this.BatchType = batchType;
                this.LayerDepth = layerDepth;
            }
            #endregion

            #region Member
            public abstract void Draw(SpriteBatch batch);
            #endregion

            #region Operators
            public override bool Equals(object obj)
            {
                if (obj is BasicDrawInfo)
                {
                    return this == (BasicDrawInfo)obj;
                }

                return false;
            }

            public override int GetHashCode()
            {
                return (int)(100 * this.LayerDepth) * ((int)this.BatchType);
            }

            public static bool operator ==(BasicDrawInfo o1, BasicDrawInfo o2)
            {
                return (o1.LayerDepth == o2.LayerDepth) && (o1.BatchType == o2.BatchType);
            }

            public static bool operator !=(BasicDrawInfo o1, BasicDrawInfo o2)
            {
                return !(o1 == o2);
            }
            #endregion
        }
        #endregion
    }
}
