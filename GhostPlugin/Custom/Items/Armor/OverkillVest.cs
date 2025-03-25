using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using InventorySystem;
using InventorySystem.Items;
using MEC;

namespace GhostPlugin.Custom.Items.Armor
{
    public class OverkillVest : CustomArmor
    {
        public override uint Id { get; set; } = 45;
        public override string Name { get; set; } = "Overkill Vest";
        public override string Description { get; set; } = "[Alt] 를 눌러 렌덤으로 총기 1개를 획득합니다!";
        public override float Weight { get; set; } = 3f;
        public override SpawnProperties SpawnProperties { get; set; }
        public override ItemType Type { get; set; } = ItemType.ArmorCombat;
        private readonly Dictionary<Player, CoroutineHandle> _altKeyCooldowns =
            new Dictionary<Player, CoroutineHandle>();
        private void GiveRandomWeapon(Player player)
        {
            ItemType[] weaponTypes = Enum.GetValues(typeof(ItemType))
                .Cast<ItemType>()
                .Where(x => x.IsWeapon())
                .ToArray();
            ItemType randomWeapon = weaponTypes[UnityEngine.Random.Range(0, weaponTypes.Length)];
            ItemBase newItem = player.Inventory.ServerAddItem(randomWeapon, ItemAddReason.AdminCommand);
            ushort serial = newItem.ItemSerial;
            player.Inventory.ServerSelectItem(serial);
            //player.AddItem(randomWeapon);
            player.ShowHint($"오버킬 능력으로 {randomWeapon} 을 받았습니다!", 5);

        }

        private IEnumerator<float> CooldownCorutine(Player player)
        {
            yield return Timing.WaitForSeconds(60f);
            _altKeyCooldowns.Remove(player);
            player.ShowHint("쿨다운이 끝났습니다.\n능력을 사용할수 있습니다.", 5);
        }

        private void OnTogglingNoClip(TogglingNoClipEventArgs ev)
        {
            if (Check(ev.Player.CurrentArmor))
            {
                if (!ev.IsAllowed)
                {
                    if (_altKeyCooldowns.ContainsKey(ev.Player))
                    {
                        ev.Player.ShowHint("쿨다운이 아직 진행중입니다..\n능력을 사용할수 없습니다..", 5);
                    }
                    else
                    {
                        CoroutineHandle handle = Timing.RunCoroutine(CooldownCorutine(ev.Player));
                        _altKeyCooldowns[ev.Player] = handle;
                        GiveRandomWeapon(ev.Player);
                    }
                }
            }
        }
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.TogglingNoClip += OnTogglingNoClip;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.TogglingNoClip -= OnTogglingNoClip;
            base.UnsubscribeEvents();
        }
    }
}