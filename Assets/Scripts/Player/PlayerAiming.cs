using UnityEngine;
using System.Collections;

public class PlayerAiming : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField]
    private float yawSpeed;
    [SerializeField]
    private float pitchSpeed;
    [SerializeField]
    private float minYawAngle;
    [SerializeField]
    private bool invertHorizontal = false;
    [SerializeField]
    private bool invertVertical = false;

    [Header("Perspective")]
    [SerializeField]
    private Vector3 firstPersonCameraOffset;
    [SerializeField]
    private Vector3 thirdPersonCameraOffset;
    [SerializeField]
    private float perspectiveTransitionTime;
    [SerializeField]
    private bool defaultThirdPerson = true;

    [Header("Component References")]
    [SerializeField]
    private Player player;
    [SerializeField]
    private new Transform camera;

    public bool IsThirdPerson { get; private set; }

    private bool perspectiveIsTransitioning = false;

    private void Start()
    {
        SetPerspective(defaultThirdPerson, true);

        SetCursorState(false);
    }

    private void Update()
    {
        // Update cursor state.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetCursorState(false);
        }
        else if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            SetCursorState(true);
        }

        // Update perspective.
        if (!perspectiveIsTransitioning)
        {
            bool altPerspectiveInput = player.Input.AimDownSights > 0.0f;
            bool altPerspectiveActive = (defaultThirdPerson != IsThirdPerson);

            if (altPerspectiveInput && !altPerspectiveActive)
            {
                SetPerspective(!defaultThirdPerson);
            }
            else if (!altPerspectiveInput && altPerspectiveActive)
            {
                SetPerspective(defaultThirdPerson);
            }
        }

        // Update camera rotation.
        float playerRotation = player.Input.Look.x * yawSpeed * Time.deltaTime;
        if (invertHorizontal)
        {
            playerRotation *= -1.0f;
        }
        transform.Rotate(Vector3.up * playerRotation, Space.Self);

        Vector3 previousPosition = camera.transform.position;
        Quaternion previousRotation = camera.transform.rotation;

        float cameraRotation = player.Input.Look.y * pitchSpeed * Time.deltaTime;
        if (invertVertical)
        {
            cameraRotation *= -1.0f;
        }
        camera.transform.RotateAround(transform.position, transform.right, cameraRotation);

        // Avoid gimble lock by disallowing rotations that align forward vector with world up.
        float angleFromY = Vector3.Angle(camera.transform.forward, Vector3.up);
        if (angleFromY < minYawAngle || angleFromY > 180.0f - minYawAngle)
        {
            camera.transform.position = previousPosition;
            camera.transform.rotation = previousRotation;
        }
    }

    private void SetPerspective(bool thirdPerson, bool snap = false)
    {
        IsThirdPerson = thirdPerson;

        Vector3 initialOffset = thirdPerson ? firstPersonCameraOffset : thirdPersonCameraOffset;
        Vector3 targetOffset = thirdPerson ? thirdPersonCameraOffset : firstPersonCameraOffset;
        float transitionTime = snap ? 0.0f : perspectiveTransitionTime;
        StartCoroutine(RunUpdatePerspective(initialOffset, targetOffset, transitionTime));
    }

    private IEnumerator RunUpdatePerspective(Vector3 initialCameraOffset, Vector3 targetCameraOffset, float duration)
    {
        perspectiveIsTransitioning = true;

        float timer = 0.0f;
        while (timer < duration)
        {
            Vector3 initial = camera.transform.TransformDirection(initialCameraOffset);
            Vector3 final = camera.transform.TransformDirection(targetCameraOffset);

            camera.transform.position = transform.position + Vector3.Lerp(initial, final, timer / perspectiveTransitionTime);
            yield return new WaitForSeconds(Time.deltaTime);
            timer += Time.deltaTime;
        }
        camera.transform.position = transform.position + camera.transform.TransformDirection(targetCameraOffset);

        perspectiveIsTransitioning = false;
    }

    private void SetCursorState(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}
