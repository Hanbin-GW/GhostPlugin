using System.Collections.Generic;
using AdminToys;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using MEC;
using UnityEngine;

namespace GhostPlugin.Methods.ParticlePrimitives
{
    public static class OrbitPrimitiveMethods
    {
        // =========================
        // Enums
        // =========================
        public enum AnchorMode { Player, Head, HandRight, FrontBody }
        public enum MotionMode { Circle, Chaos }
        public enum PatternMode { Orbit, BackRing }

        // =========================
        // Data
        // =========================
        private class OrbitData
        {
            public List<Primitive> Prims = new();
            public List<Vector2> Seeds = new(); // Chaos용
            public CoroutineHandle Coroutine;
        }

        private class TrailData
        {
            public List<Primitive> Segments = new();
            public CoroutineHandle Coroutine;
            public int Index;
            public Vector3 LastPos;
            public bool HasLast;
            public Primitive Target;
        }

        private static readonly Dictionary<int, OrbitData> ActiveOrbit = new();
        private static readonly Dictionary<int, TrailData> ActiveTrail = new();

        // =========================
        // ORBIT / BACKRING
        // =========================
        public static void StartOrbit(
            Player player,
            PrimitiveType primitiveType = PrimitiveType.Sphere,
            int count = 8,
            Color? color = null,
            Vector3? scale = null,
            PrimitiveFlags flags = PrimitiveFlags.Visible,

            PatternMode pattern = PatternMode.Orbit,
            MotionMode motion = MotionMode.Circle,
            AnchorMode anchorMode = AnchorMode.HandRight,

            // Orbit / Chaos
            float radius = 0.25f,
            float speed = 2.0f,
            float chaosRadius = 0.35f,
            float chaosHeight = 0.15f,
            float chaosSpeed = 2.5f,

            // BackRing
            float ringRadius = 0.85f,
            float ringThickness = 0.035f
        )
        {
            if (player == null || !player.IsAlive)
                return;

            StopOrbit(player);

            var data = new OrbitData();
            Color col = color ?? Color.magenta;
            Vector3 scl = scale ?? Vector3.one * 0.08f;

            for (int i = 0; i < count; i++)
            {
                var prim = Primitive.Create(
                    primitiveType,
                    flags,
                    player.Position,
                    Vector3.zero,
                    scl,
                    spawn: true,
                    color: col
                );

                prim.Flags = flags;
                data.Prims.Add(prim);

                data.Seeds.Add(new Vector2(Random.Range(0f, 999f), Random.Range(0f, 999f)));
            }

            data.Coroutine = Timing.RunCoroutine(
                OrbitCoroutine(
                    player, data,
                    pattern, motion, anchorMode,
                    radius, speed,
                    chaosRadius, chaosHeight, chaosSpeed,
                    ringRadius, ringThickness
                )
            );

            ActiveOrbit[player.Id] = data;
        }

        public static void StopOrbit(Player player)
        {
            if (player == null)
                return;

            if (!ActiveOrbit.TryGetValue(player.Id, out var data))
                return;

            Timing.KillCoroutines(data.Coroutine);

            foreach (var p in data.Prims)
                p.Destroy();

            ActiveOrbit.Remove(player.Id);
        }

