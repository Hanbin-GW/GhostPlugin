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
                Team.ChaosInsurgency => "red",
                Team.Scientists => "cyan",
                Team.ClassD => "orange",
                Team.SCPs => "purple",
                _ => "white"
            };

            return $"<color={color}>●</color>";
        }
    }
}