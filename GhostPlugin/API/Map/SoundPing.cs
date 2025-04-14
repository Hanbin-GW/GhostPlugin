using PlayerRoles;

namespace GhostPlugin.API.Map
{
    public class SoundPing
    {
        public int X;
        public int Y;
        public Team Team;
        public float ExpireTime;

        public string GetColoredDot()
        {
            string color = Team switch
            {
                Team.FoundationForces => "blue",
                Team.ChaosInsurgency => "#518a4c",
                Team.Scientists => "cyan",
                Team.ClassD => "#ffd500",
                Team.SCPs => "red",
                _ => "white"
            };

            return $"<color={color}>●</color>";
        }
    }
}