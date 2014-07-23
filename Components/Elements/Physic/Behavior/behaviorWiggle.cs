using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;

namespace TikiEngine.Elements.Physic
{

    /*
     *     
     *     INSEL :D 
     * -----------------------------------------------
     * \                                             /
     *  \         A                          A      /   
     *   \           O#########x#########O         /
     *    \       A                          A    /
     *     \ ______/\__________/\________________/
     * 
     *     LEGENDE
     *     x = position/origin
     *     O localAnchor
     *     A Ancherpunkt
     * 
     *     VERÄNDERBAR
     * 
     *     Rotation     Winkel in Degrees(+-Grad) um den origin(x)
     *     Angle        Je Höher der Winkel desto weiter sind die Ancherpunkte(A) vom LocalAnchor(O)
     *     Space        Die Entfernung zwischen A und x auf der X-Achse
     *                  0 -> A und O sind auf gleicher Höhe
     *                  + -> A liegt weiter von x entfernt als O
     *                  - -> A liegt näher als O an x
     *     Frequency    Frequenz in der die Joints versuchen ihre Länge zu erreichen.
     *                  Je höher die Frequenz, desto schneller pendeln sich die Inseln aus bzw haben weniger Resonanz.
     *                  Je geringer die Frequenz, desto mehr wackeln die Inseln.
     *     
     */

    [Serializable]
    public class behaviorWiggle : Behavior
    {
        #region Vars
        private float frequency = 2f;

        private Vector2 localAnchor;

        private float angle = 10f;
        private float space = 2f;
        private float rotation = 0f;

        private FixedDistanceJoint _joinTL;
        private FixedDistanceJoint _joinTR;
        private FixedDistanceJoint _joinBL;
        private FixedDistanceJoint _joinBR;

        private List<FixedDistanceJoint> joints = new List<FixedDistanceJoint>();
        #endregion

        #region Init
        public behaviorWiggle(NameObjectPhysic nop)
            : base(nop)
        {
            this.localAnchor = new Vector2(ConvertUnits.ToSimUnits(nop.TextureSize.X), 0) / 3;
        }

        public behaviorWiggle(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Member
#if DEBUG
        public override void Draw(GameTime gameTime)
        {
            if (!GI.DEBUG)
                return;

            Texture2D texture = GI.Content.Load<Texture2D>("background");

            foreach (Joint j in joints)
            {
                GI.SpriteBatch.Draw(texture,
                    new Rectangle(
                        (int)ConvertUnits.ToDisplayUnits(j.WorldAnchorA.X),
                        (int)ConvertUnits.ToDisplayUnits(j.WorldAnchorA.Y),
                        10,
                        10),
                        null,
                        Color.White,
                        0f,
                        new Vector2(5),
                        SpriteEffects.None,
                        (float)LayerDepthEnum.Debug);
                GI.SpriteBatch.Draw(texture,
                    new Rectangle(
                        (int)ConvertUnits.ToDisplayUnits(j.WorldAnchorB.X),
                        (int)ConvertUnits.ToDisplayUnits(j.WorldAnchorB.Y),
                        10,
                        10),
                        null,
                        Color.Red,
                        0f,
                        new Vector2(5),
                        SpriteEffects.None,
                        (float)LayerDepthEnum.Debug);
            }
        }
#endif

        public override void ApplyChanges()
        {
            if (joints.Count != 0)
            {
                joints.ForEach(j => world.RemoveJoint(j));
                joints.Clear();
            }

            body.BodyType = BodyType.Dynamic;

            body.Rotation = MathHelper.ToRadians(-this.rotation);

            Vector2 position = body.Position;

            Vector2 distance = new Vector2(0, -localAnchor.X - space);

            float rot = 270f - rotation;

            Matrix distanceM = Matrix.CreateRotationZ(MathHelper.ToRadians(rot));

            Matrix rot1M = Matrix.CreateRotationZ(MathHelper.ToRadians(rot + angle));
            Matrix rot2M = Matrix.CreateRotationZ(MathHelper.ToRadians(rot - angle));

            Matrix rot3M = Matrix.CreateRotationZ(MathHelper.ToRadians(rot - 180 + angle));
            Matrix rot4M = Matrix.CreateRotationZ(MathHelper.ToRadians(rot - 180 - angle));

            Vector2 tl = Vector2.Transform(distance, rot1M);
            Vector2 bl = Vector2.Transform(distance, rot2M);
            Vector2 tr = Vector2.Transform(distance, rot3M);
            Vector2 br = Vector2.Transform(distance, rot4M);

            distance = Vector2.Transform(distance, distanceM);

            float length = (distance - tl).Length();

            _joinTL = new FixedDistanceJoint(
                body, -localAnchor, position + tl);
            _joinBL = new FixedDistanceJoint(
                body, -localAnchor, position + bl);

            _joinTR = new FixedDistanceJoint(
                body, localAnchor, position + tr);
            _joinBR = new FixedDistanceJoint(
                body, localAnchor, position + br);

            this.joints.Add(_joinTL);
            this.joints.Add(_joinBL);
            this.joints.Add(_joinTR);
            this.joints.Add(_joinBR);

            this.Frequency = this.frequency;

            joints.ForEach(j => world.AddJoint(j));
            joints.ForEach(j => j.Length = length);
        }

        public override void Update(GameTime gameTime)
        {
        }
        #endregion

        #region Properties
        public float Rotation
        {
            get { return this.rotation; }
            set
            {
                this.rotation = value;
                this.ApplyChanges();
            }
        }

        public float Space
        {
            get { return this.space; }
            set
            {
                this.space = value;
                this.ApplyChanges();
            }
        }

        public float Angle
        {
            get { return this.angle; }
            set
            {
                this.angle = value;
                this.ApplyChanges();
            }
        }

        public float Frequency
        {
            get { return frequency; }
            set
            {
                this.frequency = value;
                joints.ForEach(j => j.Frequency = value);
            }
        }
        #endregion
    }
}
