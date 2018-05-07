run: build
	mono Server.exe --test

build: Server.exe

Server.exe:
	mcs -optimize+ -unsafe -t:exe -out:Server.exe -nowarn:219,414 -d:MONO -recurse:Server/*.cs -reference:System.IO.Compression.FileSystem.dll

clean:
	rm -f Server.exe

