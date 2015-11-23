using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{
    [Header("Perspective")]
    [SerializeField]
    private Vector3 firstPersonCameraOffset;
    [SerializeField]
    private Vector3 thirdPersonCameraOffset;
    [SerializeField]
    private float perspectiveTransitionTime;
    [SerializeField]
    private bool defaultThirdPerson = true;

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
                SetPerspective(isThirdPerson);
            }
        }
    }

    private void Awake()
    {
        isThirdPerson = defaultThirdPerson;
        SetPerspective(isThirdPerson, true);
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
            transform.localPosition = Vector3.Lerp(initialCameraOffset, targetCameraOffset, timer / perspectiveTransitionTime);
            yield return new WaitForSeconds(Time.deltaTime);
            timer += Time.deltaTime;
        }
        transform.localPosition = targetCameraOffset;

        perspectiveIsTransitioning = false;
    }
}
