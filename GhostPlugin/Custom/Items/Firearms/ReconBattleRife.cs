using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Firearms.Attachments;
using LabApi.Events.Arguments.PlayerEvents;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunE11SR)]
    public class ReconBattleRife : CustomWeapon
    {
        public override uint Id { get; set; } = 3;
        public override string Name { get; set; } = "<color=#5c7aff>FTAC Recon</color>";
        public override string Description { get; set; } = "[T] 키를 눌를시 피격당한 적을 추적할수있습니다";
        public override float Weight { get; set; } = 12.5f;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 2,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>()
            {
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.InsideGr18,
                    Chance = 25,
                },
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.InsideSurfaceNuke,
                    Chance = 25,
                },
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.InsideHidUpper,
                    Chance = 25,
                }
            },
            RoomSpawnPoints = new List<RoomSpawnPoint>()
            {
                new RoomSpawnPoint()
                {
                    Room = RoomType.HczNuke,
                    Chance = 25,
                }
            }
        };
        public override float Damage { get; set; } = 45;
        public override ItemType Type { get; set; } = ItemType.GunE11SR;
        public override byte ClipSize { get; set; } = 10;
        public string lastHitRoomName = string.Empty;
        private Player lastHitPlayer;

        protected override void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Player == null || ev.Attacker == null || ev.Attacker.CurrentItem == null)
            {
                return;
            }
            
            if (Check(ev.Attacker.CurrentItem))
            {
                lastHitRoomName = ev.Player.CurrentRoom.Name;
                lastHitPlayer = ev.Player;
                lastHitPlayer.ShowHint($"<color=red>⚠ 알림 ⚠</color>\n당신은 {ev.Attacker} 한테 추적을 받고있습니다...!", 5);
            }
            
            float recoilX = Random.Range(-15f, 15f); 
            float recoilY = Random.Range(16f, 20f); 

            Vector3 currentRotation = ev.Player.CameraTransform.eulerAngles;

            currentRotation.x = Mathf.Clamp(currentRotation.x - recoilY, -90f, 90f);
            currentRotation.y += recoilX;

            ev.Player.CameraTransform.eulerAngles = currentRotation;
        }

        protected override void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (ev.Player == null || ev.Player.CurrentItem == null)
            {
                return;
            }
            
            if (Check(ev.Item))
            {
                if (ev.IsThrown)
                {
                    ev.IsThrown = false;
                    // 가장 최근에 공격당한 플레이어의 현재 위치한 방의 이름을 가져옵니다.
                    //var roomName = lastHitPlayer.CurrentRoom.Name;
                    if (lastHitPlayer == null)
                    {
                        ev.Player.ShowHint("추적 대상자를 찾을 수 없습니다.", 5);
                        return; // lastHitPlayer가 null이면 여기서 처리를 중단합니다.
                    }

                    if (lastHitPlayer.IsDead)
                    {
                        ev.Player.ShowHint("추적대상자 생명신호 감지 안됨..", 5);
                    }

                    if (lastHitPlayer.CurrentRoom == null)
                    {
                        ev.Player.ShowHint("추적대상자의 현재 위치를 확인할 수 없습니다.", 5);
                    }
                    else
                    {
                        ev.Player.ShowHint($"추적 대상자는 현재 {lastHitPlayer.CurrentRoom.Name}에 있습니다.", 5);
                    }
                }
            }
        }

        protected override void OnShot(ShotEventArgs ev)
        {
            ev.Firearm.DamageFalloffDistance = 200f;
            base.OnShot(ev);
        }

        private void OnDying(DyingEventArgs ev)
        {
            if (ev == null || ev.Attacker == null || ev.Attacker.CurrentItem == null)
            {
                return;
            }

            if (Check(ev.Attacker.CurrentItem))
            {
                if (lastHitPlayer.IsDead)
                {
                    lastHitPlayer = null;
                }
            }

        }
        
        private void OnChangingAttachments(ChangingAttachmentsEventArgs ev)
        {
            if (!Check(ev.Player.CurrentItem))
                return;
            string selectedAttachment = "기본 탄창"; // 디버깅용 변수
            foreach (var attachment in ev.Firearm.Attachments)
            {
                switch (attachment.Name)
                {
                    case AttachmentName.DrumMagFMJ:
                        ClipSize = 15;
                        selectedAttachment = "DrumMagFMJ";
                        break;
                    case AttachmentName.LowcapMagAP:
                        ClipSize = 5;
                        selectedAttachment = "LowcapMagAP";
                        break;
                }
            }
            Log.Send($"{selectedAttachment} 적용됨: 탄창 크기 {ClipSize}", LogLevel.Debug, ConsoleColor.DarkRed);
        }

        protected override void SubscribeEvents()
        {
            //Exiled.Events.Handlers.Item.ChangingAttachments += OnChangingAttachment;
            Exiled.Events.Handlers.Player.Dying += OnDying;
            base.SubscribeEvents();
        }
        protected override void UnsubscribeEvents()
        {
            //Exiled.Events.Handlers.Item.ChangingAttachments -= OnChangingAttachment;
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            base.UnsubscribeEvents();
        }
    }
}