using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Collections.Generic;

namespace Celeste.Mod.Celestrail
{
    [CustomEntity("Celestrail/Flag")]
    public class Flag : Entity
    {
        private Color[] trailColors = TrailConfig.TransFlagColors;
        private const float waveSpeed = 7.5f;
        private Dictionary<string, MTexture> SpriteDict = [];
        private EntityData entityData = null;
        private Vector2 flagOffset;
        private bool right;
        private float randomOffset;

        public Flag(EntityData data, Vector2 offset)
            : base(data.Position + offset)
        {
            entityData = data;
            right = data.Bool("right", false);
            Depth = Depths.Player + 1;
            SpriteDict["Bottom"] = GFX.Game["flag/flagBottom00"];
            SpriteDict["Middle"] = GFX.Game["flag/flagMid00"];
            SpriteDict["Top"] = GFX.Game["flag/flagTop00"];
            
            flagOffset = new Vector2(-11 + (right ? 16 : 0), 4);
            int seed = (int)(Position.X * 1000 + Position.Y);
            randomOffset = new Random(seed).NextFloat(MathHelper.TwoPi);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Render()
        {
            base.Render();

            TrailManager manager = Scene.Tracker.GetEntity<TrailManager>();
            if (manager != null)
            {
                trailColors = manager.trailColors;
            }

            float time = Scene.TimeActive * waveSpeed * (right ? -1 : 1) + randomOffset;
            float stripeHeight = 2f;
            int segments = 7;
            float segmentWidth = 2f;
            

            // Add sinHeight variable to control wave amplitude
            float sinHeight = 1.5f;  // Adjust this value to change the wave height (vertical displacement)

            float maxYOffset = float.MinValue;
            float minYOffset = float.MaxValue;

            // First pass to calculate max and min yOffsets with sinHeight factor
            for (int i = 0; i < segments; i++)
            {
                float yOffset = MathF.Sin(i * 0.5f + time) * sinHeight + flagOffset.Y;
                maxYOffset = MathF.Max(maxYOffset, yOffset);
                minYOffset = MathF.Min(minYOffset, yOffset);
            }

            // Now we render with shading based on the max and min offsets
            for (int j = 0; j < trailColors.Length; j++)
            {
                Color color = trailColors[j];
                if (color == Color.Transparent)
                    continue;

                for (int i = 0; i < segments; i++)
                {
                    float x = i * segmentWidth + flagOffset.X;
                    float yOffset = MathF.Sin(i * 0.5f + time) * sinHeight + flagOffset.Y;

                    // Calculate how far the current yOffset is from the min and max offsets
                    float range = maxYOffset - minYOffset;
                    float normalizedDisplacement = (yOffset - minYOffset) / range; // Normalized displacement between 0 and 1

                    // Darken the flag when it's closer to the neutral position (minYOffset)
                    float darknessFactor = MathHelper.Clamp(normalizedDisplacement, 0f, 1f);

                    // Reduce the intensity of the darkening effect by applying a scaling factor
                    darknessFactor *= 0.3f;  // Apply a scaling factor to make the darkening less intense

                    // Apply darkening based on displacement (higher displacement = less dark)
                    Color shadedColor = new Color(
                        (byte)(color.R - (color.R * darknessFactor)),
                        (byte)(color.G - (color.G * darknessFactor)),
                        (byte)(color.B - (color.B * darknessFactor)),
                        color.A
                    );

                    // Draw the flag stripes with the shaded color
                    Draw.Rect(
                        Position.X + x,
                        Position.Y + j * stripeHeight + yOffset,
                        segmentWidth,
                        stripeHeight,
                        shadedColor
                    );
                }
            }

            int segmentCount = (int)MathF.Round(entityData.Height / 8);

            for (int i = 0; i < segmentCount - 1; i++)
            {
                SpriteDict["Middle"].DrawJustified(Position + entityData.Height * Vector2.UnitY - 8*i * Vector2.UnitY, Vector2.UnitY);
            }

            SpriteDict["Bottom"].DrawJustified(Position + entityData.Height * Vector2.UnitY, Vector2.UnitY);
            SpriteDict["Top"].DrawJustified(Position + 8 * Vector2.UnitY, Vector2.UnitY);
        }
    }
}
