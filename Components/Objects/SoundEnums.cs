using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TikiEngine
{
    public enum TikiSound
    {
        [Music]
        Atmo,
        [Music]
        Wind,
        [Music]
        music_01,
        [Music]
        music_02,

        Robo_Grass_Jump,
        Robo_Grass_Land,
        Robo_Grass_Loop,

        Robo_Metal_Jump,
        Robo_Metal_Land,
        Robo_Metal_Loop,
        Robo_Slide,

        Robo_Wood_Jump,
        Robo_Wood_Land,
        Robo_Wood_Loop,

        Cube_Destroy,
        Cube_Spawn,
        Cube_Grass,
        Cube_Metal,
        Cube_Wood,
        Cube_Pushing_Grass,
        Cube_Pushing_Metal,
        Cube_Pushing_Wood,
        Cube_Slide,

        Shoot,
        Shoot_Hit,
        Shoot_Grass,
        Shoot_Metal,
        Shoot_Wood,

        Menu_Click,
        Menu_Hover,

        Island_Break_On_Touch,
        Island_Destruction,

        Collect_Gear,

        Newton_01,
        Newton_02,
    }

    public class MusicAttribute : Attribute
    { 
    }

    public static class SoundExtensions
    {
        #region Vars
        private static Type _type = typeof(TikiSound);
        private static Type _typeMusic = typeof(MusicAttribute);
        #endregion

        #region Member
        public static bool IsMusic(this TikiSound sound)
        {
            return _type.GetMember(sound.ToString()).First().GetCustomAttributes(_typeMusic, true).Length != 0;
        }
        #endregion
    }
}
