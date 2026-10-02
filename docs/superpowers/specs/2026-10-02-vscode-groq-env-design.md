# VS Code GROQ_API_KEY Debug Configuration

## Goal

Allow the C# application to read `GROQ_API_KEY` when launched with the VS Code debugger, without storing the key in tracked files or changing application code.

## Design

Add `.vscode/launch.json` with a C# debugger configuration whose `envFile` points to `${workspaceFolder}/.env`. The repository already ignores both `.vscode/` and `.env`, so the launch configuration remains local and the key remains untracked.

Do not create `.env`, put a key or placeholder in the repository, change `Program.cs`, or modify project dependencies. The developer creates `.env` locally and supplies their own key.

## Verification

Validate the launch configuration as JSON and confirm its `envFile` path and `.gitignore` coverage. Do not run the application because no API key is available in the workspace.
