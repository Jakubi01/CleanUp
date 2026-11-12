using Assets.Scripts.Manager;
using Item.Tools;
using UnityEngine;

namespace Item.Garbage
{
    public class GarbageBase : ItemBase
    {
        public GarbageData data;               // 프리팹마다 에셋 하나 지정
        public ToolType requiredToolType = ToolType.None;
        public int scoreValue = 10;

        protected override void Awake()
        {
            base.Awake();
            OnAction += ActionCall;
        }

        private void ActionCall()
        {
            Debug.Log("GarbageBase::OnAction");

            GameManager.Instance.UnRegister(this);
        }
    }
}