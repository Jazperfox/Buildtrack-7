using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace proyecto02.Pages.Visual
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public LoginModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public Credential Credential { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }
        public void OnPost()
        {


            if (!ModelState.IsValid) return;
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT COUNT(1) FROM Usuarios WHERE usuario = @Username AND contrasenia = @Password";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", Credential.Username);
                    command.Parameters.AddWithValue("@Password", Credential.Password);

                    int count = (int)command.ExecuteScalar();
                    if (count == 1)
                    {

                        Response.Redirect("/Index");
                    }
                    else
                    {
                        // Credenciales incorrectas
                        ErrorMessage = "Usuario o contraseña incorrectos.";
                    }
                }
            }

        }
    }

    public class Credential
    {
        [Required]
        [Display(Description = "User Name")]
        public string Username { get; set; } = string.Empty;

        [Display(Description = "Password")]
        public string Password { get; set; } = string.Empty;
    }
}

