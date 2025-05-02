using System;
using Exiled.API.Features;
using ProjectMER.Features;
using ProjectMER.Features.Objects;
using UnityEngine;

namespace GhostPlugin.Methods.MER
{
    public class ObjectManager
    {
        public static SchematicObject SpawnObject(String schematicName, Vector3 spawnPos, Vector3 rotation)
        {
            SchematicObject schematicObject = ObjectSpawner.SpawnSchematic(schematicName, spawnPos, rotation);
            if (schematicObject != null)
            {
                Log.Debug($"Schematic '{schematicName}' has been successfully spawned.");
                GameObject schematicGameObject = schematicObject.gameObject;
                Rigidbody rigidbody = schematicGameObject.AddComponent<Rigidbody>();
                rigidbody.useGravity = true;
            }
            return schematicObject;
        }

        public static void RemoveObject(SchematicObject schematicObject)
        {
            if (schematicObject != null && schematicObject.gameObject != null)
            {
                UnityEngine.Object.Destroy(schematicObject.gameObject);
            }
        }
    }
}