using TMPro;
using UnityEngine;

namespace UserInterface
{
    public class ItemDescriptor : UserInterface
    {
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private float uiOffsetUp = 0.5f;
        
        private CanvasGroup _canvasGroup;
        private Transform _mainCameraTransform;
        private Transform _parentItemTransform;
        private bool _uiVisible;
        
        public void Init(string description)
        {
            if (descriptionText != null)
            {
                descriptionText.text = description;
                Debug.Log("Init");
            }
        }
        
        private void Start()
        {
            if (Camera.main)
            {
                _mainCameraTransform = Camera.main.transform;
            }

            _parentItemTransform = transform.parent;
            
            _canvasGroup = GetComponent<CanvasGroup>();
            if (!_canvasGroup)
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            
            Hide();
        }

        private void LateUpdate()
        {
            UIOffset();
        }

        public void Show()
        {
            CanvasGroupVisibleControl(true);
        }

        public void Hide()
        {
            CanvasGroupVisibleControl(false);
        }

        private void CanvasGroupVisibleControl(bool value)
        {
            if (!_canvasGroup) return;

            _uiVisible = value;
            _canvasGroup.alpha = value ? 1f : 0f;
            _canvasGroup.interactable = value;
            _canvasGroup.blocksRaycasts = value;
        }

        private void UIOffset()
        {
            if (!_uiVisible) return;
            
            if (!_canvasGroup || !(_canvasGroup.alpha > 0)) return;
            
            if (_parentItemTransform)
            {
                transform.position = _parentItemTransform.position + Vector3.up * uiOffsetUp;
            }

            if (_mainCameraTransform)
            {
                transform.rotation = Quaternion.LookRotation(_mainCameraTransform.forward, _mainCameraTransform.up);
            }
        }
    }
}
