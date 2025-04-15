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

        /*private int WorldToMapX(float x) => Mathf.Clamp(Mathf.RoundToInt(x + MapSize / 2f), 0, MapSize - 1);
        private int WorldToMapY(float z) => Mathf.Clamp(Mathf.RoundToInt(z + MapSize / 2f), 0, MapSize - 1);*/
        private int WorldToMapX(float x) => Mathf.Clamp(Mathf.RoundToInt(x / 8f + MapSize / 2f), 0, MapSize - 1);
        private int WorldToMapY(float z) => Mathf.Clamp(Mathf.RoundToInt(z / 8f + MapSize / 2f), 0, MapSize - 1);



        private void InitializeMap()
        {
            for (int x = 0; x < MapSize; x++)
            for (int y = 0; y < MapSize; y++)
                grid[x, y] = CellType.Wall;

            foreach (Room room in Room.List)
            {
                Vector3 pos = room.Position;
                int cx = WorldToMapX(pos.x);
                int cy = WorldToMapY(pos.z);

                Log.Info($"[MiniMap] Room: {room.Name} at grid ({cx}, {cy})");

                int roomRadius = 2;
                for (int dx = -roomRadius; dx <= roomRadius; dx++)
                for (int dy = -roomRadius; dy <= roomRadius; dy++)
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
                    player.ShowHint(hint, 1f);
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
            Log.Debug($"[MiniMap] {player.Nickname} grid pos: ({px}, {py})");
        }

        private string GenerateHint(Player player)
        {
            int playerX = WorldToMapX(player.Position.x);
            int playerY = WorldToMapY(player.Position.z);

            const int range = 15; // 좌우/상하 범위
            StringBuilder sb = new();

            for (int y = range; y >= -range + 1; y--)
            {
                for (int x = -range; x < range; x++)
                {
                    int gx = playerX + x;
                    int gy = playerY + y;

                    bool drewPing = false;
                    foreach (var ping in soundPings)
                    {
                        if (ping.X == gx && ping.Y == gy)
                        {
                            sb.Append(ping.GetColoredDot());
                            drewPing = true;
                            break;
                        }
                    }

                    if (drewPing) continue;

                    if (gx < 0 || gy < 0 || gx >= MapSize || gy >= MapSize)
                    {
                        sb.Append(" ");
                        continue;
                    }

                    switch (grid[gx, gy])
                    {
                        case CellType.Wall: sb.Append("<color=#4f4f4f>■</color>"); break;
                        case CellType.Path: sb.Append("<color=black>■</color>"); break;
                        case CellType.Player: sb.Append("<color=green>■</color>"); break;
                        default: sb.Append(" "); break;
                    }
                }
                sb.AppendLine();
            }

            return "<align=left><size=7>" + sb.ToString() + "</size>";
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
