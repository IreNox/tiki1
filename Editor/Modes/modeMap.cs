using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TikiEngine.Elements;
using TikiEngine.Editor.Controls;

namespace TikiEngine.Editor.Modes
{
    class modeMap : EditorMode
    {
        #region Vars
        private Level _level;
        private NameObject _selectedObject;

        private Texture2D _texturePixel;
        #endregion

        #region Init
        public modeMap()
        {
            this.UseCamera = true;
            this.UsePreferences = true;
        }

        public override void Init()
        {
            _level = new Level();

            _texturePixel = new Texture2D(GI.Device, 1, 1);
            _texturePixel.SetData<Color>(
                new Color[] { Color.White }
            );

            this.AddTabPage<ucLevelAllObjects>("Add Object");
            this.AddTabPage<ucLevelElements>("Elements");
            this.AddTabPage<ucLevelProperties>("Propertys");
        }
        #endregion

        #region Private Member
        private void _loadLevel()
        {
            //comboLayer.ComboBox.DataSource = _stateMap.CurrentLevel.TileMaps;
            //comboLayer.ComboBox.DisplayMember = "Name";

            //_currentMap = comboLayer.ComboBox.SelectedItem as TileMap;

            //if (_currentMap != null && _currentMap.Texture != null)
            //{
            //    foreach (Image img2 in imagesTiles.Images)
            //    {
            //        img2.Dispose();
            //    }
            //    imagesTiles.Images.Clear();
            //    imagesTiles.ImageSize = new System.Drawing.Size(
            //        _currentMap.TileWidth,
            //        _currentMap.TileHeight
            //    );
            //    listTiles.TileSize = imagesTiles.ImageSize;

            //    Image img;
            //    MemoryStream mem = new MemoryStream();
            //    _currentMap.Texture.SaveAsPng(
            //        mem,
            //        _currentMap.Texture.Width,
            //        _currentMap.Texture.Height
            //    );
            //    img = Image.FromStream(mem);
            //    mem.Dispose();

            //    int index = 0;
            //    List<ListViewItem> items = new List<ListViewItem>();
            //    for (int y = 0; y < _currentMap.TextureRows; y++)
            //    {
            //        for (int x = 0; x < _currentMap.TextureColumns; x++)
            //        {
            //            Image part = new Bitmap(_currentMap.TileWidth, _currentMap.TileHeight);
            //            Graphics g = Graphics.FromImage(part);

            //            g.DrawImage(
            //                img,
            //                new Rectangle(0, 0, _currentMap.TileWidth, _currentMap.TileHeight),
            //                new Rectangle(
            //                    x * _currentMap.TileWidth,
            //                    y * _currentMap.TileHeight,
            //                    _currentMap.TileWidth,
            //                    _currentMap.TileHeight
            //                ),
            //                GraphicsUnit.Pixel
            //            );
            //            g.Dispose();

            //            imagesTiles.Images.Add(part);

            //            items.Add(
            //                new ListViewItem()
            //                {
            //                    ImageIndex = imagesTiles.Images.Count - 1,
            //                    Text = imagesTiles.Images.Count.ToString()
            //                }
            //            );

            //            index++;
            //        }
            //    }

            //    listTiles.Items.AddRange(
            //        items.ToArray()
            //    );
            //}
        }
        #endregion

        #region Member
        public override void Activate()
        {
            this.RaiseRefreshGUI();
        }
        #endregion

        #region Member - XNA
        public override void Draw(GameTime gameTime)
        {
            _level.Draw(gameTime);

            Point pos = GI.Camera.GetPoint(_level.StartPosition);
            GI.SpriteBatch.Draw(
                _texturePixel,
                new Rectangle(
                    pos.X,
                    pos.Y,
                    100,
                    100
                ),
                new Color(0f, 1f, 0f, 0.5f)
            );
        }

        public override void Update(GameTime gameTime)
        {
            _level.Update(gameTime);

            if (GI.Control.KeyboardPressed(Microsoft.Xna.Framework.Input.Keys.Back))
            {
                GI.Camera.CurrentPosition = Vector2.Zero;
            }
        }
        #endregion

        #region Member - File
        public override void New()
        {
            throw new NotImplementedException();
        }

        public override void Open(string name)
        {
            Level obj = DataManager.GetObject<Level>(name);

            this.CurrentLevel = obj;
        }

        public override void Save()
        {
            DataManager.SetObject(_level);
        }

        public override void SaveAs(string name)
        {
            _level.Name = name;

            DataManager.SetObject<Level>(_level);
        }

        public override void ShowPreferences()
        {
            this.GetTabControl<ucLevelProperties>().SelectedObject = _level;;

            Program.FormMain.SelectTabPage<ucLevelProperties>();
        }
        #endregion

        #region Properties
        public override Type ObjectType
        {
            get { return typeof(Level); }
        }

        public override NameObject CurrentObject
        {
            get { return this.CurrentLevel; }
        }

        public Level CurrentLevel
        {
            get { return _level; }
            set
            {
                if (value != null)
                {
                    _level = value;

                    this.RaiseObjectChanged();
                }
            }
        }

        public override string ObjectName
        {
            get { return (_level == null ? null : _level.Name); }
            set
            {
                if (_level != null) _level.Name = value;
            }
        }

        public NameObject SelectedObject
        {
            get { return _selectedObject; }
            set { _selectedObject = value; }
        }
        #endregion
    }
}
