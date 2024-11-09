using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject5.StateManagement;
using System.Collections.Generic;
using GameProject5.Models;
using GameProject5.Camera;


namespace GameProject5.Screens
{
    // This screen implements the actual game logic. It is just a
    // placeholder to get the idea across: you'll probably want to
    // put some more interesting gameplay in here!
    public class GameplayScreen : GameScreen
    {
        // The content manager
        private ContentManager _content;

        // The font for the game
        private SpriteFont _gameFont;

        // The sound manager for the game (sfx)
        private SoundManager _sound;

        // a random number generator
        private readonly Random _random = new Random();

        // The pause alpha
        private float _pauseAlpha;

        // The pause action for the game
        private readonly InputAction _pauseAction;

        // The list of crates that are in the game world
        List<Crate> crates = new List<Crate>();

        // The starting platform for the tower
        Platform platform;

        // The initial background color - to simulate being on the ground
        Color groundColor = Color.ForestGreen;

        // The second background color - to simulate being in the sky
        Color skyColor = Color.SkyBlue;

        // The final background color - to simulate being in space
        Color spaceColor = Color.Black;

        // The max amount of crates before the player wins (used for transitioning to the win screen and background color)
        int maxCrates = 100;

        // Used for transitioning between the ground and sky colors
        int halfMax = 50;

        // The current crate that the player is placing
        Crate currentCrate;

        // Determines if the player is currently placing a crate
        bool isPlacing = true;
        
        // The speed at which the crate moves left and right (incremented by 0.3f every successful placement)
        float moveSpeed = 5f;

        // The direction to move the crate
        float moveDirection = 1f;

        // The max horizontal distance the crate will travel
        float maxHorizontalDistance = 5f;

        // The number of lives the player has 
        int lives = 3;

        // The previous mouse state
        MouseState previousMouseState;

        // The current mouse state
        MouseState currentMouseState;

        // The gravity constant
        private const float Gravity = 9.8f;

        // The support threshold for the crates (determines if a placement is good or bad)
        private const float SupportThreshold = 0.7f;

        // The camera for the game
        CrateCamera camera;

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back }, true);
        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _gameFont = _content.Load<SpriteFont>("PressStart2P-Regular");

            _sound = new SoundManager();
            _sound.LoadContent(_content);

            platform = new Platform(ScreenManager.Game, maxHorizontalDistance * 2);

            camera = new CrateCamera(ScreenManager.Game, new Vector3(0, 10, -20));
            camera.SetTarget(new Vector3(0, 1.0f, 0));

            Vector3 startPosition = new Vector3(0, 2, 0);
            currentCrate = new Crate(ScreenManager.Game, startPosition);

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();
        }

        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                previousMouseState = currentMouseState;
                currentMouseState = Mouse.GetState();
                
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

                if(lives <= 0)
                {
                    ScreenManager.AddScreen(new LoseScreen(crates.Count), ControllingPlayer);
                }

                if(crates.Count >= maxCrates)
                {
                    ScreenManager.AddScreen(new WinScreen(), ControllingPlayer);
                }

                if (isPlacing)
                {
                    Vector3 currentPosition = currentCrate.World.Translation;

                    currentPosition.X += moveSpeed * moveDirection * deltaTime;

                    if(Math.Abs(currentPosition.X) >= maxHorizontalDistance)
                    {
                        moveDirection *= -1;
                        currentPosition.X = MathHelper.Clamp(currentPosition.X, -maxHorizontalDistance, maxHorizontalDistance);
                    }

                    currentCrate.SetPosition(currentPosition);

                    if(currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed)
                    {
                        Vector3 newCratePosition;
                        if (crates.Count > 0)
                        {

                            Crate lastStableCrate = GetLastStableCrate();

                            
                            Vector3 topCratePosition = lastStableCrate.World.Translation;
                            float overlap = Math.Abs(currentPosition.X - topCratePosition.X);

                            if (overlap < SupportThreshold)
                            {                               
                                currentCrate.IsFalling = false;
                                crates.Add(currentCrate);
                                _sound.PlayGoodPlaceSound();

                                
                                camera.SetTarget(new Vector3(0, currentPosition.Y + 2f, 0));

                                newCratePosition = new Vector3(currentPosition.X, currentPosition.Y + 2f, 0);

                                moveSpeed += 0.3f;
                            }
                            else
                            {               
                                currentCrate.IsFalling = true;
                                newCratePosition = currentPosition;
                                crates.Add(currentCrate);
                                _sound.PlayBadPlaceSound();
                                lives--;
                            }
                        }
                        else
                        {
                            // First crate, no support check needed, add to the stack
                            crates.Add(currentCrate);
                            _sound.PlayGoodPlaceSound();
                            newCratePosition = new Vector3(currentPosition.X, currentPosition.Y + 2f, 0);
                        }
                        currentCrate = new Crate(ScreenManager.Game, newCratePosition);


                    }
                }

                ApplyGravity(deltaTime);

                camera.Update(gameTime);
            }
        }

        /// <summary>
        /// Gets the last stable crate that was added to the list
        /// </summary>
        /// <returns>The crate that was last placed 'correctly' in the list</returns>
        public Crate GetLastStableCrate()
        {
            for(int i = crates.Count - 1; i >= 0; i--)
            {
                if (!crates[i].IsFalling)
                {
                    return crates[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Applys gravity to the crates that are falling
        /// </summary>
        /// <param name="deltaTime"></param>
        private void ApplyGravity(float deltaTime)
        {
            foreach(var crate in crates)
            {
                if (crate.IsFalling)
                {
                    Vector3 position = crate.World.Translation;

                    if(position.X > 0)
                    {
                        position.X += 0.1f;
                    }
                    else
                    {
                        position.X -= 0.1f;
                    }

                    position.Y -= Gravity * deltaTime;
                    crate.SetPosition(position);

                    crate.UpdateRotation(deltaTime);

                    if(position.Y < -15f)
                    {
                        crate.IsFalling = false;
                    }
                }
            }
        }

        // Unlike the Update method, this will only be called when the gameplay screen is active.
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // Otherwise move the player position.
                
            }
        }

        /// <summary>
        /// Draws the gameplay screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            float ratio;

            Color backgroundColor;

            if(crates.Count <= halfMax)
            {
                ratio = (float)crates.Count / halfMax;
                backgroundColor = Color.Lerp(groundColor, skyColor, ratio);
            }
            else
            {
                ratio = (float)(crates.Count - halfMax) / (maxCrates - halfMax);
                backgroundColor = Color.Lerp(skyColor, spaceColor, ratio);
            }

            ScreenManager.GraphicsDevice.Clear(backgroundColor);

            ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            // Our player and enemy are both actually just text strings.

            platform.Draw(camera.View, camera.Projection);

            foreach(var crate in crates)
            {
                crate.Draw(camera);
            }

            currentCrate.Draw(camera);
            

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}