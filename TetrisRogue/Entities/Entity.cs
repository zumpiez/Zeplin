using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TetrisRogue.Entities
{
    public class Entity
    {
        CombatStyle combatStyle;
        CombatClass combatClass;
        Stats stats;
        Entity target; //gameboard tile coordinate

        bool stationary; //true if this entity can never move (Treasure, Door)

        public void Think()
        {
            throw new NotImplementedException();

            //find most interesting target

            //if not in range: get in range

            //if in range: act
        }

        Personality personality;
        int InfluenceThreshold;
    }

    public struct Stats
    {
        int Wizzin;
        int Wile;
        int Whale;
        int HP;
        int HPMax;
        int Level;

        //these are ignored for non-heroes
        int XP;
        int NextLevelXP;
    }

    public enum CombatStyle
    {
        Dirty,      //will target WeakEnemy type
        Neutral,    //will target Enemy type
        Noble       //will target StrongEnemy type
    }

    public enum CombatClass
    {
        Melee,      //requires adjacency to attack
        Ranged,     //requires line of sight to attack
        Magic       //can hit any target in the same room
    }

    public enum Personality
    {
        TreasureHunter, //will prioritize Treasure targets
        Brawler, //will prioritize Enemy targets
        Explorer, //will prioritize Door targets
        Inert, //will not take targets
        Monster //only targets heroes
    }

    public enum EntityClass
    {
        Enemy, //any enemy.
        WeakEnemy,
        StrongEnemy,
        Treasure,
        Door,
        //(the heroes)
        Fighter,
        Wizard,
        Ranger
    }
}
