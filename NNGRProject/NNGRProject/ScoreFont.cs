using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNGRProject
{
    public class ScoreFont : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private string text;
        private Vector2 position;
        private Color color;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public ScoreFont(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont, Color color)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
            this.text = string.Empty;
            this.position = Vector2.Zero;
            this.color = color;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, text, position, color);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}