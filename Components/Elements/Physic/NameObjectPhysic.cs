using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using TikiEngine.Elements.Graphics;
using TikiEngine.Elements.Audio;

namespace TikiEngine.Elements.Physic
{
    [Serializable]
    public abstract class NameObjectPhysic : NameObjectTextured
    {
        #region Vars
        protected Body body;
        protected World world = GI.World;
        protected List<Behavior> rulesOfConduct = new List<Behavior>();

        private bool drawAttachedAssets = true;
        protected List<AttachedElement> attachedAssets = new List<AttachedElement>();
        protected List<IAttachableCreator> attachableCreators = new List<IAttachableCreator>();

        protected float density = 1f;
        protected BodyType bodyType = BodyType.Dynamic;

        protected Material material = Material.island;

        private bool refreshBody = false;

        private SoundProfil soundProfil;
        #endregion

        #region Init
        public NameObjectPhysic()
            : this(null, null)
        { 
        }

        public NameObjectPhysic(string texture)
            : this(null, texture)
        { 
        }

        public NameObjectPhysic(Body body, string texture)
            : base(texture)
        {
            this.body = body;
        }

        public NameObjectPhysic(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Member
        public override void Reset()
        {
            drawAttachedAssets = true;

            base.Reset();
        }

        public override void Dispose()
        {
            body.Dispose();

            base.Dispose();
        }

        public AttachedElement AddAttachedElement(AttachedElement attachedAsset)
        {
            attachedAsset.NameObjectPhysic = this;
            attachedAssets.Add(attachedAsset);
            
            return attachedAsset;
        }

        public AttachedElement AddAttachedElement(IAttachable element, Vector2 offset, float layerDepth)
        {
            AttachedElement ae = new AttachedElement(element, offset, layerDepth);

            return this.AddAttachedElement(ae);
        }

        public void ApplyAttachableCreators()
        {
            if (attachableCreators.Count != 0)
            {
                attachedAssets.Clear();

                foreach (IAttachableCreator creator in attachableCreators)
                {
                    this.AddAttachedElement(
                        creator.CreateAttachableElement()
                    );
                }
            }
        }
        #endregion

        #region Member - Behavior
        public T AddBehavior<T>()
            where T : Behavior
        {
            if (!this.Ready)
            {
                throw new Exception("NameObjectPhysic is not Ready");
            }

            T obj = (T)Activator.CreateInstance(
                typeof(T),
                this
            );

            obj.ApplyChanges();

            rulesOfConduct.Add(obj);

            return obj;
        }

        public T GetBehavior<T>()
            where T : Behavior
        {
            return rulesOfConduct.OfType<T>().FirstOrDefault();
        }
        #endregion

        #region Member - Protected
        protected abstract void CreateBody();

        protected override void ApplyChanges()
        {
            this.ApplyAttachableCreators();

            if (body == null || refreshBody)
            {
                refreshBody = false;

                if (body != null)
                {
                    if (body.IsReady()) body.Dispose();
                    body = null;
                }

                if (this.Ready) this.CreateBody();
            } 

            rulesOfConduct.ForEach(
                b => b.ApplyChanges()
            );
        }
        #endregion

        #region Member - Xna - Draw
        public override void Draw(GameTime gameTime)
        {
            if (drawAttachedAssets)
            {
                foreach (AttachedElement aa in this.AttachedAssets)
                {
                    aa.Draw(gameTime);
                }
            }
        }
        #endregion

        #region Member - Xna - Update
        public override void Update(GameTime gameTime)
        {
            if (this.soundProfil != null)
                this.soundProfil.Update(gameTime);

            rulesOfConduct.ForEach(
                b => b.Update(gameTime)
            );

            attachedAssets.ForEach(
                aa => aa.Update(gameTime)
            );
        }
        #endregion

        #region Properties

        public SoundProfil SoundProfil
        {
            get { return this.soundProfil; }
            set { this.soundProfil = value; }
        }
        public virtual Material Material
        {
            get { return material; }
            set { material = value; }
        }

        public Body Body
        {
            get { return this.body; }
        }

        [Browsable(false)]
        public World World
        {
            get { return this.world; }
        }

        public float Density
        {
            get { return density; }
            set
            {
                density = value;
                if (body != null)
                {
                    foreach (Fixture f in body.FixtureList)
                    {
                        f.Shape.Density = value;
                    }
                    body.ResetMassData();
                }
            }
        }

        protected bool RefreshBody
        {
            get { return refreshBody; }
            set
            {
                refreshBody = value;
                this.ApplyChanges();
            }
        }

        public BodyType BodyType
        {
            get { return (body == null ? bodyType : body.BodyType); }
            set
            {
                bodyType = value;
                if (body != null) body.BodyType = value;
            }
        }

        public override Vector2 CurrentPosition
        {
            get { return (body == null ? positionCurrent : body.Position); }
            set
            {
                positionCurrent = value;
                if (body != null) body.Position = value;
            }
        }

        public List<Behavior> RulesOfConduct
        {
            get { return rulesOfConduct; }
        }

        public bool DrawAttachedAssets
        {
            get { return drawAttachedAssets; }
            set { drawAttachedAssets = value; }
        }

        public List<AttachedElement> AttachedAssets
        {
            get { return attachedAssets; }
        }

        public List<IAttachableCreator> AttachableCreators
        {
            get { return attachableCreators; }
        }
        #endregion
    }
}
