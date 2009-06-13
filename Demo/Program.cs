using System;
using Zeplin;
using Microsoft.Xna.Framework;

namespace Demo
{
    static class Program
    {
        static void TestCollisionVolume()
        {
            //Two non-touching squares
            CollisionVolume cv = new CollisionVolume();
            CollisionVolume cv2 = new CollisionVolume();

            cv.AddVertex(Vector2.Zero);
            cv.AddVertex(new Vector2(0, 1));
            cv.AddVertex(Vector2.One);
            cv.AddVertex(new Vector2(1, 0));

            cv2.AddVertex(new Vector2(2, 0));
            cv2.AddVertex(new Vector2(2, 1));
            cv2.AddVertex(new Vector2(3, 1));
            cv2.AddVertex(new Vector2(3, 0));

            System.Diagnostics.Debug.Assert(cv.TestCollision(cv2) == false);

            //Two overlapping squares
            cv = new CollisionVolume();
            cv2 = new CollisionVolume();

            cv.AddVertex(Vector2.Zero);
            cv.AddVertex(new Vector2(0, 1));
            cv.AddVertex(Vector2.One);
            cv.AddVertex(new Vector2(1, 0));

            cv2.AddVertex(new Vector2(0.9f, 0));
            cv2.AddVertex(new Vector2(0.9f, 1));
            cv2.AddVertex(new Vector2(2, 1));
            cv2.AddVertex(new Vector2(2, 0));

            System.Diagnostics.Debug.Assert(cv.TestCollision(cv2) == true);

            //two non-overlapping triangles
            cv = new CollisionVolume();
            cv2 = new CollisionVolume();

            cv.AddVertex(0, 0);
            cv.AddVertex(0, 1);
            cv.AddVertex(1, 1);

            cv2.AddVertex(2, 0);
            cv2.AddVertex(3, 1);
            cv2.AddVertex(3, 0);

            System.Diagnostics.Debug.Assert(cv.TestCollision(cv2) == false);

            //two overlapping triangles
            cv = new CollisionVolume();
            cv2 = new CollisionVolume();

            cv.AddVertex(0, 0);
            cv.AddVertex(0, 1);
            cv.AddVertex(1, 1);

            cv2.AddVertex(0, 0);
            cv2.AddVertex(1, 1);
            cv2.AddVertex(1, 0);

            System.Diagnostics.Debug.Assert(cv.TestCollision(cv2) == true);

            //two overlapping triangles that are both defined counterclockwise
            cv = new CollisionVolume();
            cv2 = new CollisionVolume();

            cv.AddVertex(0, 0);
            cv.AddVertex(1, 1);
            cv.AddVertex(0, 1);

            cv2.AddVertex(0, 0);
            cv2.AddVertex(1, 0);
            cv2.AddVertex(1, -1);

            System.Diagnostics.Debug.Assert(cv.TestCollision(cv2) == true);

            //two overlapping triangles that are both defined in negative space counterclockwise
            cv = new CollisionVolume();
            cv2 = new CollisionVolume();

            cv.AddVertex(0, 0);
            cv.AddVertex(-1, -1);
            cv.AddVertex(0, -1);

            cv2.AddVertex(0, 0);
            cv2.AddVertex(-1, 0);
            cv2.AddVertex(-1, -1);

            System.Diagnostics.Debug.Assert(cv.TestCollision(cv2) == true);

            //two overlapping triangles that are both defined in negative space
            cv = new CollisionVolume();
            cv2 = new CollisionVolume();

            cv.AddVertex(0, 0);
            cv.AddVertex(0, 1);
            cv.AddVertex(-1, 1);

            cv2.AddVertex(0, 0);
            cv2.AddVertex(-1, 1);
            cv2.AddVertex(-1, 0);

            System.Diagnostics.Debug.Assert(cv.TestCollision(cv2) == true);

            //two non-overlapping triangles on opposite sides of the x axis
            cv = new CollisionVolume();
            cv2 = new CollisionVolume();
            cv.AddVertex(0, 0);
            cv.AddVertex(0, 1);
            cv.AddVertex(1, 1);

            cv2.AddVertex(0, -1);
            cv2.AddVertex(1, -1);
            cv2.AddVertex(1, -2);

            System.Diagnostics.Debug.Assert(cv.TestCollision(cv2) == false);
        }
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            //TestCollisionVolume();

            Demo game = new Demo();
        }
    }
}

