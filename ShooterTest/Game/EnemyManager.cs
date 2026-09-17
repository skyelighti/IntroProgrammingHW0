using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace ShooterTest.Game
{

    public class EnemyManager
    {
        public static EnemyManager Instance { get; private set; }
        private ObjectPool<BaseEnemy> EnemyPool;
        private Rectangle EnemySpawnRect;
        private List<Rectangle> EnemySpawnPoints;
        int EnemySpawnsNeeded;
        //amount needed to be spawned still if all rects are full.
        public EnemyManager(ContentManager content)
        {
            int enemyX = 64;
            int enemyY = 64;
            Instance = this;
            EnemySpawnRect = new Rectangle(30, 30, Main.ScreenBounds.Width - 30, (int)(Main.ScreenBounds.Height * 0.3f));
            EnemySpawnPoints = new List<Rectangle>();
            int j = EnemySpawnRect.Top;
            int i = EnemySpawnRect.Left;
            while (j < EnemySpawnRect.Height)
            {
                //temp hardcoded values
                while(i <EnemySpawnRect.Width) 
                {
                    Rectangle r = new Rectangle(i, j, enemyX, enemyY);
                    EnemySpawnPoints.Add(r);
                    i += enemyX;
                }
                j += enemyY;
            }
            EnemyPool = new ObjectPool<BaseEnemy>(50, CreateEnemy);
            RequestSpawn(10);
            //make sure a couple of enemies are spawned correctly at runtime.
        }

        public BaseEnemy CreateEnemy()
        {
            BaseEnemy e;
            int rand = Random.Shared.Next(0, 2);
            if (rand == 0)
            {
                e = new AdvancedEnemy(Main.contentManager, "AdvancedEnemy1", "AdvancedEnemy");
            }
            else {
                e = new BaseEnemy(Main.contentManager, "BaseEnemy1", "Enemy");
            }
            e.Deactivate();
            CollisionManager.Instance.AddCollidable(e);
            return e;
            //will need to accomodate multiple enemy types. 
        }
        public void SpawnEnemy() {

            foreach (Rectangle r in EnemySpawnPoints) {
                bool clear = true;
                if (EnemySpawnsNeeded == 0)
                {
                    return;
                }
                foreach (BaseEnemy b in EnemyPool.GetActiveObjects())
                {
                    if (r.Intersects(b.BoxCollider))
                    {
                        clear = false;
                        break;
                    }

                }
                if (!clear) { continue; }
                    //spawn location
                BaseEnemy be = EnemyPool.GetPooledObj();
                if (be == null) {
                    return; 
                }
                be.target = Vector2.Zero;
                be.timer = 0f;
                be.enemySpeed = Vector2.One;
                be.UpdateLocation(r.Left, r.Top);
                be.Activate();
                EnemySpawnsNeeded -= 1;

            }
            //needs to make sure 0,0 or spawn area is empty, cannot have overlap before spawn.
            //while loop? make a rect to overlap and check before spawn, until it can spawn? 
        }
        public void RequestSpawn(int count) {
            EnemySpawnsNeeded += count;
        }
        public void Update(GameTime gameTime)
        {
            SpawnEnemy();
            EnemyPool.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            EnemyPool.Draw(spriteBatch);
        }
    }
}
