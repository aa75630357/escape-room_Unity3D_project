using UnityEngine;
using DG.Tweening;
using TMPro;

public class InteractUI : MonoBehaviour
{
    public static InteractUI Instance;  //靜態可直接被呼叫
    //SerializeField是為了顯示在Inspector可以拖拉不用public是為了安全不讓外部腳本改變到
    // 拖入 LabelText 物件
    [SerializeField] private TextMeshProUGUI labelText;
    // 拖入 Canvas 本身的 CanvasGroup
    [SerializeField] private CanvasGroup canvasGroup;     

    void Awake()
    {
        Instance = this;
        canvasGroup.alpha = 0;          //預設隱藏
    }
    public void Show(string label, Vector3 worldPos)
    {
        transform.position = worldPos;  // 移動到目標位置
        labelText.text = label;         // 設定文字內容
        DOTween.Kill(canvasGroup);      //停止淡出或淡入
        canvasGroup.DOFade(1f,0.2f);    //淡入
    }
    public void Hide()
    {
        DOTween.Kill(canvasGroup);      //停止淡出或淡入
        canvasGroup.DOFade(0f,0.2f);    //淡出
    }
}
