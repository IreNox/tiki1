using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TikiEngine
{
    public class RotatedRectangle
    {
        public Rectangle collisionRectangle;
        private float rotation;
        private Vector2 origin;
        public static Texture2D texture = GI.Content.Load<Texture2D>("Elements/boundingBox");
        private Color color  = Color.White;
        private Vector2 offset = Vector2.Zero;

        public RotatedRectangle(RotatedRectangle rotRec, Vector2 size)
            :this(new Rectangle(rotRec.X,rotRec.Y,rotRec.Width + (int)size.X, rotRec.Height + (int)size.Y))
        {
            offset = size;
        }

        public RotatedRectangle(Rectangle collisionRectangle, float rotation)
            :this(collisionRectangle)
        {
            this.rotation = MathHelper.ToRadians(rotation);
        }

        public RotatedRectangle(Rectangle collisionRectangle)
        {
            this.collisionRectangle = collisionRectangle;

            Origin = new Vector2((int)collisionRectangle.Width / 2, (int)collisionRectangle.Height / 2);
        }

        public void Update(GameTime gameTime)
        {
            
        }

#if DEBUG
        public void Draw(GameTime gameTime)
        {
            if (!GI.DEBUG)
                return;
            Rectangle aPositionAdjusted = new Rectangle(X + (Width / 2), Y + (Height / 2), Width, Height);

            GI.SpriteBatch.Draw(
                RotatedRectangle.texture,
                aPositionAdjusted,
                new Rectangle(0,0,100,100),
                color,
                Rotation,
                new Vector2(50),// Origin,
                SpriteEffects.None,
                LayerDepthEnum.Debug);
        }
#endif

        public void ChangePosition(Vector2 position)
        {
            collisionRectangle.X = (int)position.X - (int)offset.X/2;// -Width / 2;
            collisionRectangle.Y = (int)position.Y - (int)offset.Y / 2;// -Height / 2;
        }
        public Vector2 CurrentPosition()
        {
            return ConvertUnits.ToSimUnits(new Vector2(collisionRectangle.X, collisionRectangle.Y));
        }

        public bool Intersects(RotatedRectangle theRectangle)
        {
            //Calculate the Axis we will use to determine if a collision has occurred
            //Since the objects are rectangles, we only have to generate 4 Axis (2 for
            //each rectangle) since we know the other 2 on a rectangle are parallel.
            List<Vector2> aRectangleAxis = new List<Vector2>();
            aRectangleAxis.Add(UpperRightCorner() - UpperLeftCorner());
            aRectangleAxis.Add(UpperRightCorner() - LowerRightCorner());
            aRectangleAxis.Add(theRectangle.UpperLeftCorner() - theRectangle.LowerLeftCorner());
            aRectangleAxis.Add(theRectangle.UpperLeftCorner() - theRectangle.UpperRightCorner());

            //Cycle through all of the Axis we need to check. If a collision does not occur
            //on ALL of the Axis, then a collision is NOT occurring. We can then exit out 
            //immediately and notify the calling function that no collision was detected. If
            //a collision DOES occur on ALL of the Axis, then there is a collision occurring
            //between the rotated rectangles. We know this to be true by the Seperating Axis Theorem
            foreach (Vector2 aAxis in aRectangleAxis)
            {
                if (!IsAxisCollision(theRectangle, aAxis))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsAxisCollision(RotatedRectangle theRectangle, Vector2 aAxis)
        {
            //Project the corners of the Rectangle we are checking on to the Axis and
            //get a scalar value of that project we can then use for comparison
            List<int> aRectangleAScalars = new List<int>();
            aRectangleAScalars.Add(GenerateScalar(theRectangle.UpperLeftCorner(), aAxis));
            aRectangleAScalars.Add(GenerateScalar(theRectangle.UpperRightCorner(), aAxis));
            aRectangleAScalars.Add(GenerateScalar(theRectangle.LowerLeftCorner(), aAxis));
            aRectangleAScalars.Add(GenerateScalar(theRectangle.LowerRightCorner(), aAxis));

            //Project the corners of the current Rectangle on to the Axis and
            //get a scalar value of that project we can then use for comparison
            List<int> aRectangleBScalars = new List<int>();
            aRectangleBScalars.Add(GenerateScalar(UpperLeftCorner(), aAxis));
            aRectangleBScalars.Add(GenerateScalar(UpperRightCorner(), aAxis));
            aRectangleBScalars.Add(GenerateScalar(LowerLeftCorner(), aAxis));
            aRectangleBScalars.Add(GenerateScalar(LowerRightCorner(), aAxis));

            //Get the Maximum and Minium Scalar values for each of the Rectangles
            int aRectangleAMinimum = aRectangleAScalars.Min();
            int aRectangleAMaximum = aRectangleAScalars.Max();
            int aRectangleBMinimum = aRectangleBScalars.Min();
            int aRectangleBMaximum = aRectangleBScalars.Max();

            //If we have overlaps between the Rectangles (i.e. Min of B is less than Max of A)
            //then we are detecting a collision between the rectangles on this Axis
            if (aRectangleBMinimum <= aRectangleAMaximum && aRectangleBMaximum >= aRectangleAMaximum)
            {
                return true;
            }
            else if (aRectangleAMinimum <= aRectangleBMaximum && aRectangleAMaximum >= aRectangleBMaximum)
            {
                return true;
            }

            return false;
        }

        private int GenerateScalar(Vector2 theRectangleCorner, Vector2 theAxis)
        {
            //Using the formula for Vector projection. Take the corner being passed in
            //and project it onto the given Axis
            float aNumerator = (theRectangleCorner.X * theAxis.X) + (theRectangleCorner.Y * theAxis.Y);
            float aDenominator = (theAxis.X * theAxis.X) + (theAxis.Y * theAxis.Y);
            float aDivisionResult = aNumerator / aDenominator;
            Vector2 aCornerProjected = new Vector2(aDivisionResult * theAxis.X, aDivisionResult * theAxis.Y);

            //Now that we have our projected Vector, calculate a scalar of that projection
            //that can be used to more easily do comparisons
            float aScalar = (theAxis.X * aCornerProjected.X) + (theAxis.Y * aCornerProjected.Y);
            return (int)aScalar;
        }

        private Vector2 RotatePoint(Vector2 thePoint, Vector2 theOrigin, float theRotation)
        {
            Vector2 aTranslatedPoint = new Vector2();
            aTranslatedPoint.X = (float)(theOrigin.X + (thePoint.X - theOrigin.X) * Math.Cos(theRotation)
                - (thePoint.Y - theOrigin.Y) * Math.Sin(theRotation));
            aTranslatedPoint.Y = (float)(theOrigin.Y + (thePoint.Y - theOrigin.Y) * Math.Cos(theRotation)
                + (thePoint.X - theOrigin.X) * Math.Sin(theRotation));
            return aTranslatedPoint;
        }

        public Vector2 UpperLeftCorner()
        {
            Vector2 aUpperLeft = new Vector2(CollisionRectangle.Left, CollisionRectangle.Top);
            aUpperLeft = RotatePoint(aUpperLeft, aUpperLeft + Origin, Rotation);
            return aUpperLeft;
        }

        public Vector2 UpperRightCorner()
        {
            Vector2 aUpperRight = new Vector2(CollisionRectangle.Right, CollisionRectangle.Top);
            aUpperRight = RotatePoint(aUpperRight, aUpperRight + new Vector2(-Origin.X, Origin.Y), Rotation);
            return aUpperRight;
        }

        public Vector2 LowerLeftCorner()
        {
            Vector2 aLowerLeft = new Vector2(CollisionRectangle.Left, CollisionRectangle.Bottom);
            aLowerLeft = RotatePoint(aLowerLeft, aLowerLeft + new Vector2(Origin.X, -Origin.Y), Rotation);
            return aLowerLeft;
        }

        public Vector2 LowerRightCorner()
        {
            Vector2 aLowerRight = new Vector2(CollisionRectangle.Right, CollisionRectangle.Bottom);
            aLowerRight = RotatePoint(aLowerRight, aLowerRight + new Vector2(-Origin.X, -Origin.Y), Rotation);
            return aLowerRight;
        }


        public Rectangle CollisionRectangle
        {
            get { return this.collisionRectangle; }
            set { this.collisionRectangle = value; }
        }
        public float Rotation
        {
            get { return this.rotation; }
            set { this.rotation = value; }
        }
        public Vector2 Origin
        {
            get { return this.origin; }
            set { this.origin = value; }
        }
        public int X
        {
            get { return CollisionRectangle.X; }
        }

        public int Y
        {
            get { return CollisionRectangle.Y; }
        }

        public int Width
        {
            get { return CollisionRectangle.Width; }
        }

        public int Height
        {
            get { return CollisionRectangle.Height; }
        }

        public Color Color
        {
            get { return this.color; }
            set { this.color = value; }
        }

        public override string ToString()
        {
            return this.CollisionRectangle.ToString();
        }
    }
}
