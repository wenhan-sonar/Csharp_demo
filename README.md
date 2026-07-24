# SonarQubeDemo

A minimal C# solution set up to be built and analyzed by SonarQube via GitHub Actions on `windows-latest`. It also carries a few third-party OSS dependencies — including intentionally vulnerable ones — to demonstrate SonarQube's dependency-risk (SCA) detection.

## Structure

- `src/SonarQubeDemo` — console app (`net8.0`) with a small `Calculator` class. Uses Serilog for logging and Newtonsoft.Json for serialization.
- `tests/SonarQubeDemo.Tests` — xUnit test project with coverage collection via `coverlet.collector`.
- `.github/workflows/build.yml` — CI pipeline: installs SonarScanner for .NET, then builds and analyzes.

## Dependencies

Managed via NuGet (`PackageReference`).

**Application (`src/SonarQubeDemo`):**

| Package | Version | Notes |
| --- | --- | --- |
| `Serilog` | 4.2.0 | Structured logging |
| `Serilog.Sinks.Console` | 6.0.0 | Console sink |
| `Newtonsoft.Json` | 12.0.3 | ⚠️ Intentionally vulnerable — see below |
| `log4net` | 2.0.8 | ⚠️ Intentionally vulnerable — see below |

**Tests (`tests/SonarQubeDemo.Tests`):** `Microsoft.NET.Test.Sdk`, `xunit`, `xunit.runner.visualstudio`, `coverlet.collector`.

### ⚠️ Intentionally vulnerable dependencies (demo only)

To showcase SonarQube's **Dependency Risks (SCA)** view, two packages are pinned to
known-vulnerable versions. **Do not use these versions in production.**

- **Newtonsoft.Json 12.0.3** — [CVE-2024-21907](https://nvd.nist.gov/vuln/detail/CVE-2024-21907) / [GHSA-5crp-9r3c-p9vr](https://github.com/advisories/GHSA-5crp-9r3c-p9vr): High-severity denial of service via deeply nested JSON. Fixed in 13.0.1.
- **log4net 2.0.8** — [CVE-2018-1285](https://nvd.nist.gov/vuln/detail/CVE-2018-1285) / [GHSA-2cwj-8chv-9pp9](https://github.com/advisories/GHSA-2cwj-8chv-9pp9): XML external entity (XXE) processing when parsing the log4net config. Fixed in 2.0.10.

These are reported as dependency risks by SonarQube; they do not fail the build unless a quality gate condition is configured to act on them.

## Local build

```bash
dotnet restore
dotnet build
dotnet test
```

## Running the analysis locally (optional)

```bash
dotnet tool install --global dotnet-sonarscanner
dotnet sonarscanner begin /k:"Cshard-test" \
  /d:sonar.host.url="<your-sonarqube-url>" \
  /d:sonar.token="<your-token>"
dotnet build
dotnet sonarscanner end /d:sonar.token="<your-token>"
```

## GitHub Actions setup

The workflow (`.github/workflows/build.yml`) runs on every push to `main`, on pull requests, and on manual dispatch. It runs on `windows-latest` and uses PowerShell.

**Repository secrets** (Settings → Secrets and variables → Actions → Secrets):
- `SONAR_TOKEN_LOCAL_ENV` — a user or project analysis token.
- `SONAR_HOST_URL_LOCAL_ENV` — the base URL of your SonarQube Server or SonarQube Cloud instance.

The analyzed project key is `Cshard-test` (set in the `Build and analyze` step). Make sure it exists on the SonarQube side, or let the first analysis auto-provision it if your instance allows that.

## Notes

- CI currently **builds and analyzes only** — it does not run tests or collect coverage. Run `dotnet test` locally for that; to feed coverage to the scanner, add `/d:sonar.cs.cobertura.reportsPaths="**/coverage.cobertura.xml"` and collect with `--collect:"XPlat Code Coverage"`.
- `fetch-depth: 0` is used on checkout so SonarQube can correctly compute new code and blame information.
- JDK 17 is required because the SonarScanner engine runs on the JVM.
