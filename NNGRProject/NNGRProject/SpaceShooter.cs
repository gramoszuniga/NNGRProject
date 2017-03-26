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
    public class SpaceShooter : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private StartScene startScene;
        private ScoreScene scoreScene;
        private HelpScene helpScene;
        private HowScene howScene;
        private AboutScene aboutScene;
        private ActionScene actionScene;
        private InputScene inputScene;
        private KeyboardState ks;

        public SpaceShooter()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 900;
            graphics.PreferredBackBufferWidth = 700;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Shared.stage = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            startScene = new StartScene(this, spriteBatch);
            this.Components.Add(startScene);

            scoreScene = new ScoreScene(this, spriteBatch);
            this.Components.Add(scoreScene);

            helpScene = new HelpScene(this, spriteBatch);
            this.Components.Add(helpScene);

            howScene = new HowScene(this, spriteBatch);
            this.Components.Add(howScene);

            aboutScene = new AboutScene(this, spriteBatch);
            this.Components.Add(aboutScene);

            actionScene = new ActionScene(this, spriteBatch, false);

            inputScene = new InputScene(this, spriteBatch);
            this.Components.Add(inputScene);

            startScene.show();
        }

        protected override void UnloadContent()
        {
        }

        private void hideAll()
        {
            GameScene gs = null;
            foreach (GameComponent item in Components)
            {
                if (item is GameScene)
                {
                    gs = (GameScene)item;
                    gs.hide();
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            int selectedIndex = 0;
            KeyboardState NewKeyState = Keyboard.GetState();
            if (startScene.Enabled)
            {
                selectedIndex = startScene.Menu.SelectedIndex;
                if (selectedIndex == 0 && ks.IsKeyDown(Keys.Enter))
                {
                    hideAll();
                    inputScene.show();
                }
                else if (selectedIndex == 1 && ks.IsKeyDown(Keys.Enter))
                {
                    hideAll();
                    scoreScene.show();
                }
                else if (selectedIndex == 2 && ks.IsKeyDown(Keys.Enter))
                {
                    hideAll();
                    helpScene.show();
                }
                else if (selectedIndex == 3 && ks.IsKeyDown(Keys.Enter))
                {
                    hideAll();
                    howScene.show();
                }
                else if (selectedIndex == 4 && ks.IsKeyDown(Keys.Enter))
                {
                    hideAll();
                    aboutScene.show();
                }
                else if (selectedIndex == 5 && ks.IsKeyDown(Keys.Enter))
                {
                    Exit();
                }
            }

            if (inputScene.Enabled)
            {
                if (NewKeyState.IsKeyDown(Keys.Enter) && ks.IsKeyUp(Keys.Enter))
                {
                    int selectedInput = inputScene.Menu.SelectedIndex;
                    if (selectedInput == 0 && NewKeyState.IsKeyDown(Keys.Enter))
                    {
                        actionScene = new ActionScene(this, spriteBatch, true);
                        this.Components.Add(actionScene);
                        hideAll();
                        actionScene.show();
                    }
                    else if (selectedInput == 1 && NewKeyState.IsKeyDown(Keys.Enter))
                    {
                        actionScene = new ActionScene(this, spriteBatch, false);
                        this.Components.Add(actionScene);
                        hideAll();
                        actionScene.show();
                    }
                }
            }

            if (scoreScene.Enabled || helpScene.Enabled || howScene.Enabled || aboutScene.Enabled || inputScene.Enabled || actionScene.Enabled)
            {
                if (ks.IsKeyDown(Keys.Escape))
                {
                    hideAll();
                    startScene.show();
                }
            }

            ks = NewKeyState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}