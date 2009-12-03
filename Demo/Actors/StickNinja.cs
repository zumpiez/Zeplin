using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeplin;
using Zeplin.CollisionShapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Demo.Actors
{
    public class StickNinja : Actor
    {
        public StickNinja(Vector2 position) : base(new Sprite(@"Images\stickninja"), new Transformation(position, new Vector2(0.75f, 0.75f), 0, new Vector2(80,95)), new SATCollisionVolume(Vector2.Zero, new Vector2(99,77)))
        {
            Transformation.Scale = new Vector2(0.75f);
            Transformation.Depth = 0;

            gravity = new Vector2(0, -0.1f);
            velocity = Vector2.Zero;

            OnUpdate += UpdateBehavior;


            (CollisionVolume as SATCollisionVolume).ShowCollisionBoundaries = true;
        }

        public void UpdateBehavior(GameTime time)
        {
            if (Input.IsKeyDown(Keys.Right))
            {
                Transformation.Position = new Vector2(Transformation.Position.X + 4, Transformation.Position.Y);
            }
            else if (Input.IsKeyDown(Keys.Left))
            {
                Transformation.Position = new Vector2(Transformation.Position.X - 4, Transformation.Position.Y);
            }

            if (Input.WasKeyPressed(Keys.Space))
            {
                //holdingDownSpace = true;
                velocity.Y = 5;
            }

            if (Input.IsKeyDown(Keys.Q))
            {
                Engine.Camera.Zoom += new Vector2(0.01f);
            }
            if (Input.IsKeyDown(Keys.E))
            {
                Engine.Camera.Zoom -= new Vector2(0.01f);
            }

            if (Input.IsKeyDown(Keys.R))
            {
                Engine.Camera.Rotation += 0.01f;
            }

            if (Engine.TestCollision<Tiles.GrassBrick>(this) != null)
            {
                if(velocity.Y < 0) velocity.Y = 0;
            }

            Transformation.Position += velocity;

            velocity += gravity;

            Engine.Camera.Center = Transformation.Position;
        }

        Vector2 velocity;
        Vector2 gravity;
    }
}
