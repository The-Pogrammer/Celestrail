using System.Collections.Generic;
using System.Diagnostics;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.Celestrail
{
    [CustomEntity("Celestrail/TrailManager")]
    [Tracked]
    public class TrailManager : Entity
    {
        private Player player;
        private Queue<TrailSegment> trailSegments; // Stores segments of the trail
        private int maxTrailLength; // Maximum number of trail segments
        private float trailFadeSpeed; // Controls how fast the trail fades
        private float trailWidth; // Width of the trail
        private Color[] trailColors;
        private float yoffset;

        public TrailManager()
            : base()
        {
            AddTag(Tags.Global);
            trailSegments = new Queue<TrailSegment>();
            UpdateTrail(TrailConfig.Trails[TrailConfig.FLAGTHEMES.Trans_Flag]);
        }

        private void UpdateSettingsValues()
        {
            trailWidth = CelestrailModule.CelestrailSettings.TrailWidth;
            trailFadeSpeed = CelestrailModule.CelestrailSettings.TrailFadeSpeed / 100f;
            maxTrailLength = CelestrailModule.CelestrailSettings.MaxTrailLength;
            yoffset = CelestrailModule.CelestrailSettings.YOffset;
        }

        private void UpdateTrail(Trail trail)
        {
            trailColors = trail.colors;
            UpdateSettingsValues();
            return;
        }

        private void CustomTrail()
        {
            trailColors = CelestrailModule.CelestrailSettings.CustomFlag.GetColors(trailColors);
            if (trailColors == null || trailColors.Length == 0) {
                CelestrailModule.CelestrailSettings.SelectedFlag = TrailConfig.FLAGTHEMES.Trans_Flag;
                UpdateTrail(TrailConfig.Trails[TrailConfig.FLAGTHEMES.Trans_Flag]);
            }
            UpdateSettingsValues();
        }

        public override void Update()
        {
            base.Update();
            player = Scene.Tracker.GetEntity<Player>();

            // If the player is null or in an intro state, we fade the trail out
            if (player == null || !player.InControl)
            {
                // Fade out all trail segments over time
                foreach (var segment in trailSegments)
                {
                    segment.Alpha -= trailFadeSpeed; // Gradually reduce alpha to 0
                }

                // Remove segments that are fully transparent
                while (trailSegments.Count > 0 && trailSegments.Peek().Alpha <= 0)
                {
                    trailSegments.Dequeue();
                }
                return; // Early exit if the player is dead or in intro state
            }

            // Update trail when player is alive
            if (CelestrailModule.CelestrailSettings.SelectedFlag == TrailConfig.FLAGTHEMES.Custom)
            {
                CustomTrail();
            }
            else
            {
                UpdateTrail(TrailConfig.Trails[CelestrailModule.CelestrailSettings.SelectedFlag]);
            }

            // Add the player's current position to the trail
            trailSegments.Enqueue(new TrailSegment(player.Center + Vector2.UnitY * yoffset, 1f)); // Alpha starts at 1

            // Remove older segments if the trail exceeds the maximum length
            while (trailSegments.Count > maxTrailLength)
            {
                trailSegments.Dequeue();
            }

            // Fade out all trail segments over time
            foreach (var segment in trailSegments)
            {
                segment.Alpha -= trailFadeSpeed;
            }

            // Remove segments that are fully transparent
            while (trailSegments.Count > 0 && trailSegments.Peek().Alpha <= 0)
            {
                trailSegments.Dequeue();
            }
        }


        public override void Render()
        {
            base.Render();

            if (trailSegments.Count < 2)
                return;

            player = Scene.Tracker.GetEntity<Player>();
            if (player != null && !player.InControl)
            {
                return;
            }

            Vector2[] positions = new Vector2[trailSegments.Count];
            float[] alphas = new float[trailSegments.Count];

            int index = 0;
            foreach (var segment in trailSegments)
            {
                positions[index] = segment.Position;
                alphas[index] = segment.Alpha;
                index++;
            }

            for (int i = 0; i < positions.Length - 1; i++)
            {
                float alpha = alphas[i];
                if (alpha <= 0)
                    continue;

                Vector2 start = positions[i];
                Vector2 end = positions[i + 1];

                // Draw quads for each color in the trans flag
                for (int j = 0; j < trailColors.Length; j++)
                {
                    if (trailColors[j] == Color.Transparent)
                    {
                        continue;
                    }
                    float offset = -trailWidth / 2 + (trailWidth / trailColors.Length) * j;
                    Color color = trailColors[j] * alpha;

                    Draw.Line(
                        start + new Vector2(0, offset),
                        end + new Vector2(0, offset),
                        color,
                        trailWidth / trailColors.Length
                    );
                }
            }
        }

        private class TrailSegment
        {
            public Vector2 Position;
            public float Alpha;

            public TrailSegment(Vector2 position, float alpha)
            {
                Position = position;
                Alpha = alpha;
            }
        }
    }
}
