using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class WoodBox_Controller : MonoBehaviour
{
    [Header("視角設定")]
    public Camera boxCamera;                    // 木箱專用 Camera
    public Camera playerCamera;                 // 玩家主 Camera
    public playerRaycaster raycasterScript;     // 關閉雷射
    public Outline Outline;                     // 關閉online
    public TextMeshProUGUI LabelText;          //關閉UI

    [Header("閉眼效果")]
    public Image blackPanel;                    // 全黑 UI Panel，拖入
    public float blinkDuration = 0.3f;          // 閉眼開眼時間

    [Header("物品定位")]
    public Transform paperAnchor;               // 紙的定位點，放在木箱後方

    private bool isBoxMode = false;             // 目前是否在木箱視角
    private Transform currentPaper = null;      //紀錄物品

    // E鍵呼叫這個
    public void UseBox()
    {
        if(!isBoxMode)
        {   
            LightBulb[] bulbs = GetComponentsInChildren<LightBulb>();
            foreach(LightBulb L in bulbs)L.OnHitBox();
            PutItem();
            EnterBoxMode();
        }
        else
        {   
            MoveItem();
            ExitBoxMode();
        }    
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && boxCamera.enabled) 
        {
            LightBulb[] bulbs = GetComponentsInChildren<LightBulb>();
            foreach(LightBulb L in bulbs)L.OffHitBox();
            MoveItem();
            ExitBoxMode();
        }
    }

    void EnterBoxMode()
    {   
        if (LabelText != null) LabelText.enabled = false;
        if (Outline != null) Outline.enabled = false;
        Debug.Log("1. 開始閉眼動畫...");
        if(raycasterScript != null) raycasterScript.enabled = false;

        // 閉眼 → 切換視角 → 開眼
        blackPanel.DOFade(1f, blinkDuration)
            .OnComplete(() =>
            {
                Debug.Log("2. 閉眼完成！正在切換相機...");
                isBoxMode = true;
                playerCamera.enabled = false;   // 關閉玩家視角
                boxCamera.enabled = true;       // 開啟木箱視角

                // 解放滑鼠
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                
                Debug.Log("3. 開始開眼動畫...");
                blackPanel.DOFade(0f, blinkDuration); // 開眼
            });
    }

    void ExitBoxMode()
    {   
        
        // 離開木箱前先鎖住滑鼠，避免游標亂飄
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 閉眼 → 切換回玩家視角 → 開眼
        blackPanel.DOFade(1f, blinkDuration)
            .OnComplete(() =>
            {
                isBoxMode = false;
                boxCamera.enabled = false;
                playerCamera.enabled = true;

                if(raycasterScript != null) raycasterScript.enabled = true;
                blackPanel.DOFade(0f, blinkDuration);
                if (LabelText != null) LabelText.enabled = true;
                if (Outline != null) Outline.enabled = true;
            });
    }

    void PutItem()
    {
        if(HotbarManager.Instance != null) HotbarManager.Instance.enabled = false;
        Transform holder = HotbarManager.Instance.itemHolder;
        if (holder != null && holder.childCount > 0)
        {
            currentPaper = holder.GetChild(0);           // 抓出手上的紙
            currentPaper.SetParent(paperAnchor);         // 把紙的爸爸換成木箱的定位點
            
  
            // 順滑地歸零座標和角度 (設定面向)
            currentPaper.DOLocalMove(Vector3.zero, 0.5f);
            currentPaper.DOLocalRotate(Vector3.zero, 0.5f); 
            // (💡 如果紙的面向不對，把上面這行的 Vector3.zero 改成對應的角度，例如 new Vector3(90, 0, 0))
        }
    }

    void MoveItem()
    {
        if(HotbarManager.Instance != null) HotbarManager.Instance.enabled = true;
        if (currentPaper != null)
        {   
            Transform holder = HotbarManager.Instance.itemHolder;
            currentPaper.SetParent(holder);
            currentPaper.DOLocalMove(Vector3.zero, 0.3f);
            currentPaper.DOLocalRotate(Vector3.zero, 0.3f);
            // 關閉 Collider，避免拿在手上的時候擋住玩家原本的視線雷射
            Collider paperCol = currentPaper.GetComponent<Collider>();
            if (paperCol != null) paperCol.enabled = false;
            
            currentPaper = null; // 清空紀錄
        }
    }

}