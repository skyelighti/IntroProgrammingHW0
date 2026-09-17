using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace ShooterTest.Game
{
    public class Player : GameObject, ICollidable
    {
        private Vector2 playerSpeed = new Vector2 (2, 2);
        private KeyboardState previousKeyboard;
        private Vector2 respawnPos;
        private float timer; 
        private float cooldown = 2f;
        public Rectangle BoxCollider
        {
            get
            {
                return new Rectangle( (int)location.X, (int)location.Y, (int)size.X, (int)size.Y);
            }
            //returns new rect everytime its called, so its always accurate
        }
        public Player(ContentManager content) : base(content, "Player", "Player") {
            //UpdateSpriteLocation(64, 0, 16, 16);
            UpdateLocation((Main.ScreenBounds.Width - size.X) / 2, Main.ScreenBounds.Height - size.Y);
            respawnPos = location;
            timer = 2f;
            CollisionManager.Instance.AddCollidable(this);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!IsActive) { return; }

            if (timer < cooldown)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            KeyboardState keyboardState = Keyboard.GetState();

            //reminder to attempt to implement command pattern if i have time gotta practive :'(
            foreach (Keys key in keyboardState.GetPressedKeys()) {
                switch (key)
                {
                    case Keys.Right or Keys.D:
                        moveRight();
                        break;
                    case Keys.Left or Keys.A:
                        moveLeft();
                        break;
                    case Keys.Up or Keys.W:
                        moveUp();
                        break;
                    case Keys.Down or Keys.S:
                        moveDown();
                        break;
                    case Keys.Enter or Keys.Space:
                        if ((!previousKeyboard.IsKeyDown(Keys.Enter) && !previousKeyboard.IsKeyDown(Keys.Space)) && timer >= cooldown)
                        {
                            Shoot();
                            //maybe redundant. might remove depending on timer. 
                        }
                        break;
                }
            }
            previousKeyboard = keyboardState;
            //collision handler implementation :'(
        }
        void Shoot()
        {
            if (!IsActive || cooldown > timer) { return; }
            timer = 0f;
            BulletManager.Instance.FireBullet(this);
        }
        void moveLeft() {
            if (location.X > 0)
            {
                UpdateLocationX(location.X - playerSpeed.X);
            }
        }
        void moveRight()
        {
            if (location.X + size.X < Main.ScreenBounds.Width)
            {
                UpdateLocationX(location.X + playerSpeed.X);
            }
        }
        void moveUp() {
            if (location.Y > 0)
            {
                UpdateLocationY(location.Y - playerSpeed.Y);
            }
        }
         void moveDown() {
            if (location.Y + size.Y < Main.ScreenBounds.Height)
            {
                UpdateLocationY(location.Y + playerSpeed.Y);
            }
        }
        public void Respawn() {
            //smth smth draw calls? 
            UpdateLocation(respawnPos.X, respawnPos.Y);
            HideFor(2f);
        }

        public void OnCollision(ICollidable other)
        {
            if (!spriteVisible) { return; }
            if (other is Bullet bullet) {
                if (bullet.owner is Player) {
                    return;
                }
                Respawn();
            }
            //enemies handle gameover
        }
    }
}
