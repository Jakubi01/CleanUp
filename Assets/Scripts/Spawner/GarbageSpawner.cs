using System;
using System.Collections.Generic;
using Assets.Scripts.Manager;
using Item.Garbage;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawner
{
    public class GarbageSpawner : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField] private List<GarbageBase> garbagePrefabs;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private float radius;
        [SerializeField] private float maxHeight;
        [SerializeField] private float rayStartHeight;

        private List<GarbageBase> _spawnedGarbageCache = new();
        private const float SpawnPositionAdjustValue = 0.5f;

        private void Start()
        {
            SpawnGarbage();
        }
        
        private void OnEnable()
        {
            _spawnedGarbageCache.Clear();
        }

        private void OnDisable()
        {
            _spawnedGarbageCache.Clear();
        }

        private void SpawnGarbage()
        {
            if (garbagePrefabs.Count <= 0) return;

            foreach (var t in garbagePrefabs)
            {
                var offset = Random.insideUnitSphere * radius;
                var origin = transform.position + new Vector3(offset.x, 0f, offset.y);
                origin.y = rayStartHeight;

                if (Physics.Raycast(origin, Vector3.down, out var hit, rayStartHeight + 2f, groundMask)
                    && hit.point.y <= maxHeight)
                {
                    var upPosition = new Vector3(hit.point.x, hit.point.y + SpawnPositionAdjustValue, hit.point.z);
                    var spawned = Instantiate(t, hit.point, Quaternion.identity);
                    spawned.transform.SetParent(transform);
                    spawned.transform.localScale = Vector3.one * 2f;
                    _spawnedGarbageCache.Add(spawned);
                }
            }

            GameManager.Instance.Register(_spawnedGarbageCache);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var cubeCenter = transform.position + Vector3.up * (maxHeight * 0.5f);
            Gizmos.DrawWireCube(cubeCenter, new Vector3(radius * 2f, maxHeight, radius * 2f));
            
            Gizmos.color = Color.green;
            var startPos = transform.position + Vector3.up * rayStartHeight;
            Gizmos.DrawRay(startPos, Vector3.down * (rayStartHeight + 2f));
        }
    }
}