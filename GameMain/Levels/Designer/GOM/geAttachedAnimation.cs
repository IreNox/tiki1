#if DESIGNER
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikiEngine.Elements.Graphics;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace TikiEngine.Levels.Designer
{
    [Serializable]
    internal class geAttachedAnimation : GameElementAttached
    {
        #region Vars
        private Animation _animation = new Animation();

        private designConstructorCall _animationCom;
        #endregion

        #region Init
        public geAttachedAnimation(GameElementIsland owner)
            : base(owner)
        {
        }

        public geAttachedAnimation(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Member
        protected override DesignAction CreateAttachableArg()
        {
            element = _animation;

            _animationCom = new designConstructorCall(
                typeof(Animation).GetConstructor(
                    new Type[] { typeof(string), typeof(int), typeof(int), typeof(int), typeof(int), typeof(float), typeof(float), typeof(bool), typeof(bool) }
                )
            );
            _animationCom.Args.Add(
                new designStaticValue(typeof(string), "") // 0 - Texture
            );
            _animationCom.Args.Add(
                new designStaticValue(typeof(int), 1) // 1 - Rows
            );
            _animationCom.Args.Add(
                new designStaticValue(typeof(int), 1) // 2 - Cols
            );
            _animationCom.Args.Add(
                new designStaticValue(typeof(int), 1) // 3 - StartFrame
            );
            _animationCom.Args.Add(
                new designStaticValue(typeof(int), 2) // 4 - StopFrame
            );
            _animationCom.Args.Add(
                new designStaticValue(typeof(float), 100.0f) // 5 - Speed
            );
            _animationCom.Args.Add(
                new designStaticValue(typeof(float), 1.0f) // 6 - Scale
            );
            _animationCom.Args.Add(
                new designStaticValue(typeof(float), 0.0f) // 7 - Angle
            );
            _animationCom.Args.Add(
                new designStaticValue(typeof(bool), 1) // 8 - Loop
            );        
            _animationCom.Args.Add(
                new designStaticValue(typeof(bool), 1) // 9 - Alive
            );

            return _animationCom;
        }

        public override void Save()
        {
        }
        #endregion

        #region Properties
        [SearchFile]
        public string TextureFile
        {
            get { return _animation.TextureFile; }
            set
            {
                _animation.TextureFile = value;
                ((designStaticValue)_animationCom.Args[0]).Value = value;
            }
        }

        public int Rows
        {
            get { return _animation.Rows; }
            set
            {
                _animation.Rows = value;
                ((designStaticValue)_animationCom.Args[1]).Value = value;
            }
        }

        public int Columns
        {
            get { return _animation.Columns; }
            set
            {
                _animation.Columns = value;
                ((designStaticValue)_animationCom.Args[2]).Value = value;
            }
        }

        public int StartFrame
        {
            get { return _animation.StartFrame; }
            set
            {
                _animation.StartFrame = value;
                ((designStaticValue)_animationCom.Args[3]).Value = value;
            }
        }

        public int StopFrame
        {
            get { return _animation.StopFrame; }
            set
            {
                _animation.StopFrame = value;
                ((designStaticValue)_animationCom.Args[4]).Value = value;
            }
        }

        public float AnimationSpeed
        {
            get { return _animation.AnimationSpeed; }
            set
            {
                _animation.AnimationSpeed = value;
                ((designStaticValue)_animationCom.Args[5]).Value = value;
            }
        }

        public float Scale
        {
            get { return _animation.Scale; }
            set
            {
                _animation.Scale = value;
                ((designStaticValue)_animationCom.Args[6]).Value = value;
            }
        }

        public float Angle
        {
            get { return _animation.Angle; }
            set
            {
                _animation.Angle = value;
                ((designStaticValue)_animationCom.Args[7]).Value = value;
            }
        }

        public bool Loop
        {
            get { return _animation.Loop; }
            set
            {
                _animation.Loop = value;
                ((designStaticValue)_animationCom.Args[8]).Value = value;
            }
        }

        public bool IsAlive
        {
            get { return _animation.IsAlive; }
            set
            {
                _animation.IsAlive = value;
                ((designStaticValue)_animationCom.Args[9]).Value = value;
            }
        }
        
        public override Vector2 Size
        {
            get { return ConvertUnits.ToSimUnits(_animation.Size); }
        }
        #endregion
    }
}
#endif