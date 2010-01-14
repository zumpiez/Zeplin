using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Zeplin.Utilities
{
    public struct HSVColor
    {
        public HSVColor(float h, float s, float v, float a)
        {
            _h = h;
            _s = s;
            _v = v;
            _a = a;
        }

        public HSVColor(HSVColor hsv, float a)
        {
            _h = hsv.H;
            _s = hsv.S;
            _v = hsv.V;
            _a = a;
        }

        public HSVColor(Color c)
        {
            ToHSVColor(c, out this);
        }

        public Color XNAColor
        {
            get
            {
                return ToRGBColor(this);
            }
            set
            {
                ToHSVColor(value, out this);
            }
        }

        /// <summary>
        /// Gets or sets H in terms of degrees
        /// </summary>
        public float HDegrees
        {
            get
            {
                return H * 360f;
            }

            set
            {
                float wrapped = value;
                
                while (wrapped > 360f)
                    wrapped -= 360f;

                H = wrapped / 360f;
            }
        }

        /// <summary>
        /// Gets or sets H in terms of radians
        /// </summary>
        public float HRadians
        {
            get
            {
                return H * 2 * (float)Math.PI;
            }
            set
            {
                float wrapped = value;
                
                while (wrapped > 2 * (float)Math.PI)
                    wrapped -= 2 * (float)Math.PI;

                H = wrapped / (2 * (float)Math.PI);
            }
        }

        /// <summary>
        /// Gets or sets the color's hue, between 0 and 1.0
        /// </summary>
        public float H
        {
            get
            {
                return _h;
            }
            set
            {
                _h = ClampValue(value);
            }
        }

        /// <summary>
        /// Gets or sets the color's saturation, between 0 and 1.0
        /// </summary>
        public float S
        {
            get 
            {
                return _s;
            }
            set 
            {
                _s = ClampValue(value);
            }
        }

        /// <summary>
        /// Gets or sets the color's value, between 0 and 1.0
        /// </summary>
        public float V
        {
            get
            {
                return _v;
            }
            set
            {
                _v = ClampValue(value);
            }
        }

        /// <summary>
        /// Gets or sets the color's alpha, between 0 and 1.0
        /// </summary>
        public float A 
        {
            get
            {
                return _a;
            }
            set
            {
                _a = ClampValue(value);
            }
        }

        private float _h, _s, _v, _a;


        /// <summary>
        /// Gets an HSVColor representing the color White.
        /// </summary>
        public static HSVColor White
        {
            get
            {
                return new HSVColor(0, 0, 1, 1);
            }
        }
        public static HSVColor One
        {
            get
            {
                return new HSVColor(1, 1, 1, 1);
            }
        }

        public static Color ToRGBColor(HSVColor hsv)
        {
            //Algo explained here:
            //http://en.wikipedia.org/wiki/HSL_and_HSV#Conversion_from_RGB_to_HSL_overview
            float H = hsv.H * 360;
            int mode = (int)Math.Floor(H / 60) % 6;
            
            float f = (H / 60) - (float)Math.Floor(H / 60);
            float p = hsv.V * (1 - hsv.S);
            float q = hsv.V * (1 - f * hsv.S);
            float t = hsv.V * (1 - (1 - f) * hsv.S);

            switch (mode)
            {
                case 0:
                    return new Color(hsv.V, t, p, hsv.A);
                case 1:
                    return new Color(q, hsv.V, p, hsv.A);
                case 2:
                    return new Color(p, hsv.V, t, hsv.A);
                case 3:
                    return new Color(p, q, hsv.V, hsv.A);
                case 4:
                    return new Color(t, p, hsv.V, hsv.A);
                case 5:
                    return new Color(hsv.V, p, q, hsv.A);
                default:
                    return Color.White;
            }
        }

        public static implicit operator Color(HSVColor hsv)
        {
            return ToRGBColor(hsv);
        }

        public static implicit operator HSVColor(Color rgb)
        {
            return ToHSVColor(rgb);
        }
        
        public static void ToHSVColor(Color rgb, out HSVColor hsv)
        {
            float H, S, V;
            float R, G, B, A;

            //convert to 0..1 values
            R = rgb.R / 256f;
            G = rgb.G / 256f;
            B = rgb.B / 256f;
            A = rgb.A / 256f;

            hsv = White;
            hsv._a = A;

            float min, max;
            min = max = R;
            max = Math.Max(G, max);
            max = Math.Max(B, max);
            min = Math.Min(G, min);
            min = Math.Min(B, min);

            //Compute hue
            if (max == min) H = 0;
            else if (max == R)
            {
                H = (60 * ((G - B) / (max - min)) + 360);
                while (H >= 360) H -= 360; //wrap value to 360
            }
            else if (max == G)
            {
                H = (60 * ((B - R) / (max - min)) + 120);
            }
            else //max == B
            {
                H = (60 * ((R - G) / (max - min)) + 240);
            }
            H = H / 360; //convert from degrees to 0..1 value

            //compute saturation
            if (max == 0) 
                S = 0;
            else
                S = 1 - min / max;

            //compute value
            V = max;

            hsv._h = H;
            hsv._s = S;
            hsv._v = V;
        }

        public static HSVColor ToHSVColor(Color rgb)
        {
            HSVColor hsv = new HSVColor();
            ToHSVColor(rgb, out hsv);
            return hsv;
        }

        private static float ClampValue(float value)
        {
            if (value < 0) return 0;
            else if (value > 1.0f) return 1.0f;
            else return value;
        }
    }
}
