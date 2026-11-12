using Assets.Scripts.Manager;
using Item.Tools;
using UnityEngine;

namespace Item.Garbage
{
    public class GarbageBase : ItemBase
    {
        public GarbageType type;
        public ToolType requiredToolType = ToolType.None;
        public int scoreValue = 10;

        protected override void Awake()
        {
            base.Awake();
            OnAction += ActionCall;
        }

        private void ActionCall()
        {
            GameManager.Instance.UnRegister(this);
        }

        public void CompleteRemove()
        {
            NotifyHolderItemDestroyed();
            Destroy(gameObject);
        }
    }
}
