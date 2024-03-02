using UnityEngine;
using UnityEngine.Events;

public class InputDetector : MonoBehaviour
{
    public static InputDetector Instance;
    
    public event UnityAction OnScreenTapped;
    
    public Vector2 GetJoystickInput => _joystickInput;

    [SerializeField] private FloatingJoystick joystick;

    private Vector2 _joystickInput;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // tapDetector.onClick.AddListener(() => OnScreenTapped?.Invoke());
        joystick.OnTapped += () => OnScreenTapped?.Invoke();
    }

    private void Update()
    {
        _joystickInput.x = joystick.Horizontal;
        _joystickInput.y = joystick.Vertical;

        // if (Input.touchCount > 0)
        // {
        //     Touch touch = Input.GetTouch(0);
        //     if (touch.phase == TouchPhase.Ended)
        //     {
        //         OnScreenTapped?.Invoke();
        //     }
        // }
        // else if(Input.GetKeyDown(KeyCode.Mouse0))
        //     OnScreenTapped?.Invoke();
    }
}
