using UnityEngine;
using System.Collections;

public class Reticle : MonoBehaviour
{
    [System.Serializable]
    private struct ReticleLayout
    {
        public Vector2 ScreenSpacePosition;
        public float ScreenSpaceSize;
        public Texture2D Texture;
    }

    [SerializeField]
    private ReticleLayout firstPersonReticle;
    [SerializeField]
    private ReticleLayout thirdPersonReticle;

    public bool UseThirdPersonReticle = true;

    private void OnGUI()
    {
        ReticleLayout currentReticle = UseThirdPersonReticle ? thirdPersonReticle: firstPersonReticle;

        float aspectRatio = currentReticle.Texture.width / currentReticle.Texture.height;
        float width = Screen.width * currentReticle.ScreenSpaceSize;
        float height = width / aspectRatio;

        Rect rect = new Rect(
            (currentReticle.ScreenSpacePosition.x * Screen.width) - (width / 2.0f), 
            (currentReticle.ScreenSpacePosition.y * Screen.height) - (height/ 2.0f), 
            width, 
            height);

        GUI.DrawTexture(rect, currentReticle.Texture);
    }
}
