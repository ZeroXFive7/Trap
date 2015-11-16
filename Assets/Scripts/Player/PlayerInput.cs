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
        public string JumpAxisName;
    }

    private class InputData
    {
        public Vector2 Movement = Vector2.zero;
        public Vector2 Look = Vector2.zero;
        public float AimDownSights = 0.0f;
        public bool Jump = false;

        public bool IsEmpty
        {
            get
            {
                return Movement.x == 0.0f &&
                    Movement.y == 0.0f &&
                    Look.x == 0.0f &&
                    Look.y == 0.0f &&
                    AimDownSights == 0.0f &&
                    Jump == false;
            }
        }
    }

    [SerializeField]
    private InputAxesConfiguration controllerConfig;
    [SerializeField]
    private InputAxesConfiguration keyboardMouseConfig;

    private InputData emptyInput = new InputData();
    private InputData controllerInput = new InputData();
    private InputData keyboardMouseInput = new InputData();
    private InputData currentInput;

    public bool UsingControllerInput { get; private set; }

    public Vector2 Movement
    {
        get
        {
            return currentInput.Movement;
        }
    }

    public Vector2 Look
    {
        get
        {
            return currentInput.Look;
        }
    }

    public float AimDownSights
    {
        get
        {
            return currentInput.AimDownSights;
        }
    }

    public bool Jump
    {
        get
        {
            return currentInput.Jump;
        }
    }

    private void Awake()
    {
        UsingControllerInput = false;
    }

    private void Update()
    {
        currentInput = emptyInput;

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

            currentInput = (UsingControllerInput ? controllerInput : keyboardMouseInput);
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

        data.AimDownSights = Input.GetAxis(config.AimDownSightsAxisName);

        data.Jump = Input.GetAxis(config.JumpAxisName) > 0.0f;
    }
}
