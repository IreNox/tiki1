using System;
using TikiEngine.Elements.Physic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TikiEngine.Elements.Graphics
{
    public class AttachedElement
    {
        #region Vars
        NameObjectPhysic nop;

        private IAttachable _attachable;
        private bool debug = false;
        private bool collectable = false;
        #endregion

        #region Init
        public AttachedElement(IAttachable attachable, Vector2 offset, float layerDepth, bool collectable)
            :this(attachable,offset,layerDepth)
        {
            this.collectable = collectable;
        }
        
        public AttachedElement(IAttachable attachable, Vector2 offset, float layerDepth)
        {
            _attachable = attachable;
            Attachable.Offset = offset;
            Attachable.LayerDepth = layerDepth;
        }

        public AttachedElement(IAttachable attachable, bool debug)
        {
            _attachable = attachable;
            this.debug = debug;
            attachable.LayerDepth = LayerDepthEnum.IslandBackground;
        }
        #endregion

        #region Member - Xna
        public void Draw(GameTime gameTime)
        {
            _attachable.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
#if DEBUG
            #region debug
            if (debug)
            {
                float f = 0.01f;
                if (GI.Control.KeyboardDown(Keys.LeftControl))
                    f = 0.1f;
                if (GI.Control.KeyboardDown(Keys.NumPad8))
                {
                    Attachable.Offset += new Vector2(0,-f);
                }
                if (GI.Control.KeyboardDown(Keys.NumPad2))
                {
                    Attachable.Offset += new Vector2(0,f);
                }
                if (GI.Control.KeyboardDown(Keys.NumPad4))
                {
                    Attachable.Offset += new Vector2(-f,0);
                }
                if (GI.Control.KeyboardDown(Keys.NumPad6))
                {
                    Attachable.Offset += new Vector2(f,0);
                }
                if (GI.Control.KeyboardPressed(Keys.G))
                {
                    Attachable.Offset = Vector2.Zero;
                }

                if (GI.Control.KeyboardPressed(Keys.OemPlus))
                {
                    if (Attachable is Animation)
                    {
                        ((Animation)Attachable).Scale += 0.1f;
                    }
                }
                if (GI.Control.KeyboardPressed(Keys.OemMinus))
                {
                    if (Attachable is Animation)
                    {
                        ((Animation)Attachable).Scale -= 0.1f;
                    }
                }
                if (GI.Control.KeyboardPressed(Keys.RightControl))
                {
                    if (Attachable.LayerDepth == LayerDepthEnum.IslandBackground)
                        Attachable.LayerDepth = LayerDepthEnum.IslandForeGround;
                    else
                        Attachable.LayerDepth = LayerDepthEnum.Foreground;
                }
                if (GI.Control.KeyboardPressed(Keys.F1))
                {
                    if (_attachable is Sprite)
                    {
                        String x = "" + Attachable.Offset.X;
                        String y = "" + Attachable.Offset.Y;

                        Console.Write(".AddAttachedElement(new AttachedElement(new Sprite(\"");
                        Console.Write(((Sprite)Attachable).TextureFile);
                        Console.Write("\"), new Vector2(");
                        Console.Write(x.Replace(',', '.') + "f,");
                        Console.Write(y.Replace(',', '.') + "f),");
                        if (Attachable.LayerDepth == LayerDepthEnum.IslandBackground)
                            Console.Write(" 0.5f));");
                        else
                            Console.Write("0.7f));");
                        Console.WriteLine("");
                    }
                    else
                    Console.WriteLine("Offset : " + _attachable.Offset);
                    if (_attachable is Animation)
                        Console.WriteLine("Scale : " + ((Animation)_attachable).Scale);
                }
            }
            #endregion
#endif
            if (collectable && !GI.Level.ElementsCollectable.Contains((NameObject)_attachable))
            {
                nop.AttachedAssets.Remove(this);
                return;
            }

            _attachable.Angle = nop.Body.Rotation;
            _attachable.CurrentPosition = nop.CurrentPosition;

            _attachable.Update(gameTime);
        }
        #endregion

        #region Properties
        public IAttachable Attachable
        {
            get { return _attachable; }
            set { _attachable = value; }
        }

        public NameObjectPhysic NameObjectPhysic
        {
            get { return this.nop; }
            set 
            { 
                this.nop = value;
                _attachable.CurrentPosition = value.CurrentPosition;
            }
        }
        #endregion
    }
}
