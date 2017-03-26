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
    public class Background : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private Vector2 pos1;
        private Vector2 pos2;
        private Vector2 speed;

        public Texture2D Tex
        {
            get { return tex; }
            set { tex = value; }
        }

        public Background(Game game, SpriteBatch spriteBatch, Texture2D tex, Vector2 speed)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            this.pos1 = new Vector2(0, 0);
            this.pos2 = new Vector2(0, -tex.Height);
            this.speed = speed;
        }
      
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            pos1.Y += speed.Y;
            pos2.Y += speed.Y;
            if (pos1.Y > Shared.stage.Y)
            {
                pos1.Y = pos2.Y - tex.Height;
            }
            if (pos2.Y > Shared.stage.Y)
            {
                pos2.Y = pos1.Y - tex.Height;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(tex, pos1, Color.White);
            spriteBatch.Draw(tex, pos2, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}