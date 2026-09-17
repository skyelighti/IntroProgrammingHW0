using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace ShooterTest.Game
{
    public class BulletManager
    {
        public static BulletManager Instance;
        private ObjectPool<Bullet> BulletPool;
        public BulletManager(ContentManager content) { 
            Instance = this;
            BulletPool = new ObjectPool<Bullet>(40, CreateBullet);
            //constructs and inits object pool here
        }
        public Bullet CreateBullet()
        {
            Bullet b = new Bullet(Main.contentManager, "Bullet", "Bullet");
            b.Deactivate();
            CollisionManager.Instance.AddCollidable(b);
            return b;
        }
        public void FireBullet(GameObject owner) {
            Debug.WriteLine("bullet has been fired");
            Bullet b = BulletPool.GetPooledObj();

            if (b == null) { return; }
            b.SetOwner(owner);
            b.UpdateLocation(owner.location.X, owner.location.Y);
            b.timer = 0f;
            //resets timer before returning to pool
            b.Activate();

            //based upon owner direction should be fixed i think unless we want to store last pressed button for player bullet direction

        }
        public void Update(GameTime gameTime) {
            BulletPool.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch) {
            BulletPool.Draw(spriteBatch);
        }
    }
}
