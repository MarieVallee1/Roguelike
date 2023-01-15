using System;
using UI;
using UnityEngine;

namespace GenPro
{
    public class EntranceTrigger : MonoBehaviour
    {
        public RoomManager linkedRoom;
        [SerializeField] private Side side;
        [HideInInspector] public Collider2D trigger;

        private const float Offset = 0.27f;

        private void Awake()
        {
            trigger = GetComponent<Collider2D>();
            switch (side)
            {
                case Side.Up:
                    trigger.offset = new Vector2(0,-Offset);
                    transform.localScale = new Vector3(1.8f, 4);
                    break;
                case Side.Right:
                    trigger.offset = new Vector2(-Offset,0);
                    transform.localScale = new Vector3(4, 1.8f);
                    break;
                case Side.Down:
                    trigger.offset = new Vector2(0,Offset);
                    transform.localScale = new Vector3(1.8f, 4);
                    break;
                case Side.Left:
                    trigger.offset = new Vector2(Offset,0);
                    transform.localScale = new Vector3(4, 1.8f);
                    break;
            }
        }

        private enum Side
        {
            Up,
            Right,
            Down,
            Left
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer != 6) return;
            if (!linkedRoom.activated) linkedRoom.Activate();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer != 6) return;
            if (IsOutside(other.transform.position))
            {
                linkedRoom.ResetTriggers();
                linkedRoom.Deactivate();
            }
            else
            {
                linkedRoom.SetTriggers();
                GameManager.instance.currentRoom = linkedRoom;
                linkedRoom.SummonDoor();
                if(linkedRoom.roomIsCleared) linkedRoom.SetArrows();
                else ArrowManager.Instance.MaskEntries();
            }
        }

        private bool IsOutside(Vector3 playerPos)
        {
            var currentPos = playerPos - transform.position;
            switch (side)
            {
                case Side.Up:
                    if (currentPos.y > 0) return true;
                    break;
                case Side.Right:
                    if (currentPos.x > 0) return true;
                    break;
                case Side.Down:
                    if (currentPos.y < 0) return true;
                    break;
                case Side.Left:
                    if (currentPos.x < 0) return true;
                    break;
            }
            return false;
        }

        public void SetTrigger()
        {
            switch (side)
            {
                case Side.Up:
                    trigger.offset = new Vector2(0,Offset);
                    break;
                case Side.Right:
                    trigger.offset = new Vector2(Offset,0);
                    break;
                case Side.Down:
                    trigger.offset = new Vector2(0,-Offset);
                    break;
                case Side.Left:
                    trigger.offset = new Vector2(-Offset,0);
                    break;
            }
        }

        public void ResetTrigger()
        {
            switch (side)
            {
                case Side.Up:
                    trigger.offset = new Vector2(0,-Offset);
                    break;
                case Side.Right:
                    trigger.offset = new Vector2(-Offset,0);
                    break;
                case Side.Down:
                    trigger.offset = new Vector2(0,Offset);
                    break;
                case Side.Left:
                    trigger.offset = new Vector2(Offset,0);
                    break;
            }
        }
    }
}
