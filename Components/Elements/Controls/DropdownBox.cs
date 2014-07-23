using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TikiEngine.Elements.Controls
{
    public class DropdownBox : DrawableGameComponent
    {
        #region Vars
        private Label _label;
        /*private SpriteBatch _sprite;
        private Texture2D _texture;

        private bool _itemsVisible;*/
        private List<DropdownBoxItem> _items = new List<DropdownBoxItem>();
        #endregion

        #region Init
        public DropdownBox()
            : base(GI.Game)
        {
            _label = new Label();
        }
        #endregion

        #region Class - DropdownBoxItem
        public class DropdownBoxItem
        { 
        }
        #endregion
    }
}
