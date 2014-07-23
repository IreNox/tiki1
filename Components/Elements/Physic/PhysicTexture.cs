using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using TikiEngine.Elements.Graphics;
using TikiEngine.Components;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TikiEngine.Elements.Physic
{
    [Serializable]
    public class PhysicTexture : NameObjectPhysic
    {
        #region Vars

        private RotatedRectangle rotatedRectangle;

        private Vector2 _size;
        private Vector2 _origin;

        private string _textureBodyFile;
        private Texture2D _textureBodyXna;

        public delegate void OnReset(PhysicTexture p);
        private OnReset onReset;

        #endregion

        #region Init
        public PhysicTexture()
        {          
        }

        public PhysicTexture(string texture)
            : base(texture)
        {
            this.TextureBodyFile = texture;

            this.ApplyChanges();
        }

        public PhysicTexture(string texture, Vector2 position, float density)
            : this(texture)
        {
            body.Position = position;
            body.BodyType = BodyType.Dynamic;
        }
        
        public PhysicTexture(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Member
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.rotatedRectangle.ChangePosition(ConvertUnits.ToDisplayUnits(this.CurrentPosition) - _origin);
            this.rotatedRectangle.Rotation = this.Body.Rotation;
        }

        public override void Draw(GameTime gameTime)
        {
            if (body == null) return;

            spriteBatch.Draw(
                this.Texture,
                ConvertUnits.ToDisplayUnits(this.CurrentPosition),
                null,
                Color.White,
                body.Rotation,
                _origin,
                1f,
                SpriteEffects.None,
                this.LayerDepth
            );

#if DEBUG
            foreach (Behavior b in rulesOfConduct)
            {
                b.Draw(gameTime);
            }

            this.rotatedRectangle.Draw(gameTime);
#endif

            base.Draw(gameTime);           
        }

        public override void Reset()
        {
            if (onReset != null)
            {
                this.onReset(this);
            }
        }
        #endregion

        #region Protected Member
        protected override void CreateBody()
        {
            string name = _textureBodyFile.Replace('/', '.').Replace('\\', '.').ToLower();
            VerticesContainer vc = DataManager.LoadObject<VerticesContainer>(name, false);

            List<Vertices> list;

            if (vc != null)
            {
                list = vc.Vertices;
                _origin = vc.Origin;
            }
            else
            {
                uint[] data = new uint[_textureBodyXna.Width * _textureBodyXna.Height];
                _textureBodyXna.GetData(data);

                Vertices textureVertices = PolygonTools.CreatePolygon(data, _textureBodyXna.Width, true);

                Vector2 centroid = -textureVertices.GetCentroid();
                textureVertices.Translate(ref centroid);

                _origin = -centroid;

                textureVertices = SimplifyTools.ReduceByDistance(textureVertices, 4f);

                Vector2 vertScale = new Vector2(ConvertUnits.ToSimUnits(1f));
                list = BayazitDecomposer.ConvexPartition(textureVertices);

                list.ForEach(
                    v => v.Scale(ref vertScale)
                );

                _size = list.SelectMany(v => v).CalculateSize();

                vc = new VerticesContainer(list, _origin);
                vc.Name = name;

                DataManager.SetObject(vc);
            }

            body = BodyFactory.CreateCompoundPolygon(world, list, density, null);
            body.Position = positionCurrent;
            body.UserData = this;
            body.BodyType = bodyType;

            this.rotatedRectangle = new RotatedRectangle(
                new Rectangle(0,
                    0,
                    _textureBodyXna.Width,
                    _textureBodyXna.Height
                )
            );
        }
        #endregion

        #region Properties
        public event OnReset OnRest
        {
            add
            {
                onReset += value;
            }
            remove
            {
                onReset -= value;
            }
        }

        public Vector2 Size
        {
            get { return _size; }
        }
        public RotatedRectangle RotatedRectangle
        {
            get { return this.rotatedRectangle; }
            set { this.rotatedRectangle = value; }
        }
        public override bool Ready
        {
            get { return this.Texture != null && _textureBodyXna != null; }
        }

        public string TextureBodyFile
        {
            get { return _textureBodyFile; }
            set
            {
                _textureBodyFile = value;
                _textureBodyXna = GI.Content.Load<Texture2D>(value);

                this.RefreshBody = true;
            }
        }
        #endregion
    }
}
