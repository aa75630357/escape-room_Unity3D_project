using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using System;

public class Cabinet02_Controller : MonoBehaviour, IManagerOPCLAll
{
    [Header("Cabinet設定")]
    public Vector3 openAngle = new Vector3 (0,90,0);   //開門角度
    public float duration = 0.5f;   //動畫時間
    public Ease openEase  = Ease.OutCubic;  // 開啟緩動
    public Ease closeEase = Ease.InCubic;   // 關閉緩動

    private bool isAnimating = false;   // 是否在動畫中
    private bool isOpen = false;        // 是否開啟
    private Quaternion closeRot;        // 關閉時的旋轉角度
    private Quaternion openRot;         // 開啟時的旋轉角度
    [Header("UI設定")]
    public string interactLabel = "櫃門";
    public Vector3 uiOffset = new Vector3(0.2f, 0.15f, 0f);
    public string GetLabel() => interactLabel;
    public Vector3 GetUIOffset() => uiOffset;

    void Start()
    {
        closeRot = transform.localRotation; //關閉的
        // 計算開啟時的旋轉角度
        // 在關閉角度的基礎上，繞 Y 軸旋轉 openAngle 度
        openRot = closeRot * Quaternion.Euler(openAngle);
    }
    public void ToggleDrawer()
    {
        // 動畫中不能再觸發
        if (isAnimating) return;

        isOpen = !isOpen;
        isAnimating = true;

        // 旋轉到開啟或關閉的角度
        transform
            .DOLocalRotateQuaternion(isOpen ? openRot : closeRot, duration)
            .SetEase(isOpen ? openEase : closeEase)
            .OnComplete(() => isAnimating = false); // 動畫結束解除鎖定
    }
}
