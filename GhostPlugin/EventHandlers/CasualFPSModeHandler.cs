using System.Collections.Generic;
using System.Text;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using UnityEngine;
using RoundEndedEventArgs = Exiled.Events.EventArgs.Server.RoundEndedEventArgs;

namespace GhostPlugin.EventHandlers
{
    public class CasualFPSModeHandler
    {
        private const int MapSize = 128;
        private CellType[,] grid = new CellType[MapSize, MapSize];
        private readonly List<SoundPing> soundPings = new();
        private CoroutineHandle updateHandle;
        public Plugin Plugin;

        public CasualFPSModeHandler(Plugin plugin) => Plugin = plugin;

        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
            Exiled.Events.Handlers.Player.Shooting += OnShooting;
        }

        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Player.Shooting -= OnShooting;
        }

        public void OnRoundStarted()
        {
            Timing.CallDelayed(1f, () =>
            {
                Log.Info("[MiniMap] Round started, initializing...");
                Start();
            });
        }

        public void OnRoundEnded(RoundEndedEventArgs ev) => Stop();

        public void Start()
        {
            InitializeMap();
            updateHandle = Timing.RunCoroutine(UpdateMap());
        }

        public void Stop()
        {
            Timing.KillCoroutines(updateHandle);
        }

        private int WorldToMapX(float x) => Mathf.Clamp(Mathf.RoundToInt(x + MapSize / 2f), 0, MapSize - 1);
        private int WorldToMapY(float z) => Mathf.Clamp(Mathf.RoundToInt(z + MapSize / 2f), 0, MapSize - 1);

        private void InitializeMap()
        {
            for (int x = 0; x < MapSize; x++)
                for (int y = 0; y < MapSize; y++)
                    grid[x, y] = CellType.Wall;

            foreach (Room room in Room.List)
            {
                int cx = WorldToMapX(room.Position.x);
                int cy = WorldToMapY(room.Position.z);

                for (int dx = -2; dx <= 2; dx++)
                    for (int dy = -2; dy <= 2; dy++)
                    {
                        int x = cx + dx;
                        int y = cy + dy;
                        if (x >= 0 && y >= 0 && x < MapSize && y < MapSize)
                            grid[x, y] = CellType.Path;
                    }
            }
        }

        private IEnumerator<float> UpdateMap()
        {
            while (true)
            {
                soundPings.RemoveAll(p => p.ExpireTime < Time.time);

                foreach (Player player in Player.List)
                {
                    UpdateGridForPlayer(player);
                    string hint = GenerateHint(player);
                    player.ShowHint(hint, 1.1f);
                }

                yield return Timing.WaitForSeconds(1f);
            }
        }

        private void UpdateGridForPlayer(Player player)
        {
            for (int x = 0; x < MapSize; x++)
                for (int y = 0; y < MapSize; y++)
                    if (grid[x, y] == CellType.Player)
                        grid[x, y] = CellType.Path;

            int px = WorldToMapX(player.Position.x);
            int py = WorldToMapY(player.Position.z);
            grid[px, py] = CellType.Player;
        }

        private string GenerateHint(Player player)
        {
            int centerX = WorldToMapX(player.Position.x);
            int centerY = WorldToMapY(player.Position.z);

            StringBuilder sb = new();

            for (int y = centerY + 5; y >= centerY - 5; y--)
            {
                for (int x = centerX - 5; x <= centerX + 5; x++)
                {
                    bool drewPing = false;

                    foreach (var ping in soundPings)
                    {
                        if (ping.X == x && ping.Y == y)
                        {
                            sb.Append(ping.GetColoredDot());
                            drewPing = true;
                            break;
                        }
                    }

                    if (drewPing) continue;

                    if (x < 0 || y < 0 || x >= MapSize || y >= MapSize)
                    {
                        sb.Append(" ");
                        continue;
                    }

                    switch (grid[x, y])
                    {
                        case CellType.Wall: sb.Append("<color=#9c9c9c>■</color>"); break;
                        case CellType.Path: sb.Append("<color=black>■</color>"); break;
                        case CellType.Player: sb.Append("<color=green>■</color>"); break;
                        default: sb.Append(" "); break;
                    }
                }
                sb.AppendLine();
            }

            return "<align=left><size=10>" + sb.ToString() + "</size>";
        }

        public void AddPing(Vector3 pos, Team team)
        {
            int x = WorldToMapX(pos.x);
            int y = WorldToMapY(pos.z);

            soundPings.Add(new SoundPing
            {
                X = x,
                Y = y,
                Team = team,
                ExpireTime = Time.time + 3f
            });
        }

        private void OnShooting(ShootingEventArgs ev)
        {
            AddPing(ev.Player.Position, ev.Player.Role.Team);
        }

        private enum CellType
        {
            Wall,
            Path,
            Player
        }

        private class SoundPing
        {
            public int X;
            public int Y;
            public Team Team;
            public float ExpireTime;

            public string GetColoredDot()
            {
                return Team switch
                {
                    Team.FoundationForces => "<color=blue>●</color>",
                    Team.ChaosInsurgency => "<color=red>●</color>",
                    Team.Scientists => "<color=cyan>●</color>",
                    Team.ClassD => "<color=orange>●</color>",
                    Team.SCPs => "<color=purple>●</color>",
                    _ => "<color=white>●</color>"
                };
            }
        }
    }
}
