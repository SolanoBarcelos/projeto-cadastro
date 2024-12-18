# Etapa Base (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Etapa de Build (SDK)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar os arquivos de projeto e restaurar dependências
COPY ["./CoreContato/CoreContato.csproj", "./CoreContato/"]
COPY ["./ConsumidorContato/PersistenciaContato.csproj", "./PersistenciaContato/"]
COPY ["./RegistoContato/RegistoContato.csproj", "./RegistoContato/"]
COPY ["./TestesCadastroPersistencia/TestesCadastroPersistencia.csproj", "./TestesCadastroPersistencia/"]

# Restaurar dependências
COPY ./ContatoMessenger.sln ./  
RUN dotnet restore "ContatoMessenger.sln"

# Copiar arquivos e compilar
COPY . ./
RUN dotnet build "./CoreContato/CoreContato.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet build "./ConsumidorContato/PersistenciaContato.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet build "./RegistoContato/RegistoContato.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet build "./TestesCadastroPersistencia/TestesCadastroPersistencia.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa de Publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish ".RegistoContato/RegistoContato.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa Final: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Configuração da conexão com o PostgreSQL na rede "mynw"
ENV ConnectionStrings__DefaultConnection="Host=postgres;Port=5432;Pooling=true;Database=db_pos_fase_1;User Id=admin;Password=1234;"

# Copiar arquivos publicados
COPY --from=publish /app/publish .

# Expor a porta configurada
EXPOSE 7070

# Configurar entrada principal
ENTRYPOINT ["dotnet", "PersistenciaContato.dll"]
