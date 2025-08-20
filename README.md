# This project was created completely by GitHub Copilot Agent mode using GPT-5
It's a test, and demonstrates the capabilities of the tool.
It can be used as a starting point for testing out how copilot can assist in building applications.

Some things that could be improved include:

- Adding capabilities to rename lists
- Modernizing the UI
- Remove superfluous GUI elements
- Adding user authentication
- Implementing a database backend

The idea is to use this as a base, and coach your Copilot to assist you in doing these things.

The original prompt is included in file [original_prompt.txt](original_prompt.txt)


# Shopping List Demo (Old-school UI)

A .NET 8 Web API + Angular 17 sample app with in-memory storage and a deliberately 2005-style UI.

## Features
- Lists shopping lists and items
- Add/delete lists (no rename)
- Add/edit/delete items
- Reset to default lists

## Run locally (Windows PowerShell)

Backend (API):

```pwsh
cd ./backend/ShoppingList.Api
 dotnet run --no-launch-profile
```

Frontend (Angular):

```pwsh
cd ./frontend/shopping-list-app
npm start
```

Open http://localhost:4200

The Angular dev server proxies /api to http://localhost:5000.

## API Endpoints
- GET /api/lists
- GET /api/lists/{id}
- POST /api/lists
- DELETE /api/lists/{id}
- POST /api/lists/{id}/items
- PUT /api/lists/{id}/items/{itemId}
- DELETE /api/lists/{id}/items/{itemId}
- POST /api/reset

## Notes
- State is in-memory and resets on API process restart or via the Reset button.
- UI is intentionally dated to be improved later.
