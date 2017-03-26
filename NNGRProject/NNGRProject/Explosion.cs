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
    public class Explosion : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private Vector2 pos;
        private Vector2 size;
        private List<Rectangle> frames;
        private int frameIndex = -1;
        private int delay;
        private int delayCounter;

        public Vector2 Position
        {
            get { return pos; }
            set { pos = value; }
        }

        public Explosion(Game game, SpriteBatch spriteBatch, Texture2D tex, Vector2 position, int delay)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            this.pos = position;
            this.delay = delay;
            size = new Vector2(64, 64);
            this.Enabled = false;
            this.Visible = false;
            CreateFrames();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            delayCounter++;
            if (delayCounter > delay)
            {
                frameIndex++;
                if (frameIndex > 24)
                {
                    frameIndex = -1;
                    this.Enabled = false;
                    this.Visible = false;
                }
                delayCounter = 0;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            if (frameIndex >= 0)
            {
                spriteBatch.Draw(tex, pos, frames.ElementAt<Rectangle>(frameIndex), Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void CreateFrames()
        {
            frames = new List<Rectangle>();
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    int x = j * (int)size.X;
                    int y = i * (int)size.Y;
                    Rectangle r = new Rectangle(x, y, (int)size.X, (int)size.Y);
                    frames.Add(r);
                }
            }
        }
    }
}