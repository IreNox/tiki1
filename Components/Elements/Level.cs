using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using TikiEngine.Elements.Physic;
using TikiEngine.Elements.Events;
using TikiEngine.Elements.Graphics;
using TikiEngine.Elements.Particle;

namespace TikiEngine.Elements
{
    [Serializable]
    public class Level : NameObject
    {
        #region Vars
        private bool _init = false;

        protected World world = new World(new Vector2(0.0f, 9.81f));

        private ParallaxSprite _parallax = new ParallaxSprite();

        private List<GameEvent> _events = new List<GameEvent>();
        private List<NameObject> _trigger = new List<NameObject>();
        private List<NameObject> _particles = new List<NameObject>();

        private List<NameObject> _elements = new List<NameObject>();
        private List<NameObject> _elementsCollectable = new List<NameObject>();

        private List<NameObjectPhysic> _elementsBuild = new List<NameObjectPhysic>();
        private List<NameObjectPhysic> _elementsDestroy = new List<NameObjectPhysic>();
        private List<NameObjectPhysic> _elementsDestroyIsland = new List<NameObjectPhysic>();

        private List<NameObject>[] _listsNO;
        private List<NameObjectPhysic>[] _listsNOP;
        #endregion

        #region Init
        public Level()
        {
            GI.World = world;

            _listsNO = new List<NameObject>[] { 
                _trigger,
                _particles,
                _elements,
                _elementsCollectable
            };

            _listsNOP = new List<NameObjectPhysic>[] { 
                _elementsBuild,
                _elementsDestroy,
                _elementsDestroyIsland
            };
        }

        public Level(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            GI.World = world;            
        }

        public virtual void Initialize()
        {
            _init = true;
        }
        #endregion

        #region Protected Member
        protected virtual NameObject[] selectAll()
        {
            return _listsNO.SelectMany(l => l).Concat(_listsNOP.SelectMany(l => l.Cast<NameObject>())).ToArray();
        }
        #endregion

        #region Member
        protected override void ApplyChanges()
        {
        }

        public void Wolken()
        {
            float c = 0f;

            #region wolkendecke
            ParallaxSprite.ParallaxLayer wolkendecke = _parallax.AddLayer();
            for (int i = 0; i < 100; i++)
            {
                Sprite sprite = new Sprite();
                sprite.TextureFile = "Layer/wolkendecke";
                sprite.Scale = 1;
                sprite.StartPosition = new Vector2(
                    i * 19.62f,
                    ConvertUnits.ToSimUnits(GI.Device.Viewport.Height)
                ); // 9.2f

                sprite.SpriteBatchType = SpriteBatchType.Parallax;
                sprite.Speed = 0.5f;

                wolkendecke.Sprites.Add(sprite);
            }
            wolkendecke.LayerDepth = LayerDepthEnum.Background1;
            _parallax.Layer.Add(wolkendecke);
            #endregion

            #region middle
            ParallaxSprite.ParallaxLayer middle = _parallax.AddLayer();
            for (int l = 0; l < 6; l++)
            {
                for (int i = 0; i < 800; i++)
                {
                    Sprite sprite = new Sprite();
                    sprite.TextureFile = String.Format(
                        "Layer/cloud_{0}{1}",
                        "m",
                        Functions.GetRandom(1, 7)
                    );
                    sprite.StartPosition = new Vector2(
                        -50 + 7 * i + Functions.GetRandom(-2f, 2),
                        5f +  - 0.5f * l +Functions.GetRandom(-0.5f, 0.5f)
                    );
                    sprite.Speed = 0.75f;
                    sprite.SpriteBatchType = SpriteBatchType.Parallax;
                    sprite.LayerDepth = LayerDepthEnum.Background3 + (c++ / 10000);

                    middle.Sprites.Add(sprite);
                }
            }
            _parallax.Layer.Add(middle);
            #endregion

            #region big
            c = 0;
            ParallaxSprite.ParallaxLayer big = _parallax.AddLayer();
            for (int i = 0; i < 300; i++)
            {
                Sprite sprite = new Sprite();
                sprite.TextureFile = String.Format(
                    "Layer/cloud_{0}{1}",
                    "b",
                    Functions.GetRandom(1, 4)
                );
                sprite.StartPosition = new Vector2(
                    -50 + 15 * i + Functions.GetRandom(-2f, 2),
                    3f + Functions.GetRandom(-1.5f, 0.5f)
                );
                sprite.Speed = 1f;
                sprite.SpriteBatchType = SpriteBatchType.Parallax;
                sprite.LayerDepth = LayerDepthEnum.Background3 + (c++ / 1000);

                big.Sprites.Add(sprite);
            }
            _parallax.Layer.Add(big);
            #endregion

            #region small
            c = 0;
            int random = 6;

            ParallaxSprite.ParallaxLayer front = _parallax.AddLayer();
            for (int i = 0; i < 100; i++)
            {
                int tmp;
                do
                {
                    tmp = Functions.GetRandom(9, 13);
                } while (tmp == random);
                random = tmp;

                Sprite sprite = new Sprite();
                sprite.TextureFile = String.Format(
                    "Layer/b_{0}{1}",
                    "i", random
                );
                sprite.StartPosition = new Vector2(
                    -50 + 6 * i + Functions.GetRandom(-2f, 2),
                    5.5f + Functions.GetRandom(-0.75f, 0.75f)
                );
                sprite.Speed = 0.4f;
                sprite.Scale = 0.75f;
                sprite.SpriteBatchType = SpriteBatchType.Parallax;
                sprite.LayerDepth = LayerDepthEnum.Background3 + (c++ / 1000);

                front.Sprites.Add(sprite);
            }
            
            front.LayerDepth = LayerDepthEnum.Background2;
            _parallax.Layer.Add(front);
            #endregion
        }

