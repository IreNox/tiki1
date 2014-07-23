using System;
using System.Collections.Generic;
using System.Linq;
using TikiEngine.Elements;
using TikiEngine.Elements.Physic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TikiEngine.States
{
    internal class stateLevel : GameState
    {
        #region Vars
        protected SaveGame _lastSaveGame;

        private List<LevelGame> _levels = new List<LevelGame>();
        #endregion

        #region Init
        public stateLevel()
        { 
            this.UseCamera = true;

#if DEBUG
            this.SetComponent<DebugView>(new DebugView());
#endif
        }
        #endregion

        #region Private Member
        private void _loadLevel<T>()
            where T : LevelGame, new()
        {
            T level = new T();
            _levels.Add(level);

            this.CurrentLevel = level;
        }

        private void _loadCallback(IAsyncResult result)
        {
            Action del = (Action)result.AsyncState;
            del.EndInvoke(result);

            game.ChangeGameState<stateLoading>().DoAction = delegate()
            {
                game.ChangeGameState<stateLevel>();
            };
        }

        #endregion

        #region Member
        public override void OnActivate()
        {
            LevelGame level = this.GetComponent<LevelGame>("level");

            if (level != null)
            {
                Setup.Reset(level);
                this.SaveLevel();

                GI.Camera.Zoom = level.Zoom;
            }
        }
        #endregion

        #region Member - Level
        public T GetLevel<T>()
            where T : LevelGame, new()
        {
            T level = _levels.OfType<T>().FirstOrDefault();

            return level;
        }

        public void SetLevel<T>()
            where T : LevelGame, new()
        {
            Action del = new Action(_loadLevel<T>);

            game.ChangeGameState<stateLoading>();
            del.BeginInvoke(_loadCallback, del);

            //T level = this.GetLevel<T>();

            //if (level == null)
            //{
            //}
            //else
            //{
            //    this.CurrentLevel = level;
            //}
        }

        public void ReloadLevel<T>()
            where T : LevelGame, new()
        {
            T level = this.GetLevel<T>();

            if (level != null)
            {
                //level.Dispose();
                _levels.Remove(level);
            }

            this.SetLevel<T>();
        }
        #endregion

        #region Member - Load/Save
        public void SaveLevel()
        {
            SaveGame sg = new SaveGame();
            sg.RoboPosition = this.CurrentLevel.Robo.CurrentPosition;

            _lastSaveGame = sg;
        }
        public void SaveLevel(Vector2 position)
        {
            SaveGame sg = new SaveGame();
            sg.RoboPosition = position;

            _lastSaveGame = sg;
        }

        public void LoadSaveGame()
        {
            if (_lastSaveGame != null)
            {
                LevelGame level = this.CurrentLevel;

                level.ElementsDestroyIsland.Clear();
                level.ElementsDestroyIsland.AddRange(_lastSaveGame.Breakables);

                foreach (NameObject n in level.Elements)
                {
                    n.Reset();
                }

                foreach (NameObject n in level.ElementsCollectable)
                {
                    n.Dispose();
                }
                level.ElementsCollectable.Clear();

                foreach(PhysicBox pb in level.ElementsBuild.ToArray())
                {
                    pb.Reset();
                }

                foreach (NameObject n in level.ElementsDestroyIsland)
                {
                    n.Reset();
                }

                GI.GameVars = _lastSaveGame.GameVars;
                level.Robo.CurrentPosition = _lastSaveGame.RoboPosition;
                level.Robo.ClearVelocity();
            }
        }
        #endregion

        #region Member - Xna - Draw
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

#if DESIGNER
            if (!this.CurrentLevel.DesignMode)
            {
                Setup.Draw(gameTime);
            }
#else
            Setup.Draw(gameTime);
#endif
        }
        #endregion

        #region Member - Xna - Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

#if DEBUG
            float zoom = (((float)GI.Control.MouseScroll / 1000) + 1) * this.CurrentLevel.Zoom;
            if (GI.Camera.Zoom != zoom && zoom > 0) GI.Camera.Zoom = zoom;

            if (GI.Control.KeyboardPressed(Keys.F1)) GI.DEBUG = !GI.DEBUG;

            if (GI.Control.KeyboardPressed(Keys.Enter)) this.CurrentLevel.Robo.CurrentPosition = this.CurrentLevel.Robo.StartPosition;
#endif
            if (GI.Control.KeyboardPressed(Keys.F6)) this.SaveLevel();
            if (GI.Control.KeyboardPressed(Keys.F9)) this.LoadSaveGame();

            //if (GI.Control.KeyboardPressed(Keys.F7)) GI.Sound.ChangeSystemVolume(-1);
            //if (GI.Control.KeyboardPressed(Keys.F8)) GI.Sound.ChangeSystemVolume(1);

#if DESIGNER
            if (this.CurrentLevel.DesignMode) return;
#endif

            if (this.CurrentLevel.Robo.CurrentPosition.Y > 75)
            {
                if (_lastSaveGame != null)
                {
                    this.LoadSaveGame();
                }
                else
                {
                    this.CurrentLevel.Robo.Body.LinearVelocity = Vector2.Zero;
                    this.CurrentLevel.Robo.CurrentPosition = this.CurrentLevel.Robo.StartPosition;
                }
            }

            Setup.Update(gameTime);
        }
        #endregion

        #region Properties
        public LevelGame CurrentLevel
        {
            get { return this.GetComponent<LevelGame>("level"); }
            set
            {
                this.SetComponent<LevelGame>("level", value);

                Setup.Reset(value);

                if (!value.Initialized)
                {
                    value.Initialize();
                }

                value.OnActivate();
            }
        }
        #endregion
    }
}
