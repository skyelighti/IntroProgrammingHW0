using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace ShooterTest.Game
{
    public class Bullet : GameObject, ICollidable
    {
        public float speed = 200f;
        float dir = 1;
        float lifetime {get;} = 10f;
        public float timer = 0f;
        //object pooling attempt..... kill me!!!!
        public GameObject owner { get; private set; }
        public Rectangle BoxCollider
        {
            get
            {
                return new Rectangle((int)location.X, (int)location.Y, (int)size.X, (int)size.Y);
            }
            //returns new rect everytime its called, so its always accurate
        }
        public Bullet(ContentManager content, string spriteName, string name) : base(content, spriteName, name)
        {
            //need ensure collision manager exists before this is initialized
            //is the bullet the straight line i got no clue lmfao
            owner = this;
            //start w owning itself, will be changed 
        }
        public override void Update(GameTime gameTime)
        {
            if (!IsActive) { return;}
            if (!Main.ScreenBounds.Intersects(BoxCollider))
            {
                Debug.WriteLine("hitwall, deactivating.");
                Deactivate();
                return;
            }
            if (timer < lifetime)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                Debug.WriteLine("lifetime has been reached, deactivating.");
                Deactivate();
                return;
                //return to da pool
            }
            this.UpdateLocation(location.X, location.Y + (dir * speed * (float)gameTime.ElapsedGameTime.TotalSeconds));
        }
        public void SetOwner(GameObject owner)
        {
            this.owner = owner;
            if (owner is Player)
            {
                dir = -1;
            }
            else 
            {
                 dir = 1;
                    //makes positive so bullet goes down the screen
            }
        }
        public void OnCollision(ICollidable other) {
            //check the base class of other object and do handling based upn class?
            //dont return to pool if owned by enemy and hits another enemy like the game
            //return to da pool
            if (other is Player p) {
                if (!p.spriteVisible) { return; }
            }
            if ((owner is Player && other is Player) || (owner is BaseEnemy && other is BaseEnemy) || other is Bullet)
            {
                return;
            }
            Debug.WriteLine("bullet has collided, deactivating.");
            Deactivate();

        }


    }
}
