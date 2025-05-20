using UnityEngine;

public class DashboardPanel : MonoBehaviour
{
    private AuthController authController;

    private void Start()
    {
        authController = GameObject.Find("AWSManager").GetComponent<AuthController>();
    }

    public void OnLogoutButtonPressed()
    {
        authController.Logout(); // Esto debe ejecutar la lógica de logout
    }
}