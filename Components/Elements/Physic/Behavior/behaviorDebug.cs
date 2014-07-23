#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;


/*
 * Manual:
 * 
 * Keys : Pfeiltaste, Strg, Alt, +, -, /, *, F1
 * 
 * / bzw Divide         -> Kamera auf Körper zentrieren/ Kamera auf Charakter zurücksetzen
 * 
 * Pfeiltasten          -> Navigation des Bezugspunktes
 * *                    -> Bezugspunkt zurücksetzen
 * Alt + Pfeiltasten    -> Inselbewegen
 * Strg                 -> Geschwindigkeitsmodifikator für obere beide Bewegungen
 * 
 * +                    -> Rotation der Insel erhöhen
 * -                    -> Rotation der Insel verringern
 * F1                   -> Werte Ausgeben lassen
 * 
 * 
 * Zweck der Klasse     Diese Klasse dient zur Findung von Punkten auf dem Asset und der Positionierung in der Welt.
 *                      Man verschiebt alles auf die gewünschten Positionen lässt sich die Werte ausgeben und hardcoded sie.
 *                      Danach Freuen :D
 */


namespace TikiEngine.Elements.Physic
{
    public class behaviorDebug : Behavior
    {
        private Vector2 position = Vector2.Zero;
        private Body trackingBody;

        public behaviorDebug(NameObjectPhysic nop)
            : base(nop)
        {

        }

        public override void ApplyChanges()
        {
            this.nop.BodyType = BodyType.Static;
            this.position = Vector2.Zero;
            this.nop.Body.CollidesWith = Category.None;
        }

#if DEBUG
        public override void Draw(GameTime gameTime)
        {
            if (!GI.DEBUG)
                return;

            Texture2D texture = GI.Content.Load<Texture2D>("Elements/circle");

            GI.SpriteBatch.Draw(texture,
                new Rectangle(
                    (int)ConvertUnits.ToDisplayUnits(this.nop.Body.Position.X),
                    (int)ConvertUnits.ToDisplayUnits(this.nop.Body.Position.Y),
                    10,
                    10),
                    null,
                    Color.CornflowerBlue,
                    0f,
                    new Vector2(5),
                    SpriteEffects.None,
                    (float)LayerDepthEnum.Debug);

            GI.SpriteBatch.Draw(texture,
                new Rectangle(
                    (int)ConvertUnits.ToDisplayUnits(this.nop.Body.Position.X + this.position.X),
                    (int)ConvertUnits.ToDisplayUnits(this.nop.Body.Position.Y + this.position.Y),
                    10,
                    10),
                    null,
                    Color.White,
                    0f,
                    new Vector2(5),
                    SpriteEffects.None,
                    (float)LayerDepthEnum.Debug);
        }
#endif

        public override void Update(GameTime gameTime)
        {

            if(GI.Control.KeyboardPressed(Keys.Z))
            {
                if (this.trackingBody == null)
                {
                    this.trackingBody = GI.Camera.TrackingBody;
                    GI.Camera.TrackingBody = this.nop.Body;
                }
                this.trackingBody.Position = this.nop.Body.Position;
                this.trackingBody.LinearVelocity = Vector2.Zero;
            }

            if (GI.Control.KeyboardPressed(Keys.NumPad0))
                Console.WriteLine("Positon des DebugPoints" + this.position);

            if(GI.Control.KeyboardPressed(Keys.Divide))
            {
                if (GI.Camera.TrackingBody != this.nop.Body)
                {
                    this.trackingBody = GI.Camera.TrackingBody;
                    GI.Camera.TrackingBody = this.nop.Body;
                }
                else
                {
                    GI.Camera.TrackingBody = this.trackingBody;
                }
            }

            bool strg = GI.Control.KeyboardDown(Keys.LeftControl) || GI.Control.KeyboardDown(Keys.RightControl);

            bool alt = GI.Control.KeyboardDown(Keys.LeftAlt);

            float strgK = 10f;
            float altK = 1f;

            //left
            if (GI.Control.KeyboardDown(Keys.Left))
            {
                if (alt)
                {
                    this.nop.CurrentPosition += new Vector2(!strg ? -0.1f : -0.1f * strgK,0);
                }
                else
                {
                    this.position += new Vector2(!strg ? -0.01f : -0.1f * altK, 0);
                }
                
            }

            //right
            if (GI.Control.KeyboardDown(Keys.Right))
            {
                if (alt)
                {
                    this.nop.CurrentPosition += new Vector2(!strg ? 0.1f : 0.1f * strgK, 0);
                }
                else
                {
                    this.position += new Vector2(!strg ? 0.01f : 0.1f * altK, 0);
                }
                
            }                

            //up
            if (GI.Control.KeyboardDown(Keys.Up))
            {
                if (alt)
                {
                    this.nop.CurrentPosition += new Vector2(0, !strg ? -0.1f : -0.1f * strgK);
                }
                else
                {
                    this.position += new Vector2(0, !strg ? -0.01f : -0.1f * altK);
                }
                
            }               

            //down
            if (GI.Control.KeyboardDown(Keys.Down))
            {
                if (alt)
                {
                    this.nop.CurrentPosition += new Vector2(0,!strg ? 0.1f : 0.1f * strgK);
                }
                else
                {
                    this.position += new Vector2(0, !strg ? 0.01f : 0.1f * altK);
                }
            }


            if(GI.Control.KeyboardPressed(Keys.Add))
            {
                this.nop.Body.Rotation += MathHelper.ToRadians(1);
            }
            if(GI.Control.KeyboardPressed(Keys.Subtract))
            {
                this.nop.Body.Rotation -= MathHelper.ToRadians(1);
            }

            if (GI.Control.KeyboardPressed(Keys.F1))
            {
                Console.WriteLine("Body Position : " + this.nop.Body.Position);
                Console.WriteLine("Relative Point Position : " + this.position);
                Console.WriteLine("Rotation : " + MathHelper.ToDegrees( -this.nop.Body.Rotation));
            }

            if (GI.Control.KeyboardPressed(Keys.Multiply))
                this.position = Vector2.Zero;
        }

    }
}
#endif