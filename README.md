# TodoApi
 
Enkel REST API for å håndtere todo-oppgaver, bygget med .NET og PostgreSQL.
 
## Forutsetninger
 
- [.NET SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for å kjøre PostgreSQL)
- [EF Core CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)
- [Anthropic API-nøkkel](https://console.anthropic.com)
## Installasjon
 
### 1. Klon prosjektet
 
```bash
git clone <repo-url>
cd TodoApi
```
 
### 2. Installer EF Core-verktøyet (kun første gang)
 
```bash
dotnet tool install --global dotnet-ef
```
 
### 3. Start PostgreSQL med Docker
 
```bash
docker run --name tododb -e POSTGRES_PASSWORD=dittpassord -e POSTGRES_DB=tododb -p 5432:5432 -d postgres
```
 
### 4. Konfigurer connection string
 
Åpne `appsettings.json` og sett riktig passord:
 
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=tododb;Username=postgres;Password=dittpassord"
  }
}
```

### 5. Konfigurer Claude API-nøkkel

```bash
dotnet user-secrets init
dotnet user-secrets set "Claude:ApiKey" "sk-ant-..."
```
 
### 6. Kjør databasemigrering
 
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```
 
### 7. Start API-et
 
```bash
dotnet run
```
 
API-et kjører nå på `http://localhost:5024`.
 
## Endepunkter
 
| Metode | URL | Beskrivelse |
|--------|-----|-------------|
| GET | `/todos` | Hent alle todos |
| POST | `/todos` | Opprett ny todo |
| POST | `/todos/categorize?id={id}` | Auto-kategoriser en todo med Claude |
| PUT | `/todos/{id}` | Oppdater en todo |
| DELETE | `/todos/{id}` | Slett en todo |
 
## Eksempel på todo-objekt
 
```json
{
  "title": "Kjøp mat",
  "description": "Melk, brød, egg",
  "isCompleted": false
}
```

## AI-kategorisering

Todos kan automatisk kategoriseres ved hjelp av Claude. Mulige kategorier:

`Arbeid` `Privat` `Helse` `Økonomi` `Sosial` `Annet`

Kategorisering skjer automatisk ved POST til `/todos/categorize?id={id}`.
 
## Stoppe og starte databasen
 
```bash
# Stopp
docker stop tododb
 
# Start igjen
docker start tododb
```
