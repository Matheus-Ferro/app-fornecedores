using App.Configurations;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using App.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllersWithViews();

// Estes metodos foram criados para organizar a main e est�o declarados na pasta Configurations
builder.Services.AddIdentityConfiguration(connectionString);
builder.Services.AddMvcConfiguration();
builder.Services.ResolveDependencies();

// DbContext dos fornecedores definido em Data.Context.AppFornecedoresDbContext.cs
// Nao se esqueça de fazer as migrations tanto desse contexto quanto do ApplicationDbContext acima.
// Vale lembrar tambem de fazer os Mappings antes das Migrations, para uma melhor alocação dos dados no banco.
builder.Services.AddDbContext<AppFornecedoresDbContext>(options =>
	options.UseSqlServer(connectionString));

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/erro/500");
	app.UseStatusCodePagesWithRedirects("/erro/{0}");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Metodo declarado dentro da pasta Configurations para organizar a main
app.UseGlobalizationConfig();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
