# ChatGPT MultiView

Windows desktop app that shows the normal ChatGPT web app in 1 to 4 panes inside one window.

## Requirements

- Windows 10/11
- .NET 8 SDK (for development)
- Microsoft Edge WebView2 Runtime

## Run

```powershell
dotnet restore ChatGPTMultiView.sln
dotnet run --project src/ChatGPTMultiView/ChatGPTMultiView.csproj
```

The app starts with four panes. Use the `1画面` / `2画面` / `3画面` / `4画面` buttons to switch layouts.

All panes use the same persistent WebView2 profile at:

```text
%LOCALAPPDATA%\ChatGPTMultiView\WebView2
```

This lets the panes share ChatGPT login cookies and preserves them across app restarts.

## Test

```powershell
dotnet test ChatGPTMultiView.sln
```

## Publish

```powershell
dotnet publish src/ChatGPTMultiView/ChatGPTMultiView.csproj `
  -c Release `
  -r win-x64 `
  --self-contained true `
  -o publish
```

Then run:

```powershell
.\publish\ChatGPTMultiView.exe
```

## Scope

Intentionally minimal: no workspaces, tabs, URL saving, prompt broadcasting, DOM automation, API integration, or settings framework.
