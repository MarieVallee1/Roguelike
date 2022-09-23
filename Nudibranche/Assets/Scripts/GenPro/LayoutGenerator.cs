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
        private int _currentRoom;
        private Vector3 _newPos;
        private void Start()
        {
            _currentRoom = 0;
            PlaceStartRoom();

            _seoRoom = Resources.LoadAll("3Room/SEO", typeof(GameObject));
            _newPos = new Vector3(0, listSalle[0].transform.lossyScale.y / 2 + _seoRoom[0].GameObject().transform.lossyScale.y / 2, 0);
            
            InstantiateRoom(_seoRoom,_newPos);
            Debug.Log(listSalle);
        }

        private void InstantiateRoom(IReadOnlyList<Object> salle,Vector3 roomPosition)
        {
            var chosenRoom = Random.Range(0, salle.Count);
            listSalle[_currentRoom] = Instantiate(salle[chosenRoom].GameObject(),roomPosition,quaternion.identity,transform);
            _currentRoom++;
        }

        private void PlaceStartRoom()
        {
            _startRoom = Resources.LoadAll("StartRoom", typeof(GameObject));
            InstantiateRoom(_startRoom, _newPos);
        }
        
    }
}
