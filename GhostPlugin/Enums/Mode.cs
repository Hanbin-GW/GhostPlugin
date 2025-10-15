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
            var cfg = global::GhostPlugin.Plugin.Instance?.Config;

            if (cfg == null) return RunMode.Limited;

            if (string.IsNullOrWhiteSpace(ip))
            {
                Exiled.API.Features.Log.Warn("[GhostPlugin] IP not resolved yet; defaulting to Limited for now.");
                return RunMode.Limited;
            }

            if (cfg.BlackListedIP.Contains(ip)) return RunMode.Blocked;
            if (cfg.AllowedIP.Contains(ip))     return RunMode.Full;

            return RunMode.Limited;
        }

    }
}