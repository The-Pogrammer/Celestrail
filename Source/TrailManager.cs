using AsmResolver.PE.DotNet.ReadyToRun;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using System.Collections.Generic;
using static MonoMod.InlineRT.MonoModRule;

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
        public Color[] trailColors;
        private float yoffset;
        private bool createCut = false;

        private Vector2 lastPlayerPos;
        private bool firstFrame = true;

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

        public void AfterPlayerUpdate()
        {
            
            if (CelestrailModule.CelestrailSettings.ToggleTrail.Pressed && CelestrailModule.TrailToggleable)
            {
                CelestrailModule.CelestrailSettings.ToggleTrail.ConsumePress();
                CelestrailModule.EnableTrail = !CelestrailModule.EnableTrail;
            }

            if (SceneAs<Level>() == null) return;

            player = SceneAs<Level>().Tracker.GetEntity<Player>();

            if (SceneAs<Level>().Paused || SceneAs<Level>().wasPaused || player == null) return;

            if (player != null)
            {
                Vector2 delta = player.Position - lastPlayerPos;

                if (!firstFrame)
                {
                    float deltaLen = delta.Length();
                    float speedLen = player.Speed.Length();

                    bool carriedBySolid = player.LiftSpeed != Vector2.Zero;

                    if (!carriedBySolid)
                    {
                        if (deltaLen > 6f && deltaLen > speedLen * 2.5f)
                        {
                            cutTrail();
                        }
                    }
                }
                else
                    firstFrame = false;

                lastPlayerPos = player.Position;
            }

            if (player == null || !player.InControl)
            {
                foreach (var segment in trailSegments)
                {
                    segment.Alpha -= trailFadeSpeed;
                }
            
                while (trailSegments.Count > 0 && trailSegments.Peek().Alpha <= 0)
                {
                    trailSegments.Dequeue();
                }
                return;
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


            player = Scene.Tracker.GetEntity<Player>();
            
            if (trailSegments.Count < 2)
                return;

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
