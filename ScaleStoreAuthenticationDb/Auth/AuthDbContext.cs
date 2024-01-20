using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;




namespace ScaleStoreAuthenticationDb.Auth;

public class AuthDbContext : DbContext
{
	public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
	{ }


}