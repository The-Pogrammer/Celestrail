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
        private bool createCut = false;

        public TrailManager()
            : base()
        {
            AddTag(Tags.Global);
            trailSegments = new Queue<TrailSegment>();
            UpdateTrail(TrailConfig.Trails[TrailConfig.FLAGTHEMES.Trans_Flag]);
        }

        public void cutTrail()
        {
            createCut = true;
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

            if (CelestrailModule.CelestrailSettings.ToggleTrail.Pressed && CelestrailModule.TrailToggleable)
            {
                CelestrailModule.CelestrailSettings.ToggleTrail.ConsumePress();
                CelestrailModule.EnableTrail = !CelestrailModule.EnableTrail;
            }

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

            trailSegments.Enqueue(new TrailSegment(player.Center + Vector2.UnitY * yoffset, 1f, createCut)); // Alpha starts at 1
            createCut = false;
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

            if (CelestrailModule.EnableTrail)
            {
                TrailSegment prevSegment = null;
                foreach (var segment in trailSegments)
                {
                    if (prevSegment == null)
                    {
                        prevSegment = segment;
                        continue;
                    }

                    float alpha = prevSegment.Alpha;
                    if (alpha <= 0 || segment.Iscut)
                    {
                        prevSegment = segment;
                        continue;
                    }

                    Vector2 start = prevSegment.Position;
                    Vector2 end = segment.Position;

                    // Draw quads for each color
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

                    prevSegment = segment;
                }
            }
        }

        private class TrailSegment
        {
            public Vector2 Position;
            public float Alpha;
            public bool Iscut;

            public TrailSegment(Vector2 position, float alpha, bool iscut = false)
            {
                Position = position;
                Alpha = alpha;
                Iscut = iscut;
            }
        }
    }
}
