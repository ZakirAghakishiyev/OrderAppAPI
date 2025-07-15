using System.ComponentModel.DataAnnotations;

namespace OrderApp.Web.Login;

public class LoginRequest
{    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}