using UnityEngine;
using System.Collections;

public class Reticle : MonoBehaviour
{
    [System.Serializable]
    private struct ReticleLayout
    {
        public Vector2 ViewportRelativePosition;
        public float ViewportRelativeSize;
        public Texture2D Texture;
    }

    [SerializeField]
    private ReticleLayout firstPersonReticle;
    [SerializeField]
    private ReticleLayout thirdPersonReticle;

    [HideInInspector]
    public bool UseThirdPersonReticle = true;

    [HideInInspector]
    public bool HighlightRed = false;

    [HideInInspector]
    public Rect Viewport;

    private void OnGUI()
    {
        ReticleLayout currentReticle = UseThirdPersonReticle ? thirdPersonReticle: firstPersonReticle;

        float width = Viewport.width * currentReticle.ViewportRelativeSize;
        float height = Viewport.height * currentReticle.ViewportRelativeSize;

        // Maintain aspect ratio with smalllest dimension.
        float aspectRatio = currentReticle.Texture.width / currentReticle.Texture.height;
        if (width < height)
        {
            height = width * aspectRatio;
        }
        else
        {
            width = height / aspectRatio;
        }

        float x = Viewport.x + Viewport.width * currentReticle.ViewportRelativePosition.x - (width / 2.0f);
        float y = Screen.height - Viewport.y - Viewport.height * currentReticle.ViewportRelativePosition.y - (height / 2.0f);

        GUI.color = HighlightRed ? Color.red : Color.white;
        GUI.DrawTexture(new Rect(x, y, width, height), currentReticle.Texture);
    }
}
