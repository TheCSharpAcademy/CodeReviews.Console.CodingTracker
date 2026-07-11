# CodingTracker

A C# console application for tracking coding sessions — insert, view, update, delete, live-time, and filter/sort sessions — built with **Spectre.Console** for a polished terminal UI and **SQLite** (via Dapper) for storage.

## Features

- **Insert Coding Session** — manually enter a start and end date/time for a session.
- **View Coding Sessions** — see all logged sessions in a formatted table.
- **Update Coding Session** — edit an existing session by ID.
- **Delete Coding Session** — remove a session by ID.
- **Time Coding Session** — a live stopwatch: press `SPACEBAR` to start/stop, `ESC` to cancel.
- **Filter Coding Sessions** — filter by date, week, month, or year, and sort ascending/descending.
- Clean, styled terminal UI using Spectre.Console (menus, tables, headers, colors).



## Getting Started

1. Clone the repo.
2. Make sure `appsettings.json` has a valid SQLite connection string under `ConnectionStrings:DefaultConnection`.
3. Run the app:
   ```
   dotnet run
   ```



## Challenges Faced (and how they were solved)

Building this project surfaced a handful of real bugs and design lessons worth documenting:



### 1. `Console.KeyAvailable` doesn't block
Used in an `if` (not a loop), `Console.KeyAvailable` only returns `true` if a key happens to already be in the buffer at that exact instant — so "Press any key" prompts were being skipped entirely because no key had been pressed yet at the moment of the check.
**Fix:** switched to `Console.ReadKey(intercept: true)` directly where a blocking wait was actually intended, and reserved `KeyAvailable` for genuine non-blocking polling loops (like the live stopwatch in `TimeCodingSession`).

### 2. Escape key silently saving empty sessions
In `TimeCodingSession()`, pressing `ESC` **before ever starting the timer** fell through a `break` into the "save session?" prompt instead of returning immediately — meaning `StartTime`/`EndTime` were still at their `DateTime.MinValue` defaults, and choosing "Yes" saved a bogus, all-default session to the database.
**Fix:** made every `ESC` branch `return` immediately instead of `break`-ing into the save flow, so the save prompt can only be reached via the legitimate "timer was properly stopped" path.

### 3. Spectre.Console "Unbalanced markup stack" exception
Spectre.Console treats `[` and `]` as markup tag delimiters. Any interpolated string containing a stray bracket (or an odd number of them) thrown into `AnsiConsole.MarkupLine(...)` crashes with `System.InvalidOperationException: Unbalanced markup stack`.
**Fix:** wrap any dynamic/user-provided content with `Markup.Escape(...)` before interpolating it into a markup string.


### 4. No persistent app header after `Console.Clear()`
`Console.Clear()` is destructive — there's no native way to "pin" a line so it survives a clear.
**Fix:** wrapped every clear with a call to a shared `ShowHeader()` method (Spectre `FigletText` + `Rule`) so the app name/banner is always redrawn immediately after clearing, simulating a persistent header.

### 5. Nullable value type gotchas
Attempted to call `DateTime?.TryParseExact(...)` directly on the nullable type and pass a `DateTime?` as the `out` parameter — neither compiles, since `TryParseExact`'s `out` parameter requires a non-nullable `DateTime`, and static methods can't be invoked through a `Nullable<T>` instance.
**Fix:** parse into a local non-nullable `DateTime`, then let it implicitly convert to `DateTime?` on return; return `null` explicitly on the failure path.
