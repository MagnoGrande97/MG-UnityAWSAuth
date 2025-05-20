using UnityEngine;
using TMPro;

public class ForgotPasswordPanel : MonoBehaviour
{
    private AuthController authController;
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField codeInput;
    [SerializeField] private TMP_InputField newPasswordInput;

    private void Start()
    {
        authController = GameObject.Find("AWSManager").GetComponent<AuthController>();
    }

    public void OnSendCodePressed()
    {
        authController.ForgotPassword(emailInput.text);
    }

    public void OnConfirmResetPressed()
    {
        authController.ConfirmPasswordReset(emailInput.text, codeInput.text, newPasswordInput.text);
    }
}