using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HotbarManager : MonoBehaviour
{
    public static HotbarManager Instance;       // 靜態單例

    [Header("格子設定")]
    public Image[] slots;                       // 4個格子的背景，拖入Slot_0~Slot_3
    public Image[] itemIcons;                   // 4個格子的圖示，拖入ItemIcon_0~ItemIcon_3
    public UnityEngine.UI.Outline[] slotOutlines;              // 4個格子的Outline元件，拖入Slot_0~Slot_3的Outline

    [Header("物品位置設定")]
    public Transform itemHolder;                // 手持位置，拖入MainCamera底下的ItemHolder空物件
    public Vector3 hidePosition = new Vector3(0, -100f, 0); // 3D物件的隱藏位置
    public float moveDuration = 0.3f;           // 3D物件順移時間

    private int currentPage = 0;                // 目前頁數
    private int currentSlot = 0;                // 目前選中格子（0~3）
    private int slotsPerPage = 4;               // 每頁幾格

    void Awake()    {Instance = this;}
    //初始化最一開始哪個亮預設第一格
    void Start()    {RefreshSelectOutline();}
    //控制滾輪
    void Update()   {HandScroll();}

    void HandScroll()           //滾輪控制
    {   //抓滾輪
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        //防呆
        if(scroll == 0)return;
        HideCurrentItem();  //先關掉3D物件
        if(scroll < 0)
            //控制欄位 往下滾，選上一個 (目前哪一格+1)到下一格 餘數%為了不要超過格數
            currentSlot = (currentSlot + 1) % slotsPerPage;
        else
            //控制欄位 往上滾，選下一個 (目前哪一格-1)到下一格 +格數是為了到從0格到3格 餘數%為了不要超過格數
            currentSlot = (currentSlot - 1 + slotsPerPage) % slotsPerPage;
        RefreshSelectOutline();     //控制亮光
        ShowCurrentItem();          //開啟3D物件
    }

    void RefreshSelectOutline() //控制亮光
    {   //控制關閉亮光和開啟亮光
        for (int i = 0; i < slotsPerPage; i++)
            //查看每個欄位如果沒到要亮的欄位i 不會等於 currentSlot(選中的格子)就會是false反之true達到關閉開啟亮
            slotOutlines[i].enabled = (i == currentSlot);
            // 顯示目前選中物品的名稱
        ItemData data = Inventory.Instance.GetItem(currentPage * slotsPerPage + currentSlot);
        
        if (data != null)   ItemNameUI.Instance.Show(data.itemName + data.itemDescription);
    }

    public void ShowCurrentItem()      //顯示3D物件在手上
    {
        GameObject model = Inventory.Instance.GetItemModel(currentPage * slotsPerPage + currentSlot);
        if (model == null || model == null) return;
        //拿起時候關掉hit box
        model.GetComponent<Collider>().enabled = false;
        model.transform.SetParent(itemHolder);                           // 設為子物件跟著相機走
        model.transform.DOLocalMove(Vector3.zero, moveDuration);         // 移到 ItemHolder 位置
}    
    void HideCurrentItem()      //隱藏3D物件在手上(移開)
    {
        GameObject model = Inventory.Instance.GetItemModel(currentPage * slotsPerPage + currentSlot);
        if (model == null || model == null) return;
        // 放下時候開啟hit box
        model.GetComponent<Collider>().enabled = true;
        model.transform.SetParent(null);                             // 取消父子關係
        model.transform.DOMove(hidePosition, moveDuration);          // 移回隱藏位置

    }

    // 拾取物品後呼叫，更新2D圖示
    public void RefreshHotbar()
    {
        for (int i = 0; i < slotsPerPage; i++)
        {
            int itemIndex = currentPage * slotsPerPage + i;
            ItemData data = Inventory.Instance.GetItem(itemIndex);

            if (data != null && data.itemIcon != null)
            {
                // 有物品顯示圖示
                itemIcons[i].sprite = data.itemIcon;
                itemIcons[i].gameObject.SetActive(true);
            }
            else
            {
                // 沒物品隱藏圖示
                itemIcons[i].gameObject.SetActive(false);
            }
        }

        RefreshSelectOutline();
    }
}