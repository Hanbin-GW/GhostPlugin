using System;
using AdminToys;
using Exiled.API.Features;
using ProjectMER.Features;
using ProjectMER.Features.Objects;
using UnityEngine;

namespace GhostPlugin.Methods.MER
{
    public class ObjectManager
    {
        public static SchematicObject SpawnObject(String schematicName, Vector3 spawnPos, Quaternion quaternion)
        {
            SchematicObject schematicObject = ObjectSpawner.SpawnSchematic(schematicName, spawnPos, quaternion);
            if (schematicObject != null)
            {
                Log.Debug($"Schematic '{schematicName}' has been successfully spawned.");
                GameObject schematicGameObject = schematicObject.gameObject;
                Rigidbody rigidbody = schematicGameObject.AddComponent<Rigidbody>();
                rigidbody.useGravity = true;
                schematicGameObject.transform.rotation = quaternion;
                rigidbody.rotation = quaternion;
            }
            return schematicObject;
        }
        public static void RecolorAllPrimitives(SchematicObject schem, Color color)
        {
            foreach (var toy in schem.GetComponentsInChildren<PrimitiveObjectToy>(true))
            {
                toy.NetworkMaterialColor = color; 
            }
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