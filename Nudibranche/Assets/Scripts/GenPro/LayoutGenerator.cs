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
        private int _chosenRoom, _shopPosition,_checkShopPosition;
        private Transform _newRoom;
        private int _lastRoomIndex;
        private Vector3 _newPos;
        private int _indexForBoss,_indexForCharacter1, _indexForCharacter2, _savedIndex;
        private Side _sideFirstLoop, _sideBoss, _sideCharacter1,_sideCharacter2, _nextSide;
        private SpecialRoom _specialRoom1, _specialRoom2, _specialRoom3;
        private bool _secondRoom4Room, _bossRoomIsSet, _inFirstLoop;

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
        
        private enum Entry
        {
            FourRoom,
            Ns,
            Ne,
            No,
            Se,
            So,
            Eo
        }

        public List<GameObject> listSalle;
        
        [SerializeField] private Vector3 startPos;
        [SerializeField] private int filler2RoomApparitionDenominator;
        [SerializeField] private int filler2RoomBigVersionDenominator;
        private void Start()
        {
            ChooseShopPosition();
            PlaceStartRoom();
            _secondRoom4Room = PlaceSecondRoom();
            PickSideFirstLoop();
            CheckFiller2Room(_sideFirstLoop);
            _specialRoom1 = PickSpecialRoomType();
            FirstLoop();
            _specialRoom2 = !_bossRoomIsSet ? SpecialRoom.Boss : SpecialRoom.Character1;
            _specialRoom3 = SpecialRoom.Character2;
            SecondLoop();
            PlaceSpecialRooms();
        }
        private void CheckFiller2Room(Side side)
        {
            if (Random.Range(0,filler2RoomApparitionDenominator)==0)
                AddFiller2Room(side);
        }
        private void AddFiller2Room(Side side)
        {
            // if (Random.Range(0,filler2RoomBigVersionDenominator)==0)
            // {
            //     PlaceBigRoom(_lastRoomIndex,side);
            // }
            // else
            {
                switch (side)
                {
                    case Side.Up:
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
                    case Side.Down:
                        switch (Random.Range(0,3))
                        {
                            case 0:
                                Place2RoomNs(_lastRoomIndex,side);
                                break;
                            case 1:
                                Place2RoomNe(_lastRoomIndex,side);
                                break;
                            case 2:
                                Place2RoomNo(_lastRoomIndex,side);
                                break;
                        }
                        break;
                    case Side.Left:
                        switch (Random.Range(0,3))
                        {
                            case 0:
                                Place2RoomNe(_lastRoomIndex,side);
                                break;
                            case 1:
                                Place2RoomSe(_lastRoomIndex,side);
                                break;
                            case 2:
                                Place2RoomEo(_lastRoomIndex,side);
                                break;
                        }
                        break;
                }
            }
        }
        private void PickSideFirstLoop()
        {
            _sideFirstLoop = Random.Range(0,2)==0 ? Side.Left : Side.Right;
            _nextSide = _sideFirstLoop;
        }
        private void FirstLoop()
        {
            _savedIndex = _lastRoomIndex;
            _inFirstLoop = true;
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
            if (_sideFirstLoop == Side.Right)
            {
                Place3RoomNse(_lastRoomIndex,_nextSide);
                FirstLoopUpRight();
            }
            else
            {
                Place3RoomNso(_lastRoomIndex,_nextSide);
                FirstLoopUpLeft();
            }
        }
        private void FirstLoopRight()
        {
            if (Random.Range(0, 2) == 0) //Up
            {
                Place3RoomNeo(_lastRoomIndex,_nextSide);
                _lastRoomIndex = _savedIndex + 1; //Algo repart de la nouvelle salle si 2nd Loop
                FirstLoopUpRight();
            }
            else //Down
            {
                Place3RoomSeo(_lastRoomIndex,_nextSide);
                _lastRoomIndex = _savedIndex + 1; //Algo repart de la nouvelle salle si 2nd Loop
                FirstLoopDownRight();
            }
        }
        private void FirstLoopDown()
        {
            if (_sideFirstLoop == Side.Right)
            {
                Place3RoomNse(_lastRoomIndex,_nextSide);
                FirstLoopDownRight();
            }
            else
            {
                Place3RoomNso(_lastRoomIndex,_nextSide);
                FirstLoopDownLeft();
            }
        }
        private void FirstLoopLeft()
        {
            if (Random.Range(0, 2) == 0) //Up
            {
                Place3RoomNeo(_lastRoomIndex,_nextSide);
                _lastRoomIndex = _savedIndex + 1; //Algo repart de la nouvelle salle si 2nd Loop
                FirstLoopUpLeft();
            }
            else //Down
            {
                Place3RoomSeo(_lastRoomIndex,_nextSide);
                _lastRoomIndex = _savedIndex + 1; //Algo repart de la nouvelle salle si 2nd Loop
                FirstLoopDownLeft();
            }
        }
        private void FirstLoopUpRight()
        {
            _nextSide = Side.Up;
            var addSpecialRoom1 = PickSpecialRoomPlacement(0,5);
            var addSpecialRoom2 = 0;
            if(!_inFirstLoop) addSpecialRoom2 = PickSpecialRoomPlacement(addSpecialRoom1,5);
            
            if (addSpecialRoom1==1||addSpecialRoom2==1) 
            {
                Place3RoomNse(_lastRoomIndex,_nextSide);
                _nextSide = Side.Right;
                var specialRoom = addSpecialRoom1 == 1 ? PickSpecialRoomNumber() : _specialRoom2;
                SetSpecialRoom(specialRoom,Side.Up);
            }
            else PossibleShop(Entry.Se);

            if ((addSpecialRoom1==2&&addSpecialRoom2==3)||(addSpecialRoom1==3&&addSpecialRoom2==2))
            {
                PossibleShop(Entry.FourRoom);
                _nextSide = Side.Down;
                var specialRoomFirst = addSpecialRoom1 == 2 ? PickSpecialRoomNumber() : _specialRoom2;
                var specialRoomSecond = addSpecialRoom1 == 3 ? PickSpecialRoomNumber() : _specialRoom2;
                SetSpecialRoom(specialRoomFirst,Side.Up);
                SetSpecialRoom(specialRoomSecond,Side.Right);
            }
            else
            {
                if (addSpecialRoom1==2||addSpecialRoom2==2)
                {
                    Place3RoomNso(_lastRoomIndex,_nextSide);
                    _nextSide = Side.Down;
                    var specialRoom = addSpecialRoom1 == 2 ? PickSpecialRoomNumber() : _specialRoom2;
                    SetSpecialRoom(specialRoom,Side.Up);
                }
                if (addSpecialRoom1==3||addSpecialRoom2==3)
                {
                    Place3RoomSeo(_lastRoomIndex,_nextSide);
                    _nextSide = Side.Down;
                    var specialRoom = addSpecialRoom1 == 3 ? PickSpecialRoomNumber() : _specialRoom2;
                    SetSpecialRoom(specialRoom,Side.Right);
                }
                else if(addSpecialRoom1!=2&&addSpecialRoom2!=2) PossibleShop(Entry.So);
            }
            
            if (addSpecialRoom1==4||addSpecialRoom2==4)
            {
                Place3RoomNeo(_lastRoomIndex,_nextSide);
                var specialRoom = addSpecialRoom1 == 4 ? PickSpecialRoomNumber() : _specialRoom2;
                SetSpecialRoom(specialRoom,Side.Right);
            }
            else PossibleShop(Entry.No);
        }
        private void FirstLoopUpLeft()
        {
            _nextSide = Side.Up;
            var addSpecialRoom1 = PickSpecialRoomPlacement(0,5);
            var addSpecialRoom2 = 0;
            if(!_inFirstLoop) addSpecialRoom2 = PickSpecialRoomPlacement(addSpecialRoom1,5);
            
            if (addSpecialRoom1==1||addSpecialRoom2==1)
            {
                Place3RoomNso(_lastRoomIndex,_nextSide);
                _nextSide = Side.Left;
                var specialRoom = addSpecialRoom1 == 1 ? PickSpecialRoomNumber() : _specialRoom2;
                SetSpecialRoom(specialRoom,Side.Up);
            }
            else PossibleShop(Entry.So);

            if ((addSpecialRoom1==2&&addSpecialRoom2==3)||(addSpecialRoom1==3&&addSpecialRoom2==2))
            {
                PossibleShop(Entry.FourRoom);
                _nextSide = Side.Down;
                var specialRoomFirst = addSpecialRoom1 == 2 ? PickSpecialRoomNumber() : _specialRoom2;
                var specialRoomSecond = addSpecialRoom1 == 3 ? PickSpecialRoomNumber() : _specialRoom2;
                SetSpecialRoom(specialRoomFirst,Side.Up);
                SetSpecialRoom(specialRoomSecond,Side.Left);
            }
            else
            {
                if (addSpecialRoom1==2||addSpecialRoom2==2)
                {
                    Place3RoomNse(_lastRoomIndex,_nextSide);
                    _nextSide = Side.Down;
                    var specialRoom = addSpecialRoom1 == 2 ? PickSpecialRoomNumber() : _specialRoom2;
                    SetSpecialRoom(specialRoom,Side.Up);
                }
                if (addSpecialRoom1==3||addSpecialRoom2==3)
                {
                    Place3RoomSeo(_lastRoomIndex,_nextSide);
                    _nextSide = Side.Down;
                    var specialRoom = addSpecialRoom1 == 3 ? PickSpecialRoomNumber() : _specialRoom2;
                    SetSpecialRoom(specialRoom,Side.Left);
                }
                else if(addSpecialRoom1!=2&&addSpecialRoom2!=2) PossibleShop(Entry.Se);
            }

            if (addSpecialRoom1==4||addSpecialRoom2==4)
            {
                Place3RoomNeo(_lastRoomIndex,_nextSide);
                var specialRoom = addSpecialRoom1 == 4 ? PickSpecialRoomNumber() : _specialRoom2;
                SetSpecialRoom(specialRoom,Side.Left);
            }
            else PossibleShop(Entry.Ne);
        }
        private void FirstLoopDownRight()
        {
            _nextSide = Side.Down;
            var addSpecialRoom1 = PickSpecialRoomPlacement(0,5);
            var addSpecialRoom2 = 0;
            if(!_inFirstLoop) addSpecialRoom2 = PickSpecialRoomPlacement(addSpecialRoom1,5);
            
            if (addSpecialRoom1==1||addSpecialRoom2==1)
            {
                Place3RoomNse(_lastRoomIndex,_nextSide);
                _nextSide = Side.Right;
                var specialRoom = addSpecialRoom1 == 1 ? PickSpecialRoomNumber() : _specialRoom2;
                SetSpecialRoom(specialRoom,Side.Down);
            }
            else PossibleShop(Entry.Ne);

            if ((addSpecialRoom1==2&&addSpecialRoom2==3)||(addSpecialRoom1==3&&addSpecialRoom2==2))
            {
                PossibleShop(Entry.FourRoom);
                _nextSide = Side.Up;
                var specialRoomFirst = addSpecialRoom1 == 2 ? PickSpecialRoomNumber() : _specialRoom2;
                var specialRoomSecond = addSpecialRoom1 == 3 ? PickSpecialRoomNumber() : _specialRoom2;
                SetSpecialRoom(specialRoomFirst,Side.Down);
                SetSpecialRoom(specialRoomSecond,Side.Right);
            }
            else
            {
                if (addSpecialRoom1==2||addSpecialRoom2==2)
                {
                    Place3RoomNso(_lastRoomIndex,_nextSide);
                    _nextSide = Side.Up;
                    var specialRoom = addSpecialRoom1 == 2 ? PickSpecialRoomNumber() : _specialRoom2;
                    SetSpecialRoom(specialRoom,Side.Down);
                }
                if (addSpecialRoom1==3||addSpecialRoom2==3)
                {
                    Place3RoomNeo(_lastRoomIndex,_nextSide);
                    _nextSide = Side.Up;
                    var specialRoom = addSpecialRoom1 == 3 ? PickSpecialRoomNumber() : _specialRoom2;
                    SetSpecialRoom(specialRoom,Side.Right);
                }
                else if(addSpecialRoom1!=2) PossibleShop(Entry.No);
            }

            if (addSpecialRoom1==4||addSpecialRoom2==4)
            {
                Place3RoomSeo(_lastRoomIndex,_nextSide);
                var specialRoom = addSpecialRoom1 == 4 ? PickSpecialRoomNumber() : _specialRoom2;
                SetSpecialRoom(specialRoom,Side.Right);
            }
            else PossibleShop(Entry.So);
        }
        private void FirstLoopDownLeft()
        {
            _nextSide = Side.Down;
            var addSpecialRoom1 = PickSpecialRoomPlacement(0,5);
            var addSpecialRoom2 = 0;
            if(!_inFirstLoop) addSpecialRoom2 = PickSpecialRoomPlacement(addSpecialRoom1,5);
            
            if (addSpecialRoom1==1||addSpecialRoom2==1)
            {
                Place3RoomNso(_lastRoomIndex,_nextSide);
                _nextSide = Side.Left;
                var specialRoom = addSpecialRoom1 == 1 ? PickSpecialRoomNumber() : _specialRoom2;
                SetSpecialRoom(specialRoom,Side.Down);
            }
            else PossibleShop(Entry.No);

            if ((addSpecialRoom1==2&&addSpecialRoom2==3)||(addSpecialRoom1==3&&addSpecialRoom2==2))
            {
                PossibleShop(Entry.FourRoom);
                _nextSide = Side.Up;
                var specialRoomFirst = addSpecialRoom1 == 2 ? PickSpecialRoomNumber() : _specialRoom2;
                var specialRoomSecond = addSpecialRoom1 == 3 ? PickSpecialRoomNumber() : _specialRoom2;
                SetSpecialRoom(specialRoomFirst,Side.Down);
                SetSpecialRoom(specialRoomSecond,Side.Left);
            }
            else
            {
                if (addSpecialRoom1==2||addSpecialRoom2==2)
                {
                    Place3RoomNse(_lastRoomIndex,_nextSide);
                    _nextSide = Side.Up;
                    var specialRoom = addSpecialRoom1 == 2 ? PickSpecialRoomNumber() : _specialRoom2;
                    SetSpecialRoom(specialRoom,Side.Down);
                }
                if (addSpecialRoom1==3||addSpecialRoom2==3)
                {
                    Place3RoomNeo(_lastRoomIndex,_nextSide);
                    _nextSide = Side.Up;
                    var specialRoom = addSpecialRoom1 == 3 ? PickSpecialRoomNumber() : _specialRoom2;
                    SetSpecialRoom(specialRoom,Side.Left);
                }
                else if(addSpecialRoom1!=2&&addSpecialRoom2!=2)PossibleShop(Entry.Ne);
            }

            if (addSpecialRoom1==4||addSpecialRoom2==4)
            {
                Place3RoomSeo(_lastRoomIndex,_nextSide);
                var specialRoom = addSpecialRoom1 == 4 ? PickSpecialRoomNumber() : _specialRoom2;
                SetSpecialRoom(specialRoom,Side.Left);
            }
            else PossibleShop(Entry.Se);
        }
        private void SecondLoop()
        {
            _savedIndex = _lastRoomIndex;
            _lastRoomIndex = 1;
            _inFirstLoop = false;
            if (_secondRoom4Room)
            {
                SecondLoopJoined();
            }
            else SecondLoopUnJoined();
        }
        private void SecondLoopJoined()
        {
            if (_sideFirstLoop==Side.Right)
            {
                SecondLoopJoinedLeft();
            }
            else SecondLoopJoinedRight();
        }
        private void SecondLoopJoinedLeft()
        {
            _nextSide = Side.Up;
            var addSpecialRoom2 = PickSpecialRoomPlacement(0,1);
            var addSpecialRoom3 = PickSpecialRoomPlacement(addSpecialRoom2,6);
            if (addSpecialRoom2==1||addSpecialRoom3==1)
            {
                Place3RoomNso(_lastRoomIndex,_nextSide); //Set 1st Room of the loop on top of the 2nd Room
                _nextSide = Side.Left;
                var specialRoom = (addSpecialRoom2 == 1) ? _specialRoom2 : _specialRoom3;
                _lastRoomIndex = _savedIndex + 1; //Emplacement de la nouvelle salle pour placer la salle spéciale plus tard
                SetSpecialRoom(specialRoom,Side.Up);
            }
            else PossibleShop(Entry.So);

            _lastRoomIndex = _savedIndex + 1; //Algo repart de la nouvelle salle

            if ((addSpecialRoom2==2&&addSpecialRoom3==3)||(addSpecialRoom2==3&&addSpecialRoom3==2))
            {
                PossibleShop(Entry.FourRoom);
                _nextSide = Side.Down;
                var specialRoomFirst = addSpecialRoom2 == 2 ? _specialRoom2 : _specialRoom3;
                var specialRoomSecond = addSpecialRoom2 == 3 ? _specialRoom2 : _specialRoom3;
                SetSpecialRoom(specialRoomFirst,Side.Up);
                SetSpecialRoom(specialRoomSecond,Side.Left);
            }
            else
            {
                if (addSpecialRoom2==2||addSpecialRoom3==2)
                {
                    Place3RoomNse(_lastRoomIndex,_nextSide);
                    _nextSide = Side.Down;
                    var specialRoom = (addSpecialRoom2 == 2) ? _specialRoom2 : _specialRoom3;
                    SetSpecialRoom(specialRoom,Side.Up);
                }
                if (addSpecialRoom2==3||addSpecialRoom3==3)
                {
                    Place3RoomSeo(_lastRoomIndex,_nextSide);
                    _nextSide = Side.Down;
                    var specialRoom = (addSpecialRoom2 == 1) ? _specialRoom2 : _specialRoom3;
                    SetSpecialRoom(specialRoom,Side.Left);
                }
                else if(addSpecialRoom2!=2&&addSpecialRoom3!=2) PossibleShop(Entry.Se);
            }
            
            if ((addSpecialRoom2==2&&addSpecialRoom3==3)||(addSpecialRoom2==3&&addSpecialRoom3==2))
            {
                PossibleShop(Entry.FourRoom);
                _nextSide = Side.Down;
                var specialRoomFirst = (addSpecialRoom2 == 2) ? _specialRoom2 : _specialRoom3;
                var specialRoomSecond = (addSpecialRoom2 == 3) ? _specialRoom2 : _specialRoom3;
                SetSpecialRoom(specialRoomFirst,Side.Up);
                SetSpecialRoom(specialRoomSecond,Side.Left);
            }
            else
            {
                if (addSpecialRoom2==4||addSpecialRoom3==4)
                {
                    Place3RoomNeo(_lastRoomIndex,_nextSide);
                    var specialRoom = (addSpecialRoom2 == 4) ? _specialRoom2 : _specialRoom3;
                    SetSpecialRoom(specialRoom,Side.Left);
                }
                if (addSpecialRoom2==5||addSpecialRoom3==5)
                {
                    Place3RoomNse(_lastRoomIndex,_nextSide);
                    var specialRoom = (addSpecialRoom2 == 5) ? _specialRoom2 : _specialRoom3;
                    SetSpecialRoom(specialRoom,Side.Down);
                }
                else if(addSpecialRoom2!=4&&addSpecialRoom3!=4) PossibleShop(Entry.Ne);
            }
        }
        private void SecondLoopJoinedRight()
        {
            _nextSide = Side.Up;
            var addSpecialRoom2 = PickSpecialRoomPlacement(0,1);
            var addSpecialRoom3 = PickSpecialRoomPlacement(addSpecialRoom2,6);
            if (addSpecialRoom2==1||addSpecialRoom3==1)
            {
                Place3RoomNse(_lastRoomIndex,_nextSide); //Set 1st Room of the loop on top of the 2nd Room
                _nextSide = Side.Right;
                var specialRoom = (addSpecialRoom2 == 1) ? _specialRoom2 : _specialRoom3;
                _lastRoomIndex = _savedIndex + 1; //Emplacement de la nouvelle salle pour placer la salle spéciale plus tard
                SetSpecialRoom(specialRoom,Side.Up);
            }
            else PossibleShop(Entry.Se);
            
            _lastRoomIndex = _savedIndex + 1; //Algo repart de la nouvelle salle

            if ((addSpecialRoom2==2&&addSpecialRoom3==3)||(addSpecialRoom2==3&&addSpecialRoom3==2))
            {
                PossibleShop(Entry.FourRoom);
                _nextSide = Side.Down;
                var specialRoomFirst = addSpecialRoom2 == 2 ? _specialRoom2 : _specialRoom3;
                var specialRoomSecond = addSpecialRoom2 == 3 ? _specialRoom2 : _specialRoom3;
                SetSpecialRoom(specialRoomFirst,Side.Up);
                SetSpecialRoom(specialRoomSecond,Side.Right);
            }
            else
            {
                if (addSpecialRoom2==2||addSpecialRoom3==2)
                {
                    Place3RoomNso(_lastRoomIndex,_nextSide);
                    _nextSide = Side.Down;
                    var specialRoom = (addSpecialRoom2 == 2) ? _specialRoom2 : _specialRoom3;
                    SetSpecialRoom(specialRoom,Side.Up);
                }
                if (addSpecialRoom2==3||addSpecialRoom3==3)
                {
                    Place3RoomSeo(_lastRoomIndex,_nextSide);
                    _nextSide = Side.Down;
                    var specialRoom = (addSpecialRoom2 == 1) ? _specialRoom2 : _specialRoom3;
                    SetSpecialRoom(specialRoom,Side.Right);
                }
                else if(addSpecialRoom2!=2&&addSpecialRoom3!=2) PossibleShop(Entry.So);
            }
            
            if ((addSpecialRoom2==2&&addSpecialRoom3==3)||(addSpecialRoom2==3&&addSpecialRoom3==2))
            {
                PossibleShop(Entry.FourRoom);
                _nextSide = Side.Down;
                var specialRoomFirst = (addSpecialRoom2 == 2) ? _specialRoom2 : _specialRoom3;
                var specialRoomSecond = (addSpecialRoom2 == 3) ? _specialRoom2 : _specialRoom3;
                SetSpecialRoom(specialRoomFirst,Side.Up);
                SetSpecialRoom(specialRoomSecond,Side.Right);
            }
            else
            {
                if (addSpecialRoom2==4||addSpecialRoom3==4)
                {
                    Place3RoomNeo(_lastRoomIndex,_nextSide);
                    var specialRoom = (addSpecialRoom2 == 4) ? _specialRoom2 : _specialRoom3;
                    SetSpecialRoom(specialRoom,Side.Right);
                }
                if (addSpecialRoom2==5||addSpecialRoom3==5)
                {
                    Place3RoomNso(_lastRoomIndex,_nextSide);
                    var specialRoom = (addSpecialRoom2 == 5) ? _specialRoom2 : _specialRoom3;
                    SetSpecialRoom(specialRoom,Side.Down);
                }
                else if(addSpecialRoom2!=4&&addSpecialRoom3!=4) PossibleShop(Entry.No);
            }
        }
        private void SecondLoopUnJoined()
        {
            if (_sideFirstLoop==Side.Right)
            {
                _nextSide = Side.Left;
                FirstLoopLeft();
            }
            else
            {
                _nextSide = Side.Right;
                FirstLoopRight();
            }
        }
        private void PlaceSpecialRooms()
        {
            Filler2RoomBeforeSpecial(_indexForCharacter1,_sideCharacter1);
            PlaceCharacterRoom(_lastRoomIndex,_nextSide);
            Filler2RoomBeforeSpecial(_indexForCharacter2,_sideCharacter2);
            PlaceCharacterRoom(_lastRoomIndex,_nextSide);
            Filler2RoomBeforeSpecial(_indexForBoss,_sideBoss);
            PlaceBossRoom(_lastRoomIndex,_nextSide);
        }
        private void Filler2RoomBeforeSpecial(int indexSpecial,Side side)
        {
            _savedIndex = _lastRoomIndex;
            _lastRoomIndex = indexSpecial;
            AddFiller2Room(side);
            _lastRoomIndex = _savedIndex + 1;
        }
        private void InstantiateRoom(int indexOldRoom,Side side,bool big)
        {
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
                tempX += -_currentRoomPool[_chosenRoom].GetComponent<RefEntry>().entreeSud.transform.position.x;
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
                tempY += -_currentRoomPool[_chosenRoom].GetComponent<RefEntry>().entreeOuest.transform.position.y;
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
                tempX += -_currentRoomPool[_chosenRoom].GetComponent<RefEntry>().entreeNord.transform.position.x;
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
                tempY += -_currentRoomPool[_chosenRoom].GetComponent<RefEntry>().entreeEst.transform.position.y;
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
        private void PlaceShopRoom(int indexOldRoom,Side side,Entry entry)
        {
            ChooseShopRoom(entry);
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
            _nextSide = side == Side.Right ? Side.Right : Side.Left;
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
            _nextSide = side == Side.Down ? Side.Down : Side.Up;
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
        private SpecialRoom PickSpecialRoomNumber()
        {
            var specialRoom = _inFirstLoop ? _specialRoom1 : _specialRoom3;
            return specialRoom;
        }
        private SpecialRoom PickSpecialRoomType()
        {
            SpecialRoom specialRoom;
            if (Random.Range(0,2) == 0)
            {
                specialRoom = SpecialRoom.Boss;
                _bossRoomIsSet = true;
            }
            else specialRoom = SpecialRoom.Character1;
            return specialRoom;
        }
        private int PickSpecialRoomPlacement(int otherRoom, int borneSup)
        {
            var placement = 0;
            while (placement == otherRoom||placement==0)
            {
                placement = Random.Range(1, borneSup);
            }
            return placement;
        }
        private void SetSpecialRoom(SpecialRoom specialRoom, Side side)
        {
            switch (specialRoom)
            {
                case SpecialRoom.Boss:
                    _indexForBoss = _lastRoomIndex;
                    Debug.Log(_indexForBoss);
                    _sideBoss = side;
                    break;
                case SpecialRoom.Character1:
                    _indexForCharacter1 = _lastRoomIndex;
                    Debug.Log(_indexForCharacter1);
                    _sideCharacter1 = side;
                    break;
                case SpecialRoom.Character2:
                    _indexForCharacter2 = _lastRoomIndex;
                    Debug.Log(_indexForCharacter2);
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
                        if (_currentRoomPool[_chosenRoom].GetComponent<RefEntry>().entreeSud.gameObject!=null)
                            entrySide = Side.Down;
                    }
                    break;
                case Side.Right:
                    while (entrySide!=Side.Left)
                    {
                        PickARoomAtRandom();
                        if (_currentRoomPool[_chosenRoom].GetComponent<RefEntry>().entreeOuest.gameObject!=null)
                            entrySide = Side.Left;
                    }
                    break;
                case Side.Down:
                    while (entrySide!=Side.Up)
                    {
                        PickARoomAtRandom();
                        if (_currentRoomPool[_chosenRoom].GetComponent<RefEntry>().entreeNord.gameObject!=null)
                            entrySide = Side.Up;
                    }
                    break;
                case Side.Left:
                    while (entrySide!=Side.Right)
                    {
                        PickARoomAtRandom();
                        if (_currentRoomPool[_chosenRoom].GetComponent<RefEntry>().entreeEst.gameObject!=null)
                            entrySide = Side.Right;
                    }
                    break;
            }
        }
        private void ChooseShopPosition()
        {
            _shopPosition = Random.Range(0, 3);
        }
        private void PossibleShop(Entry entry)
        {
            switch (entry)
            {
                case Entry.FourRoom:
                    if(_checkShopPosition==_shopPosition) PlaceShopRoom(_lastRoomIndex,_nextSide,entry);
                    else Place4Room(_lastRoomIndex,_nextSide);
                    break;
                case Entry.Ns:
                    if(_checkShopPosition==_shopPosition) PlaceShopRoom(_lastRoomIndex,_nextSide,entry);
                    else Place2RoomNs(_lastRoomIndex,_nextSide);
                    break;
                case Entry.Ne:
                    if(_checkShopPosition==_shopPosition) PlaceShopRoom(_lastRoomIndex,_nextSide,entry);
                    else Place2RoomNe(_lastRoomIndex,_nextSide);
                    break;
                case Entry.No:
                    if(_checkShopPosition==_shopPosition) PlaceShopRoom(_lastRoomIndex,_nextSide,entry);
                    else Place2RoomNo(_lastRoomIndex,_nextSide);
                    break;
                case Entry.Se:
                    if(_checkShopPosition==_shopPosition) PlaceShopRoom(_lastRoomIndex,_nextSide,entry);
                    else Place2RoomSe(_lastRoomIndex,_nextSide);
                    break;
                case Entry.So:
                    if(_checkShopPosition==_shopPosition) PlaceShopRoom(_lastRoomIndex,_nextSide,entry);
                    else Place2RoomSo(_lastRoomIndex,_nextSide);
                    break;
                case Entry.Eo:
                    if(_checkShopPosition==_shopPosition) PlaceShopRoom(_lastRoomIndex,_nextSide,entry);
                    else Place2RoomEo(_lastRoomIndex,_nextSide);
                    break;
            }
            _checkShopPosition++;
        }
        private void ChooseShopRoom(Entry entry)
        {
            switch (entry)
            {
                case Entry.FourRoom:
                    _currentRoomPool = Resources.LoadAll("Shop4Room", typeof(GameObject));
                    PickARoomAtRandom();
                    break;
                case Entry.Ns:
                    _currentRoomPool = Resources.LoadAll("Shop2Room/Ns", typeof(GameObject));
                    PickARoomAtRandom();
                    _nextSide = _nextSide == Side.Down ? Side.Down : Side.Up;
                    break;
                case Entry.Ne:
                    _currentRoomPool = Resources.LoadAll("Shop2Room/Ne", typeof(GameObject));
                    PickARoomAtRandom();
                    _nextSide = _nextSide == Side.Left ? Side.Up : Side.Right;
                    break;
                case Entry.No:
                    _currentRoomPool = Resources.LoadAll("Shop2Room/No", typeof(GameObject));
                    PickARoomAtRandom();
                    _nextSide = _nextSide == Side.Right ? Side.Up : Side.Left;
                    break;
                case Entry.Se:
                    _currentRoomPool = Resources.LoadAll("Shop2Room/Se", typeof(GameObject));
                    PickARoomAtRandom();
                    _nextSide = _nextSide == Side.Left ? Side.Down : Side.Right;
                    break;
                case Entry.So:
                    _currentRoomPool = Resources.LoadAll("Shop2Room/So", typeof(GameObject));
                    PickARoomAtRandom();
                    _nextSide = _nextSide == Side.Right ? Side.Down : Side.Left;
                    break;
                case Entry.Eo:
                    _currentRoomPool = Resources.LoadAll("Shop2Room/Eo", typeof(GameObject));
                    PickARoomAtRandom();
                    _nextSide = _nextSide == Side.Right ? Side.Right : Side.Left;
                    break;
            }
        }
    }
}