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
    public class Asteroid : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private Vector2 pos, origin, speed;
        private float scale = 1.0f;
        private float rotation = 0f;
        private float rotationSpeed = 0.03f;
        private Color color;
        private Color[] colors = new Color[] { Color.White, Color.SandyBrown, Color.Gray };

        public Vector2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }

        public Asteroid(Game game, SpriteBatch spriteBatch, Texture2D tex, Vector2 pos, Vector2 speed, float rotationSpeed)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            this.pos = pos;
            origin = new Vector2(tex.Width / 2, tex.Height / 2);
            this.speed = speed;
            this.rotationSpeed = rotationSpeed;
            Random random = new Random();
            int randomColor = random.Next(0, colors.Length);
            this.color = colors[randomColor];
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            rotation -= rotationSpeed;
            pos.Y += speed.Y;
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
            spriteBatch.Draw(tex, pos, new Rectangle(0, 0, tex.Width, tex.Height), color, rotation, origin, scale, SpriteEffects.None, 0f);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public Rectangle GetBoundaries()
        {
            return new Rectangle((int)pos.X, (int)pos.Y, tex.Width, tex.Height);
        }
    }
}