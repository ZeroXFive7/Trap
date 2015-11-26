using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private Reticle reticle = null;

    [HideInInspector]
    public PlayerController Player = null;

    private void Update()
    {
        if (Player == null)
        {
            return;
        }

        Rect screenViewport = new Rect()
        {
            x = Player.Camera.Viewport.x * Screen.width,
            y = Player.Camera.Viewport.y * Screen.height,
            width = Player.Camera.Viewport.width * Screen.width,
            height = Player.Camera.Viewport.height * Screen.height
        };

        reticle.Viewport = screenViewport;
        reticle.UseThirdPersonReticle = Player.Camera.IsThirdPerson;
    }
}
