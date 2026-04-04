using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("物品資料")]
    public string itemName;         // 物品名稱
    public string itemDescription;  // 物品描述
    public Sprite itemIcon;         // Hotbar 顯示的 2D 圖示
    //public GameObject itemModel;    // 場景中的 3D 模型參考
}