using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameLibrary.Graphics;


namespace ShooterTest.Game
{
    public interface ICollidable
    {
        bool IsActive { get; }
        //pooling.... maybe new interface? but too fragmented
        Rectangle BoxCollider { get; }
        Vector2 location { get; }
        TextureRegion textureRegion { get; }

        void OnCollision(ICollidable other);
        //each object can manage it's reaction to collisions
    }
}
