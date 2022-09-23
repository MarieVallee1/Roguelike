using System;
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
        
        private enum Side
        {
            Up,
            Right,
            Down,
            Left,
            Start
        }

        [SerializeField] private Vector3 startPos;
        private Vector3 _newPos;
        private void Start()
        {
            PlaceStartRoom();
            PlaceSecondRoom();
            TestLayout();
        }

        private void TestLayout()
        {
            Place3RoomNse(1,Side.Left);
            PlaceBossRoom(2,Side.Up);
            Place2RoomNo(2,Side.Down);
            Place2RoomNe(4,Side.Left);
            Place2RoomSe(5,Side.Up);
        }

        private void InstantiateRoom(IReadOnlyList<Object> salle,int indexOldRoom,Side side)
        {
            Debug.Log(indexOldRoom);
            var chosenRoom = Random.Range(0, salle.Count);
            var newRoom = salle[chosenRoom].GameObject().transform;
            switch (side)
            {
                case Side.Up:
                    UpPlacement(listSalle[indexOldRoom].transform,newRoom);
                    break;
                case Side.Right:
                    RightPlacement(listSalle[indexOldRoom].transform,newRoom);
                    break;
                case Side.Down:
                    DownPlacement(listSalle[indexOldRoom].transform,newRoom);
                    break;
                case Side.Left:
                    LeftPlacement(listSalle[indexOldRoom].transform,newRoom);
                    break;
                default:
                    _newPos = startPos;
                    break;
            }
            listSalle.Add( Instantiate(salle[chosenRoom].GameObject(),_newPos,quaternion.identity,transform));
        }
        
        private void UpPlacement(Transform oldRoom,Transform newRoom)
        {
            var tempX = oldRoom.position.x;
            var tempY = oldRoom.position.y + oldRoom.lossyScale.y / 2 + newRoom.lossyScale.y / 2;
            if (oldRoom.gameObject.GetComponent<RefEntryBigRoom>())
            {
                tempX = oldRoom.gameObject.GetComponent<RefEntryBigRoom>().entreeSud.transform.position.x;
            }
            _newPos = new Vector3(tempX,tempY, 0);
        }
        
        private void RightPlacement(Transform oldRoom,Transform newRoom)
        {
            var tempX = oldRoom.position.x+ oldRoom.lossyScale.x / 2 + newRoom.lossyScale.x / 2;
            var tempY = oldRoom.position.y;
            if (oldRoom.gameObject.GetComponent<RefEntryBigRoom>())
            {
                tempY = oldRoom.gameObject.GetComponent<RefEntryBigRoom>().entreeOuest.transform.position.y;
            }
            _newPos = new Vector3(tempX,tempY, 0);
        }
        
        private void DownPlacement(Transform oldRoom,Transform newRoom)
        {
            var tempX = oldRoom.position.x;
            var tempY = oldRoom.position.y - oldRoom.lossyScale.x / 2 - newRoom.lossyScale.x / 2;
            if (oldRoom.gameObject.GetComponent<RefEntryBigRoom>())
            {
                tempX = oldRoom.gameObject.GetComponent<RefEntryBigRoom>().entreeNord.transform.position.x;
            }
            _newPos = new Vector3(tempX,tempY, 0);
        }
        
        private void LeftPlacement(Transform oldRoom,Transform newRoom)
        {
            var tempX = oldRoom.position.x - oldRoom.lossyScale.x / 2 - newRoom.lossyScale.x / 2;
            var tempY = oldRoom.position.y;
            if (oldRoom.gameObject.GetComponent<RefEntryBigRoom>())
            {
                tempY = oldRoom.gameObject.GetComponent<RefEntryBigRoom>().entreeEst.transform.position.y;
            }
            _newPos = new Vector3(tempX,tempY, 0);
        }
        
        #region PlaceRoom
        private void PlaceStartRoom()
        {
            _startRoom = Resources.LoadAll("StartRoom", typeof(GameObject));
            InstantiateRoom(_startRoom, 0,Side.Start);
        }
        private void PlaceSecondRoom()
        {
            _seoRoom = Resources.LoadAll("3Room/SEO", typeof(GameObject));
            InstantiateRoom(_seoRoom,0,Side.Up);
        }
        
        private void PlaceBossRoom(int indexOldRoom,Side side)
        {
            _seoRoom = Resources.LoadAll("BossRoom", typeof(GameObject));
            InstantiateRoom(_seoRoom,indexOldRoom,side);
        }
        
        private void PlaceCharacterRoom(int indexOldRoom,Side side)
        {
            _seoRoom = Resources.LoadAll("BossRoom", typeof(GameObject));
            InstantiateRoom(_seoRoom,indexOldRoom,side);
        }
        
        private void PlaceSecretRoom(int indexOldRoom,Side side)
        {
            _seoRoom = Resources.LoadAll("BossRoom", typeof(GameObject));
            InstantiateRoom(_seoRoom,indexOldRoom,side);
        }
        
        private void PlaceShopRoom(int indexOldRoom,Side side)
        {
            _seoRoom = Resources.LoadAll("BossRoom", typeof(GameObject));
            InstantiateRoom(_seoRoom,indexOldRoom,side);
        }
        
        private void Place4Room(int indexOldRoom,Side side)
        {
            _seoRoom = Resources.LoadAll("4Room", typeof(GameObject));
            InstantiateRoom(_seoRoom,indexOldRoom,side);
        }
        
        private void Place3RoomNeo(int indexOldRoom,Side side)
        {
            _seoRoom = Resources.LoadAll("3Room/NEO", typeof(GameObject));
            InstantiateRoom(_seoRoom,indexOldRoom,side);
        }
        
        private void Place3RoomNse(int indexOldRoom,Side side)
        {
            _seoRoom = Resources.LoadAll("3Room/NSE", typeof(GameObject));
            InstantiateRoom(_seoRoom,indexOldRoom,side);
        }
        
        private void Place3RoomNso(int indexOldRoom,Side side)
        {
            _seoRoom = Resources.LoadAll("3Room/NSO", typeof(GameObject));
            InstantiateRoom(_seoRoom,indexOldRoom,side);
        }
        
        private void Place3RoomSeo(int indexOldRoom,Side side)
        {
            _seoRoom = Resources.LoadAll("3Room/SEO", typeof(GameObject));
            InstantiateRoom(_seoRoom,indexOldRoom,side);
        }
        
        private void Place2RoomEo(int indexOldRoom,Side side)
        {
            _seoRoom = Resources.LoadAll("2Room/EO", typeof(GameObject));
            InstantiateRoom(_seoRoom,indexOldRoom,side);
        }
        
        private void Place2RoomNe(int indexOldRoom,Side side)
        {
            _seoRoom = Resources.LoadAll("2Room/NE", typeof(GameObject));
            InstantiateRoom(_seoRoom,indexOldRoom,side);
        }
        
        private void Place2RoomNo(int indexOldRoom,Side side)
        {
            _seoRoom = Resources.LoadAll("2Room/NO", typeof(GameObject));
            InstantiateRoom(_seoRoom,indexOldRoom,side);
        }
        
        private void Place2RoomNs(int indexOldRoom,Side side)
        {
            _seoRoom = Resources.LoadAll("2Room/NS", typeof(GameObject));
            InstantiateRoom(_seoRoom,indexOldRoom,side);
        }
        
        private void Place2RoomSe(int indexOldRoom,Side side)
        {
            _seoRoom = Resources.LoadAll("2Room/SE", typeof(GameObject));
            InstantiateRoom(_seoRoom,indexOldRoom,side);
        }
        
        private void Place2RoomSo(int indexOldRoom,Side side)
        {
            _seoRoom = Resources.LoadAll("2Room/SO", typeof(GameObject));
            InstantiateRoom(_seoRoom,indexOldRoom,side);
        }
        #endregion
    }
}