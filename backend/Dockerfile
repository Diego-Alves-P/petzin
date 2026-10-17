FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY Petzin.Api/Petzin.Api.csproj Petzin.Api/
RUN dotnet restore Petzin.Api/Petzin.Api.csproj
COPY Petzin.Api/ Petzin.Api/
RUN dotnet publish Petzin.Api/Petzin.Api.csproj -c Release -o /app /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Petzin.Api.dll"]
