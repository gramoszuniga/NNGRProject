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
    public class EnemyShip : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private Vector2 pos;
        private Vector2 speed;
        private Color color;
        private Color[] colors = new Color[] { Color.RoyalBlue, Color.Violet, Color.Orange, Color.Yellow };
        private bool isRandom;

        public Texture2D Tex
        {
            get { return tex; }
            set { tex = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public Vector2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }

        public EnemyShip(Game game, SpriteBatch spriteBatch, Texture2D tex, Vector2 speed, bool isRandom)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            this.pos = new Vector2(Shared.stage.X / 2 - tex.Width / 2,
                0 - tex.Height);
            this.speed = speed;
            Random random = new Random();
            int randomColor = random.Next(0, colors.Length);
            this.color = colors[randomColor];
            this.isRandom = isRandom;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            pos.Y += speed.Y;
            pos.X += speed.X;

            if (pos.X < 0)
            {
                speed.X = Math.Abs(speed.X);
            }
            else if (pos.X > Shared.stage.X - tex.Width)
            {
                speed.X = -Math.Abs(speed.X);
            }

            if (pos.Y > Shared.stage.Y)
            {
                this.Enabled = false;
                this.Visible = false;
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