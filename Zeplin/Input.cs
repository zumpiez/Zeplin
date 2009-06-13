//Zeplin Engine - Input.cs
//Jeff Hutchins 2009
//Some rights reserved http://creativecommons.org/licenses/by-sa/3.0/us/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Zeplin
{
    /// <summary>
    /// Wraps XNA's input methods to facilitate the detection of events in addition to states.
    /// </summary>
    public static class Input
    {
        static KeyboardState currentKeyState;
        static KeyboardState previousKeyState;
        
        internal static void UpdateInput()
        {
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();
        }

        /// <summary>
        /// Test if a key has been pressed since the last update.
        /// </summary>
        /// <param name="key">The key to test</param>
        /// <returns>True if the key was up during the previous update and down during the current update.</returns>
        public static bool WasKeyPressed(Keys key)
        {
            if (currentKeyState.IsKeyDown(key) && previousKeyState.IsKeyUp(key)) return true;
            else return false;
        }

        /// <summary>
        /// Test if a key has been released since the last update.
        /// </summary>
        /// <param name="key">The key to test</param>
        /// <returns>True if the key was down during the previous update and up during the current update.</returns>
        public static bool WasKeyReleased(Keys key)
        {
            if (currentKeyState.IsKeyUp(key) && previousKeyState.IsKeyDown(key)) return true;
            else return false;
        }

        /// <summary>
        /// Tests if a key is currently down.
        /// </summary>
        /// <param name="key">The key to test</param>
        /// <returns>True if key is down during the current update.</returns>
        public static bool IsKeyDown(Keys key)
        {
            return currentKeyState.IsKeyDown(key);
        }

        /// <summary>
        /// Tests if a key is currently up.
        /// </summary>
        /// <param name="key">The key to test</param>
        /// <returns>True if the key is up during the current update.</returns>
        public static bool IsKeyUp(Keys key)
        {
            return currentKeyState.IsKeyUp(key);
        }
    }
}
