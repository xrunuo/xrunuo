clean:
	dotnet clean

build:
	dotnet build

run: build
	dotnet run --project Server
