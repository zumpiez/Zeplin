//Zeplin Engine - CollisionVolume.cs
//Jeff Hutchins 2009
//Some rights reserved http://creativecommons.org/licenses/by-sa/3.0/us/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zeplin.Utilities;

namespace Zeplin.CollisionShapes
{
    /// <summary>
    /// Defines a collidable boundary that can test intersection with all other instances of the same.
    /// </summary>
    public class SATCollisionVolume : ICollisionVolume
    {
        /// <summary>
        /// Constructs an empty collision colume
        /// </summary>
        public SATCollisionVolume()
        {
            vertices = new List<Vector2>();
            cachedWorldCoordinateVertices = new List<Vector2>();
            testAxes = new List<Vector2>();
        }

        /// <summary>
        /// Gets or sets a flag that enables drawing the collision volume on top of the sprite.
        /// </summary>
        public bool ShowCollisionBoundaries
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a rectangular collisionvolume. TODO: Make this based on top-left because that's how the vertices are defined?
        /// </summary>
        /// <param name="origin">The bottom left corner of the collision volume</param>
        /// <param name="dimensions">The width and height of the collision volume</param>
        /// <remarks>The origin point assumes the lower left corner of the sprite is 0,0. This keeps confusing me when I use it to make demos, so I am considering changing it.</remarks>
        public SATCollisionVolume(Vector2 origin, Vector2 dimensions) : this()
        {
            Vector2 topRight = origin + dimensions;
            Vector2 topLeft = new Vector2(origin.X, topRight.Y);
            Vector2 bottomRight = new Vector2(topRight.X, origin.Y);

            AddVertex(origin);
            AddVertex(topLeft);
            AddVertex(topRight);
            AddVertex(bottomRight);
        }

        /// <summary>
        /// Tests if two collision volumes are overlapping
        /// </summary>
        /// <param name="otherCV">The collision volume to test against</param>
        /// <returns>True if there is an overlap.</returns>
        public bool TestCollision(ICollisionVolume other)
        {
            if (other == this) return false;
            if (TestCollisionCompatibility(other) == false) return false;
            SATCollisionVolume otherCV = (SATCollisionVolume)other;
            

            float thisMin, thisMax, otherMin, otherMax;
            float location; //projected location of the vertex along the axis

            List<Vector2> allAxes = new List<Vector2>();

            //initialize collection of all axes to be tested
            foreach (Vector2 axis in TestAxes)
                allAxes.Add(axis);
            foreach (Vector2 axis in otherCV.TestAxes)
                allAxes.Add(axis);

            foreach (Vector2 axis in allAxes)
            {
                //reset the min and max for this axis test
                thisMin = otherMin = float.PositiveInfinity;
                thisMax = otherMax = float.NegativeInfinity;

                //find this object's min and max when projected against this axis
                foreach (Vector2 vertex in cachedWorldCoordinateVertices)
                {
                    location = Vector2.Dot(vertex, axis);
                    if (location < thisMin) thisMin = location;
                    if (location > thisMax) thisMax = location;
                }

                //find the other object's min and max when projected against this axis
                foreach (Vector2 vertex in otherCV.cachedWorldCoordinateVertices)
                {
                    location = Vector2.Dot(vertex, axis);
                    if (location < otherMin) otherMin = location;
                    if (location > otherMax) otherMax = location;
                }
                
                //determine if the segments overlap
                if (thisMax < otherMin || thisMin > otherMax)
                    return false;
            }
            
            //if the loop has completed and the projected segments overlap on all axes then the objects are touching.
            return true;
        }

        /// <summary>
        /// Gets the centroid of the defined vertices
        /// </summary>
        /// <returns>The centroid point in coordinate space</returns>
        private Vector2 GetCenter()
        {
            Vector2 centroid = new Vector2();
            foreach (Vector2 vertex in vertices)
            {
                centroid += vertex;
            }
            centroid /= vertices.Count();

            return centroid;
        }

        /// <summary>
        /// Uses the defined vertices to generate the axis tests that will be performed by the separating axis theorem algorithm
        /// </summary>
        private void GenerateTestAxes()
        {
            int nextVertex;
            testAxes.Clear();
            Vector2 edge;
            Vector2 normal;

            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices.Count < 2)
                    break;

                nextVertex = i + 1;
                try
                {
                    //Gets the edge between two vertices
                    edge = vertices[nextVertex] - vertices[i];
                }
                catch(ArgumentOutOfRangeException)
                {
                    //This gets the edge between the nth vertex and the 0th vertex.
                    nextVertex = 0;
                    edge = vertices[nextVertex] - vertices[i];
                }

