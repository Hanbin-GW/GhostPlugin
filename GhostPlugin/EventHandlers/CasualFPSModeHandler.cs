using System.Collections.Generic;
using System.Text;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.API.Map;
using LabApi.Events.Arguments.ServerEvents;
using MEC;
using PlayerRoles;
using UnityEngine;
using RoundEndedEventArgs = Exiled.Events.EventArgs.Server.RoundEndedEventArgs;

namespace GhostPlugin.EventHandlers
{
    public class CasualFPSModeHandler
    {

        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        }

        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        }
        
        public void ShowMapHint(Player player)
        {
            string roomName = player.CurrentRoom?.Name ?? "Unknown location";
            player.ShowHint($"<b><color=yellow>You are in:</color></b> {roomName}", 1);
        }

        private CoroutineHandle updateHandle;
        private const int MapSize = 32;
        private CellType[,] grid = new CellType[MapSize, MapSize];
        private List<SoundPing> soundPings = new();

        public void OnRoundStarted()
        {
            Start();
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            Stop();
        }


        public void Start()
        {
            InitializeMap();
            updateHandle = Timing.RunCoroutine(UpdateMap());
            Exiled.Events.Handlers.Player.Shooting += OnShooting;
        }

        public void Stop()
        {
            Exiled.Events.Handlers.Player.Shooting -= OnShooting;
            Timing.KillCoroutines(updateHandle);
        }

        private IEnumerator<float> UpdateMap()
        {
            while (true)
            {
                // 사운드 핑 제거
                soundPings.RemoveAll(p => p.ExpireTime < Time.time);

                foreach (Player player in Player.List)
                {
                    UpdatePlayerPos(player);
                    string hint = GenerateHint(player);
                    player.ShowHint(hint, 1.1f);
                }
                yield return Timing.WaitForSeconds(1f);
            }
        }

        private void InitializeMap()
        {
            for (int x = 0; x < MapSize; x++)
                for (int y = 0; y < MapSize; y++)
                    grid[x, y] = (x == 0 || y == 0 || x == MapSize - 1 || y == MapSize - 1)
                        ? CellType.Wall
                        : CellType.Path;
        }

        private void UpdatePlayerPos(Player player)
        {
            for (int x = 0; x < MapSize; x++)
                for (int y = 0; y < MapSize; y++)
                    if (grid[x, y] == CellType.Player)
                        grid[x, y] = CellType.Path;

            int px = Mathf.Clamp(Mathf.RoundToInt(player.Position.x + MapSize / 2), 0, MapSize - 1);
            int py = Mathf.Clamp(Mathf.RoundToInt(player.Position.z + MapSize / 2), 0, MapSize - 1);

            grid[px, py] = CellType.Player;
        }

        private string GenerateHint(Player player)
        {
            int centerX = Mathf.Clamp(Mathf.RoundToInt(player.Position.x + MapSize / 2), 5, MapSize - 6);
            int centerY = Mathf.Clamp(Mathf.RoundToInt(player.Position.z + MapSize / 2), 5, MapSize - 6);

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

                    if (drewPing)
                        continue;

                    switch (grid[x, y])
                    {
                        case CellType.Wall: sb.Append("<color=gray>■</color>"); break;
                        case CellType.Path: sb.Append("<color=black>■</color>"); break;
                        case CellType.Player: sb.Append("<color=green>■</color>"); break;
                    }
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public void AddPing(Vector3 pos, Team team)
        {
            int x = Mathf.Clamp(Mathf.RoundToInt(pos.x + MapSize / 2), 0, MapSize - 1);
            int y = Mathf.Clamp(Mathf.RoundToInt(pos.z + MapSize / 2), 0, MapSize - 1);

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
    }
}