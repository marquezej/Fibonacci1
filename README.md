Running from Docker:

	docker build --no-cache -t fibonacci .
	docker run -p 5037:5037 --cpus=1.00 -m 512m fibonacci

It also needed line 7 of Fibonacci.csproj uncommented in order to build.

I would recommend running it from VS Code and that will require the C# extionsion and .NET 6.0 SDK (links below)

	https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp
	https://dotnet.microsoft.com/en-us/download/dotnet/6.0