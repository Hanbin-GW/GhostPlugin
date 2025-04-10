using Exiled.API.Features;
using MapGeneration.RoomConnectors;
using Mirror;
using UnityEngine;

namespace GhostPlugin.Methods.Objects
{
    public class SpawnDoorType
    {
        public void SpawnDoor(Vector3 position, Quaternion rotation)
        {
            if (RoomConnectorDistributorSettings.TryGetTemplate(SpawnableRoomConnectorType.HczBulkDoor, out var connector))
            {
                GameObject prefab = connector.gameObject;

                GameObject instance = Object.Instantiate(prefab, position, rotation);
                NetworkServer.Spawn(instance); // Mirror 네트워크에 등록
            }
            else
            {
                Log.Error("HCZ_BULK_DOOR 프리팹을 찾을 수 없습니다!");
            }
            //RoomConnectorDistributorSettings.TryGetTemplate(SpawnableRoomConnectorType.HczBulkDoor);
        }
    }
}