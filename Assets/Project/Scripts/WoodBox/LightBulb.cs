using UnityEngine;

public class LightBulb : MonoBehaviour
{
    [Header("燈泡設定")]
    public Outline bulbOutline;     // 拖入這顆燈泡身上的 Outline 組件 (你說的 online)
    public GameObject lightSource;  // 拖入你要開關的光源 (例如 red_Light, blue_Light)
    public Camera boxCamera;        // 木箱專用 Camera判斷是否開啟hit Box  
    private bool isLightOn = false; // 記錄燈現在是開還是關
    
    void Start()
    {
        // 遊戲一開始，確保 Outline (外框) 是關閉的
        if (bulbOutline != null) bulbOutline.enabled = false;
    }
   

    // 當滑鼠游標「碰到」這顆燈泡時 (亮外框)
    void OnMouseEnter()
    {
        if (bulbOutline != null) bulbOutline.enabled = true;
    }

    // 當滑鼠游標「離開」這顆燈泡時 (熄滅外框)
    void OnMouseExit()
    {
        if (bulbOutline != null) bulbOutline.enabled = false;
    }

    // 當滑鼠「點擊左鍵」時 (開關燈)
    void OnMouseDown()
    {
        isLightOn = !isLightOn; // 狀態反轉 (開變關、關變開)
        
        if (lightSource != null)
        {
            lightSource.SetActive(isLightOn); // 打開或關閉實際發光的光源物件
        }
    }
    public void OnHitBox(){GetComponent<Collider>().enabled = true;}    //開啟hitBox
    public void OffHitBox(){GetComponent<Collider>().enabled = false;}  //關閉hitBox
}