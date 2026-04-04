using UnityEngine;
using TMPro;
using DG.Tweening;

public class ItemNameUI : MonoBehaviour
{
    public static ItemNameUI Instance;          // 靜態單例，方便其他腳本呼叫

    [SerializeField] private TextMeshProUGUI itemNameText;  // 拖入 ItemNameText
    [SerializeField] private CanvasGroup canvasGroup;       // 拖入 ItemNamePanel 的 CanvasGroup

    [Header("顯示設定")]
    public float displayDuration = 2f;        // 顯示幾秒後淡出
    public float fadeInDuration  = 0.2f;        // 淡入時間
    public float fadeOutDuration = 0.5f;        // 淡出時間

    private Tween currentTween;                 // 記錄目前的動畫，方便中途取消

    void Awake()
    {
        Instance = this;
        canvasGroup.alpha = 0;                  // 預設隱藏
    }

    // 顯示物品名稱，換物品時呼叫這個
    public void Show(string itemName)
    {
        //刪掉時間計算
        currentTween?.Kill();                   //?是安全措施只有不是null才會執行
        //刪掉淡出淡入動畫
        DOTween.Kill(canvasGroup);

        itemNameText.text = itemName;           // 設定文字

        // 淡入 → 等待 displayDuration 秒 → 淡出
        //淡入執行 fadeInDuration秒
        canvasGroup.DOFade(1f, fadeInDuration)
            //動畫結束後執行
            .OnComplete(() =>
            {
                //DOVirtual 是 DOTween 提供的工具類別，專門做一些「不綁在物件上」的虛擬動畫，像是純計時、數值變化等。
                currentTween = DOVirtual.DelayedCall(displayDuration, () =>
                {
                    canvasGroup.DOFade(0f, fadeOutDuration);
                });
            });
    }
}