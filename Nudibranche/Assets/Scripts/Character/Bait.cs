using UnityEngine;

namespace Character
{
    public class Bait : MonoBehaviour
    {
        private float _countdown;
        [SerializeField] private float baitDuration;

        private void OnEnable()
        {
            _countdown = 0;
        }

        private void Update()
        {
            if (_countdown < baitDuration) _countdown += Time.deltaTime;
            else gameObject.SetActive(false);
        }
    }
}
