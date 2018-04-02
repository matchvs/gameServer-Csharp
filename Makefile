.PHONY: build http_api clean release linux_build test fmt vet

BINARY = bin/

all: build 

build: 
	dotnet restore gameServer
	dotnet build gameServer -o $(BINARY)

clean:
	rm -rf build

release: clean build
	mkdir -p build
	cp -r CHANGELOG.md ./gameServer/bin  ./gameServer/script build
	cp -r ./gameServer/conf ./build/bin
image:
	sudo docker build -t $(IMAGE) ./gameServer
	sudo docker tag $(IMAGE) $(HARBOR)/$(IMAGE)
	sudo docker push $(HARBOR)/$(IMAGE)
	sudo docker rmi $(IMAGE) $(HARBOR)/$(IMAGE)
test:
