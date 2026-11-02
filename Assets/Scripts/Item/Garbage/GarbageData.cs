using UnityEngine;

namespace Item.Garbage
{
    public enum GarbageType { General, Recycle, Hazardous }
    
    [CreateAssetMenu(menuName = "Item/Garbage Data")]
    public class GarbageData : ScriptableObject
    {
        public GarbageType type;
        public GameObject prefab;   // 다시 꺼낼 때 스폰할 프리팹
        public Sprite icon;         // UI 연결용
        public int maxStack = 5;
    }
}