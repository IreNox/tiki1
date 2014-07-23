using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TikiEngine.Elements.Effects;

namespace TikiEngine
{
    public class PolygonBatch : BasicBatch<PolygonBatch.PolygonDrawInfo>
    {
        #region Vars
        private shaderBasic _effect;

        private RenderTarget2D _renderTarget;
        #endregion

        #region Init
        public PolygonBatch(GraphicsDevice device)
            : base(device)
        {
            _effect = new shaderBasic();

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

        protected override void EndInit()
        {
            device.SamplerStates[0] = SamplerState.AnisotropicClamp;
        }

        protected override void SpriteBatchBegin(PolygonDrawInfo info)
        {
        }

        protected override void SpriteBatchEnd()
        {
        }

        protected override void DrawRenderTarget(PolygonDrawInfo info, RenderTarget2D target)
        {
            Effect effect = (info.PostProcessingShader != null ? info.PostProcessingShader.EffectXna : null);

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
            
            base.DrawRenderTarget(info, _renderTarget);
        }
        #endregion

        #region Member - Draw - Lines
        public void DrawLine(Vector2 p1, Vector2 p2, Color color, float layerDepth)
        {
            _effect.UpdateMatrices();

            VertexPositionColor[] list = new VertexPositionColor[] {
                new VertexPositionColor(new Vector3(p1, 0), color),
                new VertexPositionColor(new Vector3(p2, 0), color)
            };

            VertexBuffer vb = new VertexBuffer(GI.Device, VertexPositionColor.VertexDeclaration, 2, BufferUsage.WriteOnly);
            vb.SetData(list);

            drawQueue.Add(
                new PolygonDrawInfo(PrimitiveType.LineList, vb, null, _effect, Matrix.Identity, true, null, layerDepth)
            );
        }

        public void DrawLine(VertexBuffer vertexBuffer, IndexBuffer indexBuffer, float layerDepth)
        {
            drawQueue.Add(
                new PolygonDrawInfo(PrimitiveType.LineList, vertexBuffer, indexBuffer, _effect, Matrix.Identity, false, null, layerDepth)
            );
        }
        #endregion

        #region Member - Draw - Triangles
        public void Draw(VertexBuffer vertexBuffer, IndexBuffer indexBuffer, Color color, float layerDepth)
        {
            shaderBasic effect = new shaderBasic();

            this.Draw(vertexBuffer, indexBuffer, effect, Matrix.Identity, null, layerDepth);
        }

        public void Draw(VertexBuffer vertexBuffer, IndexBuffer indexBuffer, Texture2D texture, float layerDepth)
        {
            shaderBasic effect = new shaderBasic();
            effect.Texture = texture;

            this.Draw(vertexBuffer, indexBuffer, effect, Matrix.Identity, null, layerDepth);
        }

        public void Draw(VertexBuffer vertexBuffer, IndexBuffer indexBuffer, ShaderEffect effect, Matrix worldMatrix, ShaderEffect postProcessingShader, float layerDepth)
        {
            drawQueue.Add(
                new PolygonDrawInfo(
                    PrimitiveType.TriangleList,
                    vertexBuffer,
                    indexBuffer,
                    effect,
                    worldMatrix,
                    false,
                    postProcessingShader,
                    layerDepth
                )
            );
        }
        #endregion

        #region Class - PolygonDrawInfo
        public class PolygonDrawInfo : BasicBatch<PolygonDrawInfo>.BasicDrawInfo
        {
            #region Vars
            public PrimitiveType PrimitiveType;

            public Matrix WorldMatrix;
            public ShaderEffect Effect;
            public ShaderEffect PostProcessingShader;

            public IndexBuffer IndexBuffer;
            public VertexBuffer VertexBuffer;

            private GraphicsDevice _device;
            private bool _destroyBuffer = false;
            #endregion

            #region Init
            public PolygonDrawInfo(PrimitiveType type, VertexBuffer vertexBuffer, IndexBuffer indexBuffer, ShaderEffect effect, Matrix worldMatrix, bool destroyBuffer, ShaderEffect postProcessingShader, float layerDepth)
                : base(layerDepth, SpriteBatchType.Default)
            {
                this.Effect = effect;
                this.WorldMatrix = worldMatrix;

                this.PrimitiveType = type;
                this.IndexBuffer = indexBuffer;
                this.VertexBuffer = vertexBuffer;

                this.PostProcessingShader = postProcessingShader;

                _device = GI.Device;
                _destroyBuffer = destroyBuffer;
            }
            #endregion

            #region Member
            public override void Draw(SpriteBatch batch)
            {
                Effect effect = this.Effect.SetMatrices(this.WorldMatrix);

                _device.SetVertexBuffer(this.VertexBuffer);

                if (this.IndexBuffer != null)
                {
                    _device.Indices = this.IndexBuffer;
                }

                if (this.PrimitiveType == PrimitiveType.TriangleList)
                {
                    foreach (var pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        GI.Device.DrawIndexedPrimitives(
                            PrimitiveType.TriangleList,
                            0,
                            0,
                            this.VertexBuffer.VertexCount,
                            0,
                            this.IndexBuffer.IndexCount / 3
                        );
                    }
                }
                else if (this.PrimitiveType == PrimitiveType.LineList)
                {
                    foreach (var pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        if (this.IndexBuffer == null)
                        {
                            GI.Device.DrawPrimitives(
                                PrimitiveType.LineList,
                                0,
                                this.VertexBuffer.VertexCount / 2
                            );
                        }
                        else
                        {
                            GI.Device.DrawIndexedPrimitives(
                                PrimitiveType.LineList,
                                0,
                                0,
                                this.VertexBuffer.VertexCount,
                                0,
                                this.IndexBuffer.IndexCount / 2
                            );
                        }
                    }

                    if (_destroyBuffer) this.VertexBuffer.Dispose();
                }                
            }
            #endregion

            #region Operators
            public override bool Equals(object obj)
            {
                if (obj is PolygonDrawInfo)
                {
                    return this == (PolygonDrawInfo)obj;
                }

                return false;
            }

            public override int GetHashCode()
            {
                return (int)(100 * this.LayerDepth) * ((int)this.BatchType) * (this.PostProcessingShader != null ? this.PostProcessingShader.GetHashCode() : 1);
            }

            public static bool operator ==(PolygonDrawInfo o1, PolygonDrawInfo o2)
            {
                return (o1.LayerDepth == o2.LayerDepth) && (o1.BatchType == o2.BatchType) && (o1.PostProcessingShader == o2.PostProcessingShader);
            }

            public static bool operator !=(PolygonDrawInfo o1, PolygonDrawInfo o2)
            {
                return !(o1 == o2);
            }
            #endregion
        }
        #endregion
    }
}
