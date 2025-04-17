using System;
using System.Collections.Generic;
using System.Text;
using Exiled.API.Enums;
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
        private readonly Dictionary<ZoneType, CellType[,]> zoneGrids = new();
        private readonly CellType[,] fallbackGrid = new CellType[MapSize, MapSize];
        private readonly List<SoundPing> soundPings = new();
        private CoroutineHandle updateHandle;
        public Plugin Plugin;

        public CasualFPSModeHandler(Plugin plugin) => Plugin = plugin;

        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
            Exiled.Events.Handlers.Server.RestartingRound += OnRestarting;
            Exiled.Events.Handlers.Player.Shooting += OnShooting;
        }

        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Server.RestartingRound -= OnRestarting;
            Exiled.Events.Handlers.Player.Shooting -= OnShooting;
        }

        public void OnRestarting()
        {
            Plugin.Instance.miniMapEnabled.Clear();
        }

        public void OnRoundStarted()
        {
            Plugin.Instance.miniMapEnabled.Clear();

            Timing.CallDelayed(1f, () =>
            {
                Log.Info("[MiniMap] Round started, initializing...");
                Start();

                foreach (Player player in Player.List)
                    Plugin.Instance.miniMapEnabled[player.Id] = true;
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

        private int WorldToMapX(float x) => Mathf.Clamp(Mathf.RoundToInt(x / 10f + MapSize / 2f), 0, MapSize - 1);
        private int WorldToMapY(float z) => Mathf.Clamp(Mathf.RoundToInt(z / 10f + MapSize / 2f), 0, MapSize - 1);

        private CellType[,] CreateEmptyGrid()
        {
            CellType[,] grid = new CellType[MapSize, MapSize];
            for (int x = 0; x < MapSize; x++)
                for (int y = 0; y < MapSize; y++)
                    grid[x, y] = CellType.Wall;

            return grid;
        }

        private void InitializeMap()
        {
            zoneGrids.Clear();
            foreach (ZoneType zone in Enum.GetValues(typeof(ZoneType)))
                zoneGrids[zone] = CreateEmptyGrid();

            foreach (Room room in Room.List)
            {
                ZoneType zone = room.Zone;
                var zoneGrid = zoneGrids[zone];

                int cx = WorldToMapX(room.Position.x);
                int cy = WorldToMapY(room.Position.z);

                for (int dx = -2; dx <= 2; dx++)
                for (int dy = -2; dy <= 2; dy++)
                {
                    int x = cx + dx;
                    int y = cy + dy;
                    if (x >= 0 && y >= 0 && x < MapSize && y < MapSize)
                        zoneGrid[x, y] = CellType.Path;
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
                    if (!Plugin.Instance.miniMapEnabled.TryGetValue(player.Id, out bool enabled) || !enabled)
                        continue;

                    UpdateGridForPlayer(player);
                    string hint = GenerateHint(player);
                    player.ShowHint(hint, 1f);
                }

                yield return Timing.WaitForSeconds(1f);
            }
        }

        private void UpdateGridForPlayer(Player player)
        {
            ZoneType zone = player.CurrentRoom?.Zone ?? ZoneType.Other;
            if (!zoneGrids.TryGetValue(zone, out var zoneGrid))
                zoneGrid = fallbackGrid;

            for (int x = 0; x < MapSize; x++)
                for (int y = 0; y < MapSize; y++)
                    if (zoneGrid[x, y] == CellType.Player)
                        zoneGrid[x, y] = CellType.Path;

            int px = WorldToMapX(player.Position.x);
            int py = WorldToMapY(player.Position.z);
            zoneGrid[px, py] = CellType.Player;
        }

        private string GenerateHint(Player player)
        {
            int playerX = WorldToMapX(player.Position.x);
            int playerY = WorldToMapY(player.Position.z);
            const int range = 15;

            StringBuilder sb = new();
            ZoneType currentZone = player.CurrentRoom?.Zone ?? ZoneType.Other;
            var zoneGrid = zoneGrids.TryGetValue(currentZone, out var g) ? g : fallbackGrid;

            for (int y = range; y >= -range + 1; y--)
            {
                for (int x = -range; x < range; x++)
                {
                    int gx = playerX + x;
                    int gy = playerY + y;

                    if (gx < 0 || gy < 0 || gx >= MapSize || gy >= MapSize)
                    {
                        sb.Append(" ");
                        continue;
                    }

                    Vector3 worldPos = new Vector3((gx - MapSize / 2f) * 10f, 0, (gy - MapSize / 2f) * 10f);
                    Room room = Room.Get(worldPos);
                    if (room == null)
                    {
                        sb.Append("<color=#6e6e6e>■</color>");
                        continue;
                    }

                    // 사운드 핑
                    bool drewPing = false;
                    foreach (var ping in soundPings)
                    {
                        if (ping.X == gx && ping.Y == gy)
                        {
                            Vector3 pingWorld = new Vector3((ping.X - MapSize / 2f) * 10f, 0, (ping.Y - MapSize / 2f) * 10f);
                            if (Room.Get(pingWorld)?.Zone == currentZone)
                            {
                                sb.Append(ping.GetColoredDot());
                                drewPing = true;
                                break;
                            }
                        }
                    }

                    if (drewPing) continue;

                    switch (zoneGrid[gx, gy])
                    {
                        case CellType.Wall: sb.Append("<color=#6e6e6e>■</color>"); break;
                        case CellType.Path: sb.Append("<color=black>■</color>"); break;
                        case CellType.Player: sb.Append("<color=#63ff8d>■</color>"); break;
                        default: sb.Append("<color=#6e6e6e>■</color>"); break;
                    }
                }

                sb.AppendLine();
            }

            string roomName = player.CurrentRoom?.Name ?? "Unknown";
            string zoneName = currentZone.ToString();
            string zoneColor = zoneName switch
            {
                "LightContainment" => "yellow",
                "HeavyContainment" => "orange",
                "Entrance" => "cyan",
                "Surface" => "green",
                _ => "gray"
            };
            string zoneInfo = $"<size=20><color={zoneColor}>Room: {roomName} | Zone: {zoneName}</color></size>";
            return $"<align=left><size=7>{sb}</size>{zoneInfo}";
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

        private void OnShooting(ShootingEventArgs ev) => AddPing(ev.Player.Position, ev.Player.Role.Team);

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
