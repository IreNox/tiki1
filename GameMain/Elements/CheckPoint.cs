using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TikiEngine.Elements.Physic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using TikiEngine.Elements.Graphics;
using Microsoft.Xna.Framework.Graphics;
using TikiEngine.States;
using TikiEngine.Components;
using Microsoft.Xna.Framework.Input;

namespace TikiEngine.Elements
{
    public class CheckPoint : IAttachable
    {
        private Sprite _cp0;
        private Sprite _cp1;

        private RotatedRectangle triggerX;

        private bool activated = false;

        public CheckPoint()
        {
            _cp0 = new Sprite(@"Environment/checkpoint_off0");
            _cp1 = new Sprite(@"Environment/checkpoint_off1");

            _cp0.LayerDepth = LayerDepthEnum.IslandForeGround + 0.01f;
            _cp1.LayerDepth = LayerDepthEnum.Character - 0.01f;

            GI.Content.Load<Texture2D>("Environment/checkpoint_on0");
            GI.Content.Load<Texture2D>("Environment/checkpoint_on1");

            triggerX = new RotatedRectangle(new Rectangle(0, 0, _cp1.Texture.Width, _cp1.Texture.Height));
        }
        public  void Draw(GameTime gameTime)
        {
            _cp0.Draw(gameTime);
            _cp1.Draw(gameTime);
#if DEBUG
            triggerX.Draw(gameTime);
#endif
        }

        public  void Update(GameTime gameTime)
        {
            if (!activated)
            {
                triggerX.Rotation = Angle;

                if (GI.RoboBounding.Intersects(triggerX))
                {
                    activated = true;

                    _cp0.TextureFile = "Environment/checkpoint_on0";
                    _cp1.TextureFile = "Environment/checkpoint_on1";

                    Program.Game.GetGameState<stateLevel>().SaveLevel(
                        triggerX.CurrentPosition() + new Vector2(_cp0.Texture.Width,_cp0.Texture.Height)/200
                    );                    
                }
            }
        }

        public float Angle
        {
            get { return _cp0.Angle; }
            set 
            { 
                _cp0.Angle = value;
                _cp1.Angle = value;

                triggerX.Rotation = value;
            }
        }

        public float LayerDepth
        {
            get { return _cp0.LayerDepth; }
            set { }
        }

        public Vector2 Offset
        {
            get { return _cp1.Offset; }
            set
            {
                value = new Vector2(value.X, value.Y + 0.1f);

                _cp0.Offset = value;
                _cp1.Offset = value;
            }
        }

        public Vector2 CurrentPosition
        {
            get { return _cp0.CurrentPosition; }
            set 
            { 
                _cp0.CurrentPosition = value;
                _cp1.CurrentPosition = value;

                Matrix rot = Matrix.CreateRotationZ(Angle);
                this.triggerX.ChangePosition(
                    ConvertUnits.ToDisplayUnits(value + Vector2.Transform(Offset, rot)) 
                    - new Vector2(triggerX.Width, triggerX.Height) / 2);
            }
        }
    }
}
