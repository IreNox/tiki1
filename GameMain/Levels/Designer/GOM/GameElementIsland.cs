#if DESIGNER
using System;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;
using System.Text;

namespace TikiEngine.Levels.Designer
{
    [Serializable]
    internal abstract class GameElementIsland : GameElement
    {
        #region Vars
        private designField _decField;
        private designDeclareField _decMain;
        private designMethodCall _decMethod;

        private string textureFile;
        protected Texture2D textureXnaC;
        protected Texture2D textureXnaN;
        protected designStaticValue textureCom;

        private float angle;
        protected designStaticValue rotationCom;

        protected designConstructorCall positionCom;

        private List<GameElementAttached> listAttached = new List<GameElementAttached>();

        internal static GameElementIsland currentSerializationTempObject;
        #endregion

        #region Init
        public GameElementIsland()
            : this(ConvertUnits.ToSimUnits(GI.Camera.CurrentPositionCenter))
        {
        }

        public GameElementIsland(Vector2 position)
        {
            textureCom = new designStaticValue(typeof(string), "");
            rotationCom = new designStaticValue(typeof(float), angle);

            positionCom = new designConstructorCall(
                typeof(Vector2).GetConstructor(
                    new Type[] { typeof(float), typeof(float) }
                )
            );
            positionCom.Args.Add(
                new designStaticValue(typeof(float), position.X)
            );
            positionCom.Args.Add(
                new designStaticValue(typeof(float), position.Y)
            );
            this.StartPosition = position;

            _decMethod = this.CreateMethodCall(_decField);

            _decField = new designField(_decMethod.Info.ReturnType, this.Name);
            _decMain = new designDeclareField(_decField, _decMethod);
        }

        public GameElementIsland(SerializationInfo info, StreamingContext context)
            : this()
        {
            currentSerializationTempObject = this;

            Serialization.ObjectDeserialize(this, info);
        }
        #endregion

        #region Member
        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override string GenerateCode()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(
                _decMain.GenerateCode() + ';'
            );

            foreach (var attached in listAttached)
            {
                sb.AppendLine(
                    attached.GenerateCode() + ';'
                );
            }

            sb.AppendLine(
                "this.Elements.Add(" + _decField.GenerateCode() + ");"
            );
            sb.AppendLine();

            return sb.ToString();
        }
        #endregion

        #region Member - Xna
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(
                textureXnaN,
                ConvertUnits.ToDisplayUnits(positionCurrent),
                null,
                Color.White,
                MathHelper.ToRadians(angle),
                ConvertUnits.ToDisplayUnits(this.Size) / 2,
                1.0f,
                SpriteEffects.None,
                0.8f
            );

            spriteBatch.Draw(
                textureXnaC,
                ConvertUnits.ToDisplayUnits(positionCurrent),
                null,
                (Color.White.ToVector4() * 0.66f).ToColor(),
                MathHelper.ToRadians(angle),
                ConvertUnits.ToDisplayUnits(this.Size) / 2,
                1.0f,
                SpriteEffects.None,
                0.9f
            );

            foreach (var attached in listAttached)
            {
                if (attached.Ready) attached.Draw(gameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var attached in listAttached)
            {
                if (attached.Ready)
                {
                    attached.Element.CurrentPosition = this.CurrentPosition;
                    attached.Update(gameTime);
                }
            }           
        }
        #endregion

        #region Member - Protected
        protected abstract designMethodCall CreateMethodCall(DesignActionVar var);
        #endregion

        #region Properties
        public designField DecField
        {
            get { return _decField; }
        }

        [Browsable(false)]
        public List<GameElementAttached> Attached
        {
            get { return listAttached; }
            set { listAttached = value; }
        }

        public override string Name
        {
            get { return base.Name; }
            set
            {
                base.Name = value;
                _decField.VarName = value;
            }
        }

        public override Vector2 Size
        {
            get
            {
                if (textureXnaN == null) return Vector2.Zero;

                return ConvertUnits.ToSimUnits(
                    new Vector2(textureXnaN.Width, textureXnaN.Height)
                );
            }
        }

        public override Vector2 CurrentPosition
        {
            get { return base.CurrentPosition; }
            set
            {
                positionCurrent = value;
                ((designStaticValue)positionCom.Args[0]).Value = value.X;
                ((designStaticValue)positionCom.Args[1]).Value = value.Y;
            }
        }

        [SearchFile]
        public string TextureFile
        {
            get { return textureFile; }
            set
            {
                if (value.EndsWith("_c") || value.EndsWith("_n"))
                {
                    value = value.Substring(0, value.Length - 2);
                }

                textureFile = value;
                textureCom.Value = value;

                try
                {
                    textureXnaC = GI.Content.Load<Texture2D>(textureFile + "_c");
                    textureXnaN = GI.Content.Load<Texture2D>(textureFile + "_n");
                }
                catch
                { 
                }
            }
        }

        public float Angle
        {
            get { return angle; }
            set
            {
                angle = value;
                rotationCom.Value = value;
            }
        }

        public override bool Ready
        {
            get { return textureXnaC != null && textureXnaN != null; }
        }
        #endregion
    }
}
#endif
