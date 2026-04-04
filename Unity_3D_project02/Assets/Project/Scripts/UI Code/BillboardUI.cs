using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    void LateUpdate()
    {   
        if (Camera.main == null) return;
        // 讓 Canvas 永遠面向相機
        transform.LookAt(Camera.main.transform);
        // 翻轉 180 度，否則文字會反過來顯示
        transform.Rotate(0, 180f, 0);
    }
}
