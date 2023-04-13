using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FindTheBirds
{
    public class RemoveObjectFromInventoryBehaviour_Bulk : MonoBehaviour
    {
        [SerializeField] private ItemSO[] _itemsToRemove;
        [SerializeField] private PlayerInventoryData _inventoryScriptable;
        [SerializeField] private int _quantity;

        [ContextMenu("Debug/Remove Objects")]
        public void RemoveObjects()
        {
            //For each item in the array, remove the quantitiy desired
            foreach(ItemSO item in _itemsToRemove)
            {
                _inventoryScriptable.RemoveItem(item.GetItemData, _quantity);
            }
        }
    }
}


