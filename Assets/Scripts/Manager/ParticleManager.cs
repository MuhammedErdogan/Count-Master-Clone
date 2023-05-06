using System.Collections.Generic;
using UnityEngine;


namespace Manager
{
    public class ParticleManager : MonoBehaviour
    {
        [System.Serializable]
        public class PooledParticle
        {
            public PoolType objectType;
            public GameObject prefab;
            public int size;
        }

        #region Singleton
        public static ParticleManager Instance { get; private set; }
        #endregion

        #region Variables
        [SerializeField] private List<PooledParticle> _pools;
        private Dictionary<PoolType, Queue<GameObject>> poolDictionary;
        #endregion

        private void Awake()
        {
            Instance = this;
        }

        private void Init()
        {
            poolDictionary = new Dictionary<PoolType, Queue<GameObject>>();
            foreach (PooledParticle pool in _pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();
                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                poolDictionary.Add(pool.objectType, objectPool);
            }
        }
    }
}