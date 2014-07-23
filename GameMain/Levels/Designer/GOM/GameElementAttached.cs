#if DESIGNER
using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using TikiEngine.Elements;
using TikiEngine.Elements.Physic;
using System.ComponentModel;

namespace TikiEngine.Levels.Designer
{
    [Serializable]
    internal abstract class GameElementAttached : GameElement
    {
        #region Vars
        private GameElementIsland _owner;

        protected designMethodCall decMethod;

        protected designStaticValue layerDepthCom;
        protected designConstructorCall offsetCom;

        protected IAttachable element;
        #endregion

        #region Init
        public GameElementAttached(GameElementIsland owner)
        { 
            _owner = owner;

            layerDepthCom = new designStaticValue(
                typeof(float),
                layerDepth
            );

            offsetCom = new designConstructorCall(
                typeof(Vector2).GetConstructor(
                    new Type[] { typeof(float), typeof(float) }
                )
            );
            offsetCom.Args.Add(
                new designStaticValue(typeof(float), 0.0f)
            );
            offsetCom.Args.Add(
                new designStaticValue(typeof(float), 0.0f)
            );

            decMethod = new designMethodCall(
                owner.DecField,
                typeof(NameObjectPhysic).GetMethod(
                    "AddAttachedElement",
                    new Type[] { typeof(IAttachable), typeof(Vector2), typeof(float) }
                )
            );
            decMethod.Args.Add(this.CreateAttachableArg());
            decMethod.Args.Add(offsetCom);
            decMethod.Args.Add(layerDepthCom);
        }

        public GameElementAttached(SerializationInfo info, StreamingContext context)
            : this(GameElementIsland.currentSerializationTempObject)
        {
            Serialization.ObjectDeserialize(this, info);
        }
        #endregion

        #region Member
        protected abstract DesignAction CreateAttachableArg();

        public override void Draw(GameTime gameTime)
        {
            element.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            element.Update(gameTime);
        }

        public override string GenerateCode()
        {
            return decMethod.GenerateCode();
        }

        public override string ToString()
        {
            return String.Format(
                "({0}) {1}",
                this.GetType().Name,
                this.Name
            );
        }
        #endregion

        #region Properties
        [Browsable(false)]
        public GameElementIsland Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        public IAttachable Element
        {
            get { return element; }
        }

        public override Vector2 CurrentPosition
        {
            get { return base.CurrentPosition; }
            set
            {
                base.CurrentPosition = _owner.CurrentPosition + value;
                ((designStaticValue)offsetCom.Args[0]).Value = value.X;
                ((designStaticValue)offsetCom.Args[1]).Value = value.Y;

                if (element != null)
                {
                    element.CurrentPosition = _owner.CurrentPosition;
                    element.Offset = value;
                }
            }
        }

        public override float LayerDepth
        {
            get { return base.LayerDepth; }
            set
            {
                base.LayerDepth = value;

                element.LayerDepth = value;
                layerDepthCom.Value = value;
            }
        }

        public override bool Ready
        {
            get { return element != null && ((NameObject)element).Ready; }
        }
        #endregion
    }
}
#endif