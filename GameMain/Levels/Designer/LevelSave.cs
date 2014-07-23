#if DESIGNER
using System;
using System.Runtime.Serialization;
using TikiEngine.Elements;
using System.Collections.Generic;

namespace TikiEngine.Levels.Designer
{
    [Serializable]
    internal class LevelSave : NameObject
    {
        #region Vars
        private List<GameElementIsland> _islands;
        #endregion

        #region Init
        public LevelSave(LevelDesigner level)
        {
            this.Name = level.Name;
            this.Islands = level.Islands;
        }

        public LevelSave(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Member
        public void LoadLevel(LevelDesigner level)        
        {
            level.Islands = this.Islands;
        }
        #endregion

        #region Properties
        public List<GameElementIsland> Islands
        {
            get { return _islands; }
            set { _islands = value; }
        }
        #endregion

        #region NameObject
        protected override void ApplyChanges()
        {
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
        }

        public override void Dispose()
        {
        }

        public override bool Ready
        {
            get { return true; }
        }
        #endregion
    }
}
#endif