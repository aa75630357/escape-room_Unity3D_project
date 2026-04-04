using UnityEngine;


public static class DrawerHighlight
{ 
    private static IInteractable current = null; // 記錄目前亮著的物件
        public static void Show(IInteractable interactable)
    {
        // 轉成 MonoBehaviour 才能用 GetComponent 和 transform
        MonoBehaviour mono = interactable as MonoBehaviour;
        if (mono == null) return;

        Outline outline = mono.GetComponent<Outline>();

        if (current != null && current != interactable) Hide();

        // 已經亮了就不重複觸發，直接跳出
        if (outline != null && outline.enabled) return;

        // 開啟 Outline 發亮
        if (outline != null) outline.enabled = true;

        // 取得抽屜資料來算 UI 位置
        {
            // 抽屜位置 + 右上方偏移 = UI 顯示位置
            Vector3 uiPos = mono.transform.position
                          + mono.transform.right    * interactable.GetUIOffset().x
                          + mono.transform.up       * interactable.GetUIOffset().y
                          + mono.transform.forward  * interactable.GetUIOffset().z;

            // UI 淡入顯示
            InteractUI.Instance.Show(interactable.GetLabel(), uiPos);
            current = interactable;
        }
    }

    public static void Hide()
    {
        // 轉成 MonoBehaviour 才能用 GetComponent
        MonoBehaviour mono = current as MonoBehaviour;
        if (mono == null) return;

        Outline outline = mono.GetComponent<Outline>();

        // 已經關了就不重複觸發，直接跳出
        if (outline != null && !outline.enabled) return;

        // 關閉 Outline 發亮
        if (outline != null) outline.enabled = false;

        // UI 淡出
        InteractUI.Instance.Hide();
        current = null; // ← 加這行！清空記錄
    }
}