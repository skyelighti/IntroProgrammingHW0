using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Diagnostics;

namespace ShooterTest.Game
{
    public class AdvancedEnemy: BaseEnemy
    {
        float timer = 0f;
        float cooldown = 5f;

        public AdvancedEnemy(ContentManager content, string spriteName, string name) : base(content, spriteName, name)
        {
            UpdateLocation(0, 0);
            frames.Clear();
            frames.Add("AdvancedEnemy1");
            frames.Add("AdvancedEnemy2");
        }
        public override void Update(GameTime gameTime) {
            if (!IsActive) { return; }
            base.Update(gameTime);
            if (timer < cooldown) {
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            Shoot();
        }
        public void Shoot() {
            if (!IsActive || cooldown > timer) { return; }
            timer = 0f;
            BulletManager.Instance.FireBullet(this);
        }
    }
}
