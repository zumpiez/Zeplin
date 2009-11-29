//Zeplin Engine - Camera.cs
//Jeff Hutchins 2009
//Some rights reserved http://creativecommons.org/licenses/by-sa/3.0/us/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Zeplin
{
    /// <summary>
    /// Defines a viewable area in the world's space.
    /// </summary>
    public class Camera
    {
        #region Constructors
        /// <summary>
        /// Initializes a camera with given dimensions, centered at the given point.
        /// </summary>
        /// <param name="center">The point in the world on which the camera will be centered</param>
        /// <param name="dimensions">The width and height of the camera</param>
        public Camera(Vector2 center, Vector2 dimensions)
        {
            this.dimensions = dimensions;
            this.center = center;
        }

        /// <summary>
        /// Initializes a camera with given dimensions
        /// </summary>
        /// <param name="dimensions">The width and height of the camera</param>
        public Camera(Vector2 dimensions)
        {
            this.dimensions = dimensions;
            this.center = Vector2.Zero;
        }

        /// <summary>
        /// Initializes a camera centered at 0,0 with a width of 0 and a height of 0.
        /// </summary>
        public Camera()
        {
            this.dimensions = Vector2.Zero;
            this.center = Vector2.Zero;
        }
        #endregion

        /// <summary>
        /// Gets a computed matrix that is used by XNA's draw command to transform everything as it's drawn, creating the effect of a camera.
        /// </summary>
        /// <param name="parallax">The parallax factor of the layer being drawn.</param>
        /// <returns>A view matrix</returns>
        internal Matrix ComputeViewMatrix(Vector2 parallax)
        {
            //maybe switch this out to use lazy initialization if it's too slow
            Vector3 matrixRotationOrigin = new Vector3(this.center.X, -this.center.Y, 0);
            Vector3 screenPosition = new Vector3(World.gameResolution.X / 2, World.gameResolution.Y / 2, 0);

            //calculate scaling based on crop mode
            float scaleFactor = 1;
            switch (mode)
            {
                case CameraCropMode.MaintainWidth:
                    scaleFactor = World.gameResolution.X / this.dimensions.X;
                    break;

                case CameraCropMode.MaintainHeight:
                    scaleFactor = World.gameResolution.Y / this.dimensions.Y;
                    break;

                case CameraCropMode.NoCorrection:
                    scaleFactor = 1;
                    break;
            }

            Matrix translateToCameraCenter = Matrix.CreateTranslation(-matrixRotationOrigin * new Vector3(parallax, 0));
            Matrix scale = Matrix.CreateScale(new Vector3(zoom * scaleFactor, 1.0f));
            Matrix rotation = Matrix.CreateRotationZ(this.rotation);
            Matrix translateToScreenPosition = Matrix.CreateTranslation(screenPosition);

            //Composite these matrices
            return translateToCameraCenter * rotation * scale * translateToScreenPosition;
        }

        CameraCropMode mode;
        /// <summary>
        /// Gets or sets the cropping mode for the camera.
        /// </summary>
        public CameraCropMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        Vector2 center;
        /// <summary>
        /// Gets or sets the location of the camera in the world.
        /// </summary>
        public Vector2 Center
        {
            get { return center; }
            set { center = value; }
        }

        Vector2 dimensions;
        /// <summary>
        /// Sets the dimensions of the camera
        /// </summary>
        /// <param name="width">The width of the camera</param>
        /// <param name="height">The height of the camera</param>
        public void SetDimensions(float width, float height)
        {
            this.dimensions = new Vector2(width, height);
        }

        Vector2 zoom = new Vector2(1, 1);
        /// <summary>
        /// Gets or sets the zoom amount in both dimensions.
        /// </summary>
        public Vector2 Zoom
        {
            get { return zoom; }
            set { zoom = value; }
        }

        float rotation;
        /// <summary>
        /// Gets or sets the rotation of the camera in radians.
        /// </summary>
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
    }

    /// <summary>
    /// Sets the camera's scaling behavior if the camera's size does not match the aspect ratio of the game's resolution
    /// </summary>
    public enum CameraCropMode
    {
        /// <summary>
        /// This mode performs no correction. The camera will be centered on the screen, and the horizontal and vertical space of the scene will be cropped or extended depending on the window dimensions.
        /// </summary>
        NoCorrection,

        /// <summary>
        /// Assures that the exact vertical space of the scene will be shown, extending or cropping the horizontal space depending on the window dimensions.
        /// </summary>
        MaintainHeight,

        /// <summary>
        /// Assures that the exact horizontal space of the scene will be shown, extending or cropping the vertical space depending on the window dimensions.
        /// </summary>
        MaintainWidth
    }
}