using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TikiEngine.Elements.Controls
{
    public class RadioButton : CheckBox
    {
        #region Vars
        private RadioCollection _collection;
        #endregion

        #region Init
        public RadioButton(RadioCollection collection)
            : base()
        {
            textureChecked.TextureFile = "controls/radio_true";
            textureUnchecked.TextureFile = "controls/radio_false";

            _collection = collection;
            _collection.Add(this);
        }
        #endregion

        #region Member
        public override void Update(GameTime gameTime)
        {
            bool check = varChecked;

            base.Update(gameTime);

            if (check != varChecked)
            {
                if (!varChecked)
                {
                    varChecked = true;
                    return;
                }

                _collection.SelectedButton = this;

                foreach (RadioButton button in _collection)
                {
                    if (button != this)
                    {
                        button.varChecked = false;
                    }
                }
            }
        }
        #endregion

        #region Properties
        public override bool Checked
        {
            get { return base.Checked; }
            set
            {
                base.Checked = value;

                if (value) _collection.SelectedButton = this;
            }
        }
        #endregion
    }

    #region Class - RadioCollection
    public class RadioCollection : List<RadioButton>
    {
        #region Vars
        private RadioButton _radioButton;
        #endregion

        #region Member
        public void Draw(GameTime gameTime)
        {
            foreach (RadioButton r in this)
            {
                r.Draw(gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (RadioButton r in this)
            {
                r.Update(gameTime);
            }
        }
        #endregion

        #region Properties
        public RadioButton SelectedButton
        {
            get { return _radioButton; }
            set { _radioButton = value; }
        }
        #endregion
    }
    #endregion
}
