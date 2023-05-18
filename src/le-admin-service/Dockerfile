#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG NUGET_GITHUB_PASSWORD
WORKDIR /src
COPY ["LE.AdminService/NuGet.config", "LE.AdminService/"]
RUN sed -i "s|</configuration>|<packageSourceCredentials><github><add key=\\\"Username\\\" value=\\\"thuongnguyen1508\\\" /><add key=\\\"ClearTextPassword\\\" value=\\\"${NUGET_GITHUB_PASSWORD}\\\" /></github></packageSourceCredentials></configuration>|" "LE.AdminService/NuGet.config"
COPY ["LE.AdminService/*.csproj", "LE.AdminService/"]
COPY ["LE.AdminService.Infrastructure/*.csproj", "LE.AdminService.Infrastructure/"]
RUN dotnet restore "LE.AdminService/LE.AdminService.csproj" --configfile ./LE.AdminService/NuGet.config
COPY ./ ./
WORKDIR "/src/LE.AdminService"
RUN dotnet build "LE.AdminService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LE.AdminService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ARG ASPNETCORE_ENVIRONMENT              
ARG SECRET_KEY
ARG DB_CONNECTION_STRING
ARG CONSUL_URL

ENV ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENVIRONMENT
ENV SECRET_KEY=$SECRET_KEY
ENV DB_CONNECTION_STRING=$DB_CONNECTION_STRING
ENV CONSUL_URL=$CONSUL_URL

ENTRYPOINT ["dotnet", "LE.AdminService.dll"]
