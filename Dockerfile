# syntax=docker/dockerfile:1
# 빌드 컨텍스트: 이 Dockerfile이 있는 솔루션 루트 (RevitFlow.API.slnx와 동일 폴더)
# 예: docker build -t revitflow-api .

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY RevitFlow.API/RevitFlow.API.csproj RevitFlow.API/
RUN dotnet restore RevitFlow.API/RevitFlow.API.csproj

COPY RevitFlow.API/ RevitFlow.API/
WORKDIR /src/RevitFlow.API
RUN dotnet publish RevitFlow.API.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# publish 결과물 실행 (컨테이너에서는 보통 `dotnet *.dll`; `dotnet run`과 동일하게 앱 호스트 기동)
ENTRYPOINT ["dotnet", "RevitFlow.API.dll"]
