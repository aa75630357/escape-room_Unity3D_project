using UnityEngine;
using TMPro;
using DG.Tweening;
//暫時停用
public class ControlHintUI : MonoBehaviour
{
    public static ControlHintUI Instance;       // 靜態單例，方便其他腳本呼叫

    [SerializeField] private TextMeshProUGUI hintText;      // 拖入 HintText
    [SerializeField] private CanvasGroup canvasGroup;       // 拖入 ControlHintPanel 的 CanvasGroup

    [Header("顯示設定")]
    public float autoHideDelay  = 1.5f;         // 幾秒沒呼叫後自動淡出
    public float fadeOutDuration = 0.3f;        // 淡出時間

    private Tween hideTween;                    // 記錄計時器，方便重置

    void Awake()
    {
        Instance = this;
        canvasGroup.alpha = 0;                  // 預設隱藏
    }
    // 顯示提示文字，每幀呼叫也沒關係
    public void ShowUI(string hint)
    {
        // 重置計時器，只要有人呼叫就不會消失
        hideTween?.Kill();

        hintText.text = hint;                   // 設定文字
        canvasGroup.alpha = 1f;                 // 直接顯示，不淡入

        // 重新開始計時，1.5秒後淡出
        hideTween = DOVirtual.DelayedCall(autoHideDelay, () =>
        {
            canvasGroup.DOFade(0f, fadeOutDuration); // 淡出
        });
    }

    // 手動隱藏（需要馬上關掉時用）
    public void HideUI()
    {
        hideTween?.Kill();                      // 停掉計時器
        canvasGroup.DOFade(0f, fadeOutDuration); // 淡出
    }
}