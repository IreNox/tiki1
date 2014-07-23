using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using TikiEngine.Elements.Graphics;

namespace TikiEngine.Elements
{
    internal partial class Robo
    {
        #region Vars
        private Vector2 origin = new Vector2(0, size.X) / 4;
        private CAnimation charakterAnimation;
        #endregion

        #region Init
        private void _initAnimation()
        {
            this.charakterAnimation = new CAnimation(this);
        }
        #endregion

        #region Member
        private void _drawAnimation(GameTime gameTime)
        {
            charakterAnimation.Draw(gameTime, this.Origin);
        }

        private void _updateAnimation(GameTime gameTime)
        {
            charakterAnimation.Update(gameTime);
        }
        #endregion

        #region Class - Animations
        private class CAnimation
        {
            #region Vars
            private Robo _robo;

            private long fallTimer = 0;

            private State animationState;
            private bool orientation = true;
            private Dictionary<State, TikiAnimation> animations;
            private Body body;
            private bool pushing = false;
            #endregion

            #region Enum - State
            public enum State
            {
                None,

                Idle,
                Idlepose,
                Idlelook,
                IdleKniebeugen,

                IdleStart,
                IdleStop,

                Slide,
                SlideLand,

                PushStart,
                Push,
                PushStop,

                StandJumpRise,
                StandJumpFall,
                StandJumpLand,

                JumpRise,
                JumpFall,
                JumpLand,
                JumpLandStand,

                WalkStandStop,
                WalkStart,
                WalkA,
                WalkB,
                WalkStopA,
                WalkStopB,
            }
            #endregion

            #region Init
            public CAnimation(Robo robo)
            {
                _robo = robo;
                this.body = robo.circle.Body;
                Init();
            }

            public void Init()
            {
                this.animations = new Dictionary<State, TikiAnimation>();
                
                #region idles
                this.animations.Add(
                    State.Idle, new TikiAnimation("idle", 8, 8, 0, 59, 500, false));

                this.animations.Add(
                    State.Idlelook, new TikiAnimation("idle_look", 8, 8, 0, 59, 500, false));

                this.animations.Add(
                    State.Idlepose, new TikiAnimation("idle_pose", 7, 8, 0, 49, 500, false));

                this.animations.Add(
                    State.IdleKniebeugen, new TikiAnimation("idle_kniebeugen", 9, 8, 0, 69, 500, false));
                #endregion

                #region idle start/stop
                this.animations.Add(
                    State.IdleStart, new TikiAnimation("idle_start", 2, 8, 0, 9, 500, false));
                this.animations.Add(
                    State.IdleStop, new TikiAnimation("idle_stop", 2, 8, 0, 9, 500, false));
                #endregion

                #region sliden
                this.animations.Add(
                    State.Slide, new TikiAnimation("slide", 3, 8, 0, 19, 500, true));
                this.animations.Add(
                    State.SlideLand, new TikiAnimation("slide_fall", 1, 8, 0, 7, 500, false));
                #endregion

                #region pushen
                this.animations.Add(
                    State.PushStart, new TikiAnimation("push_start", 2, 8, 0, 12, 100, false));
                this.animations.Add(
                    State.Push, new TikiAnimation("push", 4, 8, 0, 31, 100, false));
                this.animations.Add(
                    State.PushStop, new TikiAnimation("push_stop", 2, 8, 0, 8, 100, false));
                #endregion

                #region stand jump
                this.animations.Add(
                    State.StandJumpRise, new TikiAnimation("stand_jump_rise", 3, 8, 0, 19, 1000, false));
                this.animations.Add(
                    State.StandJumpFall, new TikiAnimation("stand_jump_fall", 1, 8, 0, 7, 1000, true));
                this.animations.Add(
                    State.StandJumpLand, new TikiAnimation("stand_jump_land", 1, 8, 0, 7, 1000, false));
                #endregion

                #region jump
                this.animations.Add(
                    State.JumpRise, new TikiAnimation("jump_rise", 3, 8, 0, 19, 700, false));
                this.animations.Add(
                    State.JumpFall, new TikiAnimation("jump_fall", 2, 8, 0, 13, 500, true));
                this.animations.Add(
                    State.JumpLand, new TikiAnimation("jump_land", 1, 8, 0, 7, 200, false));
                this.animations.Add(
                    State.JumpLandStand, new TikiAnimation("jump_land_stand", 1, 7, 0, 6, 200, false));
                #endregion

                #region walk
                this.animations.Add(
                    State.WalkStandStop, new TikiAnimation("run_start_stop", 3, 8, 7, 20, 100, false));
                this.animations.Add(
                    State.WalkStart, new TikiAnimation("run_start", 2, 8, 0, 14, 500, false));
                this.animations.Add(
                    State.WalkA, new TikiAnimation("run_loop", 3, 8, 0, 9, 850, false));
                this.animations.Add(
                    State.WalkB, new TikiAnimation("run_loop", 3, 8, 10, 20, 850, false));
                this.animations.Add(
                    State.WalkStopA, new TikiAnimation("run_stop1", 2, 8, 0, 12, 450, false));
                this.animations.Add(
                    State.WalkStopB, new TikiAnimation("run_stop2", 2, 8, 0, 11, 450, false));
                #endregion

                foreach (Animation a in animations.Values)
                {
                    a.TimePerFrame = 31;
                }

                this.animations[State.IdleStart].TimePerFrame = 20;
                this.animations[State.IdleStop].TimePerFrame = 20;
                this.animations[State.Push].TimePerFrame = 40;

                this.animations[State.IdleKniebeugen].TimePerFrame = 45;
                //this.animations[State.Idlepose].TimePerFrame = 60;
                
                this.animationState = State.Idle;
                this.animations[animationState].IsAlive = true;

                this.Depth = LayerDepthEnum.Character;
            }
            #endregion

