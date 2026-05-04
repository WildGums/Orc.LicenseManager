# Orc.LicenseManager

Orc.LicenseManager is a library that makes it very easy to manage licenses for commercial software. It provides client and WPF UI components for license validation, network licensing, and license visualization.

The solution consists of the following projects:

- `Orc.LicenseManager.Client` — Core library with license management, validation, and service abstractions.
- `Orc.LicenseManager.Client.WPF` — WPF UI components (views, view models, converters) for license dialogs.
- `Orc.LicenseManager.Server` — Server-side license validation support.
- `Orc.LicenseManager.Tests` — Unit and integration tests for the client libraries.

---

## Critical Rules (Read First)

These rules are **non-negotiable**. Violating them causes broken builds, crashes, or downstream breakage.

### 1. Never Edit Generated Files

Files matching `*.generated.cs` are auto-generated.

- **NEVER** manually edit these files

### 2. ABI / API Stability

This project maintains stable ABI / API. Breaking changes break downstream apps.

| Allowed | Never |
|---------|-------|
| Add new overloads | Modify existing signatures |
| Add new methods | Remove public APIs |
| Add new classes | Change return types |

### 3. Tests Are Mandatory

**Building alone is NOT sufficient.** Run tests before claiming completion (see [Commands](#commands)).

Public API snapshot tests (`PublicApiFacts`) will fail if public API surface changes unexpectedly. Update the `.verified.txt` snapshots only when an intentional API change has been made and reviewed.

### 4. Branch Protection (COMPLIANCE REQUIRED)

**Direct commits to protected branches are a policy violation.**

| Repository | Protected Branches |
|------------|-------------------|
| Orc.LicenseManager | `master` |
| Orc.LicenseManager | `develop` |

**Required workflow:**

1. **Create a feature branch FIRST** — Use naming convention: `feature/issue-NNNN-description`
2. **Make all commits on the feature branch** — Never commit directly to protected branches
3. **Submit a Pull Request** — Changes must be reviewed by a human before merging

```bash
# CORRECT — Always create a feature branch first
git checkout -b feature/issue-1234-fix-description

# NEVER DO THIS — Policy violation
git checkout develop && git commit  # FORBIDDEN

# NEVER DO THIS — Policy violation
git checkout master && git commit  # FORBIDDEN
```

---

## Commands

Single source of truth for all commands:

| Task | Command |
|------|---------|
| **Build** | `dotnet cake --target=build` |
| **Test** | `dotnet cake --target=test` |
| **Build and test** | `dotnet cake --target=buildandtest` |

---

## Architecture & Directories

### Project Overview

```
Orc.LicenseManager.Client     => Core license management (platform-independent)
Orc.LicenseManager.Client.WPF => WPF UI layer (views, view models, dialogs)
Orc.LicenseManager.Server     => Server-side validation support
Orc.LicenseManager.Tests      => Tests for client libraries
```

### Directory Guide

| Directory | Editable? | Notes |
|-----------|-----------|-------|
| `*.generated.cs` | No | Leave as-is |
| `src/Orc.LicenseManager.Client/` | Yes | Core services, models, interfaces |
| `src/Orc.LicenseManager.Client.WPF/` | Yes | WPF views, view models, converters |
| `src/Orc.LicenseManager.Server/` | Yes | Server-side models and services |
| `src/Orc.LicenseManager.Tests/` | Yes | Tests — keep passing |
| `deployment/` | No | Deployment / build scripts |

### Key Namespaces

| Namespace | Purpose |
|-----------|---------|
| `Orc.LicenseManager` | Core services, models, and interfaces |
| `Orc.LicenseManager` (WPF) | Views, view models, converters, and markup extensions |

---

## Writing Code

### Coding Style

- Follow the style used by the [.NET Foundation coding guidelines](https://github.com/dotnet/corefx/blob/master/Documentation/coding-guidelines/coding-style.md), with the following exceptions:
  - Apply `readonly` on class-level private variables that are assigned in the constructor
  - Use **4 spaces** for indentation — tabs do not exist

### Anti-Patterns (Never Do This)

| Anti-Pattern | Why |
|-------------|-----|
| Modifying method signatures | ABI breaking |
| Manual edits to `*.generated.cs` | Overwritten on regenerate |
| Using default parameters in public APIs | ABI breaking |
| **Skipping failing tests** | **Unacceptable — tests must pass** |

---

## Testing & Debugging

### Running Tests

```bash
dotnet cake --target=test
```

### Tests MUST Pass

> **NON-NEGOTIABLE:** Tests must PASS before claiming completion.
>
> - Do NOT skip failing tests
> - Do NOT claim completion if tests fail
> - Do NOT use `SkipException` to work around failures

### Writing Tests

1. Use NUnit to write tests
2. Create a Facts class for a feature
3. Use PascalCase words separated by underscores for test method names (e.g. `Feature_Does_Work`)

```csharp
[Test]
public void Feature_Does_Work()
{
    var result = 47 - 5;

    Assert.That(result, Is.EqualTo(42));
}
```

### Public API Snapshot Tests

The test project includes `PublicApiFacts` tests that capture the public API surface of each assembly into `.verified.txt` files. These tests will fail if the public API changes unexpectedly.

- If you intentionally change a public API, update the corresponding `.verified.txt` file under `src/Orc.LicenseManager.Tests/`.
- Never modify these files unless an intentional, reviewed API change has been made.

**Philosophy:** Tests FAIL when wrong, never skip (except missing hardware).

### Debugging Methodology

1. **Establish baseline** — What's the known-good state?
2. **One change at a time** — Verify each change before proceeding
3. **Track changes in a table** — Log what you changed and the result
4. **Platform differences are signals** — If X works and Y fails, the difference IS the answer
5. **Revert if worse** — Don't pile fixes on top of failures

---

## Further Reading

| Topic | Document |
|-------|----------|
| Contributing guidelines | [CONTRIBUTING.md](.github/CONTRIBUTING.md) |
| Pull request template | [PULL_REQUEST_TEMPLATE.md](.github/PULL_REQUEST_TEMPLATE.md) |
| Documentation portal | https://opensource.wildgums.com |
