using UnityEngine;

public class PlaceableZone : MonoBehaviour
{
    [Header("放置設定")]
    public Vector3 placeOffset = Vector3.zero;      // 放置位置偏移
    public Vector3 placeRotation = Vector3.zero;    // 放置角度

    private bool isOccupied = false;                // 是否已有物品放置
    private GameObject placedItem = null;           // 目前放置的物品

    // 是否可以放置
    public bool CanPlace() => !isOccupied;

    // 放置物品
    public void PlaceItem(GameObject item)
    {
        isOccupied = true;
        placedItem = item;
        // 移動到放置位置並對齊角度
        item.transform.position = transform.position + placeOffset;
        item.transform.eulerAngles = placeRotation;
    }

    // 拿走物品
    public GameObject TakeItem()
    {
        isOccupied = false;
        GameObject item = placedItem;
        placedItem = null;
        return item;
    }

    // 取得放置的物品
    public GameObject GetPlacedItem() => placedItem;
}