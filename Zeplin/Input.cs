//Zeplin Engine - Input.cs
//Jeff Hutchins 2009
//Some rights reserved http://creativecommons.org/licenses/by-sa/3.0/us/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Zeplin
{
    public enum MouseButtons
    {
        LeftButton, MiddleButton, RightButton, XButton1, XButton2
    }

    /// <summary>
    /// Wraps XNA's input methods to facilitate the detection of events in addition to states.
    /// </summary>
    public static class Input
    {
        static KeyboardState currentKeyState;
        static KeyboardState previousKeyState;

        static MouseState currentMouseState;
        static MouseState previousMouseState;
        
        internal static void UpdateInput()
        {
            previousKeyState = currentKeyState;
            previousMouseState = currentMouseState;
            currentKeyState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
        }

        public static Vector2 MousePosition
        {
            get
            {
                return new Vector2(currentMouseState.X, -currentMouseState.Y);
            }
            set
            {
                Mouse.SetPosition((int)value.X, (int)value.Y);
            }
        }

        /// <summary>
        /// Test if a button has been clicked since the last update.
        /// </summary>
        /// <param name="button">The button to test</param>
        /// <returns>True if the button is down on this update and was up on the previous update.</returns>
        public static bool WasMouseButtonClicked(MouseButtons button)
        {
            ButtonState currentButtonState = GetMouseButtonState(button, currentMouseState);
            ButtonState previousButtonState = GetMouseButtonState(button, previousMouseState);

            return (currentButtonState == ButtonState.Pressed && previousButtonState == ButtonState.Released);
        }

        /// <summary>
        /// Test if a button has been released since the last update.
        /// </summary>
        /// <param name="button">The button to test</param>
        /// <returns>True if the button is up on this update and was down on the previous update.</returns>
        public static bool WasMouseButtonReleased(MouseButtons button)
        {
            ButtonState currentButtonState = GetMouseButtonState(button, currentMouseState);
            ButtonState previousButtonState = GetMouseButtonState(button, previousMouseState);

            return (currentButtonState == ButtonState.Released && previousButtonState == ButtonState.Pressed);
        }

        /// <summary>
        /// Tests if a mouse button is currently down.
        /// </summary>
        /// <param name="button">The button to test</param>
        /// <returns>True if button is down during the current update.</returns>
        public static bool IsMouseButtonDown(MouseButtons button)
        {
            ButtonState currentButtonState = GetMouseButtonState(button, currentMouseState);
            return (currentButtonState == ButtonState.Pressed);
        }

        /// <summary>
        /// Tests if a mouse button is currently up.
        /// </summary>
        /// <param name="button">The button to test</param>
        /// <returns>True if button is up during the current update.</returns>
        public static bool IsMouseButtonUp(MouseButtons button)
        {
            return !IsMouseButtonDown(button);
        }

        static ButtonState GetMouseButtonState(MouseButtons button, MouseState state)
        {
            switch (button)
            {
                case MouseButtons.LeftButton:
                    return state.LeftButton;
                case MouseButtons.MiddleButton:
                    return state.MiddleButton;
                case MouseButtons.RightButton:
                    return state.RightButton;
                case MouseButtons.XButton1:
                    return state.XButton1;
                case MouseButtons.XButton2:
                    return state.XButton2;
                default:
                    return new ButtonState();
            }
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
