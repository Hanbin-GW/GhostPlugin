using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;

namespace GhostPlugin.Custom.Items.Keycard
{
    [CustomItem(ItemType.KeycardChaosInsurgency)]
    public class HackingDevice : CustomKeycard
    {
        private readonly Dictionary<Player, CoroutineHandle> altKeyCooldowns = new Dictionary<Player, CoroutineHandle>();
        public override uint Id { get; set; } = 1;
        public override string Name { get; set; } = "<color=#1aff00>Hacking Device</color>";
        public override string Description { get; set; } = "Lock the door for 7 seconds when interacting with the door. (10 seconds of cool down)";
        public override float Weight { get; set; } = 1f;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
            RoomSpawnPoints = new List<RoomSpawnPoint>()
            {
                new RoomSpawnPoint()
                {
                    Room = RoomType.Lcz330,
                    Chance = 50,
                }
            }
        };
        public override KeycardPermissions Permissions { get; set; } = KeycardPermissions.None;
        public override ItemType Type { get; set; } = ItemType.KeycardChaosInsurgency;
        private void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                if (altKeyCooldowns.ContainsKey(ev.Player))
                {
                    ev.Player.ShowHint("쿨다운이 아직 진행중입니다..\n능력을 사용할수 없습니다..",5);
                }
                else
                {
                    LockDoor(ev.Door);
                    CoroutineHandle handle = Timing.RunCoroutine(CooldownCorutine(ev.Player));
                    altKeyCooldowns[ev.Player] = handle;
                    ev.Player.ShowHint("문이 잠겼습니다.. 10초 쿨다운이 시작됩니다..");
                }
            }
        }
        private void LockDoor(Door door)
        { 
            door.Lock(7, DoorLockType.AdminCommand);
        }
        private IEnumerator<float> CooldownCorutine(Player player)
        {
            yield return Timing.WaitForSeconds(10f);
            altKeyCooldowns.Remove(player);
            player.ShowHint("쿨다운이 끝났습니다.\n능력을 사용할수 있습니다.",5);
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
            base.UnsubscribeEvents();
        }
    }
}