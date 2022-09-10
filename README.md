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
			- Agregar clase Models/UserModel (Nota: En V14 2.30 renombramos a User)
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

					- Implementar las queries.
						- En Addressbook.Infrastructure agregar Queries/AccountQueries : IAccountQueries
						
				- Ninject
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
							
							
					- Definir lo que se inyectará usando Ninject V13 0.40
						- en Addressbook.Web
							- Crear en la raíz el folder Modules
							- Agregar clase MainModule
							```cs
								using Addressbook.Core.Interface.Managers;
								using Addressbook.Core.Interface.Queries;
								using Addressbook.Core.Managers;
								using Addressbook.Infrastructure.Queries;
								using Addressbook.Infrastructure;
								using Ninject.Modules;
								using Ninject.Web.Common;
								using System;
								using System.Collections.Generic;
								using System.Data.Entity;
								using System.Linq;
								using System.Web;
								
								namespace Addressbook.Web.Modules
								{
									public class MainModule : NinjectModule
									{
										public override void Load()
										{
											Bind<DbContext>().To<DataContext>().InRequestScope();
											Bind<IAccountQueries>().To<AccountQueries>();
											Bind<IAccountManager>().To<AccountManager>();
										}
									}
								}
							```
							
					- Testear
						- En el .Test añadir referencia a los otros 3 proyectos. 
						- Instalar paquetes:
							- Install-Package Moq -Version 4.18.2
							- Install-Package Ninject -Version 3.3.6
						- Agregar al proyecto Addressbook.Test la class NinjectTests
							- https://gist.github.com/odytrice/243fe6c4bf14aedb584c3fc876b9fe42
							```cs
								namespace Addressbook.Tests
								{
									[TestClass]
									public class NinjectTests
									{
										[TestMethod]
										public void TestBindings()
										{
											//Create Kernel and Load Assembly Application.Web
											var kernel = new StandardKernel();
											kernel.Load(new Assembly[] { Assembly.Load("Addressbook.Web") });
								
											var query = from types in Assembly.Load("AddressBook.Core").GetExportedTypes()
														where types.IsInterface
														where types.Namespace.StartsWith("AddressBook.Core.Interface")
														select types;
											foreach (var i in query.ToList())
											{
												kernel.Get(i);
											}
										}
									}
								}
							```
						- Ir a Test/Test Explorer/ y ejecutar
						
				- Use Operational class V14
					- Código fuente: github.com/odytrice/Operation
					- Instalar en todos los proyectos
						- Install-Package Operation -Version 1.1.2
						
			- Quitar manejo manual de claims y dejar que UserManager cree automáticamente. V14 2.16
				- Crear un objeto que representa al usuario. Usaremos UserModel en .Core/Models
					- Renombrar el AddressBook.Web/Models/UserModel.cs a Users.cs que creamos en Signning V5 - V8 3.40 (Agregar clase Models/UserModel (Nota: En V14 2.30 renombramos a User))
					- Hacer que herede de IUser<int>
					
			- Implementar UserStore (Este es el que le dice a EF como CRUD)
				- Crear en Addressbook.Web.Utils/UserStore : IUserStore<User, int>
					- Implementa los siguientes métodos
						- CreateAsync(User user)
						- DeleteAsync(User user)
						- Dispose()
						- FindByIdAsync(int userId)
						- FindByNameAsync(string userName)
						- UpdateAsync(User user)

					- Para user usará el "using Addressbook.Web.Models;"
					- Agregar un constructor inyectar IAccountManager account
					
						```cs
							using Addressbook.Web.Models;
							using Microsoft.AspNet.Identity;
							using System;
							using System.Collections.Generic;
							using System.Linq;
							using System.Threading.Tasks;
							using System.Web;
							
							namespace Addressbook.Web.Utils
							{
								public class UserStore : IUserStore<User, int>
								{
									private IAccountManager _account;
							
									public UserStore(IAccountManager account)
									{
										_account = account;
									}
									....
						```
					
					- Hacer que AddressBook.Web/Models/User herede de UserModel (De using Addressbook.Core.Models;) V14 - V|5 1.21
						```cs
							namespace Addressbook.Web.Models
							{
								public class User : UserModel, IUser<int>
								{
									public int Id { 
										get { return UserId; } 
										set { UserId = value; } 
									}
							
									public string UserName { 
										get { return Email; } 
										set { Email = value; }
									}
								}
							}
						```
				- Hacer que Adressbook.Web.Utils/UserStore implemente : IUserPasswordStore<User, int> V15 3.35
					- Implementa los siguientes métodos
						- GetPasswordHashAsync
						- HasPasswordAsync
						- SetPasswordHashAsync
						
				- Deploy some methos from Addressbook.Web.Utils/UserStore 
					- CreateAsync(User user)
						```cs
							public Task CreateAsync(User user)
							{
								return _account.CreateUser(user).AsTask();
							}
						```
						
				- Modify some methods from AccountController
					- Add constructor
					- Modify Login HttpPost
					- Modify ValidateUser
					- Modify SignIn
					
						```cs
							using Addressbook.Core.Interface.Managers;
							using Addressbook.Core.Models;
							using Addressbook.Web.Models;
							using Addressbook.Web.Utils;
							using Microsoft.AspNet.Identity;
							using Microsoft.Owin.Security;
							using System;
							using System.Collections.Generic;
							using System.Linq;
							using System.Security.Claims;
							using System.Web;
							using System.Web.Mvc;
							
							namespace Addressbook.Web.Controllers
							{
								public class AccountController : Controller
								{
									private readonly UserManager<User, int> _user;
									
									public IAuthenticationManager Authentication => HttpContext.GetOwinContext().Authentication; 
							
									public AccountController(IAccountManager account)
									{
										_user = new UserManager<User, int>(new UserStore(account)); //v15 5.17
									}
							
									// GET: Login
									public ActionResult Login()
									{
										return View();
									}
							
									[HttpPost]
									public ActionResult Login(LoginModel model, string returnUrl)
									{
										var validateAndSigIn = from user in ValidateUser(model)
															from signIn in SignIn(user, model.RememberMe)
															select user;
										
										//Perform validation
										var isValid = validateAndSigIn.Succeeded;
							
										//Sign User In
										if (isValid)
										{
											if (!string.IsNullOrEmpty(returnUrl))
											{
												//return Redirect(returnUrl);
												return RedirectToAction("Index", "Home"); //v7. 1.51
											}
											else
											{
												return RedirectToAction("Index", "Home");
											}
										}
										else
										{
											return View(model);
										} 
									}
							
									private Operation<User> ValidateUser(LoginModel model) //v16 0.16
									{
										return Operation.Create(() =>
										{
											if (ModelState.IsValid)
											{
												var user = _user.Find(model.Email, model.Password);
												if (user == null)
													throw new Exception("Invalid Username");
												return user;
											}
											else
											{
												var error = ModelState.Values
												.SelectMany(v => v.Errors)
												.Select(e => e.ErrorMessage)
												.Aggregate((ag, e) => ag + ", " + e);
							
												throw new Exception(error);
											}
										});
									}
							
									private Operation<ClaimsIdentity> SignIn(User model, bool rememberMe) //v6 7.29 //remember me v15 8.23
									{
										return Operation.Create(() =>
										{
											var identity = _user.CreateIdentity(model, DefaultAuthenticationTypes.ApplicationCookie); //v5 3.44 - v8 1.42
										
											//optionally add additional claims
											Authentication.SignIn(new AuthenticationProperties { IsPersistent = rememberMe }, identity); //v5 3.18 //V15 9.04
							
											return identity;
										});
									}
							
									public ActionResult LogOut()
									{
										Authentication.SignOut();
										return Redirect("login");
									}
								}
							}
						```
						
				- Agregar extension Method. V18 1.15
					- Agregar a raíz del proyecto Addressbook.Core la clase  Extensions
						- Esta tendrá el namespace Addressbook a fin que se pueda usar en todos los sitios.
						- Agregamos el tag [DebuggerStepThrough] al método Assign para que no haga Debug.
						
							```cs
								using System;
								using System.Collections.Generic;
								using System.Diagnostics;
								using System.Linq;
								using System.Text;
								using System.Threading.Tasks;
								
								namespace Addressbook
								{
									public static class Extensions
									{
										#region Generic Type Extensions
										/// <summary>
										/// Update properties with properties of the object Supplied (typically anonymous)
										/// </summary>
										/// <typeparam name="T">Type of Source Object</typeparam>
										/// <param name="destination">Object whose property you want to update</param>
										/// <param name="source">destination object (typically anonymous) you want to take values from</param>
										/// <returns>Update reference to same Object</returns>
										[DebuggerStepThrough]
										public static T Assign<T>(this T destination, object source)
										{
											if (destination != null && source != null)
											{
												var query = from sourceProperty in source.GetType().GetProperties()
															join destProperty in destination.GetType().GetProperties()
															on sourceProperty.Name.ToLower() equals destProperty.Name.ToLower()             //Case Insensitive Match
															where destProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType)   //Properties can be Assigned
															where destProperty.GetSetMethod() != null                                       //Destination Property is not Readonly
															select new { sourceProperty, destProperty };
								
								
												foreach (var pair in query)
												{
													//Go ahead and Assign the value on the destination
													pair.destProperty
														.SetValue(destination,
															value: pair.sourceProperty.GetValue(obj: source, index: new object[] { }),
															index: new object[] { });
												}
											}
											return destination;
										}
								
										public static U Pipe<T, U>(this T value, Func<T, U> projection) => projection(value);
										#endregion;
									}
								}
							```
				- Agregar constructores para Addressbook.Web.Models/Users V17 8.40
					- Para no tener que hacer
						this.Email = model.Email;
						this.Password = model.Password;
						
					Usamos
						this.Assign(model);
				
				
					```cs
						namespace Addressbook.Web.Models
						{
							public class User : UserModel, IUser<int>
							{
								....
								
								public User()
								{
						
								}
						
								public User(UserModel model)
								{
									//v17 9.18
									this.Assign(model);
						
								}
							}
						}
					```
					
				- Hacer que Adressbook.Web.Utils/UserStore implemente : IUserRoleStore<User, int> V16 9.01
					- Implementa los siguientes métodos
						- AddToRoleAsync
						- GetRolesAsync
						- IsInRoleAsync
						- RemoveFromRoleAsync
						
				- Colocar código en métodos existentes en UserStore V16 9.42 - V17 V18 4.23 - 8.29
					- DeleteAsync
					- Dispose
					- FindByIdAsync
					- FindByNameAsync
					- GetPasswordHashAsync
					- GetRolesAsync
					- HasPasswordAsync
					- IsInRoleAsync
					- RemoveFromRoleAsync
					- SetPasswordHashAsync
					- UpdateAsync
					
				- Definir métodos en IAccountManager
					- Operation DeleteUser(UserModel user);
					- Operation<UserModel> UpdateUser(UserModel user);
					- Operation<string> SetPasswordHash(UserModel user, string passwordHash);
					- Operation RemoveFromRole(UserModel user, string roleName);
					- Operation<bool> IsUserInRole(UserModel user, string roleName);
					- Operation<IList<string>> GetRoles(UserModel user);
					- Operation<string> GetPasswordHash(UserModel user);
					- Operation<UserModel> FindByEmail(string userId);
					- Operation<UserModel> FindById(int userId);
					
					
			- Implement Authorization V19 0.38 
				- Crear Atributos en AddressBook.web.Utils
					- Crear clase AuthorizeUserAttribute : AuthorizeAttribute
					- Esta clase utilizará permisions no roles BA
					```cs
						namespace Addressbook.Web.Utils
						{
							public class AuthorizeUserAttribute : AuthorizeAttribute
							{
								private string[] _permissions;
								private IAccountManager _account;
								public AuthorizeUserAttribute(params string[] permissions)
								{
									_permissions = permissions;
									_account = NinjectContainer.Resolve<IAccountManager>();
								}
								protected override bool AuthorizeCore(HttpContextBase httpContext)
								{
									//First Make Sure that the User is Authenticated
									if (httpContext.User.Identity.IsAuthenticated)
									{
										//Get Permissions List in Session
										var permissions = httpContext.Session["Permissions"] as string[];
										if (permissions == null)
										{
											//Fetch Permissions
											var getPermissions = _account.GetPermissions(httpContext.User.Identity.GetUserId<int>());
											if (getPermissions.Any())
											{
												//Cache Permissions
												httpContext.Session["Permissions"] = getPermissions.Select(p => p.Name).ToArray();
						
												//Check to See if User Has all the Required Permissions
												var query = from permission in _permissions
															join userpermission in getPermissions
															on permission.ToLower() equals userpermission.Name.ToLower()
															select permission;
												return query.Any();
											}
										}
										else
										{
											var query = from permission in _permissions
														join userpermission in permissions
														on permission.ToLower() equals userpermission.ToLower()
														select permission;
											return query.Any();
										}
									}
									return false;
								}
						
								protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
								{
									filterContext.Result = new RedirectResult("/account/notauthorized");
								}
							}
						}
					```

			- Se modifica momentaneamente el método AddressBook.web.Utils/AuthorizeUserAttribute/AuthorizeCore ya que está presentando un error, para hacer pruebas y luego corregir.
				```cs
					....
					if (permissions == null)
					{
						//Fetch Permissions
						//var getPermissions = _account.GetPermissions(httpContext.User.Identity.GetUserId<int>());
						IList<PermissionModel> getPermissions = new List<PermissionModel> //Hago esto x q la expresión anterior en este momento me está dando errores en el Any.
						{
							new PermissionModel
							{
								PermissionID = 1,
								Name = "Home-Page"
							},
							new PermissionModel
							{
								PermissionID = 1,
								Name = "Account-Page"
							}
						};
						
						....
					
				```
				
				