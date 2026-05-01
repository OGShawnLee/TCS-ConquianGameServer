using ConquiánServidor.Contracts.ServiceContracts;
using ConquiánServidor.Services;
using CoreWCF;
using CoreWCF.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using NLog;

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

                builder.Services.AddServiceModelServices();
                builder.Services.AddServiceModelMetadata();

                builder.WebHost.ConfigureKestrel(options =>
                {
                    options.ListenAnyIP(8080);
                });
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
                ((IApplicationBuilder)app).UseServiceModel(builder =>
                {
                    builder.AddService<SignUp>();
                    builder.AddServiceEndpoint<SignUp>(typeof(ISignUp), new BasicHttpBinding(), "/signUp");

                    builder.AddService<Login>();
                    builder.AddServiceEndpoint<Login>(typeof(ILogin), new BasicHttpBinding(), "/login");
                    
                    builder.AddService<UserProfile>();
                    builder.AddServiceEndpoint<UserProfile>(typeof(IUserProfile), new BasicHttpBinding(), "/userprofile");
                    
                    builder.AddService<FriendList>();
                    builder.AddServiceEndpoint<FriendList>(typeof(IFriendList), new BasicHttpBinding(), "/friendlist");
                    
                    builder.AddService<Invitation>();
                    builder.AddServiceEndpoint<Invitation>(typeof(IInvitationService), new NetTcpBinding(), "/invitation");
                    
                    builder.AddService<PasswordRecovery>();
                    builder.AddServiceEndpoint<PasswordRecovery>(typeof(IPasswordRecovery), new BasicHttpBinding(), "/password-recovery");
                    
                    builder.AddService<Presence>();
                    builder.AddServiceEndpoint<Presence>(typeof(IPresence), new NetTcpBinding(), "/presence");
                    
                    builder.AddService<GuestInvitation>();
                    builder.AddServiceEndpoint<GuestInvitation>(typeof(IGuestInvitation), new BasicHttpBinding(), "/guest");
                    
                    builder.AddService<Lobby>();
                    builder.AddServiceEndpoint<Lobby>(typeof(ILobby), new NetTcpBinding(), "/lobby");
                    
                    builder.AddService<Game>();
                    builder.AddServiceEndpoint<Game>(typeof(IGame), new NetTcpBinding(), "/game");
                });

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[OK] Todos los servicios registrados.");
                Console.ResetColor();

                return app;
            }
            catch (AddressAlreadyInUseException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[FAIL] - Puerto en uso: {ex.Message}");
                Console.ResetColor();
                throw;
            }
            catch (CommunicationException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[FAIL] - Error de comunicación: {ex.Message}");
                Console.ResetColor();
                throw;
            }
        }
    }
}
