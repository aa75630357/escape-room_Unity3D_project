using UnityEngine;
using DG.Tweening;
public class Drawer20_Controller : MonoBehaviour, IManagerOPCLAll
{
    [Header("抽屜設定")]
    public float slideDistance = 0.004f;      //移動距離
    public float duration = 0.5f;           //時間
    //滑動方向  Vector3.up = (0,1,0)  ，  Vector3.forward = (0,0,1)
    //slideDirection是可開啟家具在沒有轉向的情況下開啟方向是哪裡
    public Vector3 slideDirection = new Vector3(0,-1,0); 
    public Ease openEase  = Ease.OutCubic;  // 開啟緩動
    public Ease closeEase = Ease.InCubic;   // 關閉緩動
    private bool isAnimating = false;        //是否在動畫中
    private bool isOpen = false;            //是否開啟
    private Vector3 closePos;               //開啟座標
    private Vector3 openPos;                //關閉座標
    [Header("互動提示")]
    public string interactLabel = "drawer";   // 每個抽屜可以填不同名字
    public string GetLabel() => interactLabel;
    // UI 顯示在抽屜右上方，可在 Inspector 微調
    public Vector3 uiOffset = new Vector3(-0.745f, -0.244f, 0.276f);
    public Vector3 GetUIOffset() => uiOffset;

    void Start()
    {
        closePos = transform.localPosition; //紀錄座標
        // 根據滑動方向與距離，計算出「開啟位置」
        // closePos 關閉位置 + (旋轉角度把角度轉換成對應的方向向量)*(確保距離計算正確為1這樣才會正常跑預設距離)*(預設距離)
        openPos = closePos + transform.localRotation * slideDirection.normalized * slideDistance;
    }
    public void ToggleDrawer()
    {   //動畫開始時候無法再使用
        if(isAnimating) return;
        isOpen = !isOpen;   //開啟或關閉
        isAnimating = true; //鎖定動畫結束前不許再觸發
        //定位 是否開啟 ? 開了就是開的位址 : 關了就是關的位址 , 動畫時間
        transform
            .DOLocalMove(isOpen ? openPos : closePos , duration)
            //SetEase：設定緩動曲線
            .SetEase(isOpen ? Ease.OutCubic : Ease.InCubic)
            //OnComplete :結束動畫
            .OnComplete(() => isAnimating = false); // 解除鎖定
    }
}
