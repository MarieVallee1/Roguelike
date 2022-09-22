using System.Collections.Generic;
using UnityEngine;

namespace Objects
{
    public class ObjectPooling : MonoBehaviour
    {
        public static ObjectPooling instance;
        private List<GameObject> _pooledObjects = new List<GameObject>();
        private int _amountToPool = 20;

        [SerializeField] private GameObject bulletPrefab;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < _amountToPool; i++)
            {
                GameObject obj = Instantiate(bulletPrefab);
                obj.SetActive(false);
                _pooledObjects.Add(obj);
            }
        }

        public GameObject GetPooledObject()
        {
            for (int i = 0; i < _pooledObjects.Count; i++)
            {
                if (!_pooledObjects[i].activeInHierarchy)
                {
                    return _pooledObjects[i];
                }
            }

            return null;
        }
    }
}
