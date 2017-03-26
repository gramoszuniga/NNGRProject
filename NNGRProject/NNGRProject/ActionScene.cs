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
    public class ActionScene : GameScene
    {
        private SpriteBatch spriteBatch;
        private Ship ship;
        private Texture2D texBeam, enemyTexBeam, asteTex, exploTex, shipTex, enemyTex, bgTex, bgTex2, flTex;
        private List<Beam> beams;
        private List<EnemyBeam> enemyBeams;
        private List<Asteroid> asteroids;
        private List<EnemyShip> enemies;
        private List<SoundEffectInstance> explosionSounds;
        private List<int> scores;
        private Background background;
        private SpriteFont spriteFont;
        private ScoreFont lives, scoreFont, gameOverFont;
        private SoundEffect explosionSound, beamSound;
        private SoundEffectInstance beamInstance, explosionInstance;
        private int fireRate = 0, enemyFireRate = 0, asteroidSpawnRate = 0, shipLives = 3, score = 0, levelTransitionCounter = 0;
        private bool isDamaged = false, isGameOver = false, isSecondLevel = false, levelTransition = false, IsGodMode = false;
        private int shipDamaged = 30;
        private const int MAX_EXPLOSION_SOUNDS = 10, MAX_LVLONE_SCORE = 1500, LVL_TRANSITION_TIME = 420, EXPLOSION_DELAY = 1;
        private Random random;
        private KeyboardState ks = Keyboard.GetState();
        private FinishLine fl;

        public ActionScene(Game game, SpriteBatch spriteBatch, bool IsMouseControlled)
            : base(game)
        {
            bgTex = game.Content.Load<Texture2D>("Images/NNGRLevel1");
            bgTex2 = game.Content.Load<Texture2D>("Images/NNGRLevel2");
            background = new Background(game, spriteBatch, bgTex, new Vector2(0, 1));
            this.Components.Add(background);
            texBeam = game.Content.Load<Texture2D>("Images/NNGRBeam");
            enemyTexBeam = game.Content.Load<Texture2D>("Images/NNGRBeam");
            this.spriteBatch = spriteBatch;
            shipTex = game.Content.Load<Texture2D>("Images/NNGRShip");
            ship = new Ship(game, spriteBatch, shipTex);
            ship.IsMouseControlled = IsMouseControlled;
            this.Components.Add(ship);
            beams = new List<Beam>();
            enemyBeams = new List<EnemyBeam>();
            asteroids = new List<Asteroid>();
            enemies = new List<EnemyShip>();
            asteTex = game.Content.Load<Texture2D>("Images/NNGRAsteroid");
            Vector2 asteSpeed = new Vector2(0, 3);
            spriteFont = game.Content.Load<SpriteFont>("Fonts/regularFont");
            lives = new ScoreFont(game, spriteBatch, spriteFont, Color.White);
            scoreFont = new ScoreFont(game, spriteBatch, spriteFont, Color.White);
            exploTex = game.Content.Load<Texture2D>("Images/NNGRExplosion");
            enemyTex = game.Content.Load<Texture2D>("Images/NNGREnemyShip");
            random = new Random();
            explosionSound = game.Content.Load<SoundEffect>("Sounds/NNGRExplosion");
            beamSound = game.Content.Load<SoundEffect>("Sounds/NNGRBeamShoot");
            this.Components.Add(lives);
            this.Components.Add(scoreFont);
            explosionSounds = new List<SoundEffectInstance>();
            flTex = game.Content.Load<Texture2D>("Images/NNGRFinishLine");
            fl = new FinishLine(Game, spriteBatch, new Vector2(0, 0), flTex);
            this.Components.Add(fl);
            fl.Visible = false;
            fl.Enabled = false;
            scores = FileManager.Load<List<int>>();
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
            MouseState mouseState = Mouse.GetState();
            KeyboardState NewKeyState = Keyboard.GetState();

            if (NewKeyState.IsKeyDown(Keys.G) && ks.IsKeyUp(Keys.G))
            {
                IsGodMode = !IsGodMode;
            }

            if (isGameOver && ks.IsKeyDown(Keys.R))
            {
                Restart();
            }

            lives.Text = "Lives: " + shipLives;
            scoreFont.Text = "Score: " + score;
            scoreFont.Position = new Vector2(Shared.stage.X - spriteFont.MeasureString(scoreFont.Text).X, 0);

            fireRate++;
            if (fireRate >= 12 && ((ks.IsKeyDown(Keys.Space) && !ship.IsMouseControlled) || (mouseState.LeftButton == ButtonState.Pressed && ship.IsMouseControlled)) && !isGameOver && !fl.Visible)
            {
                Beam beam = new Beam(Game, spriteBatch, new Vector2(ship.Pos.X + ship.Tex.Width / 2 - texBeam.Width / 2, ship.Pos.Y - texBeam.Height / 2), texBeam);
                this.beams.Add(beam);
                this.Components.Add(beam);
                beamInstance = beamSound.CreateInstance();
                beamInstance.Play();
                fireRate = 0;
            }


            if (!levelTransition)
            {
                asteroidSpawnRate++;
            }
            if (asteroidSpawnRate >= 30)
            {
                int randPosition = random.Next(asteTex.Width, (int)Shared.stage.X - asteTex.Width);
                int randSpeed = random.Next(3, 9);
                float randRotationSpeed = (float)random.Next(1, 9) / 100;
                Asteroid asteroid = new Asteroid(Game, spriteBatch, asteTex, new Vector2(randPosition, 0 - asteTex.Height), new Vector2(0, randSpeed), randRotationSpeed);
                this.asteroids.Add(asteroid);
                this.Components.Add(asteroid);
                EnemyShip enemy;
                randPosition = random.Next(asteTex.Width, (int)Shared.stage.X - asteTex.Width);
                if (isSecondLevel)
                {
                    int horizontalSpeed = random.Next(1, 3 + 1);
                    int direction = random.Next(0, 1 + 1);
                    if (direction < 1)
                    {
                        horizontalSpeed = -horizontalSpeed;
                    }
                    enemy = new EnemyShip(Game, spriteBatch, enemyTex, new Vector2(horizontalSpeed, 3), true);
                }
                else
                {
                    enemy = new EnemyShip(Game, spriteBatch, enemyTex, new Vector2(0, 3), false);
                }
                enemy.Pos = new Vector2(randPosition, 0 - enemyTex.Height);
                this.enemies.Add(enemy);
                this.Components.Add(enemy);
                asteroidSpawnRate = 0;
            }

            foreach (Asteroid a in asteroids)
            {
                if (ship.GetBoundaries().Intersects(a.GetBoundaries()))
                {
                    Explosion explosion = new Explosion(Game, spriteBatch, exploTex, a.Pos, EXPLOSION_DELAY);
                    this.Components.Add(explosion);
                    explosion.Enabled = true;
                    explosion.Visible = true;
                    if (explosionSounds.Count < MAX_EXPLOSION_SOUNDS)
                    {
                        explosionInstance = explosionSound.CreateInstance();
                        explosionSounds.Add(explosionInstance);
                        explosionInstance.Play();
                    }
                    a.Enabled = false;
                    a.Visible = false;
                    ship.Color = Color.Red;
                    isDamaged = true;
                    if (!IsGodMode)
                    {
                        shipLives--;
                    }
                    if (shipLives == 0)
                    {
                        Explosion explosion2 = new Explosion(Game, spriteBatch, exploTex, ship.Pos, EXPLOSION_DELAY);
                        this.Components.Add(explosion);
                        explosion.Enabled = true;
                        explosion.Visible = true;
                        if (explosionSounds.Count < MAX_EXPLOSION_SOUNDS)
                        {
                            explosionInstance = explosionSound.CreateInstance();
                            explosionSounds.Add(explosionInstance);
                            explosionInstance.Play();
                        }
                        ship.Visible = false;
                        ship.Enabled = false;
                        ship.Pos = new Vector2(0 - ship.Tex.Width, 0 - ship.Tex.Height);
                        gameOverFont = new ScoreFont(Game, spriteBatch, spriteFont, Color.White);
                        gameOverFont.Text = "Game Over\n Press R to restart!";
                        gameOverFont.Position = new Vector2(Shared.stage.X / 2 - spriteFont.MeasureString(scoreFont.Text).X / 2,
                        Shared.stage.Y / 2 - spriteFont.MeasureString(scoreFont.Text).Y / 2);
                        this.Components.Add(gameOverFont);
                        isGameOver = true;
                        scores.Add(score);
                        FileManager.Save(scores);
                    }
                }
            }
            if (isDamaged)
            {
                shipDamaged--;
            }
            if (shipDamaged == 0)
            {
                shipDamaged = 30;
                isDamaged = false;
                ship.Color = Color.White;
            }

            foreach (Asteroid a in asteroids)
            {
                foreach (Beam b in beams)
                {
                    if (a.GetBoundaries().Intersects(b.GetBoundaries()))
                    {
                        Explosion explosion = new Explosion(Game, spriteBatch, exploTex, a.Pos, EXPLOSION_DELAY);
                        this.Components.Add(explosion);
                        explosion.Enabled = true;
                        explosion.Visible = true;
                        if (explosionSounds.Count < MAX_EXPLOSION_SOUNDS)
                        {
                            explosionInstance = explosionSound.CreateInstance();
                            explosionSounds.Add(explosionInstance);
                            explosionInstance.Play();
                        }
                        a.Enabled = false;
                        a.Visible = false;
                        b.Enabled = false;
                        b.Visible = false;
                        if (shipLives > 0)
                        {
                            score += 5;
                        }
                    }
                }
            }

            foreach (EnemyShip e in enemies)
            {
                foreach (Beam b in beams)
                {
                    if (e.GetBoundaries().Intersects(b.GetBoundaries()))
                    {
                        Explosion explosion = new Explosion(Game, spriteBatch, exploTex, e.Pos, EXPLOSION_DELAY);
                        this.Components.Add(explosion);
                        explosion.Enabled = true;
                        explosion.Visible = true;
                        if (explosionSounds.Count < MAX_EXPLOSION_SOUNDS)
                        {
                            explosionInstance = explosionSound.CreateInstance();
                            explosionSounds.Add(explosionInstance);
                            explosionInstance.Play();
                        }
                        e.Enabled = false;
                        e.Visible = false;
                        b.Enabled = false;
                        b.Visible = false;
                        if (shipLives > 0)
                        {
                            score += 10;
                        }
                    }
                }
                if (ship.GetBoundaries().Intersects(e.GetBoundaries()))
                {
                    Explosion explosion = new Explosion(Game, spriteBatch, exploTex, e.Pos, EXPLOSION_DELAY);
                    this.Components.Add(explosion);
                    explosion.Enabled = true;
                    explosion.Visible = true;
                    if (explosionSounds.Count < MAX_EXPLOSION_SOUNDS)
                    {
                        explosionInstance = explosionSound.CreateInstance();
                        explosionSounds.Add(explosionInstance);
                        explosionInstance.Play();
                    }
                    e.Enabled = false;
                    e.Visible = false;
                    ship.Color = Color.Red;
                    isDamaged = true;
                    if (!IsGodMode)
                    {
                        shipLives--;
                    }
                    if (shipLives == 0)
                    {
                        Explosion explosion2 = new Explosion(Game, spriteBatch, exploTex, ship.Pos, EXPLOSION_DELAY);
                        this.Components.Add(explosion);
                        explosion.Enabled = true;
                        explosion.Visible = true;
                        if (explosionSounds.Count < MAX_EXPLOSION_SOUNDS)
                        {
                            explosionInstance = explosionSound.CreateInstance();
                            explosionSounds.Add(explosionInstance);
                            explosionInstance.Play();
                        }
                        ship.Visible = false;
                        ship.Enabled = false;
                        ship.Pos = new Vector2(0 - ship.Tex.Width, 0 - ship.Tex.Height);
                        gameOverFont = new ScoreFont(Game, spriteBatch, spriteFont, Color.White);
                        gameOverFont.Text = "Game Over\n Press R to restart!";
                        gameOverFont.Position = new Vector2(Shared.stage.X / 2 - spriteFont.MeasureString(scoreFont.Text).X / 2,
                        Shared.stage.Y / 2 - spriteFont.MeasureString(scoreFont.Text).Y / 2);
                        this.Components.Add(gameOverFont);
                        isGameOver = true;
                        scores.Add(score);
                        FileManager.Save(scores);
                    }
                }
            }

            foreach (EnemyBeam eb in enemyBeams)
            {
                if (ship.GetBoundaries().Intersects(eb.GetBoundaries()))
                {
                    Explosion explosion = new Explosion(Game, spriteBatch, exploTex, eb.Pos, EXPLOSION_DELAY);
                    this.Components.Add(explosion);
                    explosion.Enabled = true;
                    explosion.Visible = true;
                    if (explosionSounds.Count < MAX_EXPLOSION_SOUNDS)
                    {
                        explosionInstance = explosionSound.CreateInstance();
                        explosionSounds.Add(explosionInstance);
                        explosionInstance.Play();
                    }
                    eb.Enabled = false;
                    eb.Visible = false;
                    ship.Color = Color.Red;
                    isDamaged = true;
                    if (!IsGodMode)
                    {
                        shipLives--;
                    }
                    if (shipLives == 0)
                    {
                        Explosion explosion2 = new Explosion(Game, spriteBatch, exploTex, ship.Pos, EXPLOSION_DELAY);
                        this.Components.Add(explosion);
                        explosion.Enabled = true;
                        explosion.Visible = true;
                        if (explosionSounds.Count < MAX_EXPLOSION_SOUNDS)
                        {
                            explosionInstance = explosionSound.CreateInstance();
                            explosionSounds.Add(explosionInstance);
                            explosionInstance.Play();
                        }
                        ship.Visible = false;
                        ship.Enabled = false;
                        ship.Pos = new Vector2(0 - ship.Tex.Width, 0 - ship.Tex.Height);
                        gameOverFont = new ScoreFont(Game, spriteBatch, spriteFont, Color.White);
                        gameOverFont.Text = "Game Over\n Press R to restart!";
                        gameOverFont.Position = new Vector2(Shared.stage.X / 2 - spriteFont.MeasureString(scoreFont.Text).X / 2,
                        Shared.stage.Y / 2 - spriteFont.MeasureString(scoreFont.Text).Y / 2);
                        this.Components.Add(gameOverFont);
                        isGameOver = true;
                        scores.Add(score);
                        FileManager.Save(scores);
                    }
                }
            }

            enemyFireRate++;
            if (enemyFireRate >= 60 && !isGameOver)
            {
                foreach (EnemyShip e in enemies)
                {
                    int randomShoot = random.Next(0, 3 + 1);
                    if (randomShoot == 1)
                    {
                        EnemyBeam beam = new EnemyBeam(Game, spriteBatch, new Vector2(e.Pos.X + enemyTexBeam.Width / 2 - texBeam.Width / 2, e.Pos.Y + enemyTexBeam.Height), texBeam);
                        this.enemyBeams.Add(beam);
                        this.Components.Add(beam);
                        beamInstance = beamSound.CreateInstance();
                        beamInstance.Play();
                    }
                }
                enemyFireRate = 0;
            }

            for (int i = 0; i < asteroids.Count(); i++)
            {
                if (!asteroids[i].Enabled)
                {
                    asteroids.Remove(asteroids[i]);
                }
            }

            for (int i = 0; i < beams.Count(); i++)
            {
                if (!beams[i].Enabled)
                {
                    beams.Remove(beams[i]);
                }
            }

            for (int i = 0; i < enemies.Count(); i++)
            {
                if (!enemies[i].Enabled)
                {
                    enemies.Remove(enemies[i]);
                }
            }

            for (int i = 0; i < enemyBeams.Count(); i++)
            {
                if (!enemyBeams[i].Enabled)
                {
                    enemyBeams.Remove(enemyBeams[i]);
                }
            }

            for (int i = 0; i < explosionSounds.Count(); i++)
            {
                if (explosionSounds[i].State == SoundState.Stopped)
                {
                    explosionSounds.Remove(explosionSounds[i]);
                }
            }

            if (score >= MAX_LVLONE_SCORE && !isSecondLevel)
            {
                asteroidSpawnRate = 0;
                levelTransition = true;
            }
            if (levelTransition)
            {
                levelTransitionCounter++;
            }
            if (levelTransitionCounter >= LVL_TRANSITION_TIME / 2)
            {
                fl.Visible = true;
                fl.Enabled = true;
                foreach (Asteroid a in asteroids)
                {
                    a.Visible = false;
                    a.Enabled = false;
                }
                foreach (Beam b in beams)
                {
                    b.Visible = false;
                    b.Enabled = false;
                }
                foreach (EnemyBeam eb in enemyBeams)
                {
                    eb.Visible = false;
                    eb.Enabled = false;
                }
                foreach (EnemyShip e in enemies)
                {
                    e.Visible = false;
                    e.Enabled = false;
                }
            }
            if (levelTransitionCounter >= LVL_TRANSITION_TIME)
            {
                levelTransitionCounter = 0;
                levelTransition = false;
                fl.Visible = false;
                fl.Enabled = false;
                isSecondLevel = true;
                background.Tex = bgTex2;
                ship.Pos = new Vector2(Shared.stage.X / 2 - shipTex.Width / 2, Shared.stage.Y - shipTex.Height);
            }

            ks = NewKeyState;
            base.Update(gameTime);
        }

        private void Restart()
        {
            foreach (Asteroid a in asteroids)
            {
                a.Visible = false;
                a.Enabled = false;
            }
            foreach (Beam b in beams)
            {
                b.Visible = false;
                b.Enabled = false;
            }
            foreach (EnemyBeam eb in enemyBeams)
            {
                eb.Visible = false;
                eb.Enabled = false;
            }
            foreach (EnemyShip e in enemies)
            {
                e.Visible = false;
                e.Enabled = false;
            }
            score = 0;
            shipLives = 3;
            gameOverFont.Text = string.Empty;
            isGameOver = false;
            ship.Pos = new Vector2(Shared.stage.X / 2 - shipTex.Width / 2, Shared.stage.Y - shipTex.Height);
            ship.Visible = true;
            ship.Enabled = true;
        }
    }
}