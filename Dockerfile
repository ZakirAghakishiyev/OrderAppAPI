# syntax=docker/dockerfile:1.4

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .

# Load secrets from Docker BuildKit secret mount
RUN --mount=type=secret,id=secrets,dst=/secrets/secrets.txt \
    export $(cat /secrets/secrets.txt | xargs) && \
    dotnet restore ./OrderApp.sln && \
    dotnet publish ./src/OrderApp.Web/OrderApp.Web.csproj -c Release -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet", "OrderApp.Web.dll"]
