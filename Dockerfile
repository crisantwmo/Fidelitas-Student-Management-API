# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copiar archivos y restaurar
COPY . ./
RUN dotnet restore

# Publicar la aplicación
RUN dotnet publish -c Release -o out

# Etapa final (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/out .

# Exponer el puerto de Render
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "FidelitasAPI.dll"]