using TMPro;
using UnityEngine;

namespace UserInterface
{
    public class ItemDescriptor : UserInterface
    {
        [SerializeField] private TMP_Text descriptionText;
        private CanvasGroup _canvasGroup;

        private Transform _mainCameraTransform;

        public void Init(string description)
        {
            if (descriptionText)
            {
                descriptionText.text = description;
            }
        }
        
        private void Start()
        {
            if (Camera.main)
            {
                _mainCameraTransform = Camera.main.transform;
            }
            
            _canvasGroup = GetComponent<CanvasGroup>();
            if (!_canvasGroup)
            {
                Debug.Log("canvas not found. add component in gameObject.");
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            
            Hide();
        }

        private void LateUpdate()
        {
            if (_canvasGroup && _canvasGroup.alpha > 0 && _mainCameraTransform)
            {
                transform.rotation = Quaternion.LookRotation(_mainCameraTransform.forward, _mainCameraTransform.up);
            }
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
            
            _canvasGroup.alpha = value ? 1f : 0f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }
    }
}
