using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace ShooterTest.Game
{
    public class BaseEnemy : GameObject, ICollidable
    {
        public Vector2 enemySpeed = new Vector2(1, 1);
        public float timer = 0;
        private float waitTime = 2f; 
        public Vector2 target = new Vector2(0, 0);
        public float animTimer = 0f;
        public List<string> frames = new List<string>();
        public int currentframe = 0;
        public float aTimer = 0.5f;
        //reverse the X when it hits bounds
        public Rectangle BoxCollider
        {
            get
            {
                return new Rectangle((int)location.X, (int)location.Y, (int)size.X, (int)size.Y);
            }
            //returns new rect everytime its called, so its always accurate
        }
        public BaseEnemy(ContentManager content, string spriteName, string name) : base(content, spriteName, name)
        {
            // add later & figure out enemy behvaior
            //regular enemy sprite 
            frames.Add("BaseEnemy1");
            frames.Add("BaseEnemy2");

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            animTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (animTimer >= aTimer && frames.Count != 0) {
                animTimer = 0;
                if (currentframe + 1 >= frames.Count)
                {
                    currentframe = 0;
                }
                else {
                    currentframe++;
                }
                UpdateSprite(frames[currentframe]);
            }
            if (timer <= waitTime) { 
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (BoxCollider.Bottom >= Main.ScreenBounds.Bottom) {
                Debug.WriteLine("GameOver");
                //Gameover
                //implement statemachine for this?
            }
                //check if it hits the client bounds for the um x directions mainly
                // needs to be adjusted in case of smth blocking path?
            if (location.X + size.X > Main.ScreenBounds.Width || location.X < 0)
            {
                if (target == Vector2.Zero) {
                    target = new Vector2(location.X, location.Y + size.Y);
                    enemySpeed.X *= -1;
                }
                UpdateLocationY(location.Y + enemySpeed.Y);
                if (location == target) { 
                    target = Vector2.Zero;
                    UpdateLocationX(location.X + enemySpeed.X);
                    //so the code doenst loop?
                }
                //revise how location is checked, orign point is a little fucked mayube.
            }
            else {
                UpdateLocationX(location.X + enemySpeed.X);
            }
            //add a check for the bottom of the screen, if it hits the bottom then dies
        }
        public void OnCollision(ICollidable other)
        {
            //check the base class of other object and do handling based upn class?
            if (other is Bullet b)
            {
                //check and cast
                if (b.owner is Player)
                {
                    Deactivate();
                    EnemyManager.Instance.RequestSpawn(2);
                }
            }
            if (other is Player)
            {
                Debug.WriteLine("player has lost.");
            }
        }

    }

}
