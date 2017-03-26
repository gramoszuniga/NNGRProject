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
    public class ScoreScene : GameScene
    {
        SpriteBatch spriteBatch;
        Texture2D tex;
        List<int> scores;
        ScoreFont scoreFont;
        SpriteFont spriteFont;
        const int MAX_SCORE_NUM = 10;

        public ScoreScene(Game game, SpriteBatch spriteBatch)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.tex = game.Content.Load<Texture2D>("Images/NNGRBackground");
            this.spriteFont = game.Content.Load<SpriteFont>("Fonts/regularFont");
            this.scoreFont = new ScoreFont(game, spriteBatch, spriteFont, Color.White);
            this.Components.Add(scoreFont);
            if (scores == null)
            {
                scores = new List<int>();
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            scores = FileManager.Load<List<int>>();
            scoreFont.Text = "High Scores: \n";
            if (scores != null)
            {
                scores.Sort();
                scores.Reverse();
                int scoresCounter = 0;
                foreach (int score in scores)
                {
                    scoreFont.Text += score + "\n";
                    if (scoresCounter >= MAX_SCORE_NUM - 1)
                    {
                        break;
                    }
                    scoresCounter++;
                }
            }
            scoreFont.Position = new Vector2(Shared.stage.X / 2 - spriteFont.MeasureString(scoreFont.Text).X / 2, Shared.stage.Y / 4);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(tex, Vector2.Zero, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}