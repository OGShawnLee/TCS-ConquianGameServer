using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ConquiánServidor.ConquiánDB;

public partial class ConquiánContext : DbContext
{
    public ConquiánContext()
    {
    }

    public ConquiánContext(DbContextOptions<ConquiánContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Friendship> Friendships { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GamePlayer> GamePlayers { get; set; }

    public virtual DbSet<Gamemode> Gamemodes { get; set; }

    public virtual DbSet<LevelRule> LevelRules { get; set; }

    public virtual DbSet<Lobby> Lobbies { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Social> Socials { get; set; }

    public virtual DbSet<SocialType> SocialTypes { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<StatusLobby> StatusLobbies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // 1. Configuramos el lector para que busque el appsettings.json en la carpeta donde corre el servidor
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // 2. Leemos la cadena de conexión por su nombre ("ConquianDB")
            string connectionString = configuration.GetConnectionString("ConquianDB");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("ERROR CRÍTICO: No se encontró la cadena de conexión 'ConquianDB' " +
                    "dentro del archivo 'appsettings.json'. Asegúrate de que el archivo esté marcado como 'Copiar siempre' en sus propiedades.");
            }

            // 3. Asignamos la cadena a SQL Server
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Friendship>(entity =>
        {
            entity.HasKey(e => e.IdFriendship).HasName("PK__Friendsh__039DB7453AC47704");

            entity.ToTable("Friendship");

            entity.Property(e => e.IdFriendship).HasColumnName("idFriendship");
            entity.Property(e => e.IdDestino).HasColumnName("idDestino");
            entity.Property(e => e.IdOrigen).HasColumnName("idOrigen");
            entity.Property(e => e.IdStatus).HasColumnName("idStatus");

            entity.HasOne(d => d.IdDestinoNavigation).WithMany(p => p.FriendshipIdDestinoNavigations)
                .HasForeignKey(d => d.IdDestino)
                .HasConstraintName("FK_Friendship_Destino");

            entity.HasOne(d => d.IdOrigenNavigation).WithMany(p => p.FriendshipIdOrigenNavigations)
                .HasForeignKey(d => d.IdOrigen)
                .HasConstraintName("FK_Friendship_Origen");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Friendships)
                .HasForeignKey(d => d.IdStatus)
                .HasConstraintName("FK_Friendship_Status");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.IdGame);

            entity.ToTable("Game");

            entity.Property(e => e.IdGame).HasColumnName("idGame");
            entity.Property(e => e.DatePlayed)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("datePlayed");
            entity.Property(e => e.GameTime).HasColumnName("gameTime");
            entity.Property(e => e.IdGamemode).HasColumnName("idGamemode");

            entity.HasOne(d => d.IdGamemodeNavigation).WithMany(p => p.Games)
                .HasForeignKey(d => d.IdGamemode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Game_Gamemode");
        });

        modelBuilder.Entity<GamePlayer>(entity =>
        {
            entity.HasKey(e => e.IdGamePlayer);

            entity.ToTable("GamePlayer");

            entity.Property(e => e.IdGamePlayer).HasColumnName("idGamePlayer");
            entity.Property(e => e.IdGame).HasColumnName("idGame");
            entity.Property(e => e.IdPlayer).HasColumnName("idPlayer");
            entity.Property(e => e.IsWinner).HasColumnName("isWinner");
            entity.Property(e => e.Score).HasColumnName("score");

            entity.HasOne(d => d.IdGameNavigation).WithMany(p => p.GamePlayers)
                .HasForeignKey(d => d.IdGame)
                .HasConstraintName("FK_GamePlayer_Game");

            entity.HasOne(d => d.IdPlayerNavigation).WithMany(p => p.GamePlayers)
                .HasForeignKey(d => d.IdPlayer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GamePlayer_Player");
        });

        modelBuilder.Entity<Gamemode>(entity =>
        {
            entity.HasKey(e => e.IdGamemode).HasName("PK__Gamemode__070E67C412BA4858");

            entity.ToTable("Gamemode");

            entity.Property(e => e.IdGamemode).HasColumnName("idGamemode");
            entity.Property(e => e.Gamemode1)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("gamemode");
        });

        modelBuilder.Entity<LevelRule>(entity =>
        {
            entity.HasKey(e => e.LevelNumber);

            entity.Property(e => e.LevelNumber).ValueGeneratedNever();
            entity.Property(e => e.RankName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Lobby>(entity =>
        {
            entity.HasKey(e => e.IdLobby).HasName("PK__Lobby__06C9F7C748013582");

            entity.ToTable("Lobby");

            entity.HasIndex(e => e.RoomCode, "UQ__Lobby__177CB9366745BC73").IsUnique();

            entity.Property(e => e.IdLobby).HasColumnName("idLobby");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("creationDate");
            entity.Property(e => e.IdGamemode).HasColumnName("idGamemode");
            entity.Property(e => e.IdHostPlayer).HasColumnName("idHostPlayer");
            entity.Property(e => e.IdStatusLobby)
                .HasDefaultValue(1)
                .HasColumnName("idStatusLobby");
            entity.Property(e => e.RoomCode)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("roomCode");

            entity.HasOne(d => d.IdGamemodeNavigation).WithMany(p => p.Lobbies)
                .HasForeignKey(d => d.IdGamemode)
                .HasConstraintName("FK_Lobby_Gamemode");

            entity.HasOne(d => d.IdHostPlayerNavigation).WithMany(p => p.Lobbies)
                .HasForeignKey(d => d.IdHostPlayer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lobby_HostPlayer");

            entity.HasOne(d => d.IdStatusLobbyNavigation).WithMany(p => p.Lobbies)
                .HasForeignKey(d => d.IdStatusLobby)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lobby_Status");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.IdPlayer).HasName("PK__Player__3EFB5EA60EDF9226");

            entity.ToTable("Player");

            entity.Property(e => e.IdPlayer).HasColumnName("idPlayer");
            entity.Property(e => e.CodeExpiryDate)
                .HasColumnType("datetime")
                .HasColumnName("codeExpiryDate");
            entity.Property(e => e.CurrentPoints).HasColumnName("currentPoints");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.IdLevel)
                .HasDefaultValue(1)
                .HasColumnName("idLevel");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lastName");
            entity.Property(e => e.Name)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Nickname)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("nickname");
            entity.Property(e => e.Password)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PathPhoto)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("pathPhoto");
            entity.Property(e => e.VerificationCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("verificationCode");

            entity.HasOne(d => d.IdLevelNavigation).WithMany(p => p.Players)
                .HasForeignKey(d => d.IdLevel)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Player_LevelRules");
        });

        modelBuilder.Entity<Social>(entity =>
        {
            entity.HasKey(e => e.IdSocial).HasName("PK__Social__C735C0DB15934A28");

            entity.ToTable("Social");

            entity.Property(e => e.IdSocial).HasColumnName("idSocial");
            entity.Property(e => e.IdPlayer).HasColumnName("idPlayer");
            entity.Property(e => e.IdSocialType).HasColumnName("idSocialType");
            entity.Property(e => e.UserLink)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("userLink");

            entity.HasOne(d => d.IdPlayerNavigation).WithMany(p => p.Socials)
                .HasForeignKey(d => d.IdPlayer)
                .HasConstraintName("FK_Social_Player");

            entity.HasOne(d => d.IdSocialTypeNavigation).WithMany(p => p.Socials)
                .HasForeignKey(d => d.IdSocialType)
                .HasConstraintName("FK_Social_SocialType");
        });

        modelBuilder.Entity<SocialType>(entity =>
        {
            entity.HasKey(e => e.IdSocialType).HasName("PK__SocialTy__375CD3E569E20035");

            entity.ToTable("SocialType");

            entity.Property(e => e.IdSocialType).HasColumnName("idSocialType");
            entity.Property(e => e.Type)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.IdStatus).HasName("PK__Status__01936F74BE5C2E90");

            entity.ToTable("Status");

            entity.Property(e => e.IdStatus).HasColumnName("idStatus");
            entity.Property(e => e.Status1)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Status");
        });

        modelBuilder.Entity<StatusLobby>(entity =>
        {
            entity.HasKey(e => e.IdStatusLobby).HasName("PK__StatusLo__FDD2755A09C8D4D2");

            entity.ToTable("StatusLobby");

            entity.HasIndex(e => e.StatusName, "UQ__StatusLo__6A50C212F06123D5").IsUnique();

            entity.Property(e => e.IdStatusLobby).HasColumnName("idStatusLobby");
            entity.Property(e => e.StatusName)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("statusName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}