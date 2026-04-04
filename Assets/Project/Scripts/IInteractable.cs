using UnityEngine; // 加這行！
// 父介面，負責發光和UI
public interface IInteractable
{
    string GetLabel();
    Vector3 GetUIOffset();
}
