.PHONY: build test run docker docker-prod deploy backup lint migrate

build:
	dotnet build src/Api/AccessControl.Api.csproj
	cd src/Frontend && npm run build

test:
	dotnet test tests/Unit/
	dotnet test tests/Integration/

run:
	cd src/Frontend && npm run dev &
	dotnet run --project src/Api/

docker:
	docker compose -f docker/docker-compose.yml up --build

docker-prod:
	docker compose -f docker/docker-compose.prod.yml up -d

deploy:
	./scripts/deploy.sh

backup:
	./scripts/backup.sh

lint:
	dotnet format --verify-no-changes
	cd src/Frontend && npm run lint

migrate:
	dotnet ef migrations add $(name) --project src/Infrastructure --startup-project src/Api
	dotnet ef database update --project src/Infrastructure --startup-project src/Api
