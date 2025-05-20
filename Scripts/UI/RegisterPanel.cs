using UnityEngine;
using TMPro;

public class RegisterPanel : MonoBehaviour
{
    private AuthController authController;
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_InputField confirmCodeInput;
    [SerializeField] private GameObject confirmPanel;

    private void Start()
    {
        authController = GameObject.Find("AWSManager").GetComponent<AuthController>();
        AuthController.Instance.OnUserNeedsConfirmation += ShowConfirmPanel;
    }

    public void OnRegisterButtonPressed()
    {
        authController.Register(emailInput.text, passwordInput.text);
    }

    private void ShowConfirmPanel(string email)
    {
        confirmPanel.SetActive(true);
        emailInput.text = email;
    }

    public void OnConfirmRegisterButtonPressed()
    {
        string email = emailInput.text.Trim();
        string code = confirmCodeInput.text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(code))
        {
            Debug.LogWarning("Missing fields to confirm registration.");
            return;
        }

        AuthController.Instance.ConfirmSignUp(email, code);
    }

    private void OnDestroy()
    {
        if (AuthController.Instance != null)
            AuthController.Instance.OnUserNeedsConfirmation -= ShowConfirmPanel;
    }
}