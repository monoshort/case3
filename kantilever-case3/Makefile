SHELL := /bin/bash

MAKEFLAGS := --no-print-directory

.DEFAULT_GOAL := help

.PHONY := help docker.build docker.run down docker.stop up start b&a build-and-start prod.up prod.start

help: ## Show the list of commands
	@echo "Please use 'make <target>' where <target> is one of"
	@awk 'BEGIN {FS = ":.*?## "} /^[a-zA-Z0-9\._-]+:.*?## / {printf "\033[36m%-30s\033[0m %s\n", $$1, $$2}' $(MAKEFILE_LIST)

prod.up: prod.start ## Same as prod.start, but shorter
prod.start: ## Start the application without building it locally, using remote images
	docker-compose -f src/docker-compose.yaml -f src/docker-compose.prod.yaml -p macedonieje up -d
	@echo "Successfully setup! The following links are available:"
	@echo "Frontend: http://localhost:4000"
	@echo "Backoffice: http://localhost:3000"

docker.build: ## Build the docker containers
	@echo "No docker_service variable defined, building all images..."
ifdef docker_service
	docker-compose -f src/docker-compose.yaml -f src/docker-compose.dev.yaml -p macedonieje build --no-cache ${docker_service}
else
	docker-compose -f src/docker-compose.yaml -f src/docker-compose.dev.yaml -p macedonieje build --no-cache
endif

down: docker.stop ## Same as stop, but shorter
docker.stop: ## Stops the application in docker
	docker-compose -f src/docker-compose.yaml -f src/docker-compose.dev.yaml -p macedonieje down --rmi local

up: start ## Same as start, but shorter
start: ## Run the application in docker
	docker-compose -f src/docker-compose.yaml -f src/docker-compose.dev.yaml -p macedonieje up -d

bs: build-and-start ## Same as build-and-start, build/start one service with: make bs service="ExampleService" docker_service="example"
build-and-start: ## Build everything and start the application, or build/start one service with: make bs service="ExampleService" docker_service="example"
ifdef service
	dotnet publish "$(CURDIR)/src/${service}/${service}/${service}.csproj" -c Debug -o "$(CURDIR)/src/${service}/${serviceerviservicee}/obj/Docker"
else
	./dotnet.publish.sh
endif
	make docker.build
	make start
	@echo "Successfully setup! The following links are available:"
	@echo "Identity: http://localhost:5000"
	@echo "Frontend: http://localhost:4000"
	@echo "Backoffice: http://localhost:3000"
	@echo "PgAdmin: http://localhost:5050"
	@echo "RabbitMQ: http://localhost:15672"
	@echo "Existing services:"
	@echo "MagazijnService: http://localhost:2021/swagger"
	@echo "Auditlog: http://localhost:2022/swagger"
	@echo "CatalogusService: http://localhost:2020/swagger"
	@echo "Use the docker logs macedonieje_<service-name>_1 -f command to see logs of a service"
