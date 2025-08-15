# CFAN SchoolMap MAUI – Feature Overview

Updated: 2025-08-15

## App summary
A .NET MAUI app for planning and recording outreach at schools and markets:
- Google Maps for visualization and interaction
- Firestore for storage and realtime updates
- Google Places for discovery (autocomplete + nearby search)
- BigQuery for historical statistics
- Country borders from embedded GeoJSON (with REST Countries fallback for centers)

## Navigation and pages
- About
- School Map
- School Statistics
- Outreach/Market Map
- Administration

## School Map
- Country selection overlay (ISO3166 list, search filter)
- Country borders drawn; camera fits bounds; inter-country pins show summary stats
- Actions/modes: Find schools → Plan visits → Visit schools
- Search flows:
  - Nearby Search (Google) for schools in visible region
  - Dedicated Search page (autocomplete near last known location, plus DB prefix search)
  - Clipboard import (lines formatted with type|plusCode|name|...)
- Place lifecycle and detail:
  - Add via map long-press or POI click (permission/mode gated)
  - Team planning A–Z and 0–9 with highlight
  - Visit form: state (done/visit later/unmark/delete/no access), attendance, conversions, note
  - Save visit and place name; navigate via native apps/Google Maps deep link

## Outreach/Market Map
- Mirrors School Map workflow for markets
- Search types: malls, stations, supermarkets
- Visit states: Not done/Done/Do again/Don’t go, “Is open field”, notes + history

## Search page
- Autocomplete (Google Places) constrained by selected country
- CfaN database prefix search for schools
- Selecting a result returns Plus Code + name to the originating page

## Statistics (Schools and Markets)
- Date dimensions: day/week/month/year (running total optional)
- Filters: country, author
- Queries BigQuery dataset cfan-schools.Schools.Visits
- Displays visits, decisions, attendance

## Administration
- Monthly data compression (move new docs to CountryData, archive old visits, backups)
- Add new user: password generation, Firebase user creation, role assignment, email notification
- Users/Roles listing and navigation to per-user roles page
- Downsize very large countries (remove ignored/not-a-place)

## Data model (selected)
- BasePoint → PlacePoint (schools), MarketPoint (markets)
  - PlusCode, Country, Type (pin state), Team, UpdatedAt; helpers like IsVisited
- CountryPlaces<T>
  - In-memory country collection; update/change helpers; next-new-place selection
- SchoolVisit / MarketInfo
  - Visit state and metrics; persisted per PlusCode
- User
  - Roles drive permissions; string serialization; admin role detection

## Repository responsibilities
- Firestore CRUD for places, names, visits/market info; live listeners
- Per-country compression/backup; BigQuery stats caching
- Current user + derived flags: HasSchoolRoles/HasMarketRoles/HasAdministrationRoles
- Tester mode bypasses writes; fallbacks when Firestore permissions fail

## Roles and visibility
- Flyout items shown based on Repository role flags (updated via messages)
- Map permissions:
  - Schools_add: add places
  - Schools_visit: plan/visit
  - Outreaches_add / visit: analogous for markets
  - Admin: admin features; set roles; monthly routines

## Mapping
- Maui.GoogleMaps for map, pins, polygons, camera updates
- CountryBorderHelper loads borders from embedded GeoJSON; computes bounds/center
- Fallback center fetch via REST Countries; persisted in Firestore for reuse

## Key dependencies
- Maui.GoogleMaps, Plugin.CloudFirestore, ISO3166, Acr.UserDialogs.Maui
- Google Cloud: Places API, BigQuery (embedded service key resource)
- OpenLocationCode (Plus Codes)

## Notable implementation notes
- SchoolMapPage currently lacks BindingContext and map behaviors wiring (MarketMap is fully bound). Wire it like MarketMap.
- Mixed DI: uses both MAUI DI and DependencyService; consider consolidating on MAUI DI.
- BigQuery service key must be present as embedded resource (CFAN.SchoolMap.BigQueryServiceKey.json).
- Avoid committing API keys; `App.Key` is hard-coded and should be moved to secure config.

## Quick wins
- Fix SchoolMapPage binding and behaviors; set `BindingContext` to `SchoolMapVM` and bind pins/polygons/camera.
- Null/exception guards around BigQuery init and queries; user feedback when resource is missing.
- Gradual migration from DependencyService to MAUI service container.

