using System;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TikiEngine;
using TikiEngine.Elements;
using TikiEngine.Elements.Graphics;
using TikiEngine.Editor.Controls;
using TikiEngine.Elements.Physic;
using FarseerPhysics.Common;
using FarseerPhysics.Common.ConvexHull;

namespace TikiEngine.Editor.Modes
{
    internal class modeBreakable : EditorMode
    {
        #region Vars
        private string _objectName;

        private Texture2D _texPoint;
        private Vector2 _originPoint;

        private Texture2D _texArrow;
        private Vector2 _originArrow;
        private Vector2 _posArrowX;
        private Vector2 _posArrowY;

        private Grid _grid50;
        private Grid _grid100;
        private Sprite _spriteObject;

        private List<Vertex> _listVertices = new List<Vertex>();
        private Dictionary<Vertex, Polygon> _listPolygons = new Dictionary<Vertex, Polygon>();
        
        private Vector2 _tempVector;
        private Vertex _currentVertex;
        private Vertex _currentVertex2;
        private Actions _currentAction;

        private Polygon _currentPolygon;

        #region Enum - Actions
        private enum Actions
        { 
            None,
            MoveX,
            MoveY,
            Line
        }
        #endregion
        #endregion

        #region Init
        public modeBreakable()
        {
            this.UseCamera = true;
            this.UsePreferences = true;

            this.BackgroundColor = Color.AliceBlue;
        }

        public override void Init()
        {
            this.AddTabPage<ucMainProperties>("Properties");

            _texPoint = new Texture2D(GI.Device, 1, 1);
            _texPoint.SetData<Color>(new Color[] { Color.White });

            _texArrow = GI.Content.Load<Texture2D>("Base/arrow");

            _spriteObject = new Sprite();

            _grid50 = new Grid();
            _grid50.CellSize = 50;
            _grid50.Color = Color.DarkGray;

            _grid100 = new Grid();
            _grid100.CellSize = 100;
            _grid100.Color = Color.Gray;
            _grid100.Size = 2;

            _originPoint = _texPoint.GetCenter();
            _originArrow = _texArrow.GetCenter();
        }
        #endregion

        #region Private Member
        private Vertex _getVertix(Dictionary<Vector2, Vertex> list, Vector2 v)
        {
            Vertex vertex = null;

            if (list.ContainsKey(v))
            {
                vertex = list[v];
            }
            else
            {
                vertex = new Vertex(v);

                list.Add(v, vertex);
            }

            return vertex;
        }

        private void _loadData(PhysicBodyBreakable physic)
        {
            Dictionary<Vector2, Vertex> vertices = new Dictionary<Vector2,Vertex>();
            Dictionary<Vertex, Polygon> polygons = new Dictionary<Vertex, Polygon>();

            foreach (Vertices verts in physic.Vertices)
            {
                Vertex vertex = _getVertix(
                    vertices,
                    verts.First(v => polygons.Values.All(p => p.BaseVertex.Vector !=  v))
                );                
                IEnumerable<Vertex> polyVertices = verts.Where(v => v != vertex.Vector).Select(v => _getVertix(vertices, v));

                Polygon poly = new Polygon(vertex);
                poly.Vertices.AddRange(polyVertices);

                polygons.Add(vertex, poly);
            }

            _listVertices = vertices.Values.ToList();
            _listPolygons = polygons;

            this.TextureFile = physic.TextureFile;
        }

        private PhysicBodyBreakable _createBody()
        {
            List<Vertices> data = new List<Vertices>();

            foreach (Polygon poly in _listPolygons.Values)
            {
                Vertices verts = new Vertices();
                verts.Add(poly.BaseVertex.Vector);
                verts.AddRange(poly.Vertices.Select(v => v.Vector));

                data.Add(
                    Melkman.GetConvexHull(verts)
                );
            }

            var breakBody = new PhysicBodyBreakable();
            breakBody.TextureFile = _spriteObject.TextureFile;
            breakBody.Vertices = data;
            breakBody.Name = _objectName;

            return breakBody;
        }

        private Vertex _getMouseNearest()
        { 
            Vector2 mouse = GI.Control.MouseSimVector();

            return _listVertices.Where(v => Vector2.Distance(v.Vector, mouse) < 0.25f).OrderBy(v => Vector2.DistanceSquared(v.Vector, mouse)).FirstOrDefault();
        }
        #endregion

        #region Member
        public override void Activate()
        {
            this.ShowPreferences();
        }

        public void TestStart()
        {
            try
            {
                Program.GameMain.GetMode<modeBreakableTest>().BodyBreakable = _createBody();
                Program.GameMain.SetMode<modeBreakableTest>();
            }
            catch
            {
                MessageBox.Show("Can't create Breakable Body.");
            }
        }

