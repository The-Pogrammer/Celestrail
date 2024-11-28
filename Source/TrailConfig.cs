using Microsoft.Xna.Framework;
using Celeste.Mod.Celestrail;
using System.Collections.Generic;

public static class TrailConfig
{
    public static readonly Color[] TransFlagColors = new Color[]
    {
    new Color(91, 206, 250),  // Light Blue
    new Color(245, 169, 184), // Pink
    Color.White,              // White
    new Color(245, 169, 184), // Pink
    new Color(91, 206, 250),  // Light Blue
    };

    public static readonly Color[] PanFlagColors = new Color[]
    {
    new Color(255, 33, 140),  // Pink
    new Color(255, 216, 0),   // Yellow
    new Color(33, 177, 255)   // Blue
    };

    public static readonly Color[] RainbowFlagColors = new Color[]
    {
    new Color(228, 3, 3),     // Red
    new Color(255, 140, 0),   // Orange
    new Color(255, 237, 0),   // Yellow
    new Color(0, 128, 38),    // Green
    new Color(0, 76, 255),    // Blue
    new Color(115, 41, 130),  // Purple
    };

    public static readonly Color[] BiFlagColors = new Color[]
    {
    new Color(214, 2, 112),   // Magenta
    new Color(155, 79, 150),  // Purple
    new Color(0, 56, 168),    // Blue
    };

    public static readonly Color[] OmnisexualFlagColors = new Color[]
    {
    new Color(254, 154, 206), // Light Pink
    new Color(255, 83, 191),  // Pink
    new Color(32, 0, 68),     // Dark Purple
    new Color(103, 96, 254),  // Blue
    new Color(142, 166, 255), // Bright Blue
    };

    public static readonly Color[] PolysexualFlagColors = new Color[]
    {
    new Color(247, 20, 1866), // Pink
    new Color(1, 214, 106),   // Green
    new Color(21, 148, 246),  // Blue
    };

    public static readonly Color[] AsexualFlagColors = new Color[]
    {
    Color.Black,              // Black
    new Color(163, 163, 163), // Gray
    Color.White,              // White
    new Color(128, 0, 128),   // Purple
    };

    public static readonly Color[] AromanticFlagColors = new Color[]
    {
    new Color(61, 165, 66),   // Green
    new Color(167, 211, 121), // Light Green
    Color.White,              // White
    new Color(169, 169, 169), // Grey
    Color.Black,              // Black
    };

    public static readonly Color[] AroaceFlagColors = new Color[]
    {
    new Color(226, 140, 0),   // Orange
    new Color(236, 205, 0),   // Yellow
    Color.White,              // White
    new Color(98, 174, 220),  // Light Blue
    new Color(32, 56, 86),    // Dark Blue
    };

    public static readonly Color[] NonbinaryFlagColors = new Color[]
    {
    new Color(252, 244, 52),  // Yellow
    Color.White,              // White
    new Color(156, 89, 209),  // Purple
    new Color(44, 44, 44),    // Black
    };

    public static readonly Color[] GenderfluidFlagColors = new Color[]
    {
    new Color(255, 118, 164), // Pink
    Color.White,              // White
    new Color(192, 17, 215),  // Purple
    Color.Black,              // Black
    new Color(47, 60, 190),   // Blue
    };

    public static readonly Color[] LesbianFlagColors = new Color[]
    {
    new Color(213, 45, 0),    // Dark Orange
    new Color(239, 118, 39),  // Orange
    new Color(255, 154, 86),  // Light Orange
    Color.White,              // White
    new Color(209, 98, 164),  // Pink
    new Color(181, 86, 144),  // Dusty Pink
    new Color(163, 2, 98),    // Dark Rose
    };

    public static readonly Color[] GayFlagColors = new Color[]
    {
    new Color(7, 141, 112),   // Dark Green
    new Color(38, 206, 170),  // Green
    new Color(152, 232, 193), // Light Green
    Color.White,              // White
    new Color(123, 173, 226), // Light Blue
    new Color(80, 73, 204),   // Indigo
    new Color(61, 26, 120),   // Blue
    };

    public static readonly Color[] GenderqueerFlagColors = new Color[]
    {
    new Color(181, 126, 220), // Lavender
    Color.White,              // White
    new Color(74, 129, 35),   // Green
    };

    public enum FLAGTHEMES
    {
        Custom,
        Trans_Flag,
        Nonbinary_Flag,
        Genderfluid_Flag,
        Genderqueer_Flag,
        Lesbian_Flag,
        Gay_Flag,
        Pan_Flag,
        Bi_Flag,
        Omnisexual_Flag,
        Polysexual_Flag,
        Asexual_Flag,
        Aromantic_Flag,
        Aroace_Flag,
        Rainbow_Flag,
    };

    public static readonly Dictionary<FLAGTHEMES, Trail> Trails = new Dictionary<FLAGTHEMES, Trail>
    {
        { FLAGTHEMES.Trans_Flag, new Trail(TransFlagColors) },
        { FLAGTHEMES.Nonbinary_Flag, new Trail(NonbinaryFlagColors) },
        { FLAGTHEMES.Genderfluid_Flag, new Trail(GenderfluidFlagColors) },
        { FLAGTHEMES.Genderqueer_Flag, new Trail(GenderqueerFlagColors) },
        { FLAGTHEMES.Lesbian_Flag, new Trail(LesbianFlagColors) },
        { FLAGTHEMES.Gay_Flag, new Trail(GayFlagColors) },
        { FLAGTHEMES.Pan_Flag, new Trail(PanFlagColors) },
        { FLAGTHEMES.Bi_Flag, new Trail(BiFlagColors) },
        { FLAGTHEMES.Omnisexual_Flag, new Trail(OmnisexualFlagColors) },
        { FLAGTHEMES.Polysexual_Flag, new Trail(PolysexualFlagColors) },
        { FLAGTHEMES.Asexual_Flag, new Trail(AsexualFlagColors) },
        { FLAGTHEMES.Aromantic_Flag, new Trail(AromanticFlagColors) },
        { FLAGTHEMES.Aroace_Flag, new Trail(AroaceFlagColors) },
        { FLAGTHEMES.Rainbow_Flag, new Trail(RainbowFlagColors) },
    };
}
