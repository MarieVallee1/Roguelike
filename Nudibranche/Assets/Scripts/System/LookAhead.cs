using UnityEngine;

namespace System
{
    public class LookAhead : MonoBehaviour
    {
        [Tooltip("This value divide the x & y axis lenght of the lookahead position")]
        [SerializeField] private float distanceModifier;
        private Transform _cursorTr;
        
        // Update is called once per frame

        private void Start()
        {
            _cursorTr = TargetCursor.instance.transform;
        }

        void Update()
        {
            var position = _cursorTr.position;
            transform.position = new Vector2(position.x / distanceModifier, position.y / distanceModifier);
        }
    }
}
