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
        private Object[] _currentRoomPool;
        private int _chosenRoom;
        private Transform _newRoom;
        private int _lastRoomIndex;
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

        [SerializeField] private int firstLoopFiller2RoomApparitionDenominator;
        [SerializeField] private int firstLoopFiller2RoomBigVersionDenominator;
        private void Start()
        {
            PlaceStartRoom();
            if (PlaceSecondRoom()) //4Room
            {
                TestLayout();
            }
            else //3RoomSEO
            {
                TestLayout();
            }
        }

        private void TestLayout()
        {
            Place4Room(1,Side.Left);
            PlaceBossRoom(2,Side.Up);
            Place2RoomNo(2,Side.Down);
            Place2RoomNe(4,Side.Left);
            Place2RoomSe(5,Side.Up);
        }
        
        private void CheckFirstLoopFiller2Room(Side side)
        {
            if (Random.Range(0,firstLoopFiller2RoomApparitionDenominator)==0)
                AddFirstLoopFiller2Room(side);
        }

        private void AddFirstLoopFiller2Room(Side side)
        {
            if (side == Side.Left)
            {
                if (Random.Range(0,firstLoopFiller2RoomBigVersionDenominator)==0)
                {
                    PlaceBigRoom(_lastRoomIndex,Side.Left);
                    return;
                }
            }
        }

        private void InstantiateRoom(int indexOldRoom,Side side,bool big)
        {
            Debug.Log(indexOldRoom);
            _newRoom = _currentRoomPool[_chosenRoom].GameObject().transform; 
            
            switch (side)
            {
                case Side.Up:
                    UpPlacement(listSalle[indexOldRoom].transform,_newRoom,big);
                    break;
                case Side.Right:
                    RightPlacement(listSalle[indexOldRoom].transform,_newRoom,big);
                    break;
                case Side.Down:
                    DownPlacement(listSalle[indexOldRoom].transform,_newRoom,big);
                    break;
                case Side.Left:
                    LeftPlacement(listSalle[indexOldRoom].transform,_newRoom,big);
                    break;
                case Side.Start:
                    _newPos = startPos;
                    break;
            }
            listSalle.Add( Instantiate(_currentRoomPool[_chosenRoom].GameObject(),_newPos,quaternion.identity,transform));
        }
        
        private void UpPlacement(Transform oldRoom,Transform newRoom,bool big)
        {
            var tempX = oldRoom.position.x;
            var tempY = oldRoom.position.y + oldRoom.lossyScale.y / 2 + newRoom.lossyScale.y / 2;
            if (oldRoom.gameObject.GetComponent<RefEntry>())
            {
                tempX = oldRoom.gameObject.GetComponent<RefEntry>().entreeNord.transform.position.x;
            }
            if (big)
            {
                tempX = _currentRoomPool[_chosenRoom].GetComponent<RefEntry>().entreeSud.transform.position.x;
            }
            _newPos = new Vector3(tempX,tempY, 0);
        }
        
        private void RightPlacement(Transform oldRoom,Transform newRoom,bool big)
        {
            var tempX = oldRoom.position.x+ oldRoom.lossyScale.x / 2 + newRoom.lossyScale.x / 2;
            var tempY = oldRoom.position.y;
            if (oldRoom.gameObject.GetComponent<RefEntry>())
            {
                tempY = oldRoom.gameObject.GetComponent<RefEntry>().entreeEst.transform.position.y;
            }
            if (big)
            {
                tempX = _currentRoomPool[_chosenRoom].GetComponent<RefEntry>().entreeOuest.transform.position.x;
            }
            _newPos = new Vector3(tempX,tempY, 0);
        }
        
        private void DownPlacement(Transform oldRoom,Transform newRoom,bool big)
        {
            var tempX = oldRoom.position.x;
            var tempY = oldRoom.position.y - oldRoom.lossyScale.x / 2 - newRoom.lossyScale.x / 2;
            if (oldRoom.gameObject.GetComponent<RefEntry>())
            {
                tempX = oldRoom.gameObject.GetComponent<RefEntry>().entreeSud.transform.position.x;
            }
            if (big)
            {
                tempX = _currentRoomPool[_chosenRoom].GetComponent<RefEntry>().entreeNord.transform.position.x;
            }
            _newPos = new Vector3(tempX,tempY, 0);
        }
        
        private void LeftPlacement(Transform oldRoom,Transform newRoom,bool big)
        {
            var tempX = oldRoom.position.x - oldRoom.lossyScale.x / 2 - newRoom.lossyScale.x / 2;
            var tempY = oldRoom.position.y;
            if (oldRoom.gameObject.GetComponent<RefEntry>())
            {
                tempY = oldRoom.gameObject.GetComponent<RefEntry>().entreeOuest.transform.position.y;
            }
            if (big)
            {
                tempX = _currentRoomPool[_chosenRoom].GetComponent<RefEntry>().entreeEst.transform.position.x;
            }
            _newPos = new Vector3(tempX,tempY, 0);
        }
        
        #region PlaceRoom
        private void PlaceStartRoom()
        {
            _currentRoomPool = Resources.LoadAll("StartRoom", typeof(GameObject));
            InstantiateRoom(0,Side.Start,false);
        }
        
        private bool PlaceSecondRoom()
        {
            if (Random.Range(0,2)==0)
            {
                _currentRoomPool = Resources.LoadAll("3Room/SEO", typeof(GameObject));
                PickARoomAtRandom();
                InstantiateRoom(0,Side.Up,false);
                return false;
            }
            _currentRoomPool = Resources.LoadAll("4Room", typeof(GameObject));
            PickARoomAtRandom();
            InstantiateRoom(0,Side.Up,false);
            return true;
        }
        
        private void PlaceBossRoom(int indexOldRoom,Side side)
        {
            _currentRoomPool = Resources.LoadAll("BossRoom", typeof(GameObject));
            PickARoomAtRandom();
            InstantiateRoom(indexOldRoom,side,false);
        }
        
        private void PlaceCharacterRoom(int indexOldRoom,Side side)
        {
            _currentRoomPool = Resources.LoadAll("CharacterRoom", typeof(GameObject));
            PickARoomAtRandom();
            InstantiateRoom(indexOldRoom,side,false);
        }
        
        private void PlaceSecretRoom(int indexOldRoom,Side side)
        {
            _currentRoomPool = Resources.LoadAll("SecretRoom", typeof(GameObject));
            PickARoomAtRandom();
            InstantiateRoom(indexOldRoom,side,false);
        }
        
        private void PlaceShopRoom(int indexOldRoom,Side side)
        {
            _currentRoomPool = Resources.LoadAll("ShopRoom", typeof(GameObject));
            PickARoomAtRandom();
            InstantiateRoom(indexOldRoom,side,false);
        }
        
        private void Place4Room(int indexOldRoom,Side side)
        {
            _currentRoomPool = Resources.LoadAll("4Room", typeof(GameObject));
            PickARoomAtRandom();
            InstantiateRoom(indexOldRoom,side,false);
        }
        
        private void Place3RoomNeo(int indexOldRoom,Side side)
        {
            _currentRoomPool = Resources.LoadAll("3Room/NEO", typeof(GameObject));
            PickARoomAtRandom();
            InstantiateRoom(indexOldRoom,side,false);
        }
        
        private void Place3RoomNse(int indexOldRoom,Side side)
        {
            _currentRoomPool = Resources.LoadAll("3Room/NSE", typeof(GameObject));
            PickARoomAtRandom();
            InstantiateRoom(indexOldRoom,side,false);
        }
        
        private void Place3RoomNso(int indexOldRoom,Side side)
        {
            _currentRoomPool = Resources.LoadAll("3Room/NSO", typeof(GameObject));
            PickARoomAtRandom();
            InstantiateRoom(indexOldRoom,side,false);
        }
        
        private void Place3RoomSeo(int indexOldRoom,Side side)
        {
            _currentRoomPool = Resources.LoadAll("3Room/SEO", typeof(GameObject));
            PickARoomAtRandom();
            InstantiateRoom(indexOldRoom,side,false);
        }
        
        private void Place2RoomEo(int indexOldRoom,Side side)
        {
            _currentRoomPool = Resources.LoadAll("2Room/EO", typeof(GameObject));
            PickARoomAtRandom();
            InstantiateRoom(indexOldRoom,side,false);
        }
        
        private void Place2RoomNe(int indexOldRoom,Side side)
        {
            _currentRoomPool = Resources.LoadAll("2Room/NE", typeof(GameObject));
            PickARoomAtRandom();
            InstantiateRoom(indexOldRoom,side,false);
        }
        
        private void Place2RoomNo(int indexOldRoom,Side side)
        {
            _currentRoomPool = Resources.LoadAll("2Room/NO", typeof(GameObject));
            PickARoomAtRandom();
            InstantiateRoom(indexOldRoom,side,false);
        }
        
        private void Place2RoomNs(int indexOldRoom,Side side)
        {
            _currentRoomPool = Resources.LoadAll("2Room/NS", typeof(GameObject));
            PickARoomAtRandom();
            InstantiateRoom(indexOldRoom,side,false);
        }
        
        private void Place2RoomSe(int indexOldRoom,Side side)
        {
            _currentRoomPool = Resources.LoadAll("2Room/SE", typeof(GameObject));
            PickARoomAtRandom();
            InstantiateRoom(indexOldRoom,side,false);
        }
        
        private void Place2RoomSo(int indexOldRoom,Side side)
        {
            _currentRoomPool = Resources.LoadAll("2Room/SO", typeof(GameObject));
            PickARoomAtRandom();
            InstantiateRoom(indexOldRoom,side,false);
        }
        
        private void PlaceBigRoom(int indexOldRoom,Side side)
        {
            _currentRoomPool = Resources.LoadAll("BigRoom", typeof(GameObject));
            ChooseSpecialRoom(side);
            InstantiateRoom(indexOldRoom,side,true);
        }
        #endregion

        private void PickARoomAtRandom()
        {
            _chosenRoom = Random.Range(0, _currentRoomPool.Length);
        }

        private void ChooseSpecialRoom(Side side)
        {
            var entrySide = Side.Start;
            switch (side)
            {
                case Side.Up:
                    while (entrySide!=Side.Down)
                    {
                        PickARoomAtRandom();
                        if (_currentRoomPool[_chosenRoom].GetComponent<RefEntry>().entreeSud)
                            entrySide = Side.Down;
                    }
                    break;
                case Side.Right:
                    while (entrySide!=Side.Left)
                    {
                        PickARoomAtRandom();
                        if (_currentRoomPool[_chosenRoom].GetComponent<RefEntry>().entreeOuest)
                            entrySide = Side.Left;
                    }
                    break;
                case Side.Down:
                    while (entrySide!=Side.Up)
                    {
                        PickARoomAtRandom();
                        if (_currentRoomPool[_chosenRoom].GetComponent<RefEntry>().entreeNord)
                            entrySide = Side.Up;
                    }
                    break;
                case Side.Left:
                    while (entrySide!=Side.Right)
                    {
                        PickARoomAtRandom();
                        if (_currentRoomPool[_chosenRoom].GetComponent<RefEntry>().entreeEst)
                            entrySide = Side.Right;
                    }
                    break;
            }
        }
    }
}