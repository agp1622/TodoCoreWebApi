FROM microsoft/dotnet:2.0-sdk AS build-env
WORKDIR /app

# copy csproj and restore as distinct layers
COPY ./TodoCoreWebApi/*.csproj ./
RUN dotnet restore

# copy everything else and build
COPY ./TodoCoreWebApi/. ./
RUN dotnet publish -c Release -o out /p:PublishWithAspNetCoreTargetManifest="false"

# build runtime image
FROM microsoft/dotnet:2.0-runtime
WORKDIR /app
COPY --from=build-env /app/out .

# not valid for Heroku
# ENTRYPOINT ["dotnet", "TodoCoreWebApi.dll"]

# this is working
CMD dotnet TodoCoreWebApi.dll
