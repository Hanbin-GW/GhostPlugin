using AdminToys;
using Mirror;
using UnityEngine;

namespace GhostPlugin.Methods.ToyUtils
{
    public static class LightUtils
    {
        public static LightSourceToy SpawnLight(Vector3 position, Quaternion rotation, float intensity = 3f,
            float range = 10f, Color? color = null)
        {
            LightSourceToy lightSourceToy = null;
            foreach (GameObject value in NetworkClient.prefabs.Values)
            {
                if (value.TryGetComponent<LightSourceToy>(out var component))
                {
                    lightSourceToy = Object.Instantiate(component);
                    //textToy.OnSpawned(player.ReferenceHub, new ArraySegment<string>(new string[0]));
                    break;
                }
            }

            if (lightSourceToy != null)
            {
                lightSourceToy.NetworkLightIntensity = intensity;
                lightSourceToy.NetworkLightRange = range;
                lightSourceToy.NetworkLightColor = color ?? Color.green;
                lightSourceToy.LightColor = color ?? Color.green;
                lightSourceToy.NetworkLightType = LightType.Point;
                lightSourceToy.NetworkShadowType = LightShadows.None;
                lightSourceToy.Position = position;
                lightSourceToy.NetworkPosition = position;
            }

            return lightSourceToy;
        }
    }
}