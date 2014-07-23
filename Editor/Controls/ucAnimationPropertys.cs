using System;
using System.Drawing;
using System.Windows.Forms;
using TikiEngine.Elements;
using TikiEngine.Editor.Modes;
using Microsoft.Xna.Framework;
using TikiEngine.Elements.Graphics;

namespace TikiEngine.Editor.Controls
{
    public partial class ucAnimationPropertys : UserControl
    {
        #region Vars
        private bool _animationLoading = false;

        private modeAnimation _modeAnimation;
        #endregion

        #region Init
        public ucAnimationPropertys()
        {
            InitializeComponent();

            _modeAnimation = Program.GameMain.GetMode<modeAnimation>();
            _modeAnimation.ObjectChanged += delegate(object sender, EventArgs e)
            {
                _objectToControls();
            };
        }
        #endregion

        #region Private Member
        private void _objectToControls()
        {
            _animationLoading = true;

            try
            {
                var animation = _modeAnimation.CurrentAnimation;

                textName.Text = animation.Name;

                textFilename.Text = animation.TextureFile;

                textRows.Text = animation.Rows.ToString();
                textColumns.Text = animation.Columns.ToString();

                textFrameStart.Text = animation.StartFrame.ToString();
                textFrameStop.Text = animation.StopFrame.ToString();

                textInterval.Text = animation.AnimationSpeed.ToString();

                checkLoop.Checked = animation.Loop;

                labelCount.Text = animation.FrameCount.ToString();
            }
            finally
            {
                _animationLoading = false;
            }
        }

        private void _controlsToObject()
        {
            if (_modeAnimation.CurrentAnimation == null || _animationLoading) return;

            _modeAnimation.CurrentAnimation.Name = textName.Text;

            _modeAnimation.CurrentAnimation.TextureFile = _parseText(textFilename, String.Copy);

            _modeAnimation.CurrentAnimation.Rows = _parseText(textRows, Int32.Parse);
            _modeAnimation.CurrentAnimation.Columns = _parseText(textColumns, Int32.Parse);

            _modeAnimation.CurrentAnimation.StartFrame = _parseText(textFrameStart, Int32.Parse);
            _modeAnimation.CurrentAnimation.StopFrame = _parseText(textFrameStop, Int32.Parse);

            _modeAnimation.CurrentAnimation.AnimationSpeed = _parseText(textInterval, Single.Parse);
            _modeAnimation.CurrentAnimation.Loop = checkLoop.Checked;

            //_modeAnimation.CurrentAnimation.StartPosition = new Vector2(
            //    (GI.Device.Viewport.Width / 2) - (_modeAnimation.CurrentAnimation.Width / 2),
            //    (GI.Device.Viewport.Height / 2) - (_modeAnimation.CurrentAnimation.Height / 2)
            //);

            labelCount.Text = (_modeAnimation.CurrentAnimation.Rows * _modeAnimation.CurrentAnimation.Columns).ToString();
        }

        private T _parseText<T>(TextBox textBox, Func<string, T> parseDel)
        {
            textBox.ForeColor = SystemColors.WindowText;
            try
            {
                return parseDel(textBox.Text);
            }
            catch
            {
                textBox.ForeColor = System.Drawing.Color.Red;
            }

            return default(T);
        }
        #endregion

        #region Member - EventHandler
        private void Control_Changed(object sender, EventArgs e)
        {
            _controlsToObject();
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            _modeAnimation.CurrentAnimation.IsAlive = !_modeAnimation.CurrentAnimation.IsAlive;
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            _modeAnimation.CurrentAnimation.CurrentFrame++;
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            _modeAnimation.CurrentAnimation.CurrentFrame++;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            _modeAnimation.CurrentAnimation.IsAlive = false;
        }
        #endregion
    }
}
