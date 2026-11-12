using System;
using System.Collections.Generic;
using Item.Garbage;
using UnityEngine;

namespace Item.InventorySystem
{
    public class Slot
    {
        public GarbageData Data;
        public int Count;
        public bool IsEmpty => Count == 0;
    }

    public class Inventory : MonoBehaviour
    {
        [SerializeField] private int slotCount = 20;

        private Slot[] _slots;

        public IReadOnlyList<Slot> Slots => _slots;
        public event Action<int> SlotChanged;

        private void Awake()
        {
            _slots = new Slot[slotCount];
            for (var i = 0; i < _slots.Length; i++)
                _slots[i] = new Slot();
        }

        // public bool CanCollect(GarbageBase garbage)
        // {
        //     if (!garbage || !garbage.data) return false;
        //
        //     foreach (var slot in _slots)
        //     {
        //         if (slot.IsEmpty) return true;
        //         if (slot.Data == garbage.data && slot.Count < garbage.data.maxStack) return true;
        //     }
        //     return false;
        // }
        //
        // /// <returns>넣지 못하고 남은 수량 (0이면 전부 수집됨)</returns>
        // public int Collect(GarbageBase garbage, int amount = 1)
        // {
        //     if (!garbage || !garbage.data || amount <= 0) 
        //         return Mathf.Max(amount, 0);
        //
        //     var data = garbage.data;
        //
        //     // 같은 종류의 기존 스택부터 채움
        //     for (var i = 0; i < _slots.Length && amount > 0; i++)
        //     {
        //         var slot = _slots[i];
        //         if (!slot.IsEmpty && slot.Data == data)
        //             amount -= Fill(i, data, amount);
        //     }
        //
        //     // 남으면 빈 칸에 새 스택 생성
        //     for (var i = 0; i < _slots.Length && amount > 0; i++)
        //     {
        //         if (_slots[i].IsEmpty)
        //             amount -= Fill(i, data, amount);
        //     }
        //
        //     return amount;
        // }

        private int Fill(int index, GarbageData data, int amount)
        {
            var slot = _slots[index];
            var added = Mathf.Min(amount, data.maxStack - slot.Count);
            if (added <= 0) return 0;

            slot.Data = data;
            slot.Count += added;
            SlotChanged?.Invoke(index);
            return added;
        }
    }
}