using UnityEngine;
using DG.Tweening;

public class WorldItem : MonoBehaviour, IPickable
{ 
    [Header("物品資料")]
    public ItemData itemData;  // ← 加這行！拖入對應的 ItemData ScriptableObject
    public GameObject itemModel;
    [Header("拾取設定")]
    public Vector3 hidePosition = new Vector3(0, -100f, 0);  // 拾取後移到這裡
    public float moveDuration = 0.5f;                         // 移動時間

    [Header("互動提示")]
    public string interactLabel = "Paper";
    public Vector3 uiOffset = new Vector3(0.2f, 0.15f, 0f);

    // 實作介面
    public string GetLabel() => interactLabel;
    public Vector3 GetUIOffset() => uiOffset;

    public void OnPickup()
    {
        Debug.Log("OnPickup 觸發！itemData = " + itemData);
        // 順移到看不見的地方
        transform.DOMove(hidePosition, moveDuration)
            .OnComplete(() =>
            {
                // 移動完成後通知 Inventory 加入物品
                Inventory.Instance.AddItem(this);
            });
    }
}