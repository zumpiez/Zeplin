using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;

namespace TetrisRogue.Entities
{
    public class Entity
    {
        [XmlElement("name")]
        public string name;

        [XmlElement("stats")]
        public Stats stats;

        [XmlElement("style")]
        public CombatStyle combatStyle;
        
        [XmlElement("class")]
        public CombatClass combatClass;

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

        Rectangle extent;
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