        private static IEnumerator<float> OrbitCoroutine(
            Player player,
            OrbitData data,
            PatternMode pattern,
            MotionMode motion,
            AnchorMode anchorMode,
            float radius,
            float speed,
            float chaosRadius,
            float chaosHeight,
            float chaosSpeed,
            float ringRadius,
            float ringThickness
        )
        {
            float angle = 0f;
            Vector3 smoothedAnchor = GetAnchor(player, anchorMode);

            while (player != null && player.IsAlive)
            {
                angle += Time.deltaTime * speed;

                Vector3 targetAnchor = GetAnchor(player, anchorMode);
                smoothedAnchor = Vector3.Lerp(smoothedAnchor, targetAnchor, 10f * Time.deltaTime);

                int n = data.Prims.Count;

                // ================= BACK RING =================
                if (pattern == PatternMode.BackRing)
                {
                    // Vector3 center =
                    //     player.Position
                    //     + Vector3.up * 1.25f
                    //     - player.Transform.forward * 0.45f;
                    
                    Vector3 center = GetAnchor(player, anchorMode) + player.Transform.forward * -0.9f;

                    Quaternion baseRot = Quaternion.LookRotation(-player.Transform.forward, Vector3.up);

                    for (int i = 0; i < n; i++)
                    {
                        float t = (Mathf.PI * 2f / n) * i + angle;

                        Vector3 local =
                            (baseRot * Vector3.right) * Mathf.Cos(t) * ringRadius +
                            (baseRot * Vector3.up)    * Mathf.Sin(t) * ringRadius;

                        Vector3 pos = center + local;

                        Vector3 tangent =
                            -(baseRot * Vector3.right) * Mathf.Sin(t) + 
                            (baseRot * Vector3.up)    * Mathf.Cos(t);

                        var prim = data.Prims[i];
                        prim.Scale = new Vector3(ringThickness, ringThickness, ringThickness * 6f);
                        prim.Rotation = Quaternion.LookRotation(tangent, baseRot * Vector3.forward);
                        prim.Position = pos;
                    }

                    yield return Timing.WaitForSeconds(0.02f);
                    continue;
                }

                // ================= ORBIT =================
                for (int i = 0; i < n; i++)
                {
                    Vector3 local;

                    if (motion == MotionMode.Circle)
                    {
                        float off = angle + (Mathf.PI * 2f / n) * i;
                        local = new Vector3(
                            Mathf.Cos(off) * radius,
                            0f,
                            Mathf.Sin(off) * radius
                        );
                    }
                    else // Chaos
                    {
                        float t = Time.time * chaosSpeed;
                        Vector2 seed = data.Seeds[i];

                        float nx = Mathf.PerlinNoise(seed.x, t) * 2f - 1f;
                        float nz = Mathf.PerlinNoise(seed.y, t + 100f) * 2f - 1f;
                        float ny = Mathf.PerlinNoise(seed.x + 200f, t + 50f) * 2f - 1f;

                        local = new Vector3(
                            nx * chaosRadius,
                            ny * chaosHeight,
                            nz * chaosRadius
                        );
                    }

                    data.Prims[i].Position = smoothedAnchor + local;
                }

                yield return Timing.WaitForSeconds(0.02f);
            }

            StopOrbit(player);
        }

        // =========================
        // TRAIL
        // =========================
        public static void StartTrail(
            Player player,
            AnchorMode anchorMode = AnchorMode.HandRight,
            int segmentCount = 12,
            float interval = 0.06f,
            float thickness = 0.03f,
            float maxSegmentLength = 0.9f,
            Color? color = null,
            PrimitiveFlags flags = PrimitiveFlags.Visible
        )
        {
            if (player == null || !player.IsAlive)
                return;

            StopTrail(player);

            var data = new TrailData();
            Color col = color ?? Color.magenta;

            for (int i = 0; i < segmentCount; i++)
            {
                var seg = Primitive.Create(
                    PrimitiveType.Cube,
                    flags,
                    player.Position,
                    Vector3.zero,
                    new Vector3(thickness, thickness, 0.01f),
                    spawn: true,
                    color: col
                );

                seg.Flags = flags;
                data.Segments.Add(seg);
            }

            data.Coroutine = Timing.RunCoroutine(
                TrailCoroutine(player, data, anchorMode, interval, thickness, maxSegmentLength)
            );

            ActiveTrail[player.Id] = data;
        }

        public static void StopTrail(Player player)
        {
            if (player == null)
                return;

            if (!ActiveTrail.TryGetValue(player.Id, out var data))
                return;

            Timing.KillCoroutines(data.Coroutine);

            foreach (var s in data.Segments)
                s.Destroy();

            ActiveTrail.Remove(player.Id);
        }
        
        public static void StartTrailOverOrbit(Player player, int targetIndex = 0,
            int segmentCount = 12, float interval = 0.06f, float thickness = 0.03f,
            float maxSegmentLength = 0.9f, Color? color = null, PrimitiveFlags flags = PrimitiveFlags.Visible)
        {
            if (player == null || !player.IsAlive)
                return;

            // Orbit이 켜져 있어야 대상 prim을 찾을 수 있음
            if (!ActiveOrbit.TryGetValue(player.Id, out var orbitData))
                return;

            if (orbitData.Prims.Count == 0)
                return;

            // 인덱스 안전 처리
            if (targetIndex < 0) targetIndex = 0;
            if (targetIndex >= orbitData.Prims.Count) targetIndex = orbitData.Prims.Count - 1;

            StopTrail(player);

            var data = new TrailData();
            data.Target = orbitData.Prims[targetIndex];

            Color col = color ?? Color.magenta;

            // 트레일 세그먼트 풀 생성
            for (int i = 0; i < segmentCount; i++)
            {
                var seg = Primitive.Create(
                    PrimitiveType.Cube,
                    flags,
                    data.Target.Position,
                    Vector3.zero,
                    new Vector3(thickness, thickness, 0.01f),
                    spawn: true,
                    color: col
                );

                seg.Flags = flags;
                data.Segments.Add(seg);
            }

            data.Coroutine = Timing.RunCoroutine(
                TrailCoroutine_TargetPrimitive(player, data, interval, thickness, maxSegmentLength)
            );

            ActiveTrail[player.Id] = data;
        }


