using UnityEngine;

[ExecuteInEditMode]
public class VHSPostProcess : MonoBehaviour
{
    public Material vhsMaterial;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (vhsMaterial != null)
            Graphics.Blit(src, dest, vhsMaterial);
        else
            Graphics.Blit(src, dest);
    }
}