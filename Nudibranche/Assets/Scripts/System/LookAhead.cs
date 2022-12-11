using Character;
using UnityEngine;

namespace System
{
    public class LookAhead : MonoBehaviour
    {
        [Header("LookAhead distance")]
        [Tooltip("This value divide the x & y axis lenght of the lookahead position")]
        [SerializeField] private float mouseDistanceModifier;
        [Tooltip("This value divide the x & y axis lenght of the lookahead position")]
        [SerializeField] private float gamepadDistanceModifier;
        
        [Header("LookAhead Boundaries")]
        [SerializeField] private float rangeX;
        [SerializeField] private float rangeY;
        private Transform _cursorTr;
        
        // Update is called once per frame

        private void Start()
        {
            _cursorTr = TargetCursor.instance.transform;
        }
        void Update()
        {
            if(PlayerController.Instance.gamepadOn) GamepadLookAhead();
            else MouseLookAhead();
        }

        
        private void MouseLookAhead()
        {
            var position = _cursorTr.position;
            var playerPos = PlayerController.Instance.characterPos;
            
            Vector2 lookAheadPos = new Vector2((position.x - playerPos.x) / mouseDistanceModifier, (position.y - playerPos.y) / mouseDistanceModifier);
            
            //Set the boundaries of the lookahead.
            float x = Mathf.Clamp(lookAheadPos.x, -rangeX, rangeX);
            float y = Mathf.Clamp(lookAheadPos.y, -rangeY, rangeY);

            transform.localPosition = new Vector2(x,y);
        }
        private void GamepadLookAhead()
        {
            transform.localPosition = PlayerController.Instance.aim * gamepadDistanceModifier;
        }
    }
}
