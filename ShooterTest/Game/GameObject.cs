using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GameObject
{
    public Vector2 location { get; private set; }
    //gameobject location in world, readonly
    public Vector2 size { get; private set; } = new Vector2(64, 64);
    //gameobject size in pixels? may be changed

    public bool IsActive { get; private set; } = true;
    //readonly mainly for pooling
    private string spriteName;
    //getting sprite file/loading?
    private string name;
    // identifier 
    public TextureRegion textureRegion { get; private set; }

    public bool spriteVisible = true;
    float invisibilityTimer = 0f;
    public GameObject(ContentManager content, string spriteName, string name)
    {
        this.spriteName = spriteName;
        this.name = name;
        textureRegion = ShooterTest.Game.Main.atlas.GetRegion(spriteName);
    }


    public virtual void Update(GameTime gameTime)
    {
        //whereever handles updates should check if each gameobject is active before updating them
        //collision handler
        if (!spriteVisible) {
            invisibilityTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (invisibilityTimer <= 0) {
                spriteVisible = true;
            }
        }
    }
    public void UpdateName(string newName)
    {
        //updates gameobject name
        name = newName;
    }
    public void UpdateSprite(string newSpriteName)
    {
        //updates gameobject sprite
        spriteName = newSpriteName;
        textureRegion = ShooterTest.Game.Main.atlas.GetRegion(spriteName);
    }
    public void UpdateLocationX(float x)
    {
        //updates gameobject location x direction
        location = new Vector2(x, location.Y);
        //reduces update calls? 
    }
    public void UpdateLocationY(float y)
    {
        //updates gameobject location y direction
        location = new Vector2(location.X, y);
    }
    public void UpdateLocation(float x, float y)
    {
        //updates gameobject location x and y direction
        location = new Vector2(x, y);
    }
    public void UpdateSize(int x, int y) {
        //updates gameobject pixel size
        size = new Vector2(x, y);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (IsActive && spriteVisible) {
            spriteBatch.Draw(textureRegion.Texture, new Rectangle((int)location.X, (int)location.Y, (int)size.X, (int)size.Y), textureRegion.SourceRectangle, Color.White);
        }
        // first rect draws location and size, second rectangle is spritesheet location and size, color is tinting

        //should the object that inherits from this handle draw? or should the base handle?
    }

    public void Activate() { 
        IsActive = true;
    }
    public void Deactivate()
    {
        IsActive = false;
    }
    public void HideFor(float seconds) {
        invisibilityTimer = seconds;
        spriteVisible = false;
    }
}
