using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class PoolManager : MonoBehaviour
    {
        [System.Serializable]
        public class Pool
        {
            public PoolType objectType;
            public GameObject prefab;
            public int size;
        }

        #region Singleton
        public static PoolManager Instance { get; private set; }
        #endregion

        [SerializeField] private List<Pool> _pools;
        private Dictionary<PoolType, Queue<GameObject>> poolDictionary;

        void Awake()
        {
            Instance = this;
            Init();
        }

        private void Init()
        {
            poolDictionary = new Dictionary<PoolType, Queue<GameObject>>();

            foreach (Pool pool in _pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab, transform);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                poolDictionary.Add(pool.objectType, objectPool);
            }
        }

        public GameObject SpawnFromPool(PoolType poolType, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(poolType))
            {
                Debug.LogWarning("Object type not found in the pool: " + poolType);
                return null;
            }

            if (poolDictionary[poolType].Peek().gameObject.activeSelf == true)
            {
                GameObject obj = Instantiate(_pools[((int)poolType)].prefab);
                poolDictionary[poolType].Enqueue(obj);
                obj.SetActive(true);
                return obj;
            }

            GameObject objectToSpawn = poolDictionary[poolType].Dequeue();
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            poolDictionary[poolType].Enqueue(objectToSpawn);

            return objectToSpawn;
        }
    }
}
