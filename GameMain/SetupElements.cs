using System;
using TikiEngine.Elements.Physic;
using Microsoft.Xna.Framework;
using TikiEngine.Elements;
using FarseerPhysics.Dynamics;
using TikiEngine.Elements.Trigger;
using TikiEngine.Elements.Audio;
using System.Collections.Generic;

namespace TikiEngine
{
    internal static partial class Setup
    {
        #region vars

        public static  float defaultIslandFriction = 50f;
        public static  float defaultIslandDensity = 1;

        private static List<NameObjectPhysic> pipes = new List<NameObjectPhysic>();
        #endregion

        #region Trigger
        #region CameraTrigger
        public static GameTrigger CreateCameraTrigger(Rectangle r, Vector2 offset, Vector2 offsetSpeed, Body body)
        {
            return new FreeTrigger(r, offset, offsetSpeed, body); ;
        }
        public static GameTrigger CreateCameraTrigger(Rectangle r, float rotation, Vector2 offset, Vector2 offsetSpeed, Body body)
        {
            return new FreeTrigger(r, rotation, offset, offsetSpeed, body); ;
        }
        #endregion
        #endregion

        #region default
#if DEBUG
        public static PhysicTexture CreateDebugIsland(String path, Vector2 pos)
        {
            PhysicTexture p = Setup.CreateIsland(path, pos);
            p.TextureFile = "Islands/" + path + "_c";
            p.AddBehavior<behaviorDebug>();

            return p;
        }
#endif

        private static PhysicTexture CreateIsland(String path, Vector2 pos)
        {
            return CreateIsland(path, pos, 50, 10f);
        }

        private static PhysicTexture CreateIsland(String path, Vector2 pos, float density)
        {
            return CreateIsland(path, pos, density, 10);
        }

        private static PhysicTexture CreateIsland(String path, float friction, Vector2 pos)
        {
            return CreateIsland(path, pos, 10f, friction);
        }

        private static PhysicTexture CreateIsland(String path, Vector2 pos, float density, float friction)
        {
            PhysicTexture p = new PhysicTexture();

            p.Density = density;

            p.TextureFile = "Islands/" + path + "_n";
            p.TextureBodyFile = "Islands/" + path + "_c";

            p.BodyType = BodyType.Dynamic;

            p.Body.Friction = friction;
            p.StartPosition = pos;

            p.LayerDepth = LayerDepthEnum.Island;
            p.Body.Restitution = 0;

            p.Material = Material.island;

            return p;
        }

        #endregion

        #region Fixed/Static
        public static PhysicTexture CreateWhipIsland(String path, Vector2 pos, Vector2 Distance)
        {
            PhysicTexture p = CreateIsland(path, pos, 1, 100);
            p.Material = Material.wood;
            p.AddBehavior<behaviorFixedDistance>().Distance = Distance;
            p.GetBehavior<behaviorFixedDistance>().AddEventListener();

            p.OnRest += new PhysicTexture.OnReset(Whip_OnRest);

            return p;
        }

        static void Whip_OnRest(PhysicTexture p)
        {
            p.CurrentPosition = p.StartPosition;
            p.BodyType = BodyType.Static;
            p.Body.LinearDamping = 3f;
        }

        public static PhysicTexture CreateStaticIsland(String path, Vector2 pos)
        {
            PhysicTexture p = CreateIsland(path, pos);
            p.BodyType = BodyType.Static;
            return p;
        }
        #endregion

        #region Pipe
        public static PhysicTexture CreatePipe(String path, Vector2 pos, float rot)
        {
            PhysicTexture p = CreateIsland(path, pos + new Vector2(0,100f));
            p.BodyType = BodyType.Static;
            p.Body.Rotation = MathHelper.ToRadians(rot);
            p.Body.Friction = 0.1f;
            p.Material = Material.slide;
            p.RotatedRectangle = new RotatedRectangle(p.RotatedRectangle, new Vector2(0, 50));
            p.LayerDepth = LayerDepthEnum.IslandBackground;

            foreach (NameObjectPhysic nop in pipes.ToArray())
            {
                if (GI.World.BodyList.Contains(nop.Body))
                {
                    nop.Body.IgnoreCollisionWith(p.Body);
                    p.Body.IgnoreCollisionWith(nop.Body);
                }
                else
                {
                    pipes.Remove(nop);
                }

            }
            pipes.Add(p);

            p.StartPosition = p.CurrentPosition - new Vector2(0,100f);
            p.CurrentPosition = p.StartPosition;
            p.Body.IsStatic = true;

            return p;
        }
        #endregion

        #region Wiggle
        public static PhysicTexture CreateWiggleIsland(String path, Vector2 pos)
        {
            PhysicTexture p = CreateWiggleIsland(path, pos, 1f, 0);
            return p;
        }

        public static PhysicTexture CreateWiggleIsland(String path, Vector2 pos, float density)
        {
            PhysicTexture p = CreateWiggleIsland(path, pos, density, 0);
            return p;
        }

        public static PhysicTexture CreateWiggleIsland(String path, float rotation, Vector2 pos)
        {
            PhysicTexture p = CreateWiggleIsland(path, pos, 1, rotation);
            return p;
        }

        public static PhysicTexture CreateWiggleIsland(String path, Vector2 pos, float density, float rotation)
        {
            PhysicTexture p = CreateIsland(path, pos, density);
            p.AddBehavior<behaviorWiggle>().Rotation = rotation;
            p.GetBehavior<behaviorWiggle>().Frequency = 2.5f;
            return p;
        }

        #endregion

        #region Move

        public static PhysicTexture CreateMovingIsland(String path, Vector2 pos, Vector2 targetPos, long time)// float speed, float distance)
        {
            PhysicTexture p = CreateIsland(path, pos);
            p.Material = Material.metal;
            p.AddBehavior<behaviorMoving>().TargetPos = targetPos;
            p.GetBehavior<behaviorMoving>().Time = time;

            p.OnRest += new PhysicTexture.OnReset(Moving_OnRest);

            return p;
        }

        static void Moving_OnRest(PhysicTexture p)
        {
            p.GetBehavior<behaviorMoving>().Reset();
        }

        #endregion

        #region Sink
        public static PhysicTexture CreateSinkIsland(String path, Vector2 pos, float limit)
        {
            PhysicTexture p = CreateIsland(path, pos);
            p.AddBehavior<behaviorSink>().Limit = limit;
            return p;
        }
        #endregion

        #region Teeter

        public static PhysicTexture CreateTeeterIsland(String path, Vector2 pos)
        {
            return CreateTeeterIsland(path, pos, Vector2.Zero, 0, 0.7f);
        }
        public static PhysicTexture CreateTeeterIsland(String path, Vector2 pos, float damping, float density)
        {
            return CreateTeeterIsland(path, pos, Vector2.Zero,damping, density);
        }
        public static PhysicTexture CreateTeeterIsland(String path, Vector2 pos, Vector2 localAnchor, float damping, float density)
        {
            PhysicTexture p = CreateIsland(path, pos, density, 0.5f);
            p.AddBehavior<behaviorTeeter>().Damping = damping;
            p.GetBehavior<behaviorTeeter>().LocalAnchor = localAnchor;
            return p;
        }

        #endregion

        public static void CreateCube(Vector2 pos)
        {
            new CubeStone(pos);
        }

        #region animation

        public static CollisionTrigger CreateAnimationTrigger(PhysicTexture pt)
        {
            CollisionTrigger ct = new CollisionTrigger(pt);
            ct.Unique = true;
            ct.OnCollision(true);
            return ct;
        }
        #endregion
    }
}
