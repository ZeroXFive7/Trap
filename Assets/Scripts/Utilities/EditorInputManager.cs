using UnityEngine;
using System.Collections;

public class EditorInputManager : SingletonMonobehaviour<EditorInputManager>
{
    private void Start()
    {
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
    }

    private void SetCursorState(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}
