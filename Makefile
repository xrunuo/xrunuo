MCS=mcs
CURPATH=`pwd`
NOWARNS=219,414
DATAPATH=${CURPATH}/Muls
DOCKERPATH=/opt/xrunuo
DOCKERDATAPATH=/opt/data
PORT=2593

run: build
	mono Server.exe

build:
	${MCS} -optimize+ -unsafe -t:exe -out:Server.exe -nowarn:${NOWARNS} -d:MONO -recurse:Server/*.cs -reference:System.IO.Compression.FileSystem.dll

test: build
	mono Server.exe --test

clean:
	rm -f Server.exe

docker-build:
	docker run -it -v ${CURPATH}:${DOCKERPATH} mono:4.8 ${MCS} -optimize+ -unsafe -t:exe -out:${DOCKERPATH}/Server.exe -nowarn:${NOWARNS} -d:MONO -recurse:${DOCKERPATH}/Server/*.cs -reference:System.IO.Compression.FileSystem.dll

docker-run: docker-build
	docker run -it -v ${CURPATH}:${DOCKERPATH} -v ${DATAPATH}:${DOCKERDATAPATH} -p ${PORT}:${PORT} mono:4.8 mono ${DOCKERPATH}/Server.exe

