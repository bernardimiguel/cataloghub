# 1. Layered architecture

Date: 2026-11-19

## Status

Accepted

## Context

CatalogHub manages product data and talks to many external systems: a SQL database, spreadsheets, Nuvemshop and Mercado Livre. These technical details will change over time.

In legacy systems, business rules often call the database or ORM directly, so changing a technology means changing business code, and testing a rule requires a real database.

We need business rules that are isolated from technical details and easy to test.

## Decision

We will split the solution into five projects:

- Domain: entities and business rules. References no project.
- Application: use cases and interfaces for external services (such as `IStorePublisher`).
- Infrastructure: implementations that use technology (database, HTTP, files).
- Api and Worker: entry points. They wire interfaces to implementations in `Program.cs`.

Dependencies always point inward, toward the Domain.

## Consequences

- Business rules live in one place and apply the same way to every database or external service.
- More projects and layers than a small system needs at this early stage.