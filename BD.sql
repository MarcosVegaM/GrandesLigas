USE [master]
GO
/****** Object:  Database [GrandesLigas]    Script Date: 20/04/2025 03:29:49 p. m. ******/
CREATE DATABASE [GrandesLigas]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'GrandesLigas', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\GrandesLigas.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'GrandesLigas_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\GrandesLigas_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [GrandesLigas] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [GrandesLigas].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [GrandesLigas] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [GrandesLigas] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [GrandesLigas] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [GrandesLigas] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [GrandesLigas] SET ARITHABORT OFF 
GO
ALTER DATABASE [GrandesLigas] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [GrandesLigas] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [GrandesLigas] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [GrandesLigas] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [GrandesLigas] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [GrandesLigas] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [GrandesLigas] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [GrandesLigas] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [GrandesLigas] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [GrandesLigas] SET  DISABLE_BROKER 
GO
ALTER DATABASE [GrandesLigas] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [GrandesLigas] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [GrandesLigas] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [GrandesLigas] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [GrandesLigas] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [GrandesLigas] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [GrandesLigas] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [GrandesLigas] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [GrandesLigas] SET  MULTI_USER 
GO
ALTER DATABASE [GrandesLigas] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [GrandesLigas] SET DB_CHAINING OFF 
GO
ALTER DATABASE [GrandesLigas] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [GrandesLigas] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [GrandesLigas] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [GrandesLigas] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [GrandesLigas] SET QUERY_STORE = OFF
GO
USE [GrandesLigas]
GO
/****** Object:  Table [dbo].[AsistenciaEntrenamiento]    Script Date: 20/04/2025 03:29:49 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AsistenciaEntrenamiento](
	[AsistenciaId] [int] IDENTITY(1,1) NOT NULL,
	[EntrenamientoId] [int] NOT NULL,
	[JugadorId] [int] NOT NULL,
	[Asistio] [bit] NOT NULL,
	[RendimientoNota] [decimal](3, 1) NULL,
PRIMARY KEY CLUSTERED 
(
	[AsistenciaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clasificaciones]    Script Date: 20/04/2025 03:29:49 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clasificaciones](
	[ClasificacionId] [int] IDENTITY(1,1) NOT NULL,
	[LigaId] [int] NOT NULL,
	[EquipoId] [int] NOT NULL,
	[JuegosJugados] [int] NULL,
	[JuegosGanados] [int] NULL,
	[JuegosPerdidos] [int] NULL,
	[CarrerasAnotadas] [int] NULL,
	[CarrerasRecibidas] [int] NULL,
	[Porcentaje] [decimal](18, 0) NULL,
PRIMARY KEY CLUSTERED 
(
	[ClasificacionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Entrenadores]    Script Date: 20/04/2025 03:29:49 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Entrenadores](
	[EntrenadorId] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Especialidad] [nvarchar](100) NULL,
 CONSTRAINT [PK__Entrenad__D0EE85657EF09071] PRIMARY KEY CLUSTERED 
(
	[EntrenadorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EntrenadoresEquipos]    Script Date: 20/04/2025 03:29:49 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntrenadoresEquipos](
	[EntrenadorEquipoId] [int] IDENTITY(1,1) NOT NULL,
	[EntrenadorId] [int] NOT NULL,
	[EquipoId] [int] NOT NULL,
	[FechaIngreso] [datetime] NULL,
 CONSTRAINT [PK__Entrenad__ABE9DA2FC5F885C3] PRIMARY KEY CLUSTERED 
(
	[EntrenadorEquipoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Entrenamiento]    Script Date: 20/04/2025 03:29:49 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Entrenamiento](
	[EntrenamientoId] [int] IDENTITY(1,1) NOT NULL,
	[EquipoId] [int] NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[Ubicacion] [nvarchar](100) NULL,
	[Descripcion] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[EntrenamientoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Equipos]    Script Date: 20/04/2025 03:29:49 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Equipos](
	[EquipoId] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Ciudad] [nvarchar](100) NULL,
	[LigaId] [int] NOT NULL,
	[Logo] [varbinary](max) NULL,
	[FotoEstadio] [varbinary](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[EquipoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstadisticaJugador]    Script Date: 20/04/2025 03:29:49 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstadisticaJugador](
	[EstadisticaId] [int] IDENTITY(1,1) NOT NULL,
	[PartidoId] [int] NOT NULL,
	[JugadorId] [int] NOT NULL,
	[Carreras] [int] NULL,
	[Hits] [int] NULL,
	[BasesRobadas] [int] NULL,
	[Ponches] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[EstadisticaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HistorialEquipo]    Script Date: 20/04/2025 03:29:49 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HistorialEquipo](
	[HistorialId] [int] IDENTITY(1,1) NOT NULL,
	[EquipoId] [int] NOT NULL,
	[Temporada] [nvarchar](50) NULL,
	[Logro] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[HistorialId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Jugadores]    Script Date: 20/04/2025 03:29:49 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Jugadores](
	[JugadorId] [int] IDENTITY(1,1) NOT NULL,
	[Posicion] [nvarchar](50) NULL,
	[FechaNacimiento] [date] NULL,
	[Foto] [varbinary](max) NULL,
	[Nombre] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__Jugador__4B5753A2F12DDFE1] PRIMARY KEY CLUSTERED 
(
	[JugadorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JugadoresEquipos]    Script Date: 20/04/2025 03:29:49 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JugadoresEquipos](
	[JugadorEquipoId] [int] IDENTITY(1,1) NOT NULL,
	[JugadorId] [int] NOT NULL,
	[EquipoId] [int] NOT NULL,
	[FechaIngreso] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[JugadorEquipoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ligas]    Script Date: 20/04/2025 03:29:49 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ligas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Temporada] [nvarchar](50) NULL,
	[Descripcion] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Partidos]    Script Date: 20/04/2025 03:29:49 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Partidos](
	[PartidoId] [int] IDENTITY(1,1) NOT NULL,
	[LigaId] [int] NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[Lugar] [nvarchar](100) NULL,
	[EquipoLocalId] [int] NOT NULL,
	[EquipoVisitanteId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PartidoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Resultados]    Script Date: 20/04/2025 03:29:49 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Resultados](
	[ResultadoId] [int] IDENTITY(1,1) NOT NULL,
	[PartidoId] [int] NOT NULL,
	[CarrerasLocal] [int] NULL,
	[CarrerasVisitante] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ResultadoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 20/04/2025 03:29:49 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[Contraseña] [nvarchar](255) NOT NULL,
	[FechaRegistro] [datetime] NULL,
	[TokenRestablecimiento] [nvarchar](255) NULL,
	[Tipo] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Clasificaciones] ADD  DEFAULT ((0)) FOR [JuegosJugados]
GO
ALTER TABLE [dbo].[Clasificaciones] ADD  DEFAULT ((0)) FOR [JuegosGanados]
GO
ALTER TABLE [dbo].[Clasificaciones] ADD  DEFAULT ((0)) FOR [JuegosPerdidos]
GO
ALTER TABLE [dbo].[Clasificaciones] ADD  DEFAULT ((0)) FOR [CarrerasAnotadas]
GO
ALTER TABLE [dbo].[Clasificaciones] ADD  DEFAULT ((0)) FOR [CarrerasRecibidas]
GO
ALTER TABLE [dbo].[EstadisticaJugador] ADD  DEFAULT ((0)) FOR [Carreras]
GO
ALTER TABLE [dbo].[EstadisticaJugador] ADD  DEFAULT ((0)) FOR [Hits]
GO
ALTER TABLE [dbo].[EstadisticaJugador] ADD  DEFAULT ((0)) FOR [BasesRobadas]
GO
ALTER TABLE [dbo].[EstadisticaJugador] ADD  DEFAULT ((0)) FOR [Ponches]
GO
ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[AsistenciaEntrenamiento]  WITH CHECK ADD FOREIGN KEY([EntrenamientoId])
REFERENCES [dbo].[Entrenamiento] ([EntrenamientoId])
GO
ALTER TABLE [dbo].[AsistenciaEntrenamiento]  WITH CHECK ADD  CONSTRAINT [FK__Asistenci__Jugad__52593CB8] FOREIGN KEY([JugadorId])
REFERENCES [dbo].[Jugadores] ([JugadorId])
GO
ALTER TABLE [dbo].[AsistenciaEntrenamiento] CHECK CONSTRAINT [FK__Asistenci__Jugad__52593CB8]
GO
ALTER TABLE [dbo].[Clasificaciones]  WITH CHECK ADD FOREIGN KEY([EquipoId])
REFERENCES [dbo].[Equipos] ([EquipoId])
GO
ALTER TABLE [dbo].[Clasificaciones]  WITH CHECK ADD FOREIGN KEY([LigaId])
REFERENCES [dbo].[Ligas] ([Id])
GO
ALTER TABLE [dbo].[EntrenadoresEquipos]  WITH CHECK ADD  CONSTRAINT [FK__Entrenado__Entre__3B75D760] FOREIGN KEY([EntrenadorId])
REFERENCES [dbo].[Entrenadores] ([EntrenadorId])
GO
ALTER TABLE [dbo].[EntrenadoresEquipos] CHECK CONSTRAINT [FK__Entrenado__Entre__3B75D760]
GO
ALTER TABLE [dbo].[EntrenadoresEquipos]  WITH CHECK ADD  CONSTRAINT [FK__Entrenado__Equip__3C69FB99] FOREIGN KEY([EquipoId])
REFERENCES [dbo].[Equipos] ([EquipoId])
GO
ALTER TABLE [dbo].[EntrenadoresEquipos] CHECK CONSTRAINT [FK__Entrenado__Equip__3C69FB99]
GO
ALTER TABLE [dbo].[Entrenamiento]  WITH CHECK ADD FOREIGN KEY([EquipoId])
REFERENCES [dbo].[Equipos] ([EquipoId])
GO
ALTER TABLE [dbo].[Equipos]  WITH CHECK ADD FOREIGN KEY([LigaId])
REFERENCES [dbo].[Ligas] ([Id])
GO
ALTER TABLE [dbo].[EstadisticaJugador]  WITH CHECK ADD  CONSTRAINT [FK__Estadisti__Jugad__4BAC3F29] FOREIGN KEY([JugadorId])
REFERENCES [dbo].[Jugadores] ([JugadorId])
GO
ALTER TABLE [dbo].[EstadisticaJugador] CHECK CONSTRAINT [FK__Estadisti__Jugad__4BAC3F29]
GO
ALTER TABLE [dbo].[EstadisticaJugador]  WITH CHECK ADD FOREIGN KEY([PartidoId])
REFERENCES [dbo].[Partidos] ([PartidoId])
GO
ALTER TABLE [dbo].[HistorialEquipo]  WITH CHECK ADD FOREIGN KEY([EquipoId])
REFERENCES [dbo].[Equipos] ([EquipoId])
GO
ALTER TABLE [dbo].[Partidos]  WITH CHECK ADD FOREIGN KEY([EquipoLocalId])
REFERENCES [dbo].[Equipos] ([EquipoId])
GO
ALTER TABLE [dbo].[Partidos]  WITH CHECK ADD FOREIGN KEY([EquipoVisitanteId])
REFERENCES [dbo].[Equipos] ([EquipoId])
GO
ALTER TABLE [dbo].[Partidos]  WITH CHECK ADD FOREIGN KEY([LigaId])
REFERENCES [dbo].[Ligas] ([Id])
GO
ALTER TABLE [dbo].[Resultados]  WITH CHECK ADD FOREIGN KEY([PartidoId])
REFERENCES [dbo].[Partidos] ([PartidoId])
GO
USE [master]
GO
ALTER DATABASE [GrandesLigas] SET  READ_WRITE 
GO
