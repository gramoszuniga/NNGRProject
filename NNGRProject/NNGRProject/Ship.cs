using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace NNGRProject
{
    public class Ship : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private Vector2 pos;
        private Vector2 speed;
        private Color color;
        private bool isMouseControlled = true;

        public bool IsMouseControlled
        {
            get { return isMouseControlled; }
            set { isMouseControlled = value; }
        }

        public Vector2 Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        public Texture2D Tex
        {
            get { return tex; }
            set { tex = value; }
        }
        public Vector2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }

        public Ship(Game game, SpriteBatch spriteBatch, Texture2D tex)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            pos = new Vector2(Shared.stage.X / 2 - tex.Width / 2,
                Shared.stage.Y - tex.Height);
            speed = new Vector2(10, 10);
            color = Color.White;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (isMouseControlled)
            {
                MouseState mouseState = Mouse.GetState();
                pos.X = mouseState.X;
                if (pos.X > Shared.stage.X - tex.Width)
                {
                    pos.X = Shared.stage.X - tex.Width;
                }
                if (pos.X < 0)
                {
                    pos.X = 0;
                }
                pos.Y = mouseState.Y;
                if (pos.Y + tex.Height > Shared.stage.Y)
                {
                    pos.Y = Shared.stage.Y - tex.Height;
                }
                if (pos.Y < 0)
                {
                    pos.Y = 0;
                }
            }
            else
            {
                KeyboardState ks = Keyboard.GetState();
                if (ks.IsKeyDown(Keys.Right))
                {
                    pos.X += speed.X;
                    if (pos.X > Shared.stage.X - tex.Width)
                    {
                        pos.X = Shared.stage.X - tex.Width;
                    }
                }
                if (ks.IsKeyDown(Keys.Left))
                {
                    pos.X -= speed.X;
                    if (pos.X < 0)
                    {
                        pos.X = 0;
                    }
                }
                if (ks.IsKeyDown(Keys.Down))
                {
                    pos.Y += speed.Y;
                    if (pos.Y + tex.Height > Shared.stage.Y)
                    {
                        pos.Y = Shared.stage.Y - tex.Height;
                    }
                }
                if (ks.IsKeyDown(Keys.Up))
                {
                    pos.Y -= speed.Y;
                    if (pos.Y < 0)
                    {
                        pos.Y = 0;
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(tex, pos, color);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public Rectangle GetBoundaries()
        {
            return new Rectangle((int)pos.X, (int)pos.Y, tex.Width, tex.Height);
        }
    }
}