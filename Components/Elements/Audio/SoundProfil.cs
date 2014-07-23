using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikiEngine.Elements.Physic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Audio
{
    public abstract class SoundProfil
    {
        protected NameObjectPhysic nop;

        public SoundProfil(NameObjectPhysic nop)
        {
            this.nop = nop;
        }
        public virtual void Update(GameTime gameTime)
        {

        }
    }
    public class CubeSoundProfil : SoundProfil
    {
        private bool collided = false;
        private long timer = 0;
        private float volume = 0;
        private float friction = 0;
        private PhysicTexture sliding;

        private List<Material> GroundMaterials = new List<Material>
        {
                        Material.island,
            Material.block,
            Material.metal,
            Material.slide,
            Material.wood,
        };

        public CubeSoundProfil(NameObjectPhysic nop)
            :base(nop)
        {
            //GI.Sound[TikiSound.Cube_Grass];
            //GI.Sound[TikiSound.Cube_Metal];
            //GI.Sound[TikiSound.Cube_Wood];
            //GI.Sound[TikiSound.Cube_Spawn].Volume = 0.7f;
            //GI.Sound[TikiSound.Cube_Destroy].Volume = 0.1f;

            this.friction = nop.Body.Friction;

            this.nop.Body.OnCollision += new OnCollisionEventHandler(Body_OnCollision);
            this.nop.Body.OnSeparation += new OnSeparationEventHandler(Body_OnSeparation);
        }

        void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            Material mat = ((NameObjectPhysic)fixtureB.Body.UserData).Material;
            //if (mat == Material.charakter)
            //{
                timer = 0;
                volume = 0;
            //}
            //if (mat == Material.slide)
            //{
            //    GI.Sound.Pause(TikiSound.Cube_Slide);
            //}
        }
        bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            //if(fixtureB.Body.UserData is PhysicCircle || fixtureB.Body.UserData is Robo)

            if (collided)
                return true;

            Material mat = ((NameObjectPhysic)fixtureB.Body.UserData).Material;
            if (mat == Material.charakter)
            {
                timer = 0;
                volume = 0;
            }

            if (mat == Material.slide)
            {
                nop.Body.Friction = 0.1f;
                sliding = (PhysicTexture)fixtureB.Body.UserData;
            }
            else if(nop.Body.Friction != 0 && mat != Material.charakter)
                nop.Body.Friction = friction;


                

            switch (mat)
            {
                case Material.island:
                    GI.Sound.PlaySFX(TikiSound.Cube_Grass, volume);
                    //if(!GI.Sound.IsAlive(TikiSound.Cube_Grass))
                    //    GI.Sound.PlaySound(TikiSound.Cube_Grass).Volume = volume;
                    break;
                case Material.metal:
                case Material.block:
                case Material.slide:
                    GI.Sound.PlaySFX(TikiSound.Cube_Metal, volume);
                    //if (!GI.Sound.IsAlive(TikiSound.Cube_Metal))
                    //    GI.Sound.PlaySound(TikiSound.Cube_Metal).Volume = volume;
                    
                    //if (Slide())
                    //{
                    //    GI.Sound.PlaySound(TikiSound.Cube_Slide);
                    //}
                    break;
                case Material.wood:
                    GI.Sound.PlaySFX(TikiSound.Cube_Wood, volume);
                    //if (!GI.Sound.IsAlive(TikiSound.Cube_Wood))
                    //    GI.Sound.PlaySound(TikiSound.Cube_Wood).Volume = volume;
                    break;
                default:
                    break;
            }
            timer = 0;
            volume = 0;
            return true;// collided = true;
        }
        public bool Slide()
        {
            return nop.Body.Friction == 0.1f;
        }
        public void Slide(GameTime gameTime)
        {
            if (sliding != null && sliding.RotatedRectangle.Intersects(((PhysicBox)nop).Bounds))
            {
                float volume = GI.Sound[TikiSound.Cube_Slide].Volume;
                volume += 0.1f;
                volume = volume > 1 ? 1 : volume;
                GI.Sound.PlaySound(TikiSound.Cube_Slide).Volume = volume;
            }
            else
            {
                float volume = GI.Sound[TikiSound.Cube_Slide].Volume;
                volume -= 0.05f;
                volume = volume > 0 ? volume : 0;
                GI.Sound.PlaySound(TikiSound.Cube_Slide).Volume = volume;
            }


        }
        public override void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds;

            float koef = ((float)timer / 200f);
            koef = koef > 0.3 ? 0.3f : koef;
            koef = nop.Body.Friction < 1 ? 0 : koef;
            volume = koef;

            Slide(gameTime);



            //if(sliding != null )

            //if (Slide())
            //    GI.Sound.PlaySound(TikiSound.Cube_Slide).Volume = 1;
            //else
            //    GI.Sound.Stop(TikiSound.Cube_Slide);


            //Console.WriteLine(timer);
            
            base.Update(gameTime);
        }
    }
}
