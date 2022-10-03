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
        private Vector3 _newPos;
        private int _indexForBoss,_indexForCharacter1, _indexForCharacter2;
        private Side _sideFirstLoop, _sideBoss, _sideCharacter1,_sideCharacter2, _nextSide;
        private bool _secondRoom4Room, _bossRoomIsSet;

        private enum Side
        {
            Up,
            Right,
            Down,
            Left,
            Start
        }

        private enum SpecialRoom
        {
            Boss,
            Character1,
            Character2
        }

        public List<GameObject> listSalle;
        
        [SerializeField] private Vector3 startPos;
        [SerializeField] private int filler2RoomApparitionDenominator;
        [SerializeField] private int filler2RoomBigVersionDenominator;
        private void Start()
        {
            PlaceStartRoom();
            _secondRoom4Room = PlaceSecondRoom();
            PickSideFirstLoop();
            CheckFiller2Room(_sideFirstLoop);
            FirstLoop();
            
        }

        private void PickSideFirstLoop()
        {
            _sideFirstLoop = Random.Range(0,2)==0 ? Side.Left : Side.Right;
        }
        
        private void FirstLoop()
        {
            switch (_nextSide)
            {
                case Side.Up:
                    FirstLoopUp();
                    break;
                case Side.Right:
                    FirstLoopRight();
                    break;
                case Side.Down:
                    FirstLoopDown();
                    break;
                case Side.Left:
                    FirstLoopLeft();
                    break;
            }
        }

        private void FirstLoopUp()
        {
            if (_sideFirstLoop == Side.Right) FirstLoopUpRight();
            else FirstLoopUpLeft();
        }

        private void FirstLoopUpRight()
        {
            Place3RoomNse(_lastRoomIndex,_nextSide);
            _nextSide = Side.Up;
            var specialRoom = PickSpecialRoom();
            var addSpecialRoom = Random.Range(1, 5);
            if (addSpecialRoom==1)
            {
                Place3RoomNse(_lastRoomIndex,_nextSide);
                _nextSide = Side.Right;
                SetSpecialRoom(specialRoom,Side.Up);
            }
            else Place2RoomSe(_lastRoomIndex,_nextSide);

            if (addSpecialRoom==2)
            {
                Place3RoomNso(_lastRoomIndex,_nextSide);
                _nextSide = Side.Down;
                SetSpecialRoom(specialRoom,Side.Up);
            }
            if (addSpecialRoom==3)
            {
                Place3RoomSeo(_lastRoomIndex,_nextSide);
                _nextSide = Side.Down;
                SetSpecialRoom(specialRoom,Side.Right);
            }
            else Place2RoomSo(_lastRoomIndex,_nextSide);
            
            if (addSpecialRoom==4)
            {
                Place3RoomNeo(_lastRoomIndex,_nextSide);
                SetSpecialRoom(specialRoom,Side.Right);
            }
            else Place2RoomNo(_lastRoomIndex,_nextSide);
        }

        private void FirstLoopUpLeft()
        {
            Place3RoomNso(_lastRoomIndex,_nextSide);
            _nextSide = Side.Up;
            var specialRoom = PickSpecialRoom();
            var addSpecialRoom = Random.Range(1, 5);
            if (addSpecialRoom==1)
            {
                Place3RoomNso(_lastRoomIndex,_nextSide);
                _nextSide = Side.Left;
                SetSpecialRoom(specialRoom,Side.Up);
            }
            else Place2RoomSo(_lastRoomIndex,_nextSide);

            if (addSpecialRoom==2)
            {
                Place3RoomNse(_lastRoomIndex,_nextSide);
                _nextSide = Side.Down;
                SetSpecialRoom(specialRoom,Side.Up);
            }
            if (addSpecialRoom==3)
            {
                Place3RoomSeo(_lastRoomIndex,_nextSide);
                _nextSide = Side.Down;
                SetSpecialRoom(specialRoom,Side.Left);
            }
            else Place2RoomSe(_lastRoomIndex,_nextSide);
            
            if (addSpecialRoom==4)
            {
                Place3RoomNeo(_lastRoomIndex,_nextSide);
                SetSpecialRoom(specialRoom,Side.Left);
            }
            else Place2RoomNe(_lastRoomIndex,_nextSide);
        }
        
        private void FirstLoopRight()
        {
            
        }
        
        private void FirstLoopDown()
        {
            if (_sideFirstLoop == Side.Right) FirstLoopDownRight();
            else FirstLoopDownLeft();
        }
        
        private void FirstLoopDownRight()
        {
            Place3RoomNse(_lastRoomIndex,_nextSide);
            _nextSide = Side.Down;
            var specialRoom = PickSpecialRoom();
            var addSpecialRoom = Random.Range(1, 5);
            if (addSpecialRoom==1)
            {
                Place3RoomNse(_lastRoomIndex,_nextSide);
                _nextSide = Side.Right;
                SetSpecialRoom(specialRoom,Side.Down);
            }
            else Place2RoomNe(_lastRoomIndex,_nextSide);

            if (addSpecialRoom==2)
            {
                Place3RoomNso(_lastRoomIndex,_nextSide);
                _nextSide = Side.Up;
                SetSpecialRoom(specialRoom,Side.Down);
            }
            if (addSpecialRoom==3)
            {
                Place3RoomNeo(_lastRoomIndex,_nextSide);
                _nextSide = Side.Up;
                SetSpecialRoom(specialRoom,Side.Right);
            }
            else Place2RoomNo(_lastRoomIndex,_nextSide);
            
            if (addSpecialRoom==4)
            {
                Place3RoomSeo(_lastRoomIndex,_nextSide);
                SetSpecialRoom(specialRoom,Side.Right);
            }
            else Place2RoomSo(_lastRoomIndex,_nextSide);
        }

        private void FirstLoopDownLeft()
        {
            Place3RoomNse(_lastRoomIndex,_nextSide);
            _nextSide = Side.Down;
            var specialRoom = PickSpecialRoom();
            var addSpecialRoom = Random.Range(1, 5);
            if (addSpecialRoom==1)
            {
                Place3RoomNso(_lastRoomIndex,_nextSide);
                _nextSide = Side.Left;
                SetSpecialRoom(specialRoom,Side.Down);
            }
            else Place2RoomNo(_lastRoomIndex,_nextSide);

            if (addSpecialRoom==2)
            {
                Place3RoomNse(_lastRoomIndex,_nextSide);
                _nextSide = Side.Up;
                SetSpecialRoom(specialRoom,Side.Down);
            }
            if (addSpecialRoom==3)
            {
                Place3RoomNeo(_lastRoomIndex,_nextSide);
                _nextSide = Side.Up;
                SetSpecialRoom(specialRoom,Side.Left);
            }
            else Place2RoomNe(_lastRoomIndex,_nextSide);
            
            if (addSpecialRoom==4)
            {
                Place3RoomSeo(_lastRoomIndex,_nextSide);
                SetSpecialRoom(specialRoom,Side.Left);
            }
            else Place2RoomSe(_lastRoomIndex,_nextSide);
        }
        
        private void FirstLoopLeft()
        {
            
        }

        private void CheckFiller2Room(Side side)
        {
            if (Random.Range(0,filler2RoomApparitionDenominator)==0)
                AddFiller2Room(side);
        }

        private void AddFiller2Room(Side side)
        {
            if (Random.Range(0,filler2RoomBigVersionDenominator)==0)
            {
                PlaceBigRoom(_lastRoomIndex,side);
            }
            else
            {
                switch (side)
                {
                    case Side.Right:
                        switch (Random.Range(0,3))
                        {
                            case 0:
                                Place2RoomEo(_lastRoomIndex,side);
                                break;
                            case 1:
                                Place2RoomNo(_lastRoomIndex,side);
                                break;
                            case 2:
                                Place2RoomSo(_lastRoomIndex,side);
                                break;
                        }
                        break;
                    case Side.Left:
                        switch (Random.Range(0,3))
                        {
                            case 0:
                                Place2RoomNs(_lastRoomIndex,side);
                                break;
                            case 1:
                                Place2RoomSe(_lastRoomIndex,side);
                                break;
                            case 2:
                                Place2RoomSo(_lastRoomIndex,side);
                                break;
                        }
                        break;
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
            _lastRoomIndex++;
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
            _lastRoomIndex--;
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
            ChooseSpecialRoom(side);
            InstantiateRoom(indexOldRoom,side,false);
        }
        
        private void PlaceCharacterRoom(int indexOldRoom,Side side)
        {
            _currentRoomPool = Resources.LoadAll("CharacterRoom", typeof(GameObject));
            ChooseSpecialRoom(side);
            InstantiateRoom(indexOldRoom,side,false);
        }

        private void PlaceShopRoom(int indexOldRoom,Side side)
        {
            _currentRoomPool = Resources.LoadAll("ShopRoom", typeof(GameObject));
            ChooseSpecialRoom(side);
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
            _nextSide = side == Side.Left ? Side.Up : Side.Right;
        } 
        
        private void Place2RoomNo(int indexOldRoom,Side side)
        {
            _currentRoomPool = Resources.LoadAll("2Room/NO", typeof(GameObject));
            PickARoomAtRandom();
            InstantiateRoom(indexOldRoom,side,false);
            _nextSide = side == Side.Right ? Side.Up : Side.Left;
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
            _nextSide = side == Side.Left ? Side.Down : Side.Right;
        }
        
        private void Place2RoomSo(int indexOldRoom,Side side)
        {
            _currentRoomPool = Resources.LoadAll("2Room/SO", typeof(GameObject));
            PickARoomAtRandom();
            InstantiateRoom(indexOldRoom,side,false);
            _nextSide = side == Side.Right ? Side.Down : Side.Left;
        }
        
        private void PlaceBigRoom(int indexOldRoom,Side side)
        {
            _currentRoomPool = Resources.LoadAll("BigRoom", typeof(GameObject));
            ChooseSpecialRoom(side);
            InstantiateRoom(indexOldRoom,side,true);
            FindRemainingEntryBigRoom(side);
        }
        #endregion

        private void FindRemainingEntryBigRoom(Side side)
        {
            var refEntry = listSalle[_lastRoomIndex].GetComponent<RefEntry>();
            switch (side)
            {
                case Side.Up:
                    if (refEntry.entreeNord != null) _nextSide = Side.Up;
                    if (refEntry.entreeEst != null) _nextSide = Side.Right;
                    if (refEntry.entreeOuest != null) _nextSide = Side.Left;
                    break;
                case Side.Right:
                    if (refEntry.entreeNord != null) _nextSide = Side.Up;
                    if (refEntry.entreeEst != null) _nextSide = Side.Right;
                    if (refEntry.entreeSud != null) _nextSide = Side.Down;
                    break;
                case Side.Down:
                    if (refEntry.entreeEst != null) _nextSide = Side.Right;
                    if (refEntry.entreeSud != null) _nextSide = Side.Down;
                    if (refEntry.entreeOuest != null) _nextSide = Side.Left;
                    break;
                case Side.Left:
                    if (refEntry.entreeNord != null) _nextSide = Side.Up;
                    if (refEntry.entreeSud != null) _nextSide = Side.Down;
                    if (refEntry.entreeOuest != null) _nextSide = Side.Left;
                    break;
            }
        }
        private void PickARoomAtRandom()
        {
            _chosenRoom = Random.Range(0, _currentRoomPool.Length);
        }
        private SpecialRoom PickSpecialRoom()
        {
            SpecialRoom specialRoom;
            if (Random.Range(0,2) == 0)
            {
                specialRoom = !_bossRoomIsSet ? SpecialRoom.Boss : SpecialRoom.Character2;
            }
            else
            {
                specialRoom = SpecialRoom.Character1;
            }
            return specialRoom;
        }

        private void SetSpecialRoom(SpecialRoom specialRoom, Side side)
        {
            switch (specialRoom)
            {
                case SpecialRoom.Boss:
                    _indexForBoss = _lastRoomIndex;
                    _sideBoss = side;
                    break;
                case SpecialRoom.Character1:
                    _indexForCharacter1 = _lastRoomIndex;
                    _sideCharacter1 = side;
                    break;
                case SpecialRoom.Character2:
                    _indexForCharacter2 = _lastRoomIndex;
                    _sideCharacter2 = side;
                    break;
            }
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