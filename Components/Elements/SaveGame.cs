using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TikiEngine.Elements.Physic;

namespace TikiEngine.Elements
{
    [Serializable]
    public class SaveGame : NameObject
    {
        #region Vars
        private Vector2 _roboPosition;

        private NameObjectPhysic[] _breakables;
        private Dictionary<string, object> _gameVars;
        #endregion

        #region Init
        public SaveGame()
        {
            _gameVars = new Dictionary<string, object>();
            _gameVars.AddRange(GI.GameVars.Select(kvp => new KeyValuePair<string, object>(kvp.Key, kvp.Value)).ToArray());
            _breakables = GI.Level.ElementsDestroyIsland.OfType<PhysicTextureBreakable>().Where(b => !b.Broken).ToArray();
        }
        #endregion

        #region Member
        protected override void ApplyChanges()
        {
            throw new NotSupportedException();
        }

        public override void Draw(GameTime gameTime)
        {
            throw new NotSupportedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotSupportedException();
        }

        public override void Dispose()
        {
            throw new NotSupportedException();
        }
        #endregion

        #region Properties
        public Vector2 RoboPosition
        {
            get { return _roboPosition; }
            set { _roboPosition = value; }
        }

        public Dictionary<string, object> GameVars
        {
            get { return _gameVars; }
        }

        public NameObjectPhysic[] Breakables
        {
            get { return _breakables; }
        }

        public override bool Ready
        {
            get { return true; }
        }
        #endregion
    }
}
