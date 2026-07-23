# SonarQubeDemo

A minimal C# solution set up to be built and analyzed by a self-hosted SonarQube server via GitHub Actions on `ubuntu-latest`.

## Structure

- `src/SonarQubeDemo` — console app (`net8.0`) with a small `Calculator` class.
- `tests/SonarQubeDemo.Tests` — xUnit test project with coverage collection via `coverlet.collector`.
- `.github/workflows/build.yml` — CI pipeline: build, test with coverage, run SonarScanner for .NET.

## Local build

```bash
dotnet restore
dotnet build
dotnet test
```

## Running the analysis locally (optional)

```bash
dotnet tool install --global dotnet-sonarscanner
dotnet sonarscanner begin /k:"sonarqube-csharp-demo" \
  /d:sonar.host.url="https://your-sonarqube-server" \
  /d:sonar.token="<your-token>" \
  /d:sonar.cs.cobertura.reportsPaths="**/coverage.cobertura.xml"
dotnet build
dotnet test --collect:"XPlat Code Coverage" --results-directory ./coverage
dotnet sonarscanner end /d:sonar.token="<your-token>"
```

## GitHub Actions setup

The workflow (`.github/workflows/build.yml`) runs on every push/PR to `main` and on manual dispatch. It needs:

**Repository secrets** (Settings → Secrets and variables → Actions → Secrets):
- `SONAR_TOKEN` — a user or project analysis token generated on your SonarQube server.
- `SONAR_HOST_URL` — the base URL of your SonarQube server (e.g. `https://sonarqube.mycompany.com`).

**Repository variable** (optional, Settings → Secrets and variables → Actions → Variables):
- `SONAR_PROJECT_KEY` — the project key as created on the SonarQube server. Defaults to `sonarqube-csharp-demo` if not set.

Make sure the project key exists on the SonarQube server (create it manually, or let the first analysis auto-provision it if your server allows that).

## Notes

- Coverage is collected by `coverlet.collector` in Cobertura format and picked up by the scanner via `sonar.cs.cobertura.reportsPaths`.
- `fetch-depth: 0` is used on checkout so SonarQube can correctly compute new code and blame information.
- JDK 17 is required because the SonarScanner engine runs on the JVM.
