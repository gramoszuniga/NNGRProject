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
    public class StartScene : GameScene
    {
        private SpriteBatch spriteBatch;
        private Menu menu;
        private string[] menuItems = { "New Game", "High Scores", "Help", "How to Play", "About", "Exit" };
        private SoundEffect backgroundMusic;

        public Menu Menu
        {
            get { return menu; }
            set { menu = value; }
        }

        public StartScene(Game game, SpriteBatch spriteBatch)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            Background background = new Background(game, spriteBatch, game.Content.Load<Texture2D>("images/NNGRBackground"), new Vector2(0, 0));
            this.Components.Add(background);
            this.menu = new Menu(game, spriteBatch,
                game.Content.Load<SpriteFont>("Fonts/regularFont"),
                game.Content.Load<SpriteFont>("Fonts/hilightFont"),
                menuItems);
            this.backgroundMusic = game.Content.Load<SoundEffect>("Sounds/NNGRBackground");
            SoundEffectInstance instance = backgroundMusic.CreateInstance();
            instance.IsLooped = true;
            this.Components.Add(menu);
            backgroundMusic.Play();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}