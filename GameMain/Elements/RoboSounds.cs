using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TikiEngine.Elements.Physic;
using FarseerPhysics.Dynamics;

namespace TikiEngine.Elements
{
    internal partial class Robo
    {
        private TikiSound currentSound;
        private float volume = 0;

        #region Update
        public void UpdateSound(GameTime gameTime)
        {
            #region controls
            if (onGround && !jump && (Left() || Right()))
                volume = (volume + 0.05f) > 1 ? 1 : (volume + 0.05f);
            else
                volume = 0;
            #endregion
            
            #region landen
            switch (currentSound)
            {
                case TikiSound.Robo_Grass_Land:
                    if (!IsAlive())
                        currentSound = TikiSound.Robo_Grass_Loop;
                    break;
                case TikiSound.Robo_Metal_Land:
                    if (!IsAlive())
                    {
                        currentSound = TikiSound.Robo_Metal_Loop;
                    }
                    break;
                case TikiSound.Robo_Wood_Land:
                    if (!IsAlive())
                        currentSound = TikiSound.Robo_Wood_Loop;
                    break;
            }
            #endregion
            #region movement
            switch (currMaterial)
            {
                #region grass
                case Physic.Material.island:
                    if (onGround && !jump)
                    {
                        if (Left() || Right())
                            FadeIn(TikiSound.Robo_Grass_Loop);
                        else
                            FadeOut(TikiSound.Robo_Grass_Loop);
                    }
                    break;
                #endregion
                #region metal
                case Physic.Material.block:
                case Physic.Material.metal:
                    if (onGround && !jump)
                    {
                        if (Left() || Right())
                            FadeIn(TikiSound.Robo_Metal_Loop);
                        else
                            FadeOut(TikiSound.Robo_Metal_Loop);
                    }
                    break;
                #endregion
                #region wood
                case Physic.Material.wood:
                    if (onGround && !jump)
                    {
                        if (Left() || Right())
                            FadeIn(TikiSound.Robo_Wood_Loop);
                        else
                            FadeOut(TikiSound.Robo_Wood_Loop);
                    }
                    break;
                #endregion
                #region slide
                case Physic.Material.slide:
                    if (onGround && !jump)
                    {

                        FadeIn(TikiSound.Robo_Slide);
                    }
                    break;  
                #endregion
            }
            #endregion
        }
        public void UpdateMaterialSound()
        {
            if (prevMaterial != currMaterial)
            {
                if(GI.Sound.IsAlive(TikiSound.Robo_Grass_Loop))
                    FadeOut(TikiSound.Robo_Grass_Loop);
                if(GI.Sound.IsAlive(TikiSound.Robo_Metal_Loop))
                    FadeOut(TikiSound.Robo_Metal_Loop);
                if (GI.Sound.IsAlive(TikiSound.Robo_Wood_Loop))
                    FadeOut(TikiSound.Robo_Wood_Loop);
                if (GI.Sound.IsAlive(TikiSound.Robo_Slide))
                    FadeOut(TikiSound.Robo_Slide); //Fade(TikiSound.Robo_Slide, -0.1f);

            }
        }


        #endregion
        #region special events
        public void GroundCollisionSound(Fixture f)
        {
            if (!jump && f == fixtureCollision )
                return;
            switch (currMaterial)
            {
                case Physic.Material.island:
                    if(IsMuting(TikiSound.Robo_Grass_Loop))
                        PlaySFX(TikiSound.Robo_Grass_Land);
                    break;
                case Physic.Material.metal:
                    if (IsMuting(TikiSound.Robo_Metal_Loop))
                        PlaySFX(TikiSound.Robo_Metal_Land);
                    break;
                case Physic.Material.block:
                    if (IsMuting(TikiSound.Robo_Metal_Loop))
                        PlaySFX(TikiSound.Robo_Metal_Land);
                    break;
                case Physic.Material.slide:
                    if (IsMuting(TikiSound.Robo_Slide))
                        PlaySFX(TikiSound.Robo_Metal_Land);
                    break;
                case Physic.Material.wood:
                    if (IsMuting(TikiSound.Robo_Wood_Loop))
                        PlaySFX(TikiSound.Robo_Wood_Land);
                    break;
            }
        }
        public bool IsMuting(TikiSound ts)
        {
            return GI.Sound.IsFading(ts);
        }
        public void SoundJump()
        {
            switch (currMaterial)
            {
                #region grass
                case Physic.Material.island:
                    FadeOut(TikiSound.Robo_Grass_Loop);
                    GI.Sound.PlaySound(TikiSound.Robo_Grass_Jump);
                    break;
                #endregion

                #region metal
                case Physic.Material.block:
                case Physic.Material.metal:
                    FadeOut(TikiSound.Robo_Metal_Loop);
                    GI.Sound.PlaySound(TikiSound.Robo_Metal_Jump);
                    break;
                #endregion

                #region wood
                case Physic.Material.wood:
                    FadeOut(TikiSound.Robo_Wood_Loop);
                    GI.Sound.PlaySound(TikiSound.Robo_Wood_Jump);
                    break;
                #endregion

                #region slide
                case Physic.Material.slide:
                    FadeOut(TikiSound.Robo_Slide);
                    GI.Sound.PlaySound(TikiSound.Robo_Metal_Jump);
                    break;
                #endregion
            }
        }
        public void SoundFall()
        {
            FadeOut(TikiSound.Robo_Grass_Loop);
            FadeOut(TikiSound.Robo_Metal_Loop);
            FadeOut(TikiSound.Robo_Wood_Loop);
            FadeOut(TikiSound.Robo_Slide);
        }
        public void SoundPushingStart()
        {
            TikiSound sound = TikiSound.Atmo;
            switch (currMaterial)
            {
                case Physic.Material.island:
                    sound = TikiSound.Cube_Pushing_Grass;
                    break;
                case Physic.Material.block:
                case Physic.Material.slide:
                case Physic.Material.metal:
                    sound = TikiSound.Cube_Pushing_Metal;
                    break;
                case Physic.Material.wood:
                    sound = TikiSound.Cube_Pushing_Wood;
                    break;
            }
            if (GI.Sound.IsAlive(sound))
            {
                float volume = GI.Sound[sound].Volume;
                volume += 0.1f;
                volume = volume > 1 ? 1 : volume;
                GI.Sound.SetVolume(sound, volume);
                //GI.Sound[sound].Volume = volume;
            }
            else
            {
                GI.Sound.Resume(sound).Volume = 0f;
            }
                
        }
        public void SoundPushingStop()
        {
            GI.Sound.Pause(TikiSound.Cube_Pushing_Grass);
            GI.Sound.Pause(TikiSound.Cube_Pushing_Metal);
            GI.Sound.Pause(TikiSound.Cube_Pushing_Wood);
        }
        #endregion
        #region toolkit

        public void FadeOut(TikiSound ts)
        {
            GI.Sound.Fade(ts, -0.1f);
        }
        public void FadeIn(TikiSound ts)
        {
            GI.Sound.Fade(ts, 0.1f);
        }
        public void Pause(TikiSound ts)
        {
            GI.Sound.Stop(ts);
            //GI.Sound.Get(Tts).
        }
        public void Resume(TikiSound ts)
        {
            //GI.Sound.Get(ts).Volume = volume;
            GI.Sound.PlaySound(ts);
            //GI.Sound.Resume(ts);
        }
        public void PlaySFX(TikiSound ts)
        {
            GI.Sound.Pause(currentSound);
            currentSound = ts;
            GI.Sound.PlaySound(ts);
        }
        public bool IsAlive()
        {
            return GI.Sound.IsAlive(currentSound);
        }
        #endregion
    }
}
