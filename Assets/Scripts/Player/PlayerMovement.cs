using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Tunables")]
    [SerializeField]
    private float speed;

    [Header("Input Settings")]
    [SerializeField]
    private string horizontalInputAxis;
    [SerializeField]
    private string forwardInputAxis;

    private void Update()
    {
        Vector3 translation = new Vector3(
            Input.GetAxis(horizontalInputAxis),
            0.0f,
            Input.GetAxis(forwardInputAxis)).normalized;

        translation = translation.normalized * speed * Time.deltaTime;
        transform.Translate(translation, Space.Self);
    }
}
