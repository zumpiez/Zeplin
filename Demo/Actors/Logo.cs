using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeplin;
using Microsoft.Xna.Framework;

namespace Demo.Actors
{
    public class Logo : Actor
    {
        private static Sprite ball = new Sprite(@"Images\ball");

        public float offset = 0;
        public float rotationSpeed = 1;

        public Logo(Vector2 screenCenter) : base(ball, new Transformation())
        {
            Transformation.Position = screenCenter;
            ball.SetLucency(0.25f);
            Transformation.Pivot = ball.GetCenter();
        }

        public override void UpdateBehavior(GameTime time)
        {
            Transformation.Rotation = offset + (float)time.TotalGameTime.TotalMilliseconds / (2000f / rotationSpeed);
        }
    }
}
