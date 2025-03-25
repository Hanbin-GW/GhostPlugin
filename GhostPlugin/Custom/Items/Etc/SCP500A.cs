using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;

namespace GhostPlugin.Custom.Items.Etc
{
    public class SCP500A : CustomItem
    {
        public override uint Id { get; set; } = 2;
        public override string Name { get; set; } = "SCP500-A";
        public override string Description { get; set; } = "WIP";
        public override float Weight { get; set; } = 1f;
        public override SpawnProperties SpawnProperties { get; set; }
    }
}