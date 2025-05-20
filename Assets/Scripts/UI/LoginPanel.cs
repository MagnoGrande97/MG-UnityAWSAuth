using UnityEngine;
using TMPro;

public class LoginPanel : MonoBehaviour
{
    private AuthController authController;
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_InputField confirmationCodeInput;
    [SerializeField] private GameObject confirmPanel;

    private void Start()
    {
        authController = GameObject.Find("AWSManager").GetComponent<AuthController>();
        AuthController.Instance.OnUserUnconfirmed += ShowConfirmPanel;
    }

    public void OnLoginButtonPressed()
    {
        authController.Login(emailInput.text, passwordInput.text);
    }

    public void OnConfirmButtonPressed()
    {
        authController.ConfirmSignUp(emailInput.text, confirmationCodeInput.text);
    }

    private void ShowConfirmPanel(string email)
    {
        confirmPanel.SetActive(true);
        emailInput.text = email;
    }

    private void OnDestroy()
    {
        if (AuthController.Instance != null)
            AuthController.Instance.OnUserUnconfirmed -= ShowConfirmPanel;
    }
}