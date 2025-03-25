namespace GhostPlugin.API
{
    using System;

    [Flags]
    public enum ExemptionType
    {
        RoundStart,
        Respawn,
        Revive,
    }
}