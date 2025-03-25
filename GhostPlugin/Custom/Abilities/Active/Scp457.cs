using System.Collections.Generic;
using System.Linq;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using MEC;
using UnityEngine;

namespace GhostPlugin.Custom.Abilities.Active
{
    public class Scp457 : ActiveAbility
    {
        public override string Name { get; set; } = "Scp457";
        public override string Description { get; set; } = "주변의 있는 적을 다 불태웁니다";
        public override float Duration { get; set; } = 20f;
        public override float Cooldown { get; set; } = 60f;
        
        private CoroutineHandle _burnEffectCoroutine;
        private float burnDuration = 20f;
        private float burnRadius = 5f;
        private float burnInterval = 0.5f;
        private float damageAmount = 5f;
        
        protected override void AbilityUsed(Player player)
        {
            if (_burnEffectCoroutine.IsRunning)
            {
                player.ShowHint("화염 효과가 이미 활성화 중입니다.", 3);
                return;
            }

            player.ShowHint("화염 효과 활성화! 근처 적에게 화염 효과 적용!", 5);
            Log.Info($"{player.Nickname}가 화염 효과를 활성화했습니다.");

            _burnEffectCoroutine = Timing.RunCoroutine(BurnEffectCoroutine(player));
        }
        protected override void AbilityEnded(Player player)
        {
            if (_burnEffectCoroutine.IsRunning)
            {
                Timing.KillCoroutines(_burnEffectCoroutine);
                player.ShowHint("화염 효과가 비활성화되었습니다.", 5);
                Log.Info($"{player.Nickname}의 화염 효과가 비활성화되었습니다.");
            }
            base.AbilityRemoved(player);
        }
        private IEnumerator<float> BurnEffectCoroutine(Player attacker)
        {
            float elapsedTime = 0f;

            while (elapsedTime < burnDuration)
            {
                Log.Debug($"화염 효과 적용 중... 남은 시간: {burnDuration - elapsedTime}초");
                ApplyFireEffectNearby(attacker);
                elapsedTime += burnInterval;
                yield return Timing.WaitForSeconds(burnInterval);
            }

            attacker.ShowHint("화염 효과가 종료되었습니다.", 5);
            Log.Debug($"{attacker.Nickname}의 화염 효과가 종료되었습니다.");
        }

        private void ApplyFireEffectNearby(Player attacker)
        {
            foreach (Player target in Player.List.Where(p => 
                         p != attacker && 
                         p.IsAlive && 
                         Vector3.Distance(attacker.Position, p.Position) <= burnRadius))
            {
                target.Hurt(damageAmount, DamageType.A7, attacker.Nickname);
                target.ShowHint("화상 데미지를 입었습니다!", 3);
                target.EnableEffect<Burned>(duration: 5);
                attacker.ShowHitMarker();
                Log.Debug($"{attacker.Nickname}가 {target.Nickname}에게 화염 효과를 적용했습니다.");
            }
        }
    }
}