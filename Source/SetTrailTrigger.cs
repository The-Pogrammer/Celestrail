using System.Collections.Generic;
using System.Diagnostics;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.Celestrail
{
    [CustomEntity("Celestrail/SetTrailTrigger")]
    public class SetTrailTrigger : Trigger
    {
        private bool TurnOffToggle;
        private bool TrailActivated;
        private bool SetTrailActivation;
        private bool Reactivateable;
        private bool Activated = false;
        public SetTrailTrigger(EntityData data, Vector2 offset) : base(data, offset)
        {
            TurnOffToggle = data.Bool("turnofftoggle", true);
            TrailActivated = data.Bool("trailactivated", false);
            SetTrailActivation = data.Bool("settrailactivation", true);
            Reactivateable = data.Bool("reactivateable", false);
        }

        public void SetTrailState()
        {
            if (SetTrailActivation)
            {
                CelestrailModule.EnableTrail = TrailActivated;
            }
            CelestrailModule.TrailToggleable = !TurnOffToggle;
        }

        public override void OnEnter(Player player)
        {
            base.OnEnter(player);
            if (Reactivateable || !Activated)
            {
                SetTrailState();
                Activated = true;
            }
        }
    }
}
