using System;
using Exiled.API.Features;
using Mirror;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GhostPlugin.Methods.TextToy
{
    public class SpawnTextToy
    {
        public static void SpawnText(Player player, Vector3 position, string format, float duration)
        {
            AdminToys.TextToy textToy = null;
            foreach (GameObject value in NetworkClient.prefabs.Values)
            {
                if (value.TryGetComponent<AdminToys.TextToy>(out var component))
                {
                    textToy = Object.Instantiate(component);
                    //textToy.OnSpawned(player.ReferenceHub, new ArraySegment<string>(new string[0]));
                    break;
                }
            }

            if (textToy != null)
            {
                textToy.TextFormat = format;
                textToy.Network_textFormat = format;
                textToy.Position = position;
                textToy.NetworkPosition = position;
                textToy.enabled = true;
                //textToy.DisplaySize = new Vector2(100, 50);
                //textToy.Network_displaySize = new Vector2(100, 50);
                NetworkServer.Spawn(textToy.gameObject);
                textToy.OnSpawned(player.ReferenceHub, new ArraySegment<string>(Array.Empty<string>()));
                Object.Destroy(textToy.gameObject, duration);
            }
        }
    }
}