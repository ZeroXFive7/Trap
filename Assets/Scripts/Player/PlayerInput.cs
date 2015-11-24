using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    [System.Serializable]
    private struct InputAxesConfiguration
    {
        public string MovementHorizontalAxisName;
        public string MovementVerticalAxisName;
        public string LookHorizontalAxisName;
        public string LookVerticalAxisName;
        public string AimDownSightsAxisName;
        public string AttackAxisName;
        public string JumpAxisName;
        public string SpringAxisName;
        public string ContextSensitiveActionAxisName;
    }

    public class InputData
    {
        public Vector2 Movement = Vector2.zero;
        public Vector2 Look = Vector2.zero;
        public bool AimDownSights = false;
        public bool Attack = false;
        public bool Jump = false;
        public bool Sprint = false;
        public bool ContextSensitiveAction = false;

        public bool IsEmpty
        {
            get
            {
                return Movement.x == 0.0f &&
                    Movement.y == 0.0f &&
                    Look.x == 0.0f &&
                    Look.y == 0.0f &&
                    AimDownSights == false &&
                    Attack == false &&
                    Jump == false &&
                    ContextSensitiveAction == false;
            }
        }
    }

    private static readonly InputAxesConfiguration controllerConfig = new InputAxesConfiguration
    {
        MovementHorizontalAxisName = "MovementHorizontalController",
        MovementVerticalAxisName= "MovementVerticalController",
        LookHorizontalAxisName = "LookHorizontalController",
        LookVerticalAxisName = "LookVerticalController",
        AimDownSightsAxisName = "AimDownSightsController",
        AttackAxisName = "AttackController",
        JumpAxisName = "JumpController",
        SpringAxisName = "SprintController",
        ContextSensitiveActionAxisName = "ContextSensitiveActionController"
    };

    private static readonly InputAxesConfiguration keyboardMouseConfig = new InputAxesConfiguration
    {
        MovementHorizontalAxisName = "MovementHorizontalKeyboardMouse",
        MovementVerticalAxisName = "MovementVerticalKeyboardMouse",
        LookHorizontalAxisName = "LookHorizontalKeyboardMouse",
        LookVerticalAxisName = "LookVerticalKeyboardMouse",
        AimDownSightsAxisName = "AimDownSightsKeyboardMouse",
        AttackAxisName = "AttackKeyboardMouse",
        JumpAxisName = "JumpKeyboardMouse",
        SpringAxisName = "SprintKeyboardMouse",
        ContextSensitiveActionAxisName = "ContextSensitiveActionKeyboardMouse"
    };

    private InputData emptyInput = new InputData();
    private InputData controllerInput = new InputData();
    private InputData keyboardMouseInput = new InputData();

    public InputData CurrentInput { get; private set; }

    public bool UsingControllerInput { get; private set; }

    private void Start()
    {
        UsingControllerInput = false;
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

        CurrentInput = emptyInput;

        if (!Cursor.visible)
        {
            ReadInput(ref controllerInput, controllerConfig);
            ReadInput(ref keyboardMouseInput, keyboardMouseConfig);

            if (UsingControllerInput && !keyboardMouseInput.IsEmpty)
            {
                UsingControllerInput = false;
            }
            else if (!UsingControllerInput && !controllerInput.IsEmpty)
            {
                UsingControllerInput = true;
            }

            CurrentInput = (UsingControllerInput ? controllerInput : keyboardMouseInput);
        }
    }

    private void ReadInput(ref InputData data, InputAxesConfiguration config)
    {
        data.Movement = new Vector2(
            Input.GetAxis(config.MovementHorizontalAxisName),
            Input.GetAxis(config.MovementVerticalAxisName));

        data.Look = new Vector2(
            Input.GetAxis(config.LookHorizontalAxisName),
            Input.GetAxis(config.LookVerticalAxisName));

        data.AimDownSights = Input.GetAxis(config.AimDownSightsAxisName) > 0.0f;

        data.Attack = Input.GetAxis(config.AttackAxisName) > 0.0f;

        data.Jump = Input.GetAxis(config.JumpAxisName) > 0.0f;

        data.Sprint = Input.GetAxis(config.SpringAxisName) > 0.0f;
    }

    private void SetCursorState(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}
