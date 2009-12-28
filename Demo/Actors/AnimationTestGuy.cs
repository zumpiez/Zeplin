using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeplin;
using Microsoft.Xna.Framework;

namespace Demo.Actors
{
    class AnimationTestGuy : Actor
    {
        /// <summary>
        /// Mushroom sprite courtesy of A. Joas
        /// </summary>
        public AnimationTestGuy()
            : base(new Sprite("Images/mushroom"), new Transformation())
        {
            /*this.FrameSize = new Point(48, 48);
            AnimationScript = new AnimationScript(new int[] { 0, 1, 2, 3, 2, 1, 0, 4, 5, 6, 7, 8, 7, 6, 5, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 }, 2.5f);
            AnimationScript.Padding = 1;
            OnUpdate += UpdateBehavior;*/
            //Todo: update this so I can uncomment it.
        }

        public void UpdateBehavior(GameTime time)
        {
            if (AnimationScript.IsAnimationFinished)
                AnimationScript.PlayFromBeginning(time);
        }
    }
}
