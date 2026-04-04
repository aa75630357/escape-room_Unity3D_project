using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
// 🔥 URP 特效專用命名空間
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; 

public class AwakeSequence : MonoBehaviour
{
    [Header("UI 參考")]
    public CanvasGroup mainUIGroup;    // 玩家原本的 UI
    public Image blackPanel;           // 全黑 UI Panel

    [Header("玩家控制閹割區")]
    public MonoBehaviour[] playerScriptsToDisable; 

    // 🔥 修正：類別名稱不能有空格！
    private Volume volume;
    private DepthOfField dof;
    private ChromaticAberration ca;

    void Start()
    {
        // 1. 初始化環境
        foreach (MonoBehaviour script in playerScriptsToDisable)
        {
            if (script != null) script.enabled = false;
        }
        if(mainUIGroup != null) mainUIGroup.alpha = 0f;
        if(blackPanel != null) blackPanel.color = new Color(0,0,0,1);
        
        // 2. 抓取場景裡的 Global Volume
        volume = GameObject.FindObjectOfType<Volume>();
        if(volume != null && volume.profile.TryGet(out dof) && volume.profile.TryGet(out ca))
        {
            // 🔥 修正：Gaussian 模式下的模糊參數叫做 gaussianEnd
            dof.gaussianEnd.value = 1f;
            ca.intensity.value = 1f;
        }

        // 3. 啟動演出
        StartWakeUp();
    }

    void StartWakeUp()
    {
        Sequence mySequence = DOTween.Sequence();

        mySequence.AppendInterval(1f);

        // 【第一次眨眼】 
        mySequence.Append(blackPanel.DOFade(0.6f, 0.5f).SetEase(Ease.InOutQuad));
        mySequence.AppendInterval(0.2f);
        mySequence.Append(blackPanel.DOFade(1f, 0.3f).SetEase(Ease.InOutQuad));

        // 【第二次眨眼】
        mySequence.Append(blackPanel.DOFade(0.3f, 0.7f).SetEase(Ease.InOutQuad));
        // 🔥 修正為 gaussianEnd
        if(dof != null) mySequence.Join(DOTween.To(()=> dof.gaussianEnd.value, x=> dof.gaussianEnd.value = x, 5f, 0.7f)); 
        
        mySequence.AppendInterval(0.4f);
        mySequence.Append(blackPanel.DOFade(1f, 0.4f).SetEase(Ease.InOutQuad));

        mySequence.AppendInterval(0.3f);

        // 【最終甦醒與對焦】 
        mySequence.Append(blackPanel.DOFade(0f, 1f).SetEase(Ease.InOutQuad));
        if(ca != null) mySequence.Join(DOTween.To(()=> ca.intensity.value, x=> ca.intensity.value = x, 0f, 1f));

        // 【視線清晰】 
        // 🔥 修正為 gaussianEnd，將數值拉高讓畫面完全清晰
        if(dof != null) mySequence.Append(DOTween.To(()=> dof.gaussianEnd.value, x=> dof.gaussianEnd.value = x, 100f, 2f));
        
        // --- 演出結束 ---
        mySequence.OnComplete(() =>
        {
            foreach (MonoBehaviour script in playerScriptsToDisable)
            {
                if (script != null) script.enabled = true;
            }
            if(mainUIGroup != null) mainUIGroup.DOFade(1f, 1f);
            if(blackPanel != null) blackPanel.gameObject.SetActive(false);
            
            Debug.Log("開場甦醒演出成功殺青！");
        });
    }
}