using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{
    [System.Serializable]
    private struct CameraConfiguration
    {
        public Vector3 LocalPosition;
        public LayerMask CullingMask;
    };

    [Header("Perspective")]
    [SerializeField]
    private CameraConfiguration firstPersonCameraConfig;
    [SerializeField]
    private CameraConfiguration thirdPersonCameraConfig;
    [SerializeField]
    private float perspectiveTransitionTime;
    [SerializeField]
    private bool defaultThirdPerson = true;

    [Header("Component References")]
    [SerializeField]
    private new Camera camera = null;
    [SerializeField]
    private Reticle reticle = null;

    public Reticle Reticle
    {
        get { return reticle; }
    }

    private bool perspectiveIsTransitioning = false;

    private bool isThirdPerson = true;
    public bool IsThirdPerson
    {
        get
        {
            return isThirdPerson;
        }
        set
        {
            if (!perspectiveIsTransitioning && isThirdPerson != value)
            {
                isThirdPerson = value;
                StartCoroutine(UpdatePerspectiveCoroutine(isThirdPerson, false));
            }
        }
    }

    private void Awake()
    {
        isThirdPerson = defaultThirdPerson;
        StartCoroutine(UpdatePerspectiveCoroutine(isThirdPerson, true));
    }

    private IEnumerator UpdatePerspectiveCoroutine(bool thirdPerson, bool snap)
    {
        CameraConfiguration initialConfig = thirdPerson ? firstPersonCameraConfig : thirdPersonCameraConfig;
        CameraConfiguration targetConfig = thirdPerson ? thirdPersonCameraConfig : firstPersonCameraConfig;
        float transitionTime = snap ? 0.0f : perspectiveTransitionTime;

        perspectiveIsTransitioning = true;

        float timer = 0.0f;
        while (timer < transitionTime)
        {
            transform.localPosition = Vector3.Lerp(initialConfig.LocalPosition, targetConfig.LocalPosition, timer / perspectiveTransitionTime);
            yield return new WaitForSeconds(Time.deltaTime);
            timer += Time.deltaTime;
        }
        transform.localPosition = targetConfig.LocalPosition;

        camera.cullingMask = targetConfig.CullingMask;
        reticle.UseThirdPersonReticle = thirdPerson;

        perspectiveIsTransitioning = false;
    }
}
