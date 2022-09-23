using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace GenPro
{
    public class LayoutGenerator : MonoBehaviour
    {
        private Object[] _startRoom;
        private Object[] _bossRoom;
        private Object[] _characterRoom;
        private Object[] _secretRoom;
        private Object[] _shopRoom;
        //4Room
        private Object[] _4Room;
        //3Room
        private Object[] _neoRoom;
        private Object[] _nseRoom;
        private Object[] _nsoRoom;
        private Object[] _seoRoom;
        //2Room
        private Object[] _eoRoom;
        private Object[] _neRoom;
        private Object[] _noRoom;
        private Object[] _nsRoom;
        private Object[] _seRoom;
        private Object[] _soRoom;
        //larger 2Room
        private Object[] _bigRoom;
        
        public List<GameObject> listSalle;

        [SerializeField] private Vector3 startPos;
        private Vector3 _newPos;
        private void Start()
        {
            PlaceStartRoom();
            PlaceSecondRoom();
        }

        private void InstantiateRoom(IReadOnlyList<Object> salle,int indexOldRoom,int side)
        {
            var chosenRoom = Random.Range(0, salle.Count);
            var newRoom = salle[chosenRoom].GameObject().transform;
            switch (side)
            {
                case 1:
                    UpPlacement(listSalle[indexOldRoom].transform,newRoom);
                    break;
                case 2:
                    RightPlacement(listSalle[indexOldRoom].transform,newRoom);
                    break;
                case 3:
                    DownPlacement(listSalle[indexOldRoom].transform,newRoom);
                    break;
                case 4:
                    LeftPlacement(listSalle[indexOldRoom].transform,newRoom);
                    break;
                default:
                    _newPos = startPos;
                    break;
            }
            listSalle.Add( Instantiate(salle[chosenRoom].GameObject(),_newPos,quaternion.identity,transform));
        }

        private void PlaceStartRoom()
        {
            _startRoom = Resources.LoadAll("StartRoom", typeof(GameObject));
            InstantiateRoom(_startRoom, 0,0);
        }

        private void PlaceSecondRoom()
        {
            _seoRoom = Resources.LoadAll("3Room/SEO", typeof(GameObject));
            InstantiateRoom(_seoRoom,0,1);
        }

        private void UpPlacement(Transform oldRoom,Transform newRoom)
        {
            var tempX = oldRoom.position.x;
            var tempY = oldRoom.position.y + oldRoom.lossyScale.y / 2 + newRoom.lossyScale.y / 2;
            _newPos = new Vector3(tempX,tempY, 0);
        }
        
        private void RightPlacement(Transform oldRoom,Transform newRoom)
        {
            var tempX = oldRoom.position.x+ oldRoom.lossyScale.x / 2 + newRoom.lossyScale.x / 2;
            var tempY = oldRoom.position.y;
            _newPos = new Vector3(tempX,tempY, 0);
        }
        
        private void DownPlacement(Transform oldRoom,Transform newRoom)
        {
            var tempX = oldRoom.position.x;
            var tempY = oldRoom.position.y - oldRoom.lossyScale.x / 2 - newRoom.lossyScale.x / 2;
            _newPos = new Vector3(tempX,tempY, 0);
        }
        
        private void LeftPlacement(Transform oldRoom,Transform newRoom)
        {
            var tempX = oldRoom.position.x - oldRoom.lossyScale.x / 2 - newRoom.lossyScale.x / 2;
            var tempY = oldRoom.position.y;
            _newPos = new Vector3(tempX,tempY, 0);
        }
    }
}
