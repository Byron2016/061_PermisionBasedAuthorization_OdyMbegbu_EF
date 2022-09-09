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
					- Crear en la raíz folder Services
					- Agregar clase AccountService
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