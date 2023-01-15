FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /Game-List-Api

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish --no-restore -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /Game-List-Api
EXPOSE 5066
ENV ASPNETCORE_URLS=http://+:5066
COPY --from=build-env /Game-List-Api/out .
ENTRYPOINT ["dotnet", "Game-List-Api.dll"]