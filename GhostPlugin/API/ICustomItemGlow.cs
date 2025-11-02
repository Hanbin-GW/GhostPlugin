using UnityEngine;

namespace GhostPlugin.API
{
    public interface ICustomItemGlow
    {
        public bool HasCustomItemGlow { get; set; }
        public Color CustomItemGlowColor { get; set; }
    }
}