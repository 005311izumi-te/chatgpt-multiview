# ChatGPT MultiView Design

## Goal

Create a small Windows-only desktop app that displays the normal ChatGPT web app in 1 to 4 panes inside a single window.

## Scope

- Windows only.
- WPF on .NET 8.
- Microsoft Edge WebView2 for embedded web content.
- One top bar with four buttons: 1, 2, 3, 4 panes.
- 1 pane: pane 1 fills the content area.
- 2 panes: panes 1 and 2 are side by side.
- 3 panes: pane 1 fills the left half; panes 2 and 3 are stacked on the right.
- 4 panes: 2 x 2 grid.
- Startup layout: 4 panes.
- Every pane opens https://chatgpt.com/.
- All panes share one persistent WebView2 environment/profile so ChatGPT login cookies are shared and survive app restarts.

## Non-goals

Do not add:

- MVVM, dependency injection, database, or settings framework.
- Workspace saving.
- Current-chat URL saving.
- Prompt broadcast or DOM automation.
- OpenAI API integration.
- Tabs, arbitrary pane counts, drag-and-drop layout editing, or custom splitters.
- Account switching or multiple WebView2 profiles.
- Custom browser controls beyond the four pane-count buttons.

## Architecture

Use one WPF `MainWindow`. The XAML contains a top button row and one 2 x 2 content `Grid` containing four `WebView2` controls. `MainWindow.xaml.cs` creates one `CoreWebView2Environment` using a user-data folder under `%LOCALAPPDATA%\ChatGPTMultiView\WebView2`, initializes all four controls with that same environment, and navigates each control to ChatGPT.

A tiny pure `LayoutPlanner` maps pane count 1-4 to row/column/span placements. `MainWindow` applies those placements and collapses unused panes. No other architecture layer is needed.

## Error handling

If WebView2 initialization fails, show a simple `MessageBox` with the exception message and keep the window open. No telemetry or retry framework.

## Acceptance criteria

1. App launches on Windows and initially shows four ChatGPT panes.
2. 1/2/3/4 buttons immediately switch to the specified layout.
3. All visible panes can be operated independently.
4. Logging into ChatGPT in one pane makes the same login available to the other panes after reload/navigation.
5. Closing and reopening the app preserves the ChatGPT login state.
6. No features outside the stated scope are present.
