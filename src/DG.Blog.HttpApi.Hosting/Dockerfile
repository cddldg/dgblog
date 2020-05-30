FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-nanoserver-1903 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1903 AS build
WORKDIR /src
COPY ["src/DG.Blog.HttpApi.Hosting/DG.Blog.HttpApi.Hosting.csproj", "src/DG.Blog.HttpApi.Hosting/"]
COPY ["src/DG.Blog.Domain.Shared/DG.Blog.Domain.Shared.csproj", "src/DG.Blog.Domain.Shared/"]
COPY ["src/DG.Blog.Domain/DG.Blog.Domain.csproj", "src/DG.Blog.Domain/"]
COPY ["src/DG.Blog.HttpApi/DG.Blog.HttpApi.csproj", "src/DG.Blog.HttpApi/"]
COPY ["src/DG.Blog.Application/DG.Blog.Application.csproj", "src/DG.Blog.Application/"]
COPY ["src/DG.Blog.Application.Contracts/DG.Blog.Application.Contracts.csproj", "src/DG.Blog.Application.Contracts/"]
COPY ["src/DG.Blog.Application.Caching/DG.Blog.Application.Caching.csproj", "src/DG.Blog.Application.Caching/"]
COPY ["src/DG.Blog.ToolKits/DG.Blog.ToolKits.csproj", "src/DG.Blog.ToolKits/"]
COPY ["src/DG.Blog.BackgroundJobs/DG.Blog.BackgroundJobs.csproj", "src/DG.Blog.BackgroundJobs/"]
COPY ["src/DG.Blog.Swagger/DG.Blog.Swagger.csproj", "src/DG.Blog.Swagger/"] 
COPY ["src/DG.Blog.EntityFrameworkCore/DG.Blog.EntityFrameworkCore.csproj", "src/DG.Blog.EntityFrameworkCore/"]
RUN dotnet restore "src/DG.Blog.HttpApi.Hosting/DG.Blog.HttpApi.Hosting.csproj"
COPY . .
WORKDIR "/src/src/DG.Blog.HttpApi.Hosting"
RUN dotnet build "DG.Blog.HttpApi.Hosting.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DG.Blog.HttpApi.Hosting.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DG.Blog.HttpApi.Hosting.dll"]
