using System;
using UnityEngine;

namespace Minimap
{
    public class Minimap : MonoBehaviour
    {
        public static Minimap Instance;
        [SerializeField] private Transform minimapCam;

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            Instance = this;
        }

        public void MinimapUpdate(Transform room)
        {
            minimapCam.position = room.position;
        }
    }
}
