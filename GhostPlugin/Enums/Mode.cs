namespace GhostPlugin.Enums
{
    public enum RunMode
    {
        Full,
        Limited,
        Blocked
    }

    public enum Feature
    {
        Core,
        Blackout,
        Items,
        Roles,
        Abilities,
        Music,
        Ssss,
        FPSMap,
        Classic,
        Perks
    }

    public static class RunModeResolver
    {
        public static RunMode Resolve()
        {
            var ip = Exiled.API.Features.Server.IpAddress;

            if (Plugin.Instance.Config.BlackListedIP.Contains(ip))
                return RunMode.Blocked;

            if (Plugin.Instance.Config.AllowedIP.Contains(ip))
                return RunMode.Full;

            // Allowed도 아니고 Blacklist도 아니면 Classic만
            return RunMode.Limited;
        }
    }
}