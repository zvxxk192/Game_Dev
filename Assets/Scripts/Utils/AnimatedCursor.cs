using UnityEngine;

public class AnimatedCursor : MonoBehaviour
{
    [Header("Animation Settings")]
    public Texture2D[] cursorFrames;         //游標動畫畫格
    public float frameRate = 10f;            //播放速度 (sheet/s)
    public Vector2 hotSpot = Vector2.zero;   //點擊中心點
    public bool OnAnimation = false;

    private int currentFrame = 1;
    private float timer = 0f;

    void Start()
    {
        if (cursorFrames.Length > 0)
        {
            Cursor.SetCursor(cursorFrames[0], hotSpot, CursorMode.Auto);
        }
    }
    void Update()
    {
        if (!OnAnimation) return; 

        if (cursorFrames.Length == 0) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = 1f / frameRate;
            currentFrame = (currentFrame + 1) % cursorFrames.Length;
            Cursor.SetCursor(cursorFrames[currentFrame], hotSpot, CursorMode.Auto);
        }
    }
}
