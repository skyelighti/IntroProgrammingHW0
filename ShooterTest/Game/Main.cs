using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Graphics;
using System;   
using System.Diagnostics;
using System.Reflection.Metadata;
namespace ShooterTest.Game
{
    public class Main
    {
        private Texture2D spaceBackgroundTexture;
        public static Rectangle ScreenBounds; 
        public static ContentManager contentManager { get; private set; }
        private CollisionManager collisionManager;
        private BulletManager bulletManager;
        private EnemyManager enemyManager;
        private Player player;
        public static TextureAtlas atlas { get; private set; }

        public Main(ContentManager content) {
            contentManager = content;
            spaceBackgroundTexture = content.Load<Texture2D>("SpaceInvaders_Background");
            Texture2D spaceInvadersSS = content.Load<Texture2D>("SpaceInvaders");
            atlas = new TextureAtlas(spaceInvadersSS);
            atlas.AddRegion("Player", 64, 0, 16, 16);
            atlas.AddRegion("Bullet", 32, 0, 16, 16);
            atlas.AddRegion("BaseEnemy1", 0, 0, 16, 16);
            atlas.AddRegion("BaseEnemy2", 16, 0, 16, 16);
            atlas.AddRegion("AdvancedEnemy1", 0, 16, 16, 16);
            atlas.AddRegion("AdvancedEnemy2", 16, 16, 16, 16);
            //pass through all images and frames, and get within the individual class


            collisionManager = new CollisionManager(content);
            bulletManager = new BulletManager(content);
            enemyManager = new EnemyManager(content);
            player = new Player(content);
        }

        //maybe implement a statemaachine for game state? :P extra work to do if i hate reading!!
        public void Update(GameTime gameTime, Rectangle screenBounds)
        {
            // update everything else here as well?
            ScreenBounds = screenBounds;
            collisionManager.Update(gameTime);
            bulletManager.Update(gameTime);
            enemyManager.Update(gameTime);
            if (player.IsActive)
            {
                player.Update(gameTime);

            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spaceBackgroundTexture, ScreenBounds, Color.White);
            bulletManager.Draw(spriteBatch);
            enemyManager.Draw(spriteBatch);
            player.Draw(spriteBatch);
        }
    }
}
