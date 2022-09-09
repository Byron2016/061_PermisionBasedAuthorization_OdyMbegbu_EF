# 061_PermisionBasedAuthorization_OdyMbegbu_EF

- OMAR MBEGBU: ASP.NET from Scratch
	- https://www.youtube.com/watch?v=Fk64W-Q-6PA&list=PLWlWcpwzY4Vke2i3vMD319qdCR3r2sf7A&index=1
	- ASP.NET Web Application(.NET Framework)
		- https://github.com/odytrice/Addressbook
		
		- V001 Crear aplicación ASP.NET Web Application(.NET Framework)
			- Carpeta relacionada 061_PermisionBasedAuthorization_OdyMbegbu/OdyMbegbu_EF
			- ASP.NET Web Application(.NET Framework)
			- Nombre:
				- Project: Addressbook.Web
				- Solution: Addressbook
			- Tipo:
				- Empty
				- MVC
				- Also create a project for unit test con nombre Addressbook.Test

		- Configurar UI
			- Agregar bootstrap
				- Install-Package bootstrap -Version 3.4.1
					- Esto agregará bootstrap, agregará jQuery
					- Modifica archivo packages.config
					- Crea carpeta fonts
					- Modifica archivo xxx.web.csproj
					
			- Agregar jquery
				- Install-Package jQuery -Version 3.4.1
					
			- Agregar jQuery.Validation
				- Install-Package jQuery.Validation -Version 1.17.0
				
			- Agregar Microsoft.jQuery.Unobtrusive.Validation
				- Install-Package Microsoft.jQuery.Unobtrusive.Validation -Version 3.2.11
				
			- Agregar Modernizr
				- Install-Package Modernizr -Version 2.8.3
				
			- Agregar Microsoft.AspNet.Web.Optimization
				- Install-Package Microsoft.AspNet.Web.Optimization -Version 1.1.3
					- Esto agregará paquetes
						- WebGrease
						- Newtonsoft.Json
						- Antlr

			- Agregar a Addressbook.Web controllador Home y la vista index
				- Esto como es un proyecto vacío agregará los siguientes componentes:
					- Addressbook.Web/Content/Site.css
					- Addressbook.Web/Views/Shared/_Layout.cshtml
					- Addressbook.Web/Views/_ViewStart.cshtml
					- Además el controlador y la vista antes indicados.

			- Agregar un BundleConfig y usarlo en _Layout
				- End-to-End ASP.NET MVC: Adding BundleConfig
					- https://www.techjunkieblog.com/2015/05/aspnet-mvc-empty-project-adding.html
						- Agregar a carpeta App_Start la clase BundleConfig
						- Llenar el contenido de los Bundles que deseen.
						- Agregar en el Global.asx.cs la referencia al BundleConfig
							- BundleConfig.RegisterBundles(BundleTable.Bundles);
						- En _Layout llamar al arroba Styles.Render("~/Content/css") y todos los que se requieran.
						- En el Views/web.config sección namespaces agregar
							- <add namespace="System.Web.Optimization"></add>
						- Cerrar aplicación y volverla a cargar desde 0
						
			- Agregar tema de bootstrap Lummen
				- Ir a https://startbootstrap.com/themes
				- Copiar el bootstrap que se indica
					- lumen-bootstrap.css
					- Ir a carpeta Content botón derecho add existing item y cargarlo.
				- Cambiar en la aplicación.
					- Agregar al body del Site.css
						- font-family: Calibri;
					- En _Layout cambiar al navbar-default
					- En BundleConfig.cs al StyleBundle q corresponde llamar al lumen-bootstrap en lugar de bootstrap.

		- Configurar OWIN middleware
			- https://docs.microsoft.com/en-us/aspnet/aspnet/overview/owin-and-katana/owin-middleware-in-the-iis-integrated-pipeline
			
			- Instalar paquete de owin V2 5.40
				- Install-Package Microsoft.Owin.Host.SystemWeb.es -Version 4.2.0
				
			- Implementar primer acercamiento a authorizacion
				- Agregar a la raíz del proyecto Addressbook.Web la clase "OWIN StartUP Class" con el nombre Startup.cs
				
				- Colocar en clase HomeController el atributo [Authorize]
					- Esto no me permite ingresar y desplegará mensajde de IIS con error 401
				- Testear que se presenta mensaje de no autorizado.
				
			- Redireccionar a Login view cuando no se este autorizado
				- Usar autenticación usando cookies
					- Agregar paquete Microsoft.Owin.Security.Cookies
						- Install-Package Microsoft.Owin.Security.Cookies -Version 3.0.1
							- Agrega automáticamente  Microsoft.Owin.Security
						
				- Agregar AccountController 
					- Crear método y vista Login
						- Ir a la página de bootstrap/Components/css/forms y compiar el ejemplo Horizontal form.
						
				- Indicar a owin a donde reedireccionar en caso de no estar autorizado.
					- En owin startup class crear un método que llame pagina de login.
						```cs
							private static void ConfigureAuth(IAppBuilder app)
							{
								app.UseCookieAuthentication(new CookieAuthenticationOptions
								{
									LoginPath = new PathString("/account/login"),
								});
							}
						```
						
		- Autentificación V4
			- Agregar clase Models/LoginModel que recibirá en post lo que se envíe de la vista de login.
			- Agregar en AccountController el Post para el método Login 
				- Recibe de parámetro un LoginModel
			- Redireccionar en la vista login para llamar a método Login post y utilizar htlm helpers para definir los inputs
			
		- Signning V5 - V8 3.40
			- Agregar clase Models/UserModel
			- Inyectar en AccountController el authentication manager.
					```cs
						namespace Addressbook.Web.Controllers
						{
							public class AccountController : Controller
							{
								//Give me the owin authentication context
								public IAuthenticationManager Authentication => HttpContext.GetOwinContext().Authentication;
					```
			- Agregar paquete Microsoft.AspNet.Identity.Core V5 9.58
				- Install-Package Microsoft.AspNet.Identity.Core -Version 2.2.3
				
			- Obtener el authentication manager.


			- Definir en el owin startup el tipo de autenticación V7 6.11
				```cs
					private static void ConfigureAuth(IAppBuilder app)
					{
						app.UseCookieAuthentication(new CookieAuthenticationOptions
						{
							AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
							LoginPath = new PathString("/account/login"),
						});
					}
				```
				
			- Definir en AccountController al crear el claim que se trata de cookie V8 1.41
				```cs
					private void SignIn(LoginModel model) 
					{
						....
			
						var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie); 
						Authentication.SignIn(identity);  
					}
				```
				
			- Definir en web.config el modo de authentication dentro de tags system.web V7 8.38
				- <authentication mode="None" />
					```cs
						<configuration>
						<appSettings>
							<add key="webpages:Version" value="3.0.0.0" />
							<add key="webpages:Enabled" value="false" />
							<add key="ClientValidationEnabled" value="true" />
							<add key="UnobtrusiveJavaScriptEnabled" value="true" />
						</appSettings>
						<system.web>
							<compilation debug="true" targetFramework="4.8" />
							<httpRuntime targetFramework="4.8" />
							<authentication mode="None" />
						</system.web>
						<runtime>
					```

			- En HomeController enviar en la vista Index el UserModel con obteniendo los datos del User.Identity
				```cs
					namespace Addressbook.Web.Controllers
					{
						[Authorize]
						public class HomeController : Controller
						{
							// GET: Home
							public ActionResult Index()
							{
								int userID = User.Identity.GetUserId<int>();
								string email = User.Identity.GetUserName();
								var user = new UserModel
								{
									UserID = userID,
									Email = email
								};
								return View(user);
							}
						}
					}
				```
				
		- LogOut Method V8 4.55
			- Agregar a AccountController el método LogOut
				```cs
					public ActionResult LogOut()
					{
						Authentication.SignOut();
						return Redirect("login");
					}
				```
			- Agregar en el _Layout template accesos a login and logout
				```cs
					<body>
						<div class="navbar navbar-default navbar-fixed-top">
							<div class="container">
								<div class="navbar-header">
									....
								</div>
								<div class="navbar-collapse collapse">
									<ul class="nav navbar-nav">
										<li>@Html.ActionLink("Login", "Login", "Account")</li>
										<li>@Html.ActionLink("LogOut", "LogOut", "Account")</li>
									</ul>
								</div>
							</div>
						</div>
				```
				
		- Implement Role Base Segurity (V.8 7.32)
			- Crear en HomeController un método Admin
			- Decorarlo con Authorize(Roles ="Admin")]
			- Agregar una vista a este método.
			- Asegurar que el usuario tiene un claim role
			- Testear cambiando en AccountController método SignIn el rol de "Admin" a "User".
			
		- Permission Based Authentication
			- Code First
				- Agregar un nuevo proyecto "Class Library (.NET Framework) A project for creating a C# class library (.dll)" 
					- con nombre Addressbook.Core
					
				- Agregar un nuevo proyecto "Class Library (.NET Framework) A project for creating a C# class library (.dll)" 
					- con nombre Addressbook.Infrastructure
					
				- Referencias
					- Agregar referencias en web a Core e Infrastructure
					- Agregar referencias en Infrastructure a Core
					
				- En libraría Addressbook.Core V9 2.34
					- Crear en la raíz folder Services (Nota en V12 1: Cambiar el nombre de Services a Managers por lo que se debe crear con el nombre definitivo de una vez.)
					- Agregar clase AccountService (Nota en V12 1: Se lo borra y se crea AccountManager, por lo que no es necesario crearlo.)
					- Agregar bussiness objets
						- Crear folder Models
						- Crear clases: 
							- UserModel
								```cs
									namespace Addressbook.Core.Models
									{
										public class UserModel
										{
											public int UserId { get; set; }
											public string Email { get; set; }
											public string Password { get; set; }
									
											//tiene multiples roles.
											public ICollection<RoleModel> Roles { get; set; } = new List<RoleModel>();
										}
									}
								```
							- RoleModel
								```cs
									namespace Addressbook.Core.Models
									{
										public class RoleModel
										{
											public int RoleId { get; set; }
											public string Name { get; set; }
									
											//roll tiene múltiples permisos.
											public ICollection<PermissionModel> Permissions { get; set; } = new List<PermissionModel>();
										}
									}
								```
							- PermissionModel
								```cs
									namespace Addressbook.Core.Models
									{
										public class PermissionModel
										{
											public int PermissionID { get; set; }
											public string Name { get; set; }
										}
									}
								```
			
				- En librería Addressbook.Infrastructure V9 7.15 - V10
					- Crear en la raíz folder Utilities
					- Crear folder Entities
						- Agregar clase User
							```cs
								namespace Addressbook.Infrastructure.Entities
								{
									public class User
									{
										public int UserID { get; set; }
										public string Email { get; set; }
								
										public string Password { get; set; }
								
										public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
									}
								}
							```
						- Agregar clase UserRole
							```cs
								namespace Addressbook.Infrastructure.Entities
								{
									public class UserRole
									{
										public int UserRoleID { get; set; }
										public int UserID { get; set; }
										public int RoleID { get; set; }
								
										public virtual Role Role { get; set; }
										public virtual User User { get; set; }
									}
								}
							```
						- Agregar clase Role
							```cs
								namespace Addressbook.Infrastructure.Entities
								{
									public class Role
									{
										public int RoleID { get; set; }
										public string Name { get; set; }
								
										public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
								
										public ICollection<RolePermission> RolePermissions { get; set; } = new HashSet<RolePermission>();
								
									}
								}
							```
						- Agregar clase RolePermission
							```cs
								namespace Addressbook.Infrastructure.Entities
								{
									public class RolePermission
									{
										public int RolePermissionID { get; set; }
										public int RoleID { get; set; }
										public int PermissionID { get; set; }
								
										public ICollection<Role> Roles { get; set; } 
								
										public ICollection<Permission> Permissions { get; set; } 
									}
								}
							```
						- Agregar clase Permission
							```cs
								namespace Addressbook.Infrastructure.Entities
								{
									public class RolePermission
									{
										public int RolePermissionID { get; set; }
										public int RoleID { get; set; }
										public int PermissionID { get; set; }
								
										public ICollection<Role> Roles { get; set; }
								
										public ICollection<Permission> Permissions { get; set; }
									}
								}
							```
						
						- Users has UserRoles, UserRoles to Roles, Roles to RolePermissions, RolePremissions to Permissions
							
					- Crear DBContext V10 5.50
						- Agregar paquete EntityFramework
							- Debe ser agregado en 
								- Infrastructure, 
								- Web (ya q en web tenemos referencia a Infrastructure)
							- Install-Package EntityFramework -Version 6.4.4
						- En raíz de la librería crear clase DataContext : DBContext
							- Crear constructor y enviar información a base
							- Crear los DbSets
								```cs
									namespace Addressbook.Infrastructure
									{
										internal class DataContext : DbContext
										{
											public DataContext() : base("DataContext")
											{
									
											}
									
											public DbSet<User> Users { get; set; }
											public DbSet<UserRole> UserRoles { get; set; }
											public DbSet<Role> Roles { get; set; }
											public DbSet<RolePermission> RolePermissions { get; set; }
											public DbSet<Permission> Permissions { get; set; }
									
										}
									}
								```
								
				- En librería Addressbook.Web V11
					- Agregar el connetionString al Web.config
								```cs
									....
									</appSettings>
									
									<connectionStrings>
										<add name="DataContext" connectionString="Data Source=localhost;Initial Catalog=db_AddressBook;persist security info=True;User ID=sa;Password=123456;MultipleActiveResultSets=True;App=EntityFramework" providerName="System.Data.SqlClient" />
									</connectionStrings>
									
									<system.web>
									....
								```
								
				- Migrar información. V11
					- Borrado tablas			
						drop Table dbo.RolePermissionPermissions;
						drop Table dbo.RoleRolePermissions;
						drop Table dbo.RolePermissions;
						drop Table dbo.UserRoles;
						drop Table dbo.Users;
						drop Table dbo.Roles;
						drop Table dbo.Permissions;
						drop Table dbo.__MigrationHistory;			
								
					- En el Package Manager ir al proyecto donde está el DBContext
						- enable-migrations -verbose
							- Esto creará dentro de project infraestructure una carpeta Migrations con una clase Configurations.
					
					- Habilitar migraciones
						- En project Addressbook.Infrastructure Migrations/Configurations.cs modificar a true AutomaticMigrationsEnabled
						
					- Migrar: 
						- Ejecutar en Package Manager del proyecto infraestructure 
							- update-database -verbose
							
					- Seed Users 
						- En project .Infrastructure Migrations/Configurations.cs/Seed agregar
							```cs
								namespace Addressbook.Infrastructure.Migrations
								{
									....
								
									internal sealed class Configuration : DbMigrationsConfiguration<Addressbook.Infrastructure.DataAccess.DataContext>
									{
										....
								
										protected override void Seed(Addressbook.Infrastructure.DataAccess.DataContext context)
										{							
											context.Users.AddOrUpdate(u => u.Email, new DataAccess.Entities.User
											{
												Email = "admin@gmail.com",
												Password = "5f4dcc3b5aa765d61d8327deb882cf99".ToUpper()
											});
										}
									}
								}
							```
						- Para enviar migración ejecutar
							- update-database -verbose
								
			- Adding DI V12
				- Instalar paquetes
					- Instalar en Addressbook.web paquete Ninject.Web.Common V12 7.00
						- Install-Package Ninject.Web.Common -Version 3.3.2

					- Instalar en Addressbook.web paquete Ninject
						- Install-Package Ninject -Version 3.3.6
						
				- Configure Ninject V12 8.15
					- Copiar de: gist.github.com/odytrice/5821087
						- A small Library to configure Ninject (A Dependency Injection Library) with an ASP.NET Application.
					- En la carpeta .Web/App_Start crear archivo de tipo clase con nombre: Ninject.Mvc.cs
						- Quitar del namespace el .App_Start a fin que el quede de la siguiente forma
							- namespace Ninject.Mvc
						- Copiar contenido de pag. web ahí.
						
					- Registrar Ninject library en Global.asax.cs agregando "NinjectContainer.RegisterAssembly();"
						```cs
							namespace Addressbook.Web
							{
								public class MvcApplication : System.Web.HttpApplication
								{
									protected void Application_Start()
									{
										NinjectContainer.RegisterAssembly();
										AreaRegistration.RegisterAllAreas();
										RouteConfig.RegisterRoutes(RouteTable.Routes);
										BundleConfig.RegisterBundles(BundleTable.Bundles);
									}
								}
							}
						```
						
				- Definir estructura
					- Agregar en raíz de Addressbook.Core folder Interface
						- Agregar dentro de este los folders Managers y Queries
					- Crear interfases
						- Interface/Manager/IAccountManager
						- Interface/Manager/IAccountQueries
					- Crear folder Managers (Si existe el folder Services cambiar el nombre del folder Services a Managers)
						- Crear clase AccountManager.cs que implementa IAccountManager (Nota aún no creado)
							- En caso de existir eliminar la clase si existe AccountServices.cs (Nota: Debe estar vacía)

					- Ya no se requiere, ya se modifico el readme. Mover contenido de carpeta Addressbook.Infrastructure.DataAccess a raíz del proyecto y cambiar namespaces en
						- Archivos dentro de entidades
						- DBContext
						- Configuration.cs

					