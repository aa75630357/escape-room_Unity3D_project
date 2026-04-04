using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class EndGameTrigger : MonoBehaviour
{
    [Header("UI 參考 (請拖入對應物件)")]
    public CanvasGroup mainUIGroup;    // 玩家原本的 UI (快捷欄、準心等)
    public TextMeshProUGUI thanksText; // "感謝遊玩" 的文字
    public TextMeshProUGUI tobeText;   // "To Be Continued" 的文字
    [Header("玩家手部")]
    public Transform itemHolder;       //讓手上東西慢慢暗掉

    [Header("玩家控制閹割區")]
    // 把會讓玩家移動、轉頭的腳本通通拖進來！
    public MonoBehaviour[] playerScriptsToDisable;
    private bool isEnding = false;

    // 當有東西走進這個觸發器時
    void OnTriggerEnter(Collider other)
    {
        // 檢查走進來的是不是玩家，且確保結局只觸發一次
        if (other.CompareTag("Player") && !isEnding)
        {
            isEnding = true;
            StartEndSequence();
        }
    }

    void StartEndSequence()
    {
        Debug.Log("結局演出開始！");
        if (itemHolder != null)
        {
            // 自動往下找，把手上有掛 Renderer (渲染器/模型) 的物件全部抓出來！
            // 不管是一張紙(3個)，還是木箱(14個)，一網打盡！
            Renderer[] allRenderers = itemHolder.GetComponentsInChildren<Renderer>();

            foreach (Renderer r in allRenderers)
            {
                // 把每個零件的顏色，花 2 秒鐘慢慢變成純黑色！
                // (URP 記得要加上 "_BaseColor" 這個密語)
                r.material.DOColor(Color.black, "_BaseColor", 2f);
            }
        }
            //玩家主 UI 淡淡消失 (花費 1 秒)
        if (mainUIGroup != null) mainUIGroup.DOFade(0f, 2f).OnComplete(() =>
        {
            //螢幕漸漸變成純黑 (花費 2 秒)
            //螢幕全黑後，鎖定玩家視角跟滑鼠 (避免他們在字幕時亂看)
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //關閉移動功能和滑鼠功能
            foreach (MonoBehaviour script in playerScriptsToDisable)
            {
                if (script != null) script.enabled = false;
            }
            if (thanksText != null)
            {
                thanksText.DOFade(1f, 2f).OnComplete(() =>
                {
                    //等待 1 秒後，浮現 "To Be Continued" (花費 2 秒)
                    DOVirtual.DelayedCall(1f, () =>
                    {
                    if (tobeText != null) tobeText.DOFade(1f, 2f);
                    });
                });
            }
        });
    }
}