# Get dotnet sdk
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine-arm64v8 AS build-env
WORKDIR /app

# Build app
COPY . ./
RUN dotnet publish Entry --self-contained -r linux-musl-arm64 -p:PublishSingleFile=true -c Release -o ./deploy

# Generate image
FROM mcr.microsoft.com/dotnet/runtime-deps:6.0-alpine-arm64v8
WORKDIR /app
ENV DOTNET_RUNNING_IN_CONTAINER=true
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
RUN addgroup -S appgroup && adduser -S appuser 
USER appuser
COPY --from=build-env --chown=appuser:appgroup /app/deploy/Entry .
ENTRYPOINT ["./Entry"]