using UnityEngine;

namespace System
{
    public class PositionGetter : MonoBehaviour
    {
        [Tooltip("0 = Hub / 1 = Shop / 2 = Boss")]
        public int positionRef;
        void Start()
        {
            switch (positionRef)
            {
                case 0 : GameManager.instance.hubPos = transform.position;
                    break;
                case 1 : GameManager.instance.shopPos = transform.position;
                    break;
                case 2 : GameManager.instance.bossPos = transform.position;
                    break;
                default: print("No Position registered");
                    break;
            }
        }
    }
}
