using UnityEngine;

namespace Objects
{
    public class Reward : MonoBehaviour
    {
        public Reward()
        {
            Debug.Log("Constructor Called");
        }

        public virtual int GetPrice()
        {
            return 0;
        }
    }
}
