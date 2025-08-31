using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;

namespace GhostPlugin.EventHandlers
{
    public class PerkEventHandlers
    {
        public Plugin Plugin;
        private readonly Dictionary<Player, List<ActiveAbility>> playerActives = new();
        private readonly Dictionary<Player, List<PassiveAbility>> playerPassives = new();
        public PerkEventHandlers(Plugin plugin) => Plugin = plugin;

        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Player.Died += OnPlayerDied;
            Exiled.Events.Handlers.Player.Left += OnPlayerLeft;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        }

        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Player.Died -= OnPlayerDied;
            Exiled.Events.Handlers.Player.Left -= OnPlayerLeft;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;

        }
        
        public void GrantAbility(Player player, ActiveAbility ability)
        {
            if (!playerActives.ContainsKey(player))
                playerActives[player] = new List<ActiveAbility>();

            playerActives[player].Add(ability);
            ability.AddAbility(player);

            player.ShowHint($"능력 '{ability.Name}' 를 획득했습니다!", 5);
        }
        
        public void GrantAbility(Player player, PassiveAbility ability)
        {
            if (!playerPassives.ContainsKey(player))
                playerPassives[player] = new List<PassiveAbility>();

            playerPassives[player].Add(ability);
            ability.AddAbility(player);

            player.ShowHint($"패시브능력 '{ability.Name}' 를 획득했습니다!", 5);
        }
        
        public void RemoveAllPassives(Player player)
        {
            if (!playerPassives.TryGetValue(player, out var abilities))
                return;

            foreach (var ability in abilities)
                ability.RemoveAbility(player);

            playerPassives.Remove(player);
        }
        
        public void RemoveAllAbilities(Player player)
        {
            if (!playerActives.ContainsKey(player))
                return;

            foreach (var ability in playerActives[player])
                ability.RemoveAbility(player);

            playerActives.Remove(player);
        }
        
        private void OnPlayerDied(DiedEventArgs ev)
        {
            if (!playerActives.ContainsKey(ev.Player))
                return;
            
            if (!playerPassives.ContainsKey(ev.Player))
                return;
            
            if (playerActives.TryGetValue(ev.Player, out var activeAbilitieslist))
            {
                foreach (var ability in activeAbilitieslist)
                    ability.RemoveAbility(ev.Player);

                playerActives.Remove(ev.Player);
            }

            if (playerPassives.TryGetValue(ev.Player, out var passiveAbilitieslist))
            {
                foreach (var ability in passiveAbilitieslist)
                    ability.RemoveAbility(ev.Player);
                playerPassives.Remove(ev.Player);
            }
        }
        
        private void OnPlayerLeft(LeftEventArgs ev)
        {
            RemoveAllAbilities(ev.Player);
            RemoveAllPassives(ev.Player);
        }
        
        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            foreach (var kvp in playerActives)
            foreach (var ability in kvp.Value)
                ability.RemoveAbility(kvp.Key);
            playerActives.Clear();

            foreach (var kvp in playerPassives)
            foreach (var ability in kvp.Value)
                ability.RemoveAbility(kvp.Key);
            playerPassives.Clear();
        }
    }
}