        public void TestStop()
        {
            Program.GameMain.SetMode<modeBreakable>();
            Program.GameMain.GetMode<modeBreakableTest>().BodyBreakable.Dispose();
        }
        #endregion

        #region  Member - File
        public override void New()
        {
            this.TextureFile = null;
            _spriteObject.TextureFile = null;

            _currentVertex = null;
            _currentVertex2 = null;
            _currentPolygon = null;
            _currentAction = Actions.None;

            _listVertices.Clear();
            _listPolygons.Clear();

            this.ShowPreferences();
        }

        public override void Open(string name)
        {
            PhysicBodyBreakable physic = DataManager.LoadObject<PhysicBodyBreakable>(name, true);

            if (physic != null)
            {
                _loadData(physic);
            }

            this.ShowPreferences();
        }

        public override void Save()
        {
            DataManager.SetObject<PhysicBodyBreakable>(
                _createBody()
            );
        }

        public override void SaveAs(string name)
        {
            _objectName = name;

            this.Save();
        }

        public override void ShowPreferences()
        {
            this.GetTabControl<ucMainProperties>().SelectedObject = this;
        }
        #endregion

        #region Member - Xna - Draw
        public override void Draw(GameTime gameTime)
        {
            _grid50.Draw(gameTime);
            _grid100.Draw(gameTime);
            if (_spriteObject.Ready) _spriteObject.Draw(gameTime);

            if (_currentVertex != null)
            {
                GI.SpriteBatch.Draw(_texArrow, _posArrowX, null, Color.Green, MathHelper.Pi / -2, _originArrow, 1.0f, SpriteEffects.None, 0);
                GI.SpriteBatch.Draw(_texArrow, _posArrowY, null, Color.Red, 0, _originArrow, 1.0f, SpriteEffects.None, 0);

                switch (_currentAction)
                { 
                    case Actions.Line:
                        GI.PolygonBatch.DrawLine(_currentVertex.Vector, _tempVector, Color.White, 1);
                        break;
                }
            }

            foreach (Polygon polygon in _listPolygons.Values)
            {
                if (polygon.Vertices.Count > 1)
                {
                    Color color = (polygon == _currentPolygon ? Color.Violet : Color.Yellow);

                    for (int i = 0; i < polygon.Vertices.Count - 1; i++)
                    {
                        GI.PolygonBatch.DrawLine(polygon.BaseVertex.Vector, polygon.Vertices[i].Vector, color, 1);
                        GI.PolygonBatch.DrawLine(polygon.Vertices[i].Vector, polygon.Vertices[i + 1].Vector, color, 1);
                    }
                    GI.PolygonBatch.DrawLine(polygon.BaseVertex.Vector, polygon.Vertices.Last().Vector, color, 1);
                }
                else
                {
                    GI.PolygonBatch.DrawLine(polygon.BaseVertex.Vector, polygon.Vertices[0].Vector, Color.Red, 1);
                }
            }

            foreach (Vertex v in _listVertices)
            {
                Point p = ConvertUnits.ToDisplayUnits(v.Vector).ToPoint();

                GI.SpriteBatch.Draw(
                    _texPoint,
                    new Rectangle(p.X, p.Y, 4, 4),
                    null,
                    Color.Green,
                    0,
                    _originPoint,
                    SpriteEffects.None,
                    0
                );
            }
        }
        #endregion