            #region Private Member
            public void _randomIdle()
            {
                int i = Functions.GetRandom(0, 7);
                switch (i)
                {
                    case 1:
                        _switchAnimation(State.Idlepose);
                        break;
                    case 2:
                        _switchAnimation(State.Idlelook);
                        break;
                    case 3:
                        _switchAnimation(State.IdleKniebeugen);
                        break;
                    default:
                        _switchAnimation(State.Idle);
                        break;
                }
            }

            private void _switchAnimation(State state)
            {
                float tmp = this.animations[this.animationState].Stop();
                this.animationState = state;
                this.animations[this.animationState].IsAlive = true;
            }

            private void _flipAnimation(State state)
            {
                float time = animations[animationState].Time;
                float frame = animations[animationState].CurrentFrame;
                _switchAnimation(state);
                animations[animationState].Time = time;
                animations[animationState].CurrentFrame = (int)frame;
            }

            public bool _leftOrRight()
            {
                return _robo.Left() || _robo.Right();
            }
            #endregion

            #region Member - Xna - Draw
            public void Draw(GameTime gameTime, Vector2 position)
            {
                if (_robo.Slide())
                    orientation = (this.body.LinearVelocity.X > 0) ? true : false;

                this.animations[this.animationState].CurrentPosition = position;
                this.animations[this.animationState].SpriteEffect = orientation ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                this.animations[this.animationState].Draw(gameTime);
            }
            #endregion

