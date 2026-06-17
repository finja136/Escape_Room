using UnityEngine;

public class MonitorSwitcher : MonoBehaviour
{
    public Renderer screenRenderer;

    public Texture[] images;

    private int currentIndex = 0;

    void Start()
    {
        ApplyImage(0);
    }

    public void Next()
    {
        currentIndex = (currentIndex + 1) % images.Length;
        ApplyImage(currentIndex);
    }

    public void SetIndex(int index)
    {
        currentIndex = Mathf.Clamp(index, 0, images.Length - 1);
        ApplyImage(currentIndex);
    }

    private void ApplyImage(int index)
    {
        screenRenderer.material.mainTexture = images[index];
    }
}