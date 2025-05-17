FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Orari/Orari.csproj", "Orari/"]
RUN dotnet restore "Orari/Orari.csproj"
COPY . .
WORKDIR "/src/Orari"
RUN dotnet build "Orari.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Orari.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final

# Install SQL Server tools
RUN apt-get update && \
    apt-get install -y wget apt-transport-https && \
    wget -O /etc/apt/trusted.gpg.d/microsoft.asc https://packages.microsoft.com/keys/microsoft.asc && \
    echo "deb [arch=amd64] https://packages.microsoft.com/debian/12/prod bookworm main" > /etc/apt/sources.list.d/microsoft-prod.list && \
    apt-get update && \
    ACCEPT_EULA=Y apt-get install -y mssql-tools18 unixodbc-dev && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

ENV PATH="$PATH:/opt/mssql-tools18/bin"

WORKDIR /app
COPY --from=publish /app/publish .
COPY Orari/wait-for-it.sh /wait-for-it.sh
RUN chmod +x /wait-for-it.sh

ENTRYPOINT ["/wait-for-it.sh", "db", "dotnet", "Orari.dll"]

