using UnityEngine;

[ExecuteInEditMode]
public class PixelateCamera : MonoBehaviour
{
    public Material pixelateMaterial;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (pixelateMaterial != null)
        {
            Graphics.Blit(src, dest, pixelateMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}