            #region Member - Xna - Update
            public void Update(GameTime gameTime)
            {
                Animation anim = this.animations[this.animationState];
                anim.Update(gameTime);
                #region switch case
                switch (this.animationState)
                {
                    case State.Idle:
                        if (!anim.IsAlive)
                        {
                            this._randomIdle();   
                        }
                        break;
                    case State.Idlepose:
                        if (!anim.IsAlive)
                        {
                            this._randomIdle();
                        }
                        break;
                    case State.Idlelook:
                        if (!anim.IsAlive)
                        {
                            this._randomIdle();
                        }
                        break;
                    case State.IdleKniebeugen:
                        if (!anim.IsAlive)
                        {
                            this._randomIdle();
                        }
                        break;
                    case State.IdleStop:
                        if (!anim.IsAlive)
                        {
                            if (_leftOrRight())
                                _switchAnimation(State.WalkStart);
                            else
                                _switchAnimation(State.IdleStart);
                        }
                        break;
                    case State.IdleStart:
                        if (!anim.IsAlive)
                        {
                            if (_leftOrRight())
                                _switchAnimation(State.IdleStop);
                            else
                                _switchAnimation(State.Idle);
                        }
                        break;
                    case State.SlideLand:
                        if (!anim.IsAlive)
                        {
                            if (_robo.Slide())
                                _switchAnimation(State.Slide);
                            else
                                _switchAnimation(State.Idle);
                        }
                        break;
                    case State.PushStart:
                        if (!anim.IsAlive)
                        {
                            if (Pushing)
                            {
                                _switchAnimation(State.Push);
                            }
                        }
                        break;
                    case State.Push:
                        if (!anim.IsAlive)
                        {
                            if (Pushing)
                            {
                                _switchAnimation(State.Push);
                            }
                            else
                            {
                                _switchAnimation(State.PushStop);
                            }
                        }
                        break;
                    case State.PushStop:
                        if (!anim.IsAlive)
                        {
                            if (_leftOrRight())
                                _switchAnimation(State.WalkStart);
                            else
                                _switchAnimation(State.IdleStart);
                        }
                        break;
                    case State.WalkStart:
                        if (!anim.IsAlive)
                        {
                            if (_leftOrRight())
                                _switchAnimation(State.WalkA);
                            else
                                _switchAnimation(State.IdleStart);
                        }
                        break;
                    case State.WalkA:
                        if (!anim.IsAlive)
                        {
                            if (_leftOrRight())
                                _switchAnimation(State.WalkB);
                            else
                                _switchAnimation(State.WalkStopA);
                        }
                        break;
                    case State.WalkB:
                        if (!anim.IsAlive)
                        {
                            if (_leftOrRight())
                                _switchAnimation(State.WalkA);
                            else
                                _switchAnimation(State.WalkStopB);
                        }
                        break;
                    case State.WalkStopA:
                        if (!anim.IsAlive)
                        {
                            if (_leftOrRight())
                                _switchAnimation(State.WalkStart);
                            else
                                _switchAnimation(State.IdleStart);
                        }
                        break;
                    case State.WalkStopB:
                        if (!anim.IsAlive)
                        {
                            if (_leftOrRight())
                                _switchAnimation(State.WalkStart);
                            else
                                _switchAnimation(State.IdleStart);
                        }
                        break;
                    case State.JumpRise:
                        if (!anim.IsAlive)
                        {
                            _switchAnimation(State.JumpFall);
                        }
                        break;
                    case State.JumpLand:
                        if (!anim.IsAlive)
                        {
                            if (_leftOrRight())
                                _switchAnimation(State.WalkA);
                            else
                                _switchAnimation(State.IdleStart);
                        }
                        break;
                    case State.JumpLandStand:
                        if (!anim.IsAlive)
                        {
                            if (_leftOrRight())
                                _switchAnimation(State.WalkStart);
                            else
                                _switchAnimation(State.IdleStart);
                        }
                        break;


                    case State.StandJumpRise:
                        if (!anim.IsAlive)
                        {
                            _switchAnimation(State.StandJumpFall);
                        }
                        break;
                    case State.StandJumpLand:
                        if (!anim.IsAlive)
                        {
                            if (_leftOrRight())
                                _switchAnimation(State.WalkStart);
                            else
                                _switchAnimation(State.IdleStart);
                        }
                        break;
                    case State.WalkStandStop:
                        break;

                }
                #endregion

                if (_leftOrRight())
                {
                    if (Pushing && animationState != State.PushStart && animationState != State.Push 
                        && animationState != State.PushStop && animationState != State.JumpRise)
                        _switchAnimation(State.PushStart);
                    if (animationState == State.Idle || animationState == State.Idlelook || 
                        animationState == State.Idlepose || animationState == State.IdleKniebeugen)
                    {
                        _switchAnimation(State.IdleStop);
                    }
                    if (animationState == State.StandJumpFall)
                        _switchAnimation(State.JumpFall);
                }
                else
                {
                    if (animationState == State.PushStart || animationState == State.Push)
                        _switchAnimation(State.PushStop);
                }

                if (_robo.Jump())
                {
                    if (_robo.Slide() && (animationState == State.Slide || animationState == State.SlideLand))
                        _switchAnimation(State.JumpRise);

                    if (animationState == State.WalkStart || animationState == State.WalkA || animationState == State.WalkB || animationState == State.JumpLand ||
                        animationState == State.PushStart || animationState == State.Push || animationState == State.PushStop)
                    {
                        _switchAnimation(State.JumpRise);
                    }
                    if (animationState == State.Idle || animationState == State.Idlelook || 
                        animationState == State.Idlepose || animationState == State.IdleKniebeugen ||
                        animationState == State.WalkStopA || animationState == State.WalkStopB ||
                        animationState == State.IdleStart || animationState == State.IdleStop || 
                        animationState == State.StandJumpLand || animationState == State.JumpLandStand)
                    {
                        _switchAnimation(State.StandJumpRise);
                    }
                }
            }
            #endregion

