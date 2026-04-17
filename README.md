# BrowserAI

BrowserAI is a Windows Forms app that embeds Microsoft WebView2 to deliver a fast, native browser UI with built‑in link collection and scraping workflows.

## What it does
1. Browse any site inside a WebView2 control.
2. Collect product links from the currently open page (and its pagination).
3. Store those links in a local SQLite database for later processing.
4. Scrape pending product pages and save structured data.

## Key features
1. Modern custom chrome layout (navigation + address bar + actions).
2. WebView2-based browsing with standard navigation controls.
3. Link collector reads from the open page, follows `rel="next"` pagination, and saves to DB.
4. Product scraper gathers title, price, description, images, and specifications.
5. Local storage via Entity Framework Core + SQLite.

## How links are collected
1. Open a category/listing page in the browser.
2. Click `Links`.
3. The app reads the current page HTML, extracts product URLs, follows `rel="next"` pages, and writes new URLs to the database.

## How scraping works
1. Click `Scrape`.
2. The app loads each pending product URL, extracts structured data, and saves it.

## Project structure
1. `BrowserForm.cs` UI, WebView2 handlers, link collection, and scraping logic.
2. `BrowserDbContext.cs` EF Core DB context.
3. `controls/ProductInfo.cs` and `controls/ProductLink.cs` database models.
4. `assets/` local start page and WebView resources.

## Requirements
1. Windows 10/11.
2. .NET 8 SDK.
3. WebView2 runtime installed.

## Build & run
1. Open `BrowserAI.sln` in Visual Studio.
2. Build and run.

## Notes
1. Link collection uses the currently open page in the browser.
2. The database is created automatically on first run.
