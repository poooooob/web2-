using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Models
{
	public class LoginUserModel
	{
		public string UserName { get; set; }
		public string? Password { get; set; }
		public string? UserType { get; set; }
	}

}