        public override void Dispose()
        {
            _elements.ForEach(
                e => e.Dispose()
            );
            _elementsBuild.ForEach(
                e => e.Dispose()
            );  
            _elementsDestroy.ForEach(
                e => e.Dispose()
            );
            _elementsDestroyIsland.ForEach(
                e => e.Dispose()
            );
            _elementsCollectable.ForEach(
                e => e.Dispose()
            );      
        }
        #endregion

        #region Member - Xna - Draw
        public override void Draw(GameTime gameTime)
        {
            foreach (NameObject e in selectAll())
            {
                e.Draw(gameTime);
            }

            _parallax.Draw(gameTime);
        }
        #endregion

        #region Member - Xna - Update
        public override void Update(GameTime gameTime)
        {
            world.Step(
                Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f))
            );

            foreach (NameObject e in selectAll())
            { 
                e.Update(gameTime);
            }

            foreach (GameEvent e in _events)
            {
                e.Update(gameTime);
            }
            
            _parallax.Update(gameTime);            
        }
        #endregion

        #region Properties
        public bool Initialized
        {
            get { return _init; }
        }

        [NonSerializedTiki]
        public World World
        {
            get { return world; }
        }

        public List<GameEvent> Events
        {
            get { return _events; }
            set { _events = value; }
        }

        public List<NameObject> Trigger
        {
            get { return _trigger; }
            set { _trigger = value; }
        }

        public List<NameObject> Particles
        {
            get { return _particles; }
        }

        public List<NameObject> Elements
        {
            get { return _elements; }
            set { _elements = value; }
        }

        public List<NameObjectPhysic> ElementsBuild
        {
            get { return _elementsBuild; }
        }

        public List<NameObject> ElementsCollectable
        {
            get { return _elementsCollectable; }
        }

        [NonSerializedTiki]
        public List<NameObjectPhysic> ElementsDestroy
        {
            get { return _elementsDestroy; }
            set { _elementsDestroy = value; }
        }

        [NonSerializedTiki]
        public List<NameObjectPhysic> ElementsDestroyIsland
        {
            get { return _elementsDestroyIsland; }
            set { _elementsDestroyIsland = value; }
        }

        public ParallaxSprite ParallaxSprite
        {
            get { return _parallax; }
            set { _parallax = value; }
        }

        public override bool Ready
        {
            get { return _init; }
        }
        #endregion
    }
}
