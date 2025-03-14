using UnityEngine;

public class MapScreenshot : MonoBehaviour
{
    public Camera screenshotCamera;
    public string filePath = "Assets/MapScreenshot.png";

    [ContextMenu("Take Screenshot")]
    void CaptureScreenshot()
    {
        RenderTexture rt = new RenderTexture(256, 256, 24);
        screenshotCamera.targetTexture = rt;
        screenshotCamera.Render();
        RenderTexture.active = rt;

        Texture2D screenshot = new Texture2D(256, 256, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
        screenshot.Apply();

        System.IO.File.WriteAllBytes(filePath, screenshot.EncodeToPNG());
        Debug.Log("Screenshot saved to: " + filePath);

        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(rt);
    }
}