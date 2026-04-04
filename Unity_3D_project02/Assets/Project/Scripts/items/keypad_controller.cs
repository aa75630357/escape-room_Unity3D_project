using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
// 🔥 引用作者的命名空間，這樣我們才能抓到他的腳本
using NavKeypad; 

public class Keypad_Controller : MonoBehaviour, IInteractable
{
    [Header("視角與腳本設定")]
    public Camera keypadCamera;                 // 密碼鎖專用 Camera
    public Camera playerCamera;                 // 玩家主 Camera
    public playerRaycaster raycasterScript;     // 玩家的射線腳本
    public KeypadInteractionFPV interactScript; // 作者寫的按鈕互動腳本！

    [Header("閉眼效果")]
    public Image blackPanel;                    // 全黑 UI Panel
    public float blinkDuration = 0.3f;          // 閉眼開眼時間

    private bool isKeypadMode = false;
    public string GetLabel()
    {
        return ""; // 射線指到時，畫面上會顯示的 UI 文字！
    }

    public Vector3 GetUIOffset()
    {
        return new Vector3(0, 0.3f, 0); // UI 顯示在密碼鎖上方多高的位置 (你可以自己微調)
    }

    void Start()
    {
        // 一開始先關閉作者的按鈕互動腳本，避免玩家在遠處亂點到
        if(interactScript != null) interactScript.enabled = false;
    }

    public void UseKeypad()
    {
        if(!isKeypadMode) EnterKeypadMode();
        else ExitKeypadMode();
    }

    void Update()
    {
        // 在密碼鎖模式下按 E 退出
        if(Input.GetKeyDown(KeyCode.E) && isKeypadMode) ExitKeypadMode();
    }

    void EnterKeypadMode()
    {   
        if(raycasterScript != null) raycasterScript.enabled = false;

        Collider myCollider = GetComponent<Collider>();
        if (myCollider != null) myCollider.enabled = false
        ;
        blackPanel.DOFade(1f, blinkDuration).OnComplete(() =>
        {
            isKeypadMode = true;
            playerCamera.enabled = false;   
            keypadCamera.enabled = true;       

            // 解放滑鼠，並開啟作者的按鈕腳本讓玩家點擊！
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if(interactScript != null) interactScript.enabled = true;
            
            blackPanel.DOFade(0f, blinkDuration);
        });
    }

    void ExitKeypadMode()
    {   
        // 離開前鎖住滑鼠，並關閉作者的按鈕腳本
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if(interactScript != null) interactScript.enabled = false;

        Collider myCollider = GetComponent<Collider>();
        if (myCollider != null) myCollider.enabled = true;
        
        blackPanel.DOFade(1f, blinkDuration).OnComplete(() =>
        {
            isKeypadMode = false;
            keypadCamera.enabled = false;
            playerCamera.enabled = true;

            if(raycasterScript != null) raycasterScript.enabled = true;
            blackPanel.DOFade(0f, blinkDuration);
        });
    }
}