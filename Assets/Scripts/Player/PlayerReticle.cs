using UnityEngine;
using System.Collections;

public class PlayerReticle : MonoBehaviour
{
    [SerializeField]
    private Vector2 screenSpacePosition = new Vector2(0.5f, 0.5f);
    [SerializeField]
    private float screenSpaceSize = 0.2f;
    [SerializeField]
    private Texture2D firstPersonReticleTexture = null;
    [SerializeField]
    private Texture2D thirdPersonReticleTexture = null;

    [Header("Component References")]
    [SerializeField]
    private Player player = null;

    private void OnGUI()
    {
        Texture texture = player.Aiming.IsThirdPerson ? thirdPersonReticleTexture : firstPersonReticleTexture;

        float aspectRatio = texture.width / texture.height;
        float width = Screen.width * screenSpaceSize;
        float height = width / aspectRatio;

        Rect rect = new Rect((Screen.width - width) / 2.0f, (Screen.height - height) / 2.0f, width, height);
        GUI.DrawTexture(rect, texture);
    }
}
