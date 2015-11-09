using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{
    [Header("Tunables")]
    [SerializeField]
    private float yawSpeed;
    [SerializeField]
    private float pitchSpeed;
    [SerializeField]
    private bool invertHorizontal = false;
    [SerializeField]
    private bool invertVertical = false;

    [Header("Perspective Tunables")]
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
    private new Transform camera;

    [Header("Input Settings")]
    [SerializeField]
    private string yawInputAxis;
    [SerializeField]
    private string pitchInputAxis;
    [SerializeField]
    private string zoomInputAxis;

    private bool isThirdPerson = false;
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

        if (!Cursor.visible)
        {
            // Update perspective.
            if (!perspectiveIsTransitioning)
            {
                bool altPerspectiveInput = Input.GetAxis(zoomInputAxis) > 0.0f;
                bool altPerspectiveActive = (defaultThirdPerson != isThirdPerson);

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
            float playerRotation = Input.GetAxis(yawInputAxis) * yawSpeed * Time.deltaTime;
            if (invertHorizontal)
            {
                playerRotation *= -1.0f;
            }
            transform.Rotate(Vector3.up * playerRotation, Space.Self);

            float cameraRotation = -Input.GetAxis(pitchInputAxis) * pitchSpeed * Time.deltaTime;
            if (invertVertical)
            {
                cameraRotation *= -1.0f;
            }
            camera.transform.RotateAround(transform.position, transform.right, cameraRotation);
        }
    }

    private void SetPerspective(bool thirdPerson, bool snap = false)
    {
        isThirdPerson = thirdPerson;

        StartCoroutine(
            RunUpdatePerspective(
                thirdPerson ? thirdPersonCameraOffset : firstPersonCameraOffset,
                snap ? 0.0f : perspectiveTransitionTime)
                );
    }

    private IEnumerator RunUpdatePerspective(Vector3 targetCameraOffset, float duration)
    {
        perspectiveIsTransitioning = true;

        Vector3 initialOffset = camera.transform.localPosition;

        float timer = 0.0f;
        while (timer < duration)
        {
            camera.transform.localPosition = Vector3.Lerp(initialOffset, targetCameraOffset, timer / perspectiveTransitionTime);
            yield return new WaitForSeconds(Time.deltaTime);
            timer += Time.deltaTime;
        }
        camera.transform.localPosition = targetCameraOffset;

        perspectiveIsTransitioning = false;
    }

    private void SetCursorState(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}
