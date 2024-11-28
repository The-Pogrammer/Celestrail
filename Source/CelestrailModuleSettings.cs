using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FMOD;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.Celestrail;

public class CelestrailModuleSettings : EverestModuleSettings
{
    [SettingSubMenu]
    public class CustomFlagMenu
    {
        [SettingMaxLength(7)]
        public string CustomColorOne { get; set; }
        [SettingMaxLength(7)]
        public string CustomColorTwo { get; set; }
        [SettingMaxLength(7)]
        public string CustomColorThree { get; set; }
        [SettingMaxLength(7)]
        public string CustomColorFour { get; set; }
        [SettingMaxLength(7)]
        public string CustomColorFive { get; set; }
        [SettingMaxLength(7)]
        public string CustomColorSix { get; set; }
        [SettingMaxLength(7)]
        public string CustomColorSeven { get; set; }
        private Color[] ColorCache;


        public static bool ValidateColor(string hex)
        {
            // Remove leading '#' and trim whitespace
            hex = hex.TrimStart('#').Trim();

            // Check if length is valid
            if (hex.Length != 6)
            {
                return false;
            }

            // Check if it contains only valid hex characters
            if (!System.Text.RegularExpressions.Regex.IsMatch(hex, "^[0-9A-Fa-f]+$"))
            {
                return false;
            }

            return true;
        }



        public static Color HexToColor(string hex)
        {
            // Ensure no leading/trailing whitespace
            hex = hex.Trim();

            // Remove '#' if present
            hex = hex.TrimStart('#');

            // Check if length is valid
            if (hex.Length != 6)
                throw new ArgumentException($"Invalid hex string length: '{hex.Length}'. Input: '{hex}'");

            // Parse RGB values
            byte r = Convert.ToByte(hex.Substring(0, 2), 16);
            byte g = Convert.ToByte(hex.Substring(2, 2), 16);
            byte b = Convert.ToByte(hex.Substring(4, 2), 16);

            // Parse Alpha value if available; otherwise, default to 255 (opaque)
            byte a = (hex.Length == 8) ? Convert.ToByte(hex.Substring(6, 2), 16) : (byte)255;

            // Return normalized Color
            return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
        }





        public Color[] GetColors(Color[] ExpectedColors)
        {
            if (ExpectedColors == ColorCache) {  return ExpectedColors; }
            Queue<Color> colors = new Queue<Color>();

            string[] customColors = new string[]
            {
                CustomColorOne, CustomColorTwo, CustomColorThree,
                CustomColorFour, CustomColorFive, CustomColorSix,
                CustomColorSeven
            };

            foreach (var colorHex in customColors)
            {
                if (colorHex.TrimStart('#') == "0" || colorHex == "#")
                {
                    colors.Enqueue(Color.Transparent);
                    continue;
                }
                if (ValidateColor(colorHex))
                {
                    colors.Enqueue(HexToColor(colorHex));
                }
            }

            ColorCache = colors.ToArray();
            return ColorCache;
        }
    }
    [SettingSubText("This setting defines the hex Colors for the custom flag.\nAny invalid or empty colors are filtered out.\n enter '#', '0', or '#0' to make gaps in the trail.")]
    public CustomFlagMenu CustomFlag { get; set; } = new();

    public TrailConfig.FLAGTHEMES SelectedFlag { get; set; } = TrailConfig.FLAGTHEMES.Trans_Flag;
    [SettingRange(min: 10, max: 40)]
    public int MaxTrailLength { get; set; } = 20;
    [SettingRange(min: 3, max: 6)]
    [SettingSubText("Higher numbers fade faster.")]
    public int TrailFadeSpeed { get; set; } = 5;
    [SettingRange(min: 5, max: 15)]
    public int TrailWidth { get; set; } = 15;
    [SettingRange(min: -3, max: 3)]
    public int YOffset { get; set; } = 0;
}