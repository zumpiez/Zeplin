using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeplin;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Demo.Actors
{
    public class StickNinja : Actor
    {
        public StickNinja(Vector2 position) : base(new Sprite(@"Images\stickninja"), new Transformation(position, new Vector2(0.75f, 0.75f), 0, new Vector2(80,95)), new SATCollisionVolume(Vector2.Zero, new Vector2(99,77)))
        {
            Scale = new Vector2(0.75f);

            //this.Sprite.color = Color.Chocolate;

            gravity = new Vector2(0, -0.1f);
            velocity = Vector2.Zero;

            CollisionVolume.ShowCollisionBoundaries = true;
            //lastState = new KeyboardState();
            //kb = Keyboard.GetState();
        }

        public override void UpdateBehavior(GameTime time)
        {
            if (Input.IsKeyDown(Keys.Right))
            {
                Translation = new Vector2(Translation.X + 4, Translation.Y);
            }
            else if (Input.IsKeyDown(Keys.Left))
            {
                Translation = new Vector2(Translation.X - 4, Translation.Y);
            }

            if (Input.WasKeyPressed(Keys.Space))
            {
                //holdingDownSpace = true;
                velocity.Y = 5;
            }

            if (Input.IsKeyDown(Keys.Q))
            {
                Engine.camera.Zoom += new Vector2(0.01f);
            }
            if (Input.IsKeyDown(Keys.E))
            {
                Engine.camera.Zoom -= new Vector2(0.01f);
            }

            if (Input.IsKeyDown(Keys.R))
            {
                Engine.camera.Rotation += 0.01f;
            }

            if (Engine.TestCollision<Tile>(this) != null)
            {
                if(velocity.Y < 0) velocity.Y = 0;
            }

            Translation += velocity;

            velocity += gravity;

            Engine.camera.Center = Translation;
        }

        Vector2 velocity;
        Vector2 gravity;
    }
}
