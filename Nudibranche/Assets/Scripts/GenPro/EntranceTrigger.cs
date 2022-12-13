using System;
using UI;
using UnityEngine;

namespace GenPro
{
    public class EntranceTrigger : MonoBehaviour
    {
        public RoomManager linkedRoom;
        [SerializeField] private Side side;

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
            if(IsOutside(other.transform.position))linkedRoom.Deactivate();
            else
            {
                GameManager.instance.currentRoom = linkedRoom;
                if(linkedRoom.roomIsCleared) linkedRoom.SetArrows();
                else
                {
                    ArrowManager.Instance.MaskEntries();
                    linkedRoom.SummonDoor();
                }
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
    }
}
