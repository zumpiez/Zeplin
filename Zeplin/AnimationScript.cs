//Zeplin Engine - AnimationScript.cs
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
    /// Defines a timed animation sequence
    /// </summary>
    public class AnimationScript
    {
        /// <summary>
        /// Constructs an animation sequence
        /// </summary>
        /// <param name="frames">A collection of frames, in frame  </param>
        /// <param name="duration">The amount of time the animation will take to play to completion</param>
        public AnimationScript(IList<Point> frames, TimeSpan duration)
        {
            this.frames = frames;
            this.Duration = duration;
        }
        
        IList<Point> frames;
        TimeSpan beginTime;
        
        /// <summary>
        /// Sets the animation to start from the beginning during the next draw.
        /// </summary>
        /// <param name="time">The current time</param>
        public void PlayFromBeginning(GameTime time)
        {
            beginTime = time.TotalGameTime;
        }

        int animationIndex = 0;
        /// <summary>
        /// Gets or sets the animation's loop setting
        /// </summary>
        public bool Loop { get; set; }
        
        /// <summary>
        /// Gets whether the animation has reached the end of its playback.
        /// </summary>
        /// <remarks>This will always return false if the animation is looping at the time IsAnimationFinished is called.</remarks>
        public bool IsAnimationFinished
        {
            get
            {
                if (Loop == true)
                    return false;
                else if (animationIndex >= frames.Count)
                    return true;
                else 
                    return false;
            }
        }

        /// <summary>
        /// Gets or sets the amount of time that the animation will take to complete.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the number of pixels that are padding the animation frames.
        /// </summary>
        public int Padding { get; set; }

        /// <summary>
        /// Gets or sets the number of frames in a row (X) and the number of rows (Y).
        /// </summary>
        public Point FramesPerSide { get; set; }

        /// <summary>
        /// Computes a rectangle that contains the current animation frame in the passed sprite.
        /// </summary>
        /// <param name="time">The current GameTime</param>
        /// <param name="sprite">The sprite being drawn</param>
        /// <returns>A rectangle containing the correct frame</returns>
        /// <remarks>You can use this method on any sprite and it will generate a result, but it for correct results the Sprite should have a framesize defined, and the order of animation frames should match those of the AnimationScript.</remarks>
        internal Rectangle ProcessAnimation(GameTime time, Point framesize, Rectangle? subrect)
        {
            //Determine the frame number to use based on duration and current time
            Point frame;
            animationIndex = (int)((time.TotalGameTime - beginTime).TotalMilliseconds * frames.Count / Duration.Milliseconds);
            if (Loop)
            {
                animationIndex %= frames.Count;
                frame = frames[animationIndex];
            }
            else
            {
                if (animationIndex >= frames.Count)
                    frame = frames[frames.Count - 1]; //show last frame of animation until restarted.
                else
                    frame = frames[animationIndex];
            }

            //Locate the rectangle containing that frame number based on the sprite's frame size
            Rectangle sourceRect = new Rectangle(0, 0, framesize.X, framesize.Y);
            
            sourceRect.X = frame.X * framesize.X + Padding * frame.X + Padding;
            sourceRect.Y = frame.Y * framesize.Y + Padding * frame.Y + Padding;

            if (subrect.HasValue)
            {
                sourceRect.X += subrect.Value.X;
                sourceRect.Y += subrect.Value.Y;
            }

            return sourceRect;
        }

        internal Rectangle ProcessAnimation(GameTime time, Point framesize)
        {
            return ProcessAnimation(time, framesize, null);
        }
    }
}
