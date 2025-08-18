using System;

namespace GhostPlugin.API
{
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
        Scp049 = 512,
        Scp079 = 1024,
        Scp096 = 2048,
        Scp106 = 4096,
        Scp173 = 8192,
        Scp939 = 16384,
        Scp3114 = 32768,
    }
}