            public void GroundCollision()
            {
                if (_robo.Slide())
                {
                    if (animationState == State.JumpFall)
                    {
                        _switchAnimation(State.SlideLand);
                        return;
                    }
                    if (animationState != State.Slide)
                        _switchAnimation(State.Slide);
                }
                if (animationState == State.JumpFall)
                {
                    if (_leftOrRight())
                        _switchAnimation(State.JumpLand);
                    else
                        _switchAnimation(State.JumpLandStand);
                }
                if (animationState == State.StandJumpFall)
                {
                    _switchAnimation(State.StandJumpLand);
                }
                this.fallTimer = 100;
            }

            public void Fall(GameTime gametime)
            {
                this.fallTimer -= gametime.ElapsedGameTime.Milliseconds;
                if (fallTimer > 0)
                    return;

                if (_leftOrRight())
                    _switchAnimation(State.JumpFall);
                else
                    _switchAnimation(State.StandJumpFall);
            }

            public float Depth
            {
                set
                {
                    foreach (Animation a in animations.Values)
                        a.LayerDepth = value;
                }
            }

            public bool Orientation
            {
                get { return this.orientation; }
                set { this.orientation = value; }
            }

            public bool Pushing
            {
                get { return this.pushing; }
                set { this.pushing = value; }
            }

            private class TikiAnimation : Animation
            {
                private Texture2D mirrorImage;

                public TikiAnimation(String file, int rows, int columns, int startFrame, int stopFrame, float animationSpeed, bool isLoop)
                    : base("Charakter/Animation/Left/" + file, rows, columns, startFrame, stopFrame, animationSpeed, isLoop)
                {
                    try
                    {
                        this.mirrorImage = GI.Content.Load<Texture2D>("Charakter/Animation/Right/" + file);
                    }
                    catch
                    {
                        Console.WriteLine("No Image found for file : " + file);
                    }
                }
                public override void Draw(GameTime gameTime)
                {

                    Vector2 offset = new Vector2();//new Vector2(0.41f, 0);

                    Point size = _size.ToPoint();

                    Texture2D texture;
                    SpriteEffects se = mirrorImage != null ? SpriteEffects.None : this.spriteEffect;

                    if (this.spriteEffect == SpriteEffects.FlipHorizontally)
                    {
                        offset *= -1;
                        if (mirrorImage != null)
                        {
                            texture = mirrorImage;
                            se = SpriteEffects.None;
                        }
                        else
                        {
                            texture = this.Texture;
                            se = SpriteEffects.FlipHorizontally;
                        }
                    }
                    else
                    {
                        texture = this.Texture;
                        se = this.spriteEffect;
                    }

                    spriteBatch.Draw(
                        texture,
                        ConvertUnits.ToDisplayUnits(this.CurrentPosition + offset),
                        new Rectangle(
                            _currentFrame % _columns * size.X,
                            _currentFrame / _columns * size.Y,
                            size.X,
                            size.Y
                        ),
                        Color.White,
                        _angle,
                        _origin - ConvertUnits.ToDisplayUnits(_offset),
                        _scale,
                        se,
                        this.LayerDepth
                    );

                }
            }
        }
        #endregion
    }
}
