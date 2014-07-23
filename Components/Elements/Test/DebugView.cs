#if DEBUG
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TikiEngine.Elements.Graphics;
using TikiEngine.Elements.Effects;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace TikiEngine.Elements
{
    public class DebugView : NameObjectGraphics
    {
        #region Vars
        private SpriteFont _font;
        private Texture2D _texture;

        private int _height = 0;
        private string _text = "";
        private StringBuilder _textBuilder = new StringBuilder();

        private bool _drawFixtures = false;
        private Dictionary<Fixture, FixtureInfo> _infos = new Dictionary<Fixture, FixtureInfo>();
        #endregion

        #region Init
        public DebugView()
            : base(SpriteBatchType.Interface)
        {
            _font = GI.Content.Load<SpriteFont>("font");

            _texture = new Texture2D(GI.Device, 1, 1);
            _texture.SetData(
                new Color[]
                {
                    Color.White
                }
            );
        }
        #endregion

        #region Private Member
        private string _getNumber(decimal num)
        {
            return "";
        }
        #endregion

        #region Member
        public override void Dispose()
        {
            _texture.Dispose();
        }

        protected override void ApplyChanges()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Member - Xna - Draw
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(
                _texture,
                new Rectangle(
                    GI.Device.Viewport.Width - 400,
                    10,
                    390,
                    _height
                ),
                new Color(1f, 1f, 1f, 0.5f)
            );

            spriteBatch.DrawString(
                _font,
                _text,
                new Vector2(
                    GI.Device.Viewport.Width - 400,
                    10
                ),
                Color.Green
            );

            if (_drawFixtures)
            {
                Transform xf;

                foreach (var kvp in _infos.ToArray())
                {
                    if (kvp.Key.Body == null || !kvp.Key.Body.IsReady())
                    {
                        _infos.Remove(kvp.Key);
                        continue;
                    }

                    kvp.Key.Body.GetTransform(out xf);

                    kvp.Value.Draw(xf);
                }
            }
        }
        #endregion

        #region Member - Xna - Update
        private void _updateVars(GameTime gameTime)
        {
            if (GI.Control.KeyboardPressed(Keys.F8)) Debugger.Break();

            GI.GameVars["Time"] = gameTime.TotalGameTime.TotalMinutes;
            GI.GameVars["Camera"] = GI.Camera.CurrentPosition;
            GI.GameVars["Zoom"] = GI.Camera.Zoom;
            GI.GameVars["Mouse"] = GI.Control.MouseSimVector();
            GI.GameVars["breakable"] = GI.Level.ElementsDestroyIsland.Count;
        }

        public override void Update(GameTime gameTime)
        {
            _updateVars(gameTime);

            _textBuilder.Clear();

            foreach (var kvp in GI.GameVars)
            {
                _textBuilder.AppendFormat(
                    "{0}: {1}\n",
                    kvp.Key,
                    kvp.Value
                );
            }

            _text = _textBuilder.ToString();
            _height = (_text.Count(c => c == '\n') * 28);

            if (GI.Control.KeyboardPressed(Keys.F2)) _drawFixtures = !_drawFixtures;

            if (_drawFixtures)
            {
                foreach (Fixture f in GI.World.BodyList.SelectMany(b => b.FixtureList))
                {
                    if (!_infos.ContainsKey(f))
                    {
                        FixtureInfo info = new FixtureInfo(f);

                        if (info.VertexBuffer != null) _infos[f] = info;
                    }
                }
            }
        }
        #endregion

        #region Properties
        public override bool Ready
        {
            get { throw new NotImplementedException(); }
        }
        #endregion

        #region Class - FixtureInfo
        internal class FixtureInfo
        {
            #region Vars
            public static shaderBasic Effect;

            private int[] _indices;
            private VertexPositionColor[] _vertices;

            public IndexBuffer IndexBuffer;
            public VertexBuffer VertexBuffer;
            #endregion

            #region Init
            static FixtureInfo()
            {
                Effect = new shaderBasic();
                Effect.Texture = null;
                ((BasicEffect)Effect.EffectXna).DiffuseColor = new Vector3(1, 1, 0);
            }

            public FixtureInfo(Fixture f)
            {
                if (f.ShapeType != ShapeType.Polygon) return;

                PolygonShape shape = (PolygonShape)f.Shape;
                int vertexCount = shape.Vertices.Count;

                List<int> indices = new List<int>();
                List<VertexPositionColor> vertices = new List<VertexPositionColor>();

                foreach (Vector2 vertex in shape.Vertices)
                {
                    vertices.Add(
                        new VertexPositionColor(
                            new Vector3(vertex, 0),
                            new Color(1, 1, 1, 0.5f)
                        )
                    );
                }

                for (int i = 1; i < vertexCount - 1; i++)
                {
                    indices.AddRange(
                        new int[] { 0, i, i + 1 }
                    );
                }

                _indices = indices.ToArray();
                _vertices = vertices.ToArray();

                this.IndexBuffer = new IndexBuffer(GI.Device, typeof(int), _indices.Length, BufferUsage.WriteOnly);
                this.IndexBuffer.SetData(_indices);

                this.VertexBuffer = new VertexBuffer(GI.Device, VertexPositionColor.VertexDeclaration, _vertices.Length, BufferUsage.WriteOnly);
                this.VertexBuffer.SetData(_vertices);
            }
            #endregion

            #region Member
            public void Draw(Transform xf)
            {
                GI.PolygonBatch.Draw(
                    this.VertexBuffer,
                    this.IndexBuffer,
                    Effect,
                    Matrix.CreateRotationZ(xf.Angle) * Matrix.CreateTranslation(xf.Position.X, xf.Position.Y, 0),
                    null,
                    1
                );
            }

            public void Dispose()
            {
                this.IndexBuffer.Dispose();
                this.VertexBuffer.Dispose();
            }
            #endregion
        }
        #endregion
    }
}
#endif