                if (edge != Vector2.Zero)
                {
                    //SAT tests against the edge normals, so generate those and cache them.
                    normal = FindNormalVector(edge);
                    normal.Normalize();
                    testAxes.Add(normal);
                }
            }
        }

        /// <summary>
        /// Gets a RectangleF that contains the collision vertices
        /// </summary>
        public RectangleF BoundingRectangle
        {
            get
            {
                Vector2 min = new Vector2(float.PositiveInfinity);
                Vector2 max = new Vector2(float.NegativeInfinity);
                
                foreach(Vector2 vertex in cachedWorldCoordinateVertices)
                {
                    if (vertex.X < min.X) min.X = vertex.X;
                    if (vertex.Y < min.Y) min.Y = vertex.Y;
                    if (vertex.X > max.X) max.X = vertex.X;
                    if (vertex.Y > max.Y) max.Y = vertex.Y;
                }

                Vector2 dimensions = max - min;

                return new RectangleF(min, dimensions);
            }
        }

        /// <summary>
        /// Translates the vertices into world coordinate space from texture coordinate space
        /// </summary>
        /// <param name="t">The transformation of the object this collision volume belongs to</param>
        Transformation lastTransformation;
        internal void TransformCollisionVolume(Transformation t)
        {
            if (lastTransformation == t) return; //avoid re-doing this work

            List<Vector2> outputVertices = new List<Vector2>();

            foreach (Vector2 vertex in vertices)
            {
                Vector2 transformed = World.ObjectToWorld(t, vertex, HorizontalFlip);
                outputVertices.Add(transformed);
            }

            cachedWorldCoordinateVertices = outputVertices;
            lastTransformation = t;
        }

        /// <summary>
        /// Gets or sets a flag that flips the collision volume horizontally.
        /// </summary>
        /// <remarks>This was added to facilitate horizontally flipping sprites (like left and right walking cycles) in the demo game. There is probably a better solution, so this might be deprecated in the next version of Zeplin.</remarks>
        public bool HorizontalFlip = false;

        /// <summary>
        /// Rotates a point in texture coordinate space around 0,0 by a specified angle.
        /// </summary>
        /// <param name="point">A point in texture coordinate space.</param>
        /// <param name="radians">A radian angle</param>
        /// <returns>The location of the point after rotation</returns>
        /// <remarks>This will probably be moved to Zeplin.Utilities in the future</remarks>
        private Vector2 RotatePoint(Vector2 point, float radians)
        {
            Vector2 rotated = new Vector2();
            rotated = Vector2.Transform(point, Matrix.CreateRotationZ(radians));
            return rotated;
        }

        /// <summary>
        /// Rotates a point in texture coordinate space around the specified origin point by a specified angle.
        /// </summary>
        /// <param name="point">A point in texture coordinate space.</param>
        /// <param name="radians">A radian angle</param>
        /// <param name="origin">The point that will serve as the origin for the rotation</param>
        /// <returns>The location of the point after rotation</returns>
        /// <remarks>This will probably be moved to Zeplin.Utilities in the future</remarks>
        private Vector2 RotatePoint(Vector2 point, float radians, Vector2 origin)
        {
            Vector2 translated = point - origin;
            translated = RotatePoint(translated, radians);
            translated += origin;
            return translated;
        }

        /// <summary>Finds the normal vector of an edge</summary>
        /// <param name="edge"></param>
        /// <remarks>This will probably be moved to Zeplin.Utilities in the future</remarks>
        private Vector2 FindNormalVector(Vector2 edge)
        {
            return RotatePoint(edge, (float)(3 * Math.PI / 2));
        }

        /// <summary>
        /// Adds a vertex to the CollisionVolume then re-generates the test axis vectors.
        /// Make sure to add vertices in a consistently clockwise or counterclockwise order or collision detection will act weird.
        /// </summary>
        /// <param name="vertex">The vertex that will be added.</param>
        public void AddVertex(Vector2 vertex)
        {
            vertices.Add(vertex);
            GenerateTestAxes();
        }

        /// <summary>
        /// Adds a vertex to the CollisionVolume then re-generates the test axis vectors.
        /// Make sure to add vertices in a consistently clockwise or counterclockwise order or collision detection will act weird.
        /// </summary>
        /// <param name="x">The x value of the vertex</param>
        /// <param name="y">The y value of the vertex</param>
        public void AddVertex(float x, float y)
        {
            vertices.Add(new Vector2(x, y));
            GenerateTestAxes();
        }

        /// <summary>
        /// Removes a vertex from the CollisionVolume and then re-generates the test axis vectors.
        /// </summary>
        /// <param name="vertex">The vertex to be removed.</param>
        public void RemoveVertex(Vector2 vertex)
        {
            vertices.Remove(vertex);
            GenerateTestAxes();
        }

        /// <summary>
        /// Removes a vertex from the CollisionVolume and then re-generates the test axis vectors.
        /// </summary>
        /// <param name="x">The x value of the vertex</param>
        /// <param name="y">The y value of the vertex</param>
        public void RemoveVertex(float x, float y)
        {
            vertices.Remove(new Vector2(x, y));
            GenerateTestAxes(); 
        }

        /// <summary>
        /// Draws the collision volume in the world
        /// </summary>
        public void Draw()
        {
            if (!ShowCollisionBoundaries) return;

            int nextVertex;

            for (int i = 0; i < cachedWorldCoordinateVertices.Count; i++)
            {
                if (cachedWorldCoordinateVertices.Count < 2)
                    break;

                nextVertex = i + 1;
                try
                {
                    Engine.DrawLine(cachedWorldCoordinateVertices[i], cachedWorldCoordinateVertices[nextVertex], 2, Color.Chartreuse);
                }
                catch 
                {
                    nextVertex = 0;
                    Engine.DrawLine(cachedWorldCoordinateVertices[i], cachedWorldCoordinateVertices[nextVertex], 2, Color.Chartreuse);
                }
            }
        }

        List<Vector2> vertices;
        List<Vector2> cachedWorldCoordinateVertices;
        List<Vector2> testAxes;
        /// <summary>
        /// Gets the set of test axes generated from the vertex points.
        /// </summary>
        internal List<Vector2> TestAxes
        {
            get
            {
                return testAxes;
            }
        }

        #region ICollisionVolume Members

        public bool TestCollisionCompatibility(ICollisionVolume other)
        {
            if (other is SATCollisionVolume) return true;
            else return false;
        }

        #endregion
    }
}
