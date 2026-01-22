# Project Architecture & Guidelines

## 1. Project Overview
**Goal:** Gamified educational platform (Pre-school) scaling to B2B/Schools.
**Key Values:** Robustness, Data Integrity, Scalability, Type Safety.

## 2. Tech Stack
- **Database:** PostgreSQL (Supabase).
- **Backend:** .NET 8/9 Web API.
- **Frontend:** Flutter (Mobile/Web).
- **Hosting:** Railway (API).

---

## 3. Database Guidelines (DBA STRICT MODE)

### 3.1. Schemas & Organization
- **Logical Separation:** Do NOT use the `public` schema for business tables.
    - `identity`: Users, Tenants, Auth integration.
    - `school`: Students, Guardians, Classes, Teachers.
    - `content`: Activities, Modules, Assets.
    - `game`: Progress, Scores, Badges.
    - `audit`: System logs.

### 3.2. Naming Conventions
- **Case:** `snake_case` for everything.
- **Pluralization:** Table names MUST be **Plural**.
- **Indices:** - Standard: `idx_[table]_[column]`
    - Unique: `uq_[table]_[column]`
- **Constraints:** Define all constraints at the bottom of the DDL.
    - PK: `pk_[table]`
    - FK: `fk_[table]_[referred_table]`
    - Check: `ck_[table]_[condition]`

### 3.3. ID Strategy (Hybrid)
- **Internal ID (`[table]_id`):** `IDENTITY` (Auto-increment).
    - `smallint`: Lookups/Enums.
    - `int`: Standard tables (Students, Tenants).
    - `bigint`: High volume (Logs, Progress).
    - *Rule:* NEVER expose this ID to the API or Frontend. Use strictly for JOINs and Foreign Keys.
- **External ID (`[table]_uuid`):** `gen_random_uuid()`.
    - *Rule:* This is the ONLY identifier exposed to the Frontend/API DTOs.

### 3.4. Security (RLS & Isolation)
- **Row Level Security (RLS):** MUST be enabled on ALL tables.
- **Context Injection:** The API is responsible for setting `app.current_tenant` and `app.current_user_id` at the start of every connection/transaction.

### 3.5. Audit & Soft Delete
- **Fields:**
    - `created_at` (timestamptz, default now())
    - `created_by` (int, FK to `identity.app_users`)
    - `updated_at` (timestamptz, managed by Trigger)
    - `updated_by` (int, FK to `identity.app_users`)
    - `deleted_at` (timestamptz, nullable) -> **Soft Delete**
- **Trigger:** All tables must use `handle_updated_at` trigger.

### 3.6. Metadata
- **Comments:** EVERY table and column must have a SQL `COMMENT` describing its purpose, validation rules, or JSON structure.

---

## 4. Backend Guidelines (.NET Web API)

### 4.1. Architecture: Clean Architecture
- **Domain:** Entities, Enums, Exceptions. *No dependencies.*
- **Application:** DTOs, Services, Validators (FluentValidation).
- **Infrastructure:** EF Core (Commands), Dapper (Queries).
- **API:** Controllers (Presentation).

### 4.2. Coding Rules
- **DTOs:** Strict separation. Never return Entities.
- **Validation:** Use `FluentValidation` in Application layer.
- **Tenant Awareness:** Every service must inject `ICurrentUserService` to filter data by Tenant.

---

## 5. Frontend Guidelines (Flutter)
- **State:** Riverpod.
- **IDs:** Only use UUIDs.
- **Models:** Freezed & JsonSerializable.