using System;
using Input;
using Interface;
using UnityEngine;
using UserInterface;

namespace Item
{
    public class ItemBase : MonoBehaviour, IInteractable
    {
        [Header("Item Info")]
        public string itemName;
        public string description;
        
        [Header("Item Description UI")]
        [SerializeField] private ItemDescriptor itemDescriptor;

        private float _time;
        private const float ActionTimeThreshold = 1f;
        private bool _canAction;

        /**
         * 각 Item은 해당 Action에 this.Action에 사용될 행동을 등록한다.
         */
        protected Action OnAction;
        
        public bool IsHeld { get; set; }

        private void Awake()
        {
            if (itemDescriptor)
            {
                itemDescriptor.Init(GetDescription());
            }
        }

        protected virtual void Update()
        {
            if (!IsHeld) return;
            
            _time += Time.deltaTime;
            _canAction = _time >= ActionTimeThreshold;
        }
        
        public string GetDescription()
        {
            return $"[{itemName}] {description}";
        }

        public virtual void Interact(XRPlayerInteract player)
        {
            if (!player.CanCollect(this)) return;
            
            player.CollectItem(this);
        }

        public virtual void Action()
        {
            // Item이 action을 수행중일 땐 타이머를 사용해서 return을 해버리고, 
            // 동일한 수행을 짧은 시간 내에 사용하지 못하게 한다.
            if(!_canAction) return;
            
            OnAction?.Invoke();
            
            _time = 0f;
            _canAction = false;
        }

        public void ShowDescription()
        {
            if (!itemDescriptor) return;
            
            itemDescriptor.Show();
        }

        public void HideDescription()
        {
            if (!itemDescriptor) return;
            
            itemDescriptor.Hide();
        }
    }
}