using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Features;
using GhostPlugin.API;
using GhostPlugin.Custom.Abilities.Passive;
using PlayerRoles;
using Player = Exiled.API.Features.Player;

namespace GhostPlugin.Custom.Roles.Foundation
{
    // Token: 0x02000027 RID: 39
    [CustomRole(RoleTypeId.FacilityGuard)]
    public class LuckyGuard : CustomRole, ICustomRole
    {
        // Token: 0x170000A2 RID: 162
        // (get) Token: 0x060001F5 RID: 501 RVA: 0x0000B623 File Offset: 0x00009823
        // (set) Token: 0x060001F6 RID: 502 RVA: 0x0000B62B File Offset: 0x0000982B
        public override uint Id { get; set; } = 14;

        // Token: 0x170000A3 RID: 163
        // (get) Token: 0x060001F7 RID: 503 RVA: 0x0000B634 File Offset: 0x00009834
        // (set) Token: 0x060001F8 RID: 504 RVA: 0x0000B63C File Offset: 0x0000983C
        public override int MaxHealth { get; set; } = 100;

        // Token: 0x170000A4 RID: 164
        // (get) Token: 0x060001F9 RID: 505 RVA: 0x0000B645 File Offset: 0x00009845
        // (set) Token: 0x060001FA RID: 506 RVA: 0x0000B64D File Offset: 0x0000984D
        public override string Name { get; set; } = "Lucky Guard";

        // Token: 0x170000A5 RID: 165
        // (get) Token: 0x060001FB RID: 507 RVA: 0x0000B656 File Offset: 0x00009856
        // (set) Token: 0x060001FC RID: 508 RVA: 0x0000B65E File Offset: 0x0000985E
        public override string Description { get; set; } = "행운이 당신을 도와줍니다..!";

        // Token: 0x170000A6 RID: 166
        // (get) Token: 0x060001FD RID: 509 RVA: 0x0000B667 File Offset: 0x00009867
        // (set) Token: 0x060001FE RID: 510 RVA: 0x0000B66F File Offset: 0x0000986F
        public override string CustomInfo { get; set; } = "Lucky Guard";

        // Token: 0x170000A7 RID: 167
        // (get) Token: 0x060001FF RID: 511 RVA: 0x0000B678 File Offset: 0x00009878
        // (set) Token: 0x06000200 RID: 512 RVA: 0x0000B680 File Offset: 0x00009880
        public override RoleTypeId Role { get; set; } = RoleTypeId.FacilityGuard;
        public override bool DisplayCustomItemMessages { get; set; } = false;
        // Token: 0x170000A8 RID: 168
        // (get) Token: 0x06000201 RID: 513 RVA: 0x0000B689 File Offset: 0x00009889
        // (set) Token: 0x06000202 RID: 514 RVA: 0x0000B691 File Offset: 0x00009891
        public override List<string> Inventory { get; set; } = new List<string>
        {
            ItemType.GunFSP9.ToString(),
            ItemType.KeycardGuard.ToString(),
            ItemType.Flashlight.ToString(),
            ItemType.Radio.ToString(),
            ItemType.ArmorLight.ToString(),
            ItemType.GrenadeFlash.ToString()
        };

        // Token: 0x06000203 RID: 515 RVA: 0x0000B69C File Offset: 0x0000989C
        private void OnHurting(HurtingEventArgs ev)
        {
            if (this.Check(ev.Player))
            {
                if (ev.Player.Health <= 15f && this.IsLucky)
                {
                    ev.Player.RandomTeleport(typeof(Room));
                    ev.Player.ShowHint("어딘가로 이동되었습니다..!", 3f);
                    this.IsLucky = false;
                    return;
                }
                ev.Player.ShowHint("<color=red>항상 행운이 따르지는 않아...\n(이동 이미 사용)</color>", 3f);
            }
        }

        // Token: 0x170000A9 RID: 169
        // (get) Token: 0x06000204 RID: 516 RVA: 0x0000B718 File Offset: 0x00009918
        // (set) Token: 0x06000205 RID: 517 RVA: 0x0000B720 File Offset: 0x00009920
        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>
        {
            new HealOnKill
            {
                Name = "Hill On Kill",
                Description = "적을 처치시 Hp 를 회복합니다."
            }
        };

        // Token: 0x170000AA RID: 170
        // (get) Token: 0x06000206 RID: 518 RVA: 0x0000B729 File Offset: 0x00009929
        // (set) Token: 0x06000207 RID: 519 RVA: 0x0000B731 File Offset: 0x00009931
        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>
        {
            { AmmoType.Nato9 ,60},
            { AmmoType.Nato556 ,20},
        };

        // Token: 0x06000208 RID: 520 RVA: 0x0000B24C File Offset: 0x0000944C
        protected override void RoleAdded(Player player)
        {
            base.RoleAdded(player);
        }

        // Token: 0x06000209 RID: 521 RVA: 0x0000B73A File Offset: 0x0000993A
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting += new CustomEventHandler<HurtingEventArgs>(this.OnHurting);
            base.SubscribeEvents();
        }

        // Token: 0x0600020A RID: 522 RVA: 0x0000B75D File Offset: 0x0000995D
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting -= new CustomEventHandler<HurtingEventArgs>(this.OnHurting);
            base.UnsubscribeEvents();
        }

        // Token: 0x170000AB RID: 171
        // (get) Token: 0x0600020B RID: 523 RVA: 0x0000B780 File Offset: 0x00009980
        // (set) Token: 0x0600020C RID: 524 RVA: 0x0000B788 File Offset: 0x00009988
        public StartTeam StartTeam { get; set; } = StartTeam.Guard;

        // Token: 0x170000AC RID: 172
        // (get) Token: 0x0600020D RID: 525 RVA: 0x0000B791 File Offset: 0x00009991
        // (set) Token: 0x0600020E RID: 526 RVA: 0x0000B799 File Offset: 0x00009999
        public int Chance { get; set; } = 100;

        // Token: 0x040000D6 RID: 214
        public bool IsLucky = true;
    }
}