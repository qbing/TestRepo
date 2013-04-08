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

namespace WindowsGame3
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        Texture2D backgroundTexture;
        Texture2D mapTexture;
        Texture2D actorTexture;

        Vector2 original = Vector2.Zero;
        Vector2  current = Vector2.Zero;

        private const float mapbasePosY = 320;
        private const float mapbasePosX = 0;
        private const float actorbasePosX = 50;
        private const float actorbasePosY = mapbasePosY - 65;

        Vector2 mapPos = new Vector2(mapbasePosX, mapbasePosY);
        Vector2 actorPos = new Vector2(actorbasePosX, actorbasePosY);
       

        float speedActor = 4f;

        private bool _isJumping;
        private bool _jumpKeyDown;
        private float _jumpHeight = 200f;
        private TimeSpan _jumpTimeSpan = TimeSpan.FromMilliseconds(500);
        private double _jumpStartTime;
        private float _jumpStartPosition;

        private float DoJump(float velocityY, GameTime gameTime)
        {
            if (_jumpKeyDown && !_isJumping)
            {
                _isJumping = true;
                _jumpStartTime = gameTime.TotalGameTime.TotalMilliseconds;
                _jumpStartPosition = velocityY;
            }

            if (_isJumping)
            {
                var jumpTime = gameTime.TotalGameTime.TotalMilliseconds - _jumpStartTime;
                var topTime = _jumpTimeSpan.TotalMilliseconds / 2;
                if (jumpTime < _jumpTimeSpan.TotalMilliseconds)
                {
                    if (jumpTime > topTime)
                    {
                        velocityY =
                            _jumpStartPosition - _jumpHeight - (float)(-(jumpTime - topTime) * _jumpHeight / (_jumpTimeSpan.TotalMilliseconds - topTime));
                    }
                    else
                    {
                        velocityY = _jumpStartPosition + (float)(-(jumpTime) * _jumpHeight / topTime);
                    }
                }
                else
                {
                    _isJumping = false;
                    velocityY = _jumpStartPosition;
                }

            }
            return velocityY;
        }



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

     
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

       
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
           
            
            backgroundTexture = Content.Load<Texture2D>(@"Images/background");
            mapTexture = Content.Load<Texture2D>(@"Images/map");
            actorTexture = Content.Load<Texture2D>(@"Images/actor");
           
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
          
             _jumpKeyDown = Keyboard.GetState().IsKeyDown(Keys.Space);
             actorPos.Y = DoJump(actorPos.Y, gameTime);

             if (_jumpKeyDown) 
                 actorPos.X += speedActor;
             if (actorPos.X > Window.ClientBounds.Width - actorTexture.Width || actorPos.X < 0)
                 speedActor *= -1;

            base.Update(gameTime);
        }

   
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
           
             spriteBatch.Draw(backgroundTexture,
                new Rectangle(0, 0, Window.ClientBounds.Width,
                Window.ClientBounds.Height), null,
                Color.White, 0, Vector2.Zero,
                 SpriteEffects.None, 0);

          spriteBatch.Draw(mapTexture,
            mapPos, null,
           Color.White, 0, Vector2.Zero,
           1,
            SpriteEffects.None,
           0); 
  
           spriteBatch.Draw(actorTexture, actorPos, null,
               Color.Chartreuse,
               0, Vector2.Zero,
               1,
               SpriteEffects.None,
               0);
            
           spriteBatch.End();
           base.Draw(gameTime);
        }
    }
}
