using UnityEngine;
using DG.Tweening;

public class PlaceableItem : MonoBehaviour
{
    [Header("預覽設定")]
    public Material previewMaterial;        // 半透明預覽 Material
    public float moveDuration = 0.3f;       // 放置動畫時間

    private Material originalMaterial;     // 原本的 Material
    private Renderer itemRenderer;         // 物件的 Renderer 控制material的控制
    private PlaceableZone currentZone;     // 目前放置的區域
    private bool isPlaced = false;         // 是否已放置
    //桌面動畫判斷
    private PlaceableZone currentPreviewZone = null;

    void Start()
    {
        itemRenderer = GetComponent<Renderer>();
        originalMaterial = itemRenderer.material;   // 記錄原本 Material
    }

    // 顯示預覽（半透明）
    public void ShowPreview(PlaceableZone zone)
    {
        if (currentPreviewZone != zone)
        {
        currentPreviewZone = zone;
        itemRenderer.material = previewMaterial;    // 換成半透明 Material
        // 殺掉之前所有的移動動畫，避免衝突
            transform.DOKill(); 
            
            // 算出桌子的目標位置與角度
            Vector3 targetPos = zone.transform.position + zone.placeOffset;
            
            // 加上滑順的 DOtween 飛過去！
            transform.DOMove(targetPos, moveDuration).SetEase(Ease.OutCubic);
            transform.DORotate(zone.placeRotation, moveDuration).SetEase(Ease.OutCubic);
        }
    }

    // 隱藏預覽，回到原本狀態
    public void HidePreview()
    {
        if (currentPreviewZone != null)
        {
            currentPreviewZone = null;
            itemRenderer.material = originalMaterial;
            // 移回 itemHolder 位置
            foreach(Transform child in GetComponentsInChildren<Transform>())
            child.gameObject.layer = LayerMask.NameToLayer("interactive furniture");
            transform.DOLocalMove(Vector3.zero, 0.2f);
        }
    }

    // 確認放置
    public void Place(PlaceableZone zone)
    {
        isPlaced = true;
        currentZone = zone;
        itemRenderer.material = originalMaterial;   // 換回原本 Material
        transform.SetParent(null);                  // ← 加這行，離開 itemHolder
        //強制定位：預防玩家點太快，木箱還沒飛完就按左鍵，所以放下的瞬間強制對齊！
        transform.DOKill();
        transform.position = zone.transform.position + zone.placeOffset;
        transform.eulerAngles = zone.placeRotation;
        zone.PlaceItem(gameObject);                 // 通知 PlaceableZone
        //開啟 hit box
        GetComponent<Collider>().enabled = true;
        // 改成可被互動射線打到的 Layer
        gameObject.layer = LayerMask.NameToLayer("interactive furniture");
        Inventory.Instance.RemoveItemByModel(gameObject);
        HotbarManager.Instance.RefreshHotbar();
    }

    // 拿走
    public void TakeBack()
    {
        isPlaced = false;
        currentZone.TakeItem();                     // 通知 PlaceableZone 清空
        currentZone = null;
    }

    public bool IsPlaced() => isPlaced;
}