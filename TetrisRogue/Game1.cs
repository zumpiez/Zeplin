using System;
using System.Collections.Generic;
using System.Linq;
using Zeplin;

namespace TetrisRogue
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        ZeplinGame game;

        public Game1()
        {
            game = new ZeplinGame();
            game.OnLoad += Load;
            game.OnUpdate += Update;
        }

        void Load()
        {
        }

        void Update()
        {
        }
    }
}
