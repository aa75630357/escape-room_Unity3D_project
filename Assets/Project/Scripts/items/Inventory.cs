using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;                       // 靜態單例，方便其他腳本呼叫

    // 存放所有拾取的 WorldItem 清單
    // List 是可變長度的陣列，可以隨時 Add 新物品
    private List<ItemData> items = new List<ItemData>();
    private List<GameObject> itemModels = new List<GameObject>();


    void Awake()
    {
        Instance = this;
    }

    // 拾取物品時呼叫
    // WorldItem item = 被拾取的物品
    public void AddItem(WorldItem worldItem)
    {
        items.Add(worldItem.itemData);                        // 加入清單
        itemModels.Add(worldItem.itemModel);
        HotbarManager.Instance.RefreshHotbar(); // 通知 HotbarManager 更新顯示
        HotbarManager.Instance.ShowCurrentItem(); // ← 加這行，拾取後馬上顯示到手上
    }

    // 取得指定索引的物品，給 HotbarManager 用
    // 索引超出範圍就回傳 null
    public ItemData GetItem(int index)
    {
        if (index < 0 || index >= items.Count) return null;
        return items[index];
    }
    public GameObject GetItemModel(int index)
    {
        if (index < 0 || index >= itemModels.Count) return null;
        return itemModels[index];
    }

    // 取得目前背包物品總數，給 HotbarManager 計算頁數用
    public int GetItemCount() => items.Count;

    //刪除已經放下的物品
    public void RemoveItemByModel(GameObject model)
    {
    int index = itemModels.IndexOf(model);  // 找到對應索引
    if(index < 0) return;
    itemModels.RemoveAt(index);
    items.RemoveAt(index);
    }


}