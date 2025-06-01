using System;
using Exiled.API.Features;
using Mirror;
using UnityEngine;
namespace GhostPlugin.Methods.TextToy
{
    public class SpawnTextToy
    {
        public static void SpawnText(Player player, Vector3 position, string format)
        {
            AdminToys.TextToy textToy = null;
            foreach (GameObject value in NetworkClient.prefabs.Values)
            {
                if (value.TryGetComponent<AdminToys.TextToy>(out var component))
                {
                    textToy = UnityEngine.Object.Instantiate(component);
                    textToy.OnSpawned(player.ReferenceHub, new ArraySegment<string>(new string[0]));
                    break;
                }
            }

            if (textToy != null)
            {
                textToy.TextFormat = format;
                textToy.Network_textFormat = format;
                textToy.Position = position;
                textToy.NetworkPosition = position;
            }
        }
    }
}