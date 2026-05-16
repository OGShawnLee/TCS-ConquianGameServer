using ConquiánServidor.ConquiánDB;            // Para tu ConquiánContext
using ConquiánServidor.Contracts.ServiceContracts;
using ConquiánServidor.Services;
using CoreWCF;
using CoreWCF.Channels;
using CoreWCF.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;          // Para UseSqlServer
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; // Para GetRequiredService
using NLog;
using System;
using System.Threading.Tasks;

namespace ConquiánServidor
{
    public class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            Console.Title = "Conquián Server";

            try
            {
                logger.Info("Starting Conquián Server...");
                PrintBanner();

                WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

                // 1. CONFIGURACIÓN DE BASE DE DATOS (EF Core)
                var connectionString = builder.Configuration.GetConnectionString("ConquianDB");
                builder.Services.AddDbContext<ConquiánContext>(options =>
                    options.UseSqlServer(connectionString));

                // 2. REGISTRO DE CORE WCF
                builder.Services.AddServiceModelServices();
                builder.Services.AddServiceModelMetadata();

                // 3. CONFIGURACIÓN DE PUERTOS
                // Configuramos Kestrel para escuchar peticiones HTTP estándar en el puerto 8080
                builder.WebHost.ConfigureKestrel(options =>
                {
                    options.ListenAnyIP(8080);
                });

                // SOLUCIÓN AL ERROR DE UseNetTcp: Se aplica directamente sobre el WebHost
                builder.WebHost.UseNetTcp(8081);

                WebApplication app = RegisterServices(builder);

                Console.WriteLine();
                Console.WriteLine("Todos los servicios están activos.");
                Console.WriteLine("Presiona ENTER para detener el servidor...");

                Task serverTask = app.RunAsync();

                Console.ReadLine();

                Console.WriteLine("Deteniendo el servidor de forma segura...");
                app.StopAsync().GetAwaiter().GetResult();

                serverTask.GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Error fatal al iniciar el servidor.");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[FATAL] {ex.Message}");
                Console.ResetColor();
                Console.ReadKey();
            }
            finally
            {
                logger.Info("Servidor detenido correctamente.");
                Console.WriteLine("Servidor apagado.");
            }
        }

        private static void PrintBanner()
        {
            Console.WriteLine();
            Console.WriteLine("        CONQUIÁN SERVER");
            Console.WriteLine();
        }

        private static WebApplication RegisterServices(WebApplicationBuilder builder)
        {
            WebApplication app = builder.Build();

            try
            {
                // SOLUCIÓN AL ERROR DE AMBIGÜEDAD:
                // Forzamos al compilador a usar la interfaz IApplicationBuilder pasándolo a una variable explícita
                IApplicationBuilder appBuilder = app;

                appBuilder.UseServiceModel(serviceBuilder =>
                {
                    var basicBinding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                    var tcpBinding = new NetTcpBinding(SecurityMode.None);

                    serviceBuilder.AddService<SignUp>();
                    serviceBuilder.AddServiceEndpoint<SignUp, ISignUp>(basicBinding, "/signUp");

                    serviceBuilder.AddService<Login>();
                    serviceBuilder.AddServiceEndpoint<Login, ILogin>(basicBinding, "/login");

                    serviceBuilder.AddService<UserProfile>();
                    serviceBuilder.AddServiceEndpoint<UserProfile, IUserProfile>(basicBinding, "/userprofile");

                    serviceBuilder.AddService<FriendList>();
                    serviceBuilder.AddServiceEndpoint<FriendList, IFriendList>(basicBinding, "/friendlist");

                    serviceBuilder.AddService<PasswordRecovery>();
                    serviceBuilder.AddServiceEndpoint<PasswordRecovery, IPasswordRecovery>(basicBinding, "/password-recovery");

                    serviceBuilder.AddService<GuestInvitation>();
                    serviceBuilder.AddServiceEndpoint<GuestInvitation, IGuestInvitation>(basicBinding, "/guest");

                    // Servicios Duplex / TCP
                    serviceBuilder.AddService<Invitation>();
                    serviceBuilder.AddServiceEndpoint<Invitation, IInvitationService>(tcpBinding, "/invitation");

                    serviceBuilder.AddService<Presence>();
                    serviceBuilder.AddServiceEndpoint<Presence, IPresence>(tcpBinding, "/presence");

                    // Solución a la ambigüedad interna de nombres (Clase vs Tabla DB)
                    serviceBuilder.AddService<ConquiánServidor.Services.Lobby>();
                    serviceBuilder.AddServiceEndpoint<ConquiánServidor.Services.Lobby, ILobby>(tcpBinding, "/lobby");

                    serviceBuilder.AddService<ConquiánServidor.Services.Game>();
                    serviceBuilder.AddServiceEndpoint<ConquiánServidor.Services.Game, IGame>(tcpBinding, "/game");
                });

                // Habilitar la exposición de metadatos WSDL
                var serviceMetadataBehavior = app.Services.GetRequiredService<CoreWCF.Description.ServiceMetadataBehavior>();
                serviceMetadataBehavior.HttpGetEnabled = true;
                serviceMetadataBehavior.HttpGetUrl = new Uri("http://localhost:8080/metadata");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[OK] Todos los servicios registrados.");
                Console.ResetColor();

                return app;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[FAIL] - Error registrando servicios: {ex.Message}");
                Console.ResetColor();
                throw;
            }
        }
    }
}