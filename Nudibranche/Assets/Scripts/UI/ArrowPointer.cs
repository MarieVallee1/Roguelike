using System;
using Character;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ArrowPointer : MonoBehaviour
    {
        [SerializeField] private RectTransform pointerRectTransform;
        [SerializeField] private Image image;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float offSet;
        [SerializeField] private float entryDistance;

        private Transform _target;
        private Vector3 _targetPosition;
        private bool _isOffScreen, _isTooFar, _isEnemy, _isShown;

        public void SetTarget(Transform targetTransform, bool isEnemy)
        {
            _target = targetTransform;
            _isEnemy = isEnemy;
            _isShown = true;
        }

        private void Update()
        {
            if (!_isShown) return;
            if (!_target.gameObject.activeInHierarchy)
            {
                _isShown = false;
                return;
            }
            
            _targetPosition = _target.position;
            
            var direction = _targetPosition - playerCamera.transform.position;
            direction.z = 0f;
            direction.Normalize();
            pointerRectTransform.localRotation = Quaternion.FromToRotation(Vector3.down, direction);

            var targetPositionScreenPoint = playerCamera.WorldToScreenPoint(_targetPosition);

            if (_isEnemy)
            {
                _isOffScreen = targetPositionScreenPoint.x <= offSet 
                               || targetPositionScreenPoint.x >= Screen.width-offSet 
                               || targetPositionScreenPoint.y <= offSet
                               || targetPositionScreenPoint.y >= Screen.height-offSet;
                image.enabled = _isOffScreen;
            }
            else
            {
                _isTooFar = (PlayerController.Instance.characterPos - (Vector2)_targetPosition).magnitude > entryDistance;
                image.enabled = _isTooFar;
            }
            
            targetPositionScreenPoint.x = Mathf.Clamp(targetPositionScreenPoint.x, 0f+offSet, Screen.width-offSet);
            targetPositionScreenPoint.y = Mathf.Clamp(targetPositionScreenPoint.y, 0f+offSet, Screen.height-offSet);
            pointerRectTransform.anchoredPosition = targetPositionScreenPoint;
        }
    }
}
