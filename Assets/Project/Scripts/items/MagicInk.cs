using UnityEngine;

public class MagicInk : MonoBehaviour
{
    [Header("解謎設定")]
    public GameObject targetLight;     // 拖入對應的燈光 (例如 red_Light)
    public Transform paperAnchor;      // 拖入木箱上的 paperAnchor
    public float fadeSpeed = 2f;       // 漸變速度 (數字越大變透明越快)

    private Material myMaterial;       // 用來存取自己的材質球

    void Start()
    {
        // 自動抓取自己身上的 Renderer，並取得材質球 
        // (Unity 會自動複製一份獨立材質，所以不用擔心改到整個專案的其他物件)
        Renderer render = GetComponent<Renderer>();
        if (render != null)
        {
            myMaterial = render.material;
        }
    }

    void Update()
    {
        if (myMaterial == null) return;

        // 我是不是在 paperAnchor 下面？
        bool isPlacedCorrectly = (paperAnchor != null && transform.IsChildOf(paperAnchor));

        // 目標燈光是不是亮著的？
        // (activeInHierarchy 會檢查這個物件在場景裡是不是真正被開啟的)
        bool isLightOn = (targetLight != null && targetLight.activeInHierarchy);

        // 目標透明度：如果「放對位置」而且「對的燈亮了」，目標就是 0 (完全透明)；否則就是 1 (不透明)
        float targetAlpha = (isPlacedCorrectly && isLightOn) ? 0f : 1f;

        // 取得現在的顏色
        Color currentColor = myMaterial.color;

        // 讓目前的透明度 (a) 慢慢平滑地靠攏目標透明度 (targetAlpha)
        currentColor.a = Mathf.MoveTowards(currentColor.a, targetAlpha, fadeSpeed * Time.deltaTime);

        // 把計算完的新顏色還給材質球
        myMaterial.color = currentColor;
    }
}