using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zeplin 
{
    public class MetaFont
    {
        public MetaFont(String faceName, String searchDir) 
        {
            if (String.IsNullOrEmpty(searchDir)) { throw new ArgumentException("Bad search directory"); }
            if (String.IsNullOrEmpty(faceName)) { throw new ArgumentException("Bad font face name"); }

            _baseFonts = new Dictionary<int, SpriteFont>();

            // find some mothafuckin' fonts
            String[] fontCandidates = Directory.GetFiles(searchDir, faceName + "*.xnb");
            Regex r = new Regex(String.Format(@".*\\({0} ([0-9]+)).xnb", faceName));

            foreach (String candidate in fontCandidates) 
            {
                Match match = r.Match(candidate);

                if (match.Groups.Count < 3)
                {
                    //Console.WriteLine("candidate turned out not to match regex; so sad :(");
                    continue;
                }

                Console.WriteLine("found base font(\"{0}\", \"{1}\") for metafont {2} at {3}, loading...", match.Groups[1], match.Groups[2], faceName, candidate);

                int sz = Int32.Parse(match.Groups[2].Value);

                _baseFonts.Add(sz, ZeplinGame.ContentManager.Load<SpriteFont>(match.Groups[1].Value));
            }
        }

        public SpriteFont GetBestFont(float size, out float scale)
        {
            KeyValuePair<int, SpriteFont>? smallestBiggerFont = null, biggestSmallerFont = null;

            foreach (KeyValuePair<int, SpriteFont> kvp in _baseFonts) 
            {
                if (kvp.Key > size && (!smallestBiggerFont.HasValue || kvp.Key < smallestBiggerFont.Value.Key)) 
                {
                    smallestBiggerFont = kvp;
                } 
                else if (kvp.Key < size && (!biggestSmallerFont.HasValue || kvp.Key > biggestSmallerFont.Value.Key)) 
                {
                    biggestSmallerFont = kvp;
                } 
                else if (kvp.Key == size) 
                {
                    scale = 1.0f;
                    //Console.WriteLine("MetaFont returning {0} as best font for size {1} (ex)",
                    //    kvp.Key, size);
                    return kvp.Value;
                }
            }

            if (smallestBiggerFont.HasValue) 
            {
                scale = size / smallestBiggerFont.Value.Key;
                //Console.WriteLine("MetaFont returning {0} @ {1}x as best font for size {2} (sb)",
                //    smallestBiggerFont.Value.Key, scale, size);
                return smallestBiggerFont.Value.Value;
            } 
            else 
            {
                scale = size / biggestSmallerFont.Value.Key;
                //Console.WriteLine("MetaFont returning {0} @ {1}x as best font for size {2} (bs)",
                //    biggestSmallerFont.Value.Key, scale, size);
                return biggestSmallerFont.Value.Value;
            }
        }

        private Dictionary<int, SpriteFont> _baseFonts;
    }
}