        private static IEnumerator<float> TrailCoroutine(
            Player player,
            TrailData data,
            AnchorMode anchorMode,
            float interval,
            float thickness,
            float maxSegmentLength
        )
        {
            while (player != null && player.IsAlive)
            {
                // change
                Vector3 pos = GetAnchor(player, anchorMode) + player.Transform.forward * -0.9f;

                if (!data.HasLast)
                {
                    data.LastPos = pos;
                    data.HasLast = true;
                    yield return Timing.WaitForSeconds(interval);
                    continue;
                }

                Vector3 a = data.LastPos;
                Vector3 b = pos;
                float dist = Vector3.Distance(a, b);

                if (dist < 0.02f)
                {
                    yield return Timing.WaitForSeconds(interval);
                    continue;
                }

                if (dist > maxSegmentLength)
                {
                    Vector3 dirClamp = (b - a).normalized;
                    b = a + dirClamp * maxSegmentLength;
                    dist = maxSegmentLength;
                }

                var seg = data.Segments[data.Index];
                data.Index = (data.Index + 1) % data.Segments.Count;

                Vector3 mid = (a + b) * 0.5f;
                Vector3 dir = (b - a);

                seg.Rotation = Quaternion.LookRotation(dir);
                seg.Scale = new Vector3(thickness, thickness, dist);
                seg.Position = mid;

                data.LastPos = pos;

                yield return Timing.WaitForSeconds(interval);
            }

            StopTrail(player);
        }

        
        private static IEnumerator<float> TrailCoroutine_TargetPrimitive(
            Player player,
            TrailData data,
            float interval,
            float thickness,
            float maxSegmentLength
        )
        {
            while (player != null && player.IsAlive && data.Target != null)
            {
                Vector3 pos = data.Target.Position; // ✅ 플레이어가 아니라 prim 위치

                if (!data.HasLast)
                {
                    data.LastPos = pos;
                    data.HasLast = true;
                    yield return Timing.WaitForSeconds(interval);
                    continue;
                }

                Vector3 a = data.LastPos;
                Vector3 b = pos;
                float dist = Vector3.Distance(a, b);

                if (dist < 0.02f)
                {
                    yield return Timing.WaitForSeconds(interval);
                    continue;
                }

                if (dist > maxSegmentLength)
                {
                    Vector3 dirClamp = (b - a).normalized;
                    b = a + dirClamp * maxSegmentLength;
                    dist = maxSegmentLength;
                }

                var seg = data.Segments[data.Index];
                data.Index = (data.Index + 1) % data.Segments.Count;

                Vector3 mid = (a + b) * 0.5f;
                Vector3 dir = (b - a);

                seg.Rotation = Quaternion.LookRotation(dir);
                seg.Scale = new Vector3(thickness, thickness, dist);
                seg.Position = mid;

                data.LastPos = pos;

                yield return Timing.WaitForSeconds(interval);
            }

            StopTrail(player);
        }

        
        // =========================
        // Anchor
        // =========================
        private static Vector3 GetAnchor(Player player, AnchorMode mode)
        {
            switch (mode)
            {
                case AnchorMode.Head:
                {
                    var cam = player.CameraTransform;
                    return cam.position + cam.up * 0.1f;
                }

                case AnchorMode.HandRight:
                {
                    var cam = player.CameraTransform;
                    return cam.position
                           + cam.forward * 0.45f
                           + cam.right * 0.15f
                           + cam.up * -0.40f;
                }

                case AnchorMode.FrontBody:
                {
                    return player.Position
                           + player.Transform.forward * 0.45f
                           + Vector3.up * 1.05f;
                }

                case AnchorMode.Player:
                default:
                    return player.Position + Vector3.up * 1.2f;
            }
        }
    }
}
