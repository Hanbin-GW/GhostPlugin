namespace GhostPlugin.API
{
    using System;

    [Flags]
    public enum StartTeam
    {
        ClassD = 1,
        Scientist = 2,
        Guard = 4,
        Ntf = 8,
        Chaos = 16,
        Scp = 32,
        Revived = 64,
        Escape = 128,
        Other = 256,
        Scp049 = 512, // 0x00000320
        Scp3114 = 1024, // 0x00000640
        NtfSergeant = 2048,
        ChaosRepressor = 4096,
        Scp096 = 8192,
        Scp106 = 16384,
        Scp939 = 32768,
        Scp173 = 65536,
        Scp079 = 131072,
        Scp0492 = 262144,
    }
}