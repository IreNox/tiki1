#if DESIGNER
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TikiEngine.Elements.Graphics;
using System.Runtime.Serialization;

namespace TikiEngine.Levels.Designer
{
    [Serializable]
    internal class geAttachedSprite : GameElementAttached
    {
        #region Vars
        private Sprite _sprite = new Sprite();

        private designConstructorCall _spriteCom;
        #endregion

        #region Init
        public geAttachedSprite(GameElementIsland owner)
            : base(owner)
        {
        }

        public geAttachedSprite(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Member
        protected override DesignAction CreateAttachableArg()
        {
            element = _sprite;

            _spriteCom = new designConstructorCall(
                typeof(Sprite).GetConstructor(
                    new Type[] { typeof(string), typeof(float), typeof(float) }
                )
            );
            _spriteCom.Args.Add(
                new designStaticValue(typeof(string), "")
            );
            _spriteCom.Args.Add(
                new designStaticValue(typeof(float), 0.0f)
            );
            _spriteCom.Args.Add(
                new designStaticValue(typeof(float), 1.0f)
            );

            return _spriteCom;
        }

        public override void Save()
        {
        }
        #endregion

        #region Properties
        public override Vector2 Size
        {
            get { return ConvertUnits.ToSimUnits(_sprite.TextureSize); }
        }

        public float Angle
        {
            get { return _sprite.Angle; }
            set
            {
                _sprite.Angle = value;
                ((designStaticValue)_spriteCom.Args[1]).Value = value;
            }
        }

        public float Scale
        {
            get { return _sprite.Scale; }
            set
            {
                _sprite.Scale = value;
                ((designStaticValue)_spriteCom.Args[2]).Value = value;
            }
        }

        [SearchFile]
        public string TextureFile
        {
            get { return _sprite.TextureFile; }
            set
            {
                _sprite.TextureFile = value;
                ((designStaticValue)_spriteCom.Args[0]).Value = value;
            }
        }
        #endregion
    }
}
#endif