using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Tunables")]
    [SerializeField]
    private float speed;

    [Header("Component References")]
    [SerializeField]
    private Player player;

    private void Update()
    {
        Vector3 translation = new Vector3(player.Input.Movement.x, 0.0f, player.Input.Movement.y).normalized;
        translation = translation.normalized * speed * Time.deltaTime;
        transform.Translate(translation, Space.Self);
    }
}
