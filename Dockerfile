# 1. AŞAMA: PAYLAŞILAN DERLEME (BUILD) ORTAMI
#----------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Önce .sln ve TÜM .csproj dosyalarını kopyala (katman önbelleklemesi için)
COPY *.sln .
COPY BlogApp.Domain/*.csproj ./BlogApp.Domain/
COPY BlogApp.Application/*.csproj ./BlogApp.Application/
COPY BlogApp.Infrastructure/*.csproj ./BlogApp.Infrastructure/
COPY BlogApp.Presentation/*.csproj ./BlogApp.Presentation/
COPY BlogApp.Web/BlogApp.Web/*.csproj ./BlogApp.Web/BlogApp.Web/
COPY BlogApp.Web/BlogApp.Web.Client/*.csproj ./BlogApp.Web/BlogApp.Web.Client/

# BÜTÜN ÇÖZÜM İÇİN RESTORE İŞLEMİNİ BİR KEZ YAP
RUN dotnet restore "BlogApp.sln"

# Geri kalan tüm kaynak kodunu kopyala
COPY . .

# 2. AŞAMA: API Projesini Yayınla (Publish)
#----------------------------------------------------
FROM build AS publish-api
WORKDIR "/src/BlogApp.Presentation"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# 3. AŞAMA: WebApp Projesini Yayınla (Publish)
#----------------------------------------------------
FROM build AS publish-webapp
WORKDIR "/src/BlogApp.Web/BlogApp.Web"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false


# 4. AŞAMA: SON API İMAJINI OLUŞTUR
#----------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS api-final
WORKDIR /app
# Sadece API'nin yayınlanmış dosyalarını kopyala
COPY --from=publish-api /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "BlogApp.Presentation.dll"]


# 5. AŞAMA: SON WEBAPP İMAJINI OLUŞTUR
#----------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS webapp-final
WORKDIR /app
# Sadece WebApp'in yayınlanmış dosyalarını kopyala
COPY --from=publish-webapp /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "BlogApp.Web.dll"]