        #region Member - Xna - Update
        public override void Update(GameTime gameTime)
        {
            Vector2 mouse = GI.Control.MouseSimVector();
            Point mouseP = ConvertUnits.ToDisplayUnits(mouse).ToPoint();

            if (_currentVertex != null)
            {
                _posArrowX = ConvertUnits.ToDisplayUnits(_currentVertex.Vector) + new Vector2(-(_originArrow.Y + 5), 0);
                _posArrowY = ConvertUnits.ToDisplayUnits(_currentVertex.Vector) + new Vector2(0, -(_originArrow.Y + 5));

                Rectangle rectX = new Rectangle((int)_posArrowX.X, (int)_posArrowX.Y, _texArrow.Height, _texArrow.Width);
                Rectangle rectY = new Rectangle((int)_posArrowY.X, (int)_posArrowY.Y, _texArrow.Width, _texArrow.Height);

                rectX.Offset((-new Vector2(_originArrow.Y, _originArrow.X)).ToPoint());
                rectY.Offset((-_originArrow).ToPoint());

                if (GI.Control.MousePressed(MouseButtons.Left))
                {
                    Vector2 diff = mouse - _currentVertex.Vector;

                    if (rectX.Contains(mouseP))
                    {
                        _currentAction = Actions.MoveX;
                        _tempVector = diff;
                    }
                    else if (rectY.Contains(mouseP))
                    {
                        _currentAction = Actions.MoveY;
                        _tempVector = diff;
                    }
                    else if (_currentVertex.GetRectangle().Contains(mouseP))
                    {
                        _currentAction = Actions.Line;
                        _tempVector = Vector2.Zero;
                    }
                }

                if (GI.Control.KeyboardPressed(Microsoft.Xna.Framework.Input.Keys.Delete))
                {
                    if (_currentPolygon != null)
                    {
                        _listPolygons.Remove(_currentPolygon.BaseVertex);
                        _currentPolygon = null;
                    }
                }
            }

            if (GI.Control.MouseClick(MouseButtons.Left) && _currentAction == Actions.None)
            {
                _currentVertex = null;
                foreach (Vertex v in _listVertices)
                {
                    if (v.GetRectangle().Contains(mouseP))
                    {
                        _currentVertex = v;
                        _currentPolygon = (_listPolygons.ContainsKey(v) ? _listPolygons[v] : null);
                    }
                }
            }
            else if (GI.Control.MouseClick(MouseButtons.Middle))
            {
                _listVertices.Add(
                    new Vertex(mouse)    
                );
            }

            if (GI.Control.MouseDown(MouseButtons.Left))
            {
                Vector2 pos = mouse - _tempVector;

                switch (_currentAction)
                {
                    case Actions.Line:
                        _currentVertex2 = _getMouseNearest();
                        _tempVector = (_currentVertex2 == null ? mouse : _currentVertex2.Vector);
                        break;
                    case Actions.MoveX:
                        _currentVertex.Vector = new Vector2(pos.X, _currentVertex.Vector.Y);
                        break;
                    case Actions.MoveY:
                        _currentVertex.Vector = new Vector2(_currentVertex.Vector.X, pos.Y);
                        break;
                }
            }
            else if (GI.Control.MouseUp(MouseButtons.Left))
            {
                switch (_currentAction)
                { 
                    case Actions.Line:
                        if (_currentVertex2 != null)
                        {
                            if (_currentPolygon == null)
                            {
                                _currentPolygon = new Polygon(_currentVertex);

                                _listPolygons.Add(_currentVertex, _currentPolygon);
                            }

                            _currentPolygon.Vertices.Add(_currentVertex2);
                        }
                        break;
                }

                _currentAction = Actions.None;
            }
        }
        #endregion

        #region Properties
        public override Type ObjectType
        {
            get { return typeof(PhysicBodyBreakable); }
        }

        public override string ObjectName
        {
            get { return _objectName; }
            set { _objectName = value; }
        }

        public override NameObject CurrentObject
        {
            get { return _createBody(); }
        }

        public bool TestEnabled
        {
            get { return Program.GameMain.CurrentMode == Program.GameMain.GetMode<modeBreakableTest>(); }
            set
            {
                if (value)
                {
                    this.TestStart();
                }
                else
                {
                    this.TestStop();
                }
            }
        }

        public string TextureFile
        {
            get { return _spriteObject.TextureFile; }
            set { _spriteObject.TextureFile = value; }
        }

        public Color GridColor
        {
            get { return _grid50.Color; }
            set
            {
                _grid50.Color = value;
                _grid100.Color = value;
            }
        }
        #endregion

        #region Class - Vertex
        private class Vertex
        {
            #region Vars
            private Vector2 _vector;
            #endregion

            #region Init
            public Vertex(Vector2 vector)
            {
                _vector = vector;
            }
            #endregion

            #region Member
            public Rectangle GetRectangle()
            {
                Point p = ConvertUnits.ToDisplayUnits(this.Vector).ToPoint();

                return new Rectangle(p.X - 4, p.Y - 4, 8, 8);
            }
            #endregion

            #region Properies
            public Vector2 Vector
            {
                get { return _vector; }
                set { _vector = value; }
            }
            #endregion
        }
        #endregion

        #region Class - Polygon
        private class Polygon
        {
            #region Vars
            private Vertex _baseVertex;

            private List<Vertex> _vertices = new List<Vertex>();
            #endregion

            #region Init
            public Polygon(Vertex baseVertex)
            {
                _baseVertex = baseVertex;
            }
            #endregion

            #region Properies
            public Vertex BaseVertex
            {
                get { return _baseVertex; }
            }

            public List<Vertex> Vertices
            {
                get { return _vertices; }
            }
            #endregion
        }
        #endregion
    }
}
