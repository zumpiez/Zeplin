using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using Zeplin;

namespace TetrisRogue.Entities
{
    public class Entity
    {
        public Entity() { }

        [XmlElement("name")]
        public string name;

        [XmlElement("stats")]
        public Stats stats;

        [XmlElement("style")]
        public CombatStyle combatStyle;
        
        [XmlElement("class")]
        public CombatClass combatClass;

        [XmlAttribute("x")]
        public int X
        {
            set
            {
                Extent.X = value * 3;
            }
        }
        [XmlAttribute("y")]
        public int Y
        {
            set
            {
                Extent.Y = value * 3;
            }
        }

        [XmlAttribute("w")]
        public int W
        {
            set
            {
                Extent.Width = value;
            }
        }

        [XmlAttribute("h")]
        public int H
        {
            set
            {
                Extent.Height = value;
            }
        }

        Personality personality = Personality.Monster;
        int InfluenceThreshold;

        Entity target; //gameboard tile coordinate

        bool stationary; //true if this entity can never move (Treasure, Door)

        public void Think()
        {
            throw new NotImplementedException();

            //find most interesting target

            //if not in range: get in range

            //if in range: act
        }

        /// <summary>
        /// This is a subrect for sprite drawin'. Blown up sprites are 24x24
        /// </summary>
        public Rectangle Extent = new Rectangle(0, 0, 24, 24);
    }

    public struct Stats
    {
        [XmlElement("wizzin")]
        public int Wizzin;

        [XmlElement("wile")]
        public int Wile;

        [XmlElement("whale")]
        public int Whale;

        [XmlElement("hpmax")]
        public int HPMax;
        int HP;

        [XmlElement("level")]
        public int Level;
        
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
