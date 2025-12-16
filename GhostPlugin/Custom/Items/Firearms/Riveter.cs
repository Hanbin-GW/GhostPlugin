using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.API;
using UnityEngine;
using YamlDotNet.Serialization;
using GhostPlugin.Methods.Objects;
using PlayerRoles;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunE11SR)]
    public class Riveter : CustomWeapon, ICustomItemGlow
    {
        public override uint Id { get; set; } = 51;
        public override string Name { get; set; } = "Riveter";
        public override string Description { get; set; } = "It's an AR, but it's a continuous shotgun that uses <color=#ebc934>.410 Gauge Dragon Breath</color>";
        public override float Weight { get; set; } = 7.5f;
        public override SpawnProperties SpawnProperties { get; set; }
        public override byte ClipSize { get; set; } = 10;
        public override ItemType Type { get; set; } = ItemType.GunE11SR;
        [YamlIgnore] 
        public override float Damage { get; set; }
        protected override void OnShot(ShotEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                ev.CanHurt = false;
                //Color glowColor = new Color(1.0f, 0.0f, 0.0f, 0.1f) * 50f;
                Color glowColor = new ();
                //var direction = ev.Position - ev.Player.Position;
                var direction = ev.Player.CameraTransform.forward.normalized;
                var laserPos = ev.Player.CameraTransform.position + direction * 0.5f;
                //var laserPos = ev.Player.Position + direction * 0.25f;
                var rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);
                //PlasmaCube.SpawmSparkBuckshot(ev.Player, ev.Firearm.Base.transform.position,13,15f,0.05f,glowColor); 
                
                switch (ev.Player.Role.Team)
                {
                    case (Team.FoundationForces):
                        glowColor = new Color(0f, 1f, 1f, 0.1f) * 50;
                        break;
                    case (Team.Scientists):
                        glowColor = new Color(1f, 1f, 0f, 0.1f) * 50;
                        break;
                    case (Team.ChaosInsurgency):
                        glowColor = new Color(0.1f, 1f, 0.1f, 0.1f) * 50;
                        break;
                    case (Team.OtherAlive):
                        glowColor = new Color(1f, 1f, 1f, 0.1f) * 50;
                        break;
                }

                if (CustomRole.Get(18)?.Check(ev.Player) == true)
                {
                    glowColor = new Color(1f, 0f, 0f, 0.1f) * 50;
                }
                
                SpawnPrimitive.spawnPrimitives(ev.Player, 10, rotation, laserPos, glowColor,5,20);
            }
            base.OnShot(ev);
        }

        public bool HasCustomItemGlow { get; set; } = true;
        public Color CustomItemGlowColor { get; set; } = new Color32(255, 45, 45, 127);
        public float GlowRange { get; set; } = 0.15f;
    }
}