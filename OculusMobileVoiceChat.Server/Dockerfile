FROM microsoft/dotnet:2.2-sdk
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./

CMD ["dotnet", "run"]
