using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace ShooterTest.Game
{

    public class CollisionManager
    {
        public static CollisionManager Instance { get; private set; }
        public List<ICollidable> CollidableObjects { get; private set; }
        //just have others add on init?
        float ascaleX;
        float ascaleY;
        float bscaleY;
        float bscaleX;
        public CollisionManager(ContentManager content)
        {
            Instance = this;
            CollidableObjects = new List<ICollidable>();
            //needs a list of everything with Icolliable & list needs to update on spawn of new objects? 
        }
        public void Update(GameTime gameTime)
        {
            //preform basic collision check, the pixel perfect if needed I think
            //i hate this i think i overcomplicated this hella
            //i miss unity :'(
            for (int i = 0; i < CollidableObjects.Count; i++)
            {
                if (!CollidableObjects[i].IsActive) { continue; }
                for (int j = i + 1; j < CollidableObjects.Count; j++)
                {
                    if (!CollidableObjects[j].IsActive) { continue; }
                    if (CollidableObjects[i].BoxCollider.Intersects(CollidableObjects[j].BoxCollider))
                    {
                        if (PixelPerfectCollision(CollidableObjects[i], CollidableObjects[j])) { 
                            CollidableObjects[i].OnCollision(CollidableObjects[j]);
                            CollidableObjects[j].OnCollision(CollidableObjects[i]);
                        }
                        //replace w piixel perfect if there time

                        if (!CollidableObjects[i].IsActive) { break; }
                        //prevent bullet taking out more than 1
                    }
                }
            }
        }
        public void AddCollidable(ICollidable col)
        {
            CollidableObjects.Add(col);
        }
        public void RemoveCollider(ICollidable col)
        {
            CollidableObjects.Remove(col);
        }
        //i think i could simplify by just pruning everything each call, but im worried about needlessly iterating? 
        //maybe implement a stack? specifically for bullets? idk TT

        public bool PixelPerfectCollision(ICollidable a, ICollidable b)
        {
            // i could also combine w basic check, if in border then also run a pixel perfect check? 
            //should take two gameobject or sprites and check the overlap
            //only take into account the pixels that are overlapping and have a positive alpha value?
            //return true if collision, false if not
            Rectangle rectoverlap = Rectangle.Intersect(a.BoxCollider, b.BoxCollider);
            if (rectoverlap.Width == 0 || rectoverlap.Height == 0) {
                return false;
            }
            ascaleX = a.BoxCollider.Width / (float)a.textureRegion.Width;
            ascaleY = a.BoxCollider.Height / (float)a.textureRegion.Height;
            bscaleX = b.BoxCollider.Width / (float)b.textureRegion.Width;
            bscaleY = b.BoxCollider.Height / (float)b.textureRegion.Height;


            for (int i = rectoverlap.X; i < rectoverlap.Right; i++) {
                for (int j = rectoverlap.Y; j < rectoverlap.Bottom; j++) { 
                    int xposA =  i - a.BoxCollider.Left ;
                    int xposB = i - b.BoxCollider.Left;
                    int yposA = j - a.BoxCollider.Top;
                    int yposB = j- b.BoxCollider.Top;
                    //pixel pos on enlarged texture

                    int maskAX = (int)MathF.Floor(xposA/ascaleX);
                    int maskAY = (int)MathF.Floor(yposA/ascaleY);
                    int maskBX = (int)MathF.Floor(xposB / bscaleX);
                    int maskBY = (int)MathF.Floor(yposB / bscaleY);
                    //convert to normal size position to check for alpha

                    bool Aalpha = a.textureRegion.alphaMask[a.textureRegion.Width * maskAY + maskAX];
                    bool Balpha = b.textureRegion.alphaMask[b.textureRegion.Width * maskBY + maskBX];
                    if (Aalpha && Balpha) {
                        return true;
                    }
                }
            }

            return false;
        }

    }
}
