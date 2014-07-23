using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Common.Decomposition;
using System.Runtime.Serialization;
using TikiEngine.Elements.Effects;

namespace TikiEngine.Elements.Physic
{
    [Serializable]
    public abstract class NameObjectPhysicPolygon : NameObjectPhysic
    {
        #region Vars
        private bool _reselect = false;
        
        private Vector2 _size;
        protected Vector2 origin;

        private string _textureBodyFile;
        private Texture2D _textureBodyXna;

        private ShaderEffect _effectXna = new shaderBasic();

        private Body[] _parts = new Body[0];
        private Dictionary<Guid, FixtureInfo> _infos = new Dictionary<Guid, FixtureInfo>();

        public event EventHandler OnReselectParts;
        #endregion

        #region Init
        public NameObjectPhysicPolygon()
        {
        }

        public NameObjectPhysicPolygon(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Private Member
        private Vector2 _calcSize(IEnumerable<Vector2> vert)
        {
            return new Vector2(
                vert.Max(v => v.X) - vert.Min(v => v.X),
                vert.Max(v => v.Y) - vert.Min(v => v.Y)
            );
        }
        #endregion

        #region Member
        public override void Dispose()
        {
            foreach (Body body in _parts)
            {
                body.Dispose();
            }

            foreach (FixtureInfo info in _infos.Values)
            {
                info.Dispose();
            }
        }
        #endregion

        #region Member - Protected
        protected abstract Body CreateBodyByList(List<Vertices> list);

        protected override void SetTexture(Texture2D texture)
        {
            if (_effectXna != null)
            {
                _effectXna.Texture = texture;
            }

            base.SetTexture(texture);
        }

        protected virtual List<Vertices> CreateVertices()
        {
            uint[] data = new uint[_textureBodyXna.Width * _textureBodyXna.Height];
            _textureBodyXna.GetData(data);

            Vertices textureVertices = PolygonTools.CreatePolygon(data, _textureBodyXna.Width, true);

            Vector2 centroid = -textureVertices.GetCentroid();
            textureVertices.Translate(ref centroid);

            int i = 1;
            while (textureVertices.Count > 100)
            {
                switch (i)
                {
                    case 1:
                        textureVertices = SimplifyTools.CollinearSimplify(textureVertices);
                        break;
                    default:
                        textureVertices = SimplifyTools.ReduceByDistance(textureVertices, i);
                        break;
                }

                i *= 2;
            }
            if (i != 1) textureVertices = SimplifyTools.CollinearSimplify(textureVertices);

            Vector2 vertScale = new Vector2(ConvertUnits.ToSimUnits(1));
            List<Vertices> triangulated = BayazitDecomposer.ConvexPartition(textureVertices);

            triangulated.ForEach(
                v => v.Scale(ref vertScale)
            );

            origin = -centroid * vertScale;

            return triangulated;
        }

        protected override void CreateBody()
        {
            List<Vertices> triangulated = this.CreateVertices();

            _size = _calcSize(
                triangulated.SelectMany(v => v)
            );

            body = this.CreateBodyByList(triangulated);
            
            foreach (var b in _parts)
            {
                b.Dispose();
            }
            _parts = new Body[0];

            foreach (var info in _infos.Values)
            {
                info.Dispose();
            }
            _infos.Clear();

            foreach (Fixture fixture in body.FixtureList)
            {
                Guid id = Guid.NewGuid();

                fixture.UserData = id;
                _infos[id] = new FixtureInfo(
                    this,
                    _effectXna,
                    fixture,
                    origin,
                    this.TextureSize
                );
            }
        }
        #endregion

        #region Member - Xna - Draw
        public override void Draw(GameTime gameTime)
        {
            foreach (Body b in _parts)
            {
                if (b.FixtureList == null) continue;

                Transform xf;
                b.GetTransform(out xf);
                foreach (Fixture f in b.FixtureList)
                {
                    if (f.UserData == null) continue;

                    Guid id = (Guid)f.UserData;
                    if (_infos.ContainsKey(id))
                    {
                        _infos[id].Draw(xf);
                    }
                }
            }

            base.Draw(gameTime);
        }
        #endregion

        #region Member - Xna - Update
        public override void Update(GameTime gameTime)
        {
            if (_reselect || _parts.Length == 0)
            {
                _parts = world.BodyList.Where(
                    b => b.FixtureList.Any(
                        f => f.UserData is Guid && _infos.ContainsKey((Guid)f.UserData)
                    )
                ).ToArray();

                if (_parts.Length != 0)
                {
                    _reselect = false;

                    if (this.OnReselectParts != null)
                    {
                        this.OnReselectParts(this, EventArgs.Empty);
                    }
                }
            }

            base.Update(gameTime);

        }
        #endregion

        #region Properties
        public Vector2 Size
        {
            get { return _size; }
        }

        public Body[] Parts
        {
            get { return _parts; }
        }

        [NonSerializedTiki]
        public ShaderEffect EffectPostProcessing { get; set; }

        [NonSerializedTiki]
        public ShaderEffect Effect
        {
            get { return _effectXna; }
            set
            {
                _effectXna = value;

                if (_effectXna != null)
                {
                    _effectXna.Texture = this.Texture;
                }

                foreach (FixtureInfo info in _infos.Values)
                {
                    info.Effect = _effectXna;
                }
            }
        }

        [NonSerializedTiki]
        public bool ReselectParts
        {
            get { return _reselect; }
            set { _reselect = value; }
        }

        public string TextureBodyFile
        {
            get { return _textureBodyFile; }
            set
            {
                _textureBodyFile = value;

                try
                {
                    _textureBodyXna = GI.Content.Load<Texture2D>(value);

                    this.RefreshBody = true;
                    this.ApplyChanges();
                }
                catch
                {
                }
            }
        }

        public override bool Ready
        {
            get { return this.Texture != null && _textureBodyXna != null && _effectXna != null; }
        }
        #endregion

        #region Class - FixtureInfo
        internal class FixtureInfo
        {
            #region Vars
            public NameObjectPhysicPolygon Owner;

            public ShaderEffect Effect;

            private int[] _indices;
            private VertexPositionTexture[] _vertices;

            public IndexBuffer IndexBuffer;
            public VertexBuffer VertexBuffer;
            #endregion

            #region Init
            public FixtureInfo(NameObjectPhysicPolygon owner, ShaderEffect effect, Fixture f, Vector2 topLeft, Vector2 textureSize)
            {
                this.Owner = owner;
                this.Effect = effect;

                PolygonShape shape = (PolygonShape)f.Shape;
                int vertexCount = shape.Vertices.Count;

                List<int> indices = new List<int>();
                List<VertexPositionTexture> vertices = new List<VertexPositionTexture>();

                foreach (Vector2 vertex in shape.Vertices)
                {
                    vertices.Add(
                        new VertexPositionTexture(
                            new Vector3(vertex, 0),
                            ConvertUnits.ToDisplayUnits(vertex + topLeft) / textureSize
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

                this.VertexBuffer = new VertexBuffer(GI.Device, VertexPositionTexture.VertexDeclaration, _vertices.Length, BufferUsage.WriteOnly);
                this.VertexBuffer.SetData(_vertices);
            }
            #endregion

            #region Member
            public void Draw(Transform xf)
            {
                GI.PolygonBatch.Draw(
                    this.VertexBuffer,
                    this.IndexBuffer,
                    this.Effect,
                    Matrix.CreateRotationZ(xf.Angle) * Matrix.CreateTranslation(xf.Position.X, xf.Position.Y, 0),
                    this.Owner.EffectPostProcessing,
                    this.Owner.layerDepth
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
