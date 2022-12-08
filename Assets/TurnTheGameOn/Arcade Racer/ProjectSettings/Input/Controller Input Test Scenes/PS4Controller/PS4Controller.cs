using UnityEngine;
using UnityEngine.UI;

public class PS4Controller : MonoBehaviour
{
    public Image leftStick;
    public Image rightStick;
    public Image L2;
    public Image R2;
    public Image L1;
    public Image R1;
    public Image x;
    public Image circle;
    public Image square;
    public Image triangle;
    public Image dPadLeft;
    public Image dPadUp;
    public Image dPadRight;
    public Image dPadDown;
    public Image share;
    public Image options;
    public Image pad;
    public Image ps;

    private void Update()
    {
        #region Joysticks
        float moveHL = Input.GetAxis("PS4_LeftStick_X") * 100;
        float moveVL = Input.GetAxis("PS4_LeftStick_Y") * 100;
        leftStick.rectTransform.localPosition = new Vector2(moveHL, moveVL);

        float moveHR = Input.GetAxis("PS4_RightStick_X") * 100;
        float moveVR = Input.GetAxis("PS4_RightStick_Y") * 100;
        rightStick.rectTransform.localPosition = new Vector2(moveHR, moveVR);
        #endregion


        #region Joystick Buttons
        bool leftJoystickIsPressed = Input.GetButton("PS4_LeftStick_Button");
        leftStick.color = leftJoystickIsPressed ? new Color(1, 1, 1) : new Color(0, 0, 0);

        bool rightJoystickIsPressed = Input.GetButton("PS4_RightStick_Button");
        rightStick.color = rightJoystickIsPressed ? new Color(1, 1, 1) : new Color(0, 0, 0);
        #endregion


        #region Shoulder Buttons
        float l2 = Input.GetAxis("PS4_L2");
        L2.color = new Color(l2, l2, l2);

        float r2 = Input.GetAxis("PS4_R2");
        R2.color = new Color(r2, r2, r2);

        bool l1IsPressed = Input.GetButton("PS4_L1");
        L1.color = l1IsPressed ? new Color(1, 1, 1) : new Color(0, 0, 0);

        bool r1IsPressed = Input.GetButton("PS4_R1");
        R1.color = r1IsPressed ? new Color(1, 1, 1) : new Color(0, 0, 0);
        #endregion


        #region D Pad
        float dpadHorizontal = Input.GetAxis("PS4_DPad_X");
        dPadLeft.color = dpadHorizontal < 0 ? new Color(1, 1, 1) : new Color(0, 0, 0);
        dPadRight.color = dpadHorizontal > 0 ? new Color(1, 1, 1) : new Color(0, 0, 0);

        float dpadVertical = Input.GetAxis("PS4_DPad_Y");
        dPadDown.color = dpadVertical < 0 ? new Color(1, 1, 1) : new Color(0, 0, 0);
        dPadUp.color = dpadVertical > 0 ? new Color(1, 1, 1) : new Color(0, 0, 0);
        #endregion


        #region Primary Buttons
        bool xButtonIsPressed = Input.GetButton("PS4_X");
        bool circleButtonIsPressed = Input.GetButton("PS4_Circle");
        bool triangleButtonIsPressed = Input.GetButton("PS4_Triangle");
        bool squareButtonIsPressed = Input.GetButton("PS4_Square");
        x.color = xButtonIsPressed ? new Color(1, 1, 1) : new Color(0, 0, 0);
        circle.color = circleButtonIsPressed ? new Color(1, 1, 1) : new Color(0, 0, 0);
        triangle.color = triangleButtonIsPressed ? new Color(1, 1, 1) : new Color(0, 0, 0);
        square.color = squareButtonIsPressed ? new Color(1, 1, 1) : new Color(0, 0, 0);
        #endregion


        #region share, options, pad, ps Buttons
        bool shareButtonIsPressed = Input.GetButton("PS4_Share");
        bool optionsButtonIsPressed = Input.GetButton("PS4_Options");
        bool padButtonIsPressed = Input.GetButton("PS4_Pad");
        bool psButtonIsPressed = Input.GetButton("PS4_PS");
        share.color = shareButtonIsPressed ? new Color(1, 1, 1) : new Color(0, 0, 0);
        options.color = optionsButtonIsPressed ? new Color(1, 1, 1) : new Color(0, 0, 0);
        pad.color = padButtonIsPressed ? new Color(1, 1, 1) : new Color(0, 0, 0);
        ps.color = psButtonIsPressed ? new Color(1, 1, 1) : new Color(0, 0, 0);
        #endregion
    }

}