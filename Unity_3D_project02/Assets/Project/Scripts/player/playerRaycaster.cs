using UnityEngine;
using DG.Tweening;
public class playerRaycaster : MonoBehaviour
{
    [Header("可觸碰距離和類型")]
    public float rayDistance = 3f;
    public LayerMask interactLayer;
    public LayerMask placeBox;
    public Transform itemHolder;                // 拖入 ItemHolder 空物件查看手上物品是否有對應code
    private bool isShow = false;
    bool isAnyPlaced = false;                   //物品放置後開啟才能去使用物品
    
    void Update()
    {   
        //計算遊戲畫面中心點位置
        //確認遊戲的camera   Camera.main.ScreenPointToRay把這相機座標轉成3D射線
        //Ray是射線的宣告(起點,方向)
        Ray ray = Camera.main.ScreenPointToRay(
        new Vector3(Screen.width / 2, Screen.height / 2, 0));
        //如果ray這位置打到interactLayer階層物品在rayDistance據理
        if(Physics.Raycast(ray, out RaycastHit hit , rayDistance,interactLayer))
        {   
            isShow = true;
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            //呼叫別的介面的code (櫃子，抽屜)
            IManagerOPCLAll  drawer = hit.collider.GetComponent<IManagerOPCLAll>();
            //呼叫別的介面的code (可拿取物品)
            IPickable Item = hit.collider.GetComponent<IPickable>();
            
            Debug.Log("打到了" + hit.collider.name);
            //控制發亮和UI(開啟)
            if(interactable != null) DrawerHighlight.Show(interactable);
            //控制拿取物品
            if(Item != null && Input.GetMouseButtonDown(0))
            {
                PlaceableItem pItem = hit.collider.GetComponent<PlaceableItem>();
                //如果拿起來更新放置點可以再放置了
                if (pItem != null && pItem.IsPlaced())pItem.TakeBack();
                Item.OnPickup();
            } 
            //如果點滑鼠右鍵
            if (drawer != null && Input.GetMouseButtonDown(1))drawer.ToggleDrawer();
            //這是控制E去使用物品
            if (Input.GetKeyDown(KeyCode.E) && isAnyPlaced)
            {   
                WoodBox_Controller WoodBox = hit.collider.GetComponentInParent<WoodBox_Controller>();
                if(WoodBox != null) {WoodBox.UseBox();}
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                Keypad_Controller keypad = hit.collider.GetComponentInParent<Keypad_Controller>();
                if(keypad != null){keypad.UseKeypad();}
            }

            //Debug.Log("打到：" + hit.collider.name);
        } 
        else if(isShow)
        {   //防止連續呼叫UI發光
            isShow = false;
            //控制發亮和UI(開啟)
            DrawerHighlight.Hide();
        }

        //這邊是放置設定
        if(Physics.Raycast(ray, out RaycastHit placeHit,rayDistance,placeBox))
        {

            PlaceableZone zone = placeHit.collider.GetComponent<PlaceableZone>();
            PlaceableItem heldItem = GetHeldPlaceableItem();   // 看手上有沒有可放置物品
            
            //Debug.Log("zone = " + zone + " heldItem = " + heldItem); //加這行看哪個是null
            
            if(zone != null && heldItem != null)
            {
                heldItem.ShowPreview(zone);                     // 顯示預覽
                if(Input.GetMouseButtonDown(0) && zone.CanPlace())
                {
                    isAnyPlaced = true;
                    heldItem.Place(zone);                       // 左鍵放置
                }
            }
        }
        else
        {
            // 沒打到放置區域，隱藏預覽
            PlaceableItem heldItem = GetHeldPlaceableItem();
            //Debug.Log("heldItem = " + heldItem);
            if(heldItem != null) heldItem.HidePreview();
        }
    }
    //這是查看手上物品是否可以放置是否有掛PlaceableItem的code
    PlaceableItem GetHeldPlaceableItem()
    {
        return itemHolder.GetComponentInChildren<PlaceableItem>();
    }
    void OnDrawGizmos()
    {   
        if (Camera.main == null) return;
        // 取得射線的起點與方向（和 Update 裡一樣的射線）
        Ray ray = Camera.main.ScreenPointToRay(
            new Vector3(Screen.width / 2, Screen.height / 2, 0));
    }
}
