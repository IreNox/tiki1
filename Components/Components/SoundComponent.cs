using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace TikiEngine.Components
{
    public class SoundComponent : GameComponent
    {
        #region Vars
        private float _musicVolume = 1;
        private float _soundVolume = 1;
        private float soundRange = 500;

        private List<SoundEffectInstance> ambientSounds = new List<SoundEffectInstance>();
        private Dictionary<TikiSound, SoundEffect> soundFX = new Dictionary<TikiSound, SoundEffect>();
        private Dictionary<TikiSound, SoundEffectInstance> sounds = new Dictionary<TikiSound, SoundEffectInstance>();

        private Dictionary<TikiSound, float> fade = new Dictionary<TikiSound, float>();
        private Dictionary<SoundEffectInstance, TikiSound> _instanceCache = new Dictionary<SoundEffectInstance, TikiSound>();
        #endregion

        #region Init
        public SoundComponent()
            : base(GI.Game)
        {
            MediaPlayer.Volume = 0;
            MediaPlayer.IsRepeating = true;

            foreach (TikiSound se in Enum.GetValues(typeof(TikiSound)))
            {
                this.LoadSound(se);
            }
            base.Initialize();

            FadeLoop(TikiSound.Robo_Grass_Loop);
            FadeLoop(TikiSound.Robo_Metal_Loop);
            FadeLoop(TikiSound.Robo_Wood_Loop);
            FadeLoop(TikiSound.Robo_Slide);


            SetLoop(TikiSound.Cube_Pushing_Grass);
            SetLoop(TikiSound.Cube_Pushing_Metal);
            SetLoop(TikiSound.Cube_Pushing_Wood);
            SetLoop(TikiSound.Cube_Slide);
            SetLoop(TikiSound.Menu_Hover);
        }
        #endregion

        #region Private Member
        private void _setSystemSound(float newSoundVolume, float newMusicVolume)
        {
            newMusicVolume = newMusicVolume > 1 ? 1 : newMusicVolume;
            newMusicVolume = newMusicVolume < 0.0001f ? 0.0001f : newMusicVolume;

            newSoundVolume = newSoundVolume > 1 ? 1 : newSoundVolume;
            newSoundVolume = newSoundVolume < 0.0001f ? 0.0001f : newSoundVolume;

            foreach (TikiSound sound in Enum.GetValues(typeof(TikiSound)))
            {
                _setInstanceVolume(
                    sound,
                    sounds[sound],
                    newSoundVolume,
                    newMusicVolume
                );
            }

            foreach (var kvp in _instanceCache)
            {
                _setInstanceVolume(
                    kvp.Value,
                    kvp.Key,
                    newSoundVolume,
                    newMusicVolume
                );                
            }

            _musicVolume = newMusicVolume;
            _soundVolume = newSoundVolume;
        }

        private void _setInstanceVolume(TikiSound sound, SoundEffectInstance instance, float newSoundVolume, float newMusicVolume)
        {
            if (sound.IsMusic())
            {
                if (_musicVolume == newMusicVolume) return;

                instance.Volume /= _musicVolume;
                instance.Volume *= newMusicVolume;
            }
            else
            {
                if (_soundVolume == newSoundVolume) return;

                instance.Volume /= _soundVolume;
                instance.Volume *= newSoundVolume;
            } 
        }
        #endregion

        #region Member
        public SoundEffectInstance PlaySound(TikiSound sound)
        {
            sounds[sound].Volume *= this._soundVolume;
            sounds[sound].Play();
            return sounds[sound];
        }
        public void PlaySFX(TikiSound sound, float volume, Vector2 position)
        {
            Vector2 delta = GI.Camera.TrackingPosition - position;
            float length = delta.LengthSquared();
            float koef = this.soundRange / delta.LengthSquared();
            koef = koef > 1 ? 1 : koef;
            koef = koef < 0.1f ? 0 : koef;

            SoundEffectInstance sei = soundFX[sound].CreateInstance();
            sei.Volume = volume * this._soundVolume * koef;
            sei.Play();

            _instanceCache.Add(sei, sound);

        }

        public void PlaySFX(TikiSound sound, float volume)
        {
            SoundEffectInstance sei = soundFX[sound].CreateInstance();
            sei.Volume = volume * this._soundVolume;
            sei.Play();

            _instanceCache.Add(sei, sound);
        }

        public SoundEffectInstance Pause(TikiSound sound)
        {
            if (!ambientSounds.Contains(sounds[sound]))
                sounds[sound].Pause();
            return sounds[sound];
        }

        public SoundEffectInstance Resume(TikiSound sound)
        {
            if (sounds[sound].State == SoundState.Paused)
                sounds[sound].Resume();
            return sounds[sound];
        }

        public void Stop(TikiSound sound)
        {
            sounds[sound].Stop();
        }

        public void AddLoopingSound(TikiSound sound, float volume)
        {
            if (!ambientSounds.Contains(sounds[sound]))
            {
                this.ambientSounds.Add(sounds[sound]);
                sounds[sound].IsLooped = true;
            }

            sounds[sound].Play();
            sounds[sound].Volume = volume;

        }

        public void SetVolume(TikiSound sound, float volume)
        {
            sounds[sound].Volume = volume * _soundVolume;
        }
        
        public void SetLoop(TikiSound sound)
        {
            sounds[sound].IsLooped = true;
            sounds[sound].Play();
            sounds[sound].Pause();
        }

        public bool IsAlive(TikiSound sound)
        {
            return sounds[sound].State == SoundState.Playing;
        }
        #endregion

        #region Member - Load
        public SoundEffect LoadSound(TikiSound file)
        {
            SoundEffect sound = GI.Content.Load<SoundEffect>("sound/"+file.ToString());
            soundFX.Add(file, sound);
            SoundEffectInstance sei = sound.CreateInstance();
            sounds.Add(file, sei);
            return sound;
        }
        #endregion

        #region Member - Fade
        public void Fade(TikiSound sound, float amount)
        {
            if (fade.Keys.Contains(sound))
                fade[sound] = amount;
        }

        public void FadeLoop(TikiSound ts)
        {
            fade[ts] = 0;
            sounds[ts].Volume = 0;
            sounds[ts].Play();
        }

        public bool IsFading(TikiSound sound)
        {
            if (fade.Keys.Contains(sound))
            {
                if (fade[sound] < 0)
                    return true;
                else
                    return false;
            }
            return false;
        }
        #endregion

        #region Member - Xna - Update
        public override void Update(GameTime gameTime)
        {
            _updateFade(gameTime);
            _updateSFX(gameTime);

            base.Update(gameTime);
        }

        private void _updateSFX(GameTime gameTime)
        {
            foreach (SoundEffectInstance sei in _instanceCache.Keys.ToArray())
            {
                if (sei.State == SoundState.Stopped)
                {
                    _instanceCache.Remove(sei);
                }
            }
        }

        private void _updateFade(GameTime gameTime)
        {
            try
            {
                foreach (TikiSound ts in fade.Keys.ToArray())
                {
                    float amount = fade[ts];
                    float volume = sounds[ts].Volume;
                    if (amount > 0)
                    {
                        sounds[ts].Volume = ((volume + amount) < 1 ? (volume + amount) : 1) * _soundVolume;
                    }
                    else
                    {
                        sounds[ts].Volume = ((volume + amount) > 0 ? (volume + amount) : 0) * _soundVolume;
                    }
                    sounds[ts].Play();
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
        #endregion

        #region Properties
        public SoundEffectInstance this[TikiSound sound]
        {
            get { return sounds[sound]; }
        }

        public float SoundVolume
        {
            get { return _soundVolume; }
            set
            {
                _setSystemSound(value, _musicVolume);
            }
        }

        public float MusicVolume
        {
            get { return _musicVolume; }
            set
            {
                _setSystemSound(_soundVolume, value);
            }
        }
        #endregion
    }
}
