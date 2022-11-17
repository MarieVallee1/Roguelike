using System.Collections.Generic;
using UnityEngine;

namespace System
{
    public class PoolingSystem: MonoBehaviour
    {
        public static PoolingSystem instance;

        private Dictionary<string, List<GameObject>> _poolDictionary = new Dictionary<string, List<GameObject>>();
    
        [SerializeField] private List<PoolData> poolData;

        private void Awake()
        {
            if (instance == null) instance = this;
        }

        private void Start()
        {
            InitialInstantiation();
        }

        void InitialInstantiation()
        {
            for (int j = 0; j < poolData.Count; j++)
            {
                _poolDictionary.Add(poolData[j].Prefab.name, new List<GameObject>());
            
                for (int i = 0; i < poolData[j].NumberOfObjects; i++)
                {
                    GameObject obj = Instantiate(poolData[j].Prefab);
                    obj.SetActive(false);
                    _poolDictionary[poolData[j].Prefab.name].Add(obj);
                }
            }
        }

        public GameObject GetObject(string objectName)
        {
            if (_poolDictionary.ContainsKey(objectName))
            {
                for (int i = 0; i < _poolDictionary[objectName].Count; i++)
                {
                    if (!_poolDictionary[objectName][i].activeInHierarchy)
                    {
                        return _poolDictionary[objectName][i];
                    }
                }
            }
            return null;
        }
    }


//It allows me to have a custom list of elements in the inspector
    [Serializable]
    public class PoolData
    {
        [TextArea]
        public string description;
        [SerializeField]
        private GameObject prefab;
        [SerializeField]
        private int numberOfObjects;
        
        public GameObject Prefab => prefab; //Get without Set 
        public int NumberOfObjects => numberOfObjects; //Get without Set 

    }
}