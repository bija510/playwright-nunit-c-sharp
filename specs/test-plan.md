# Playwright Test Plan

## Application Overview

End-to-end and regression test plan for the Playwright-based web application in this repository. Covers authentication, registration, personal-details management, navigation/stability, and accessibility/visual checks. Assumes tests run against a fresh environment (no user logged in) unless a step states otherwise.

## Test Scenarios

### 1. Authentication

**Seed:** `seed.spec.ts`

#### 1.1. Login — Happy Path

**File:** `specs/authentication/login-happy.spec.ts`

**Steps:**
  1. Assumption: Start with fresh browser, app at home page, no user logged in.
  2. Navigate to the app home or login page.
  3. Enter valid email and password for an existing test account.
  4. Click the ‘Sign in’ / ‘Login’ button.
  5. Wait for navigation to the authenticated landing/dashboard page.
  6. Verify user-specific UI (e.g., username, logout link) is visible.
  7. Log out and verify return to unauthenticated state.

**Expected Results:**
  - User is successfully authenticated and redirected to dashboard.
  - User-specific elements (username, profile, logout) are visible.
  - Logout returns to public/unauthenticated view. Failure: Login rejected, or dashboard not shown.

#### 1.2. Login — Validation and Error Messages

**File:** `specs/authentication/login-validation.spec.ts`

**Steps:**
  1. Assumption: App at login page with no saved credentials.
  2. Leave email empty, enter a password, submit.
  3. Enter malformed email and valid password, submit.
  4. Enter valid email and wrong password, submit.
  5. Enter valid email and valid password, but server returns slow (simulate using throttling).

**Expected Results:**
  - Empty email shows appropriate inline validation error.
  - Malformed email shows correct validation message and prevents submit.
  - Wrong password shows authentication error (e.g., 'invalid credentials') without revealing sensitive info.
  - On slow response the UI shows spinner/loading; eventual success/failure message appears.

#### 1.3. Session Handling and Timeout

**File:** `specs/authentication/session-timeout.spec.ts`

**Steps:**
  1. Assumption: User able to log in; use a test account.
  2. Log in successfully and stay idle for configured session timeout period (or simulate session expiry by clearing token).
  3. Perform an action requiring authentication after timeout (e.g., navigate to profile/edit page).

**Expected Results:**
  - After timeout, protected pages should require re-authentication (redirect to login) or show session-expired modal.
  - Actions requiring authentication must fail gracefully with clear message.

### 2. Registration

**Seed:** `seed.spec.ts`

#### 2.1. Register — Happy Path

**File:** `specs/registration/register-happy.spec.ts`

**Steps:**
  1. Assumption: Fresh test environment with no existing test email.
  2. Navigate to registration page.
  3. Fill required fields with valid data (email, password, name, required profile fields).
  4. Submit registration and follow any email-confirmation flow if present (simulate or use test hook).
  5. Verify new user is created and redirected to onboarding or dashboard.

**Expected Results:**
  - User account is created; user is authenticated or shown next steps (confirmation).
  - UI contains welcome/onboarding indicators or profile shows correct data.

#### 2.2. Register — Validation & Duplicate Handling

**File:** `specs/registration/register-validation.spec.ts`

**Steps:**
  1. Assumption: There is an existing account using test@example.com.
  2. Attempt to register with missing required fields and submit.
  3. Attempt with invalid password (too short / missing complexity).
  4. Attempt to register with an email already in use.
  5. Try rapid repeated submissions to check rate-limiting.
  6. Check server returns appropriate HTTP error codes for duplicate/invalid data.

**Expected Results:**
  - Missing/invalid fields show inline validation errors and prevent submission.
  - Duplicate email shows a clear, non-sensitive message (e.g., 'email already registered') and prevents account creation.
  - Rapid submissions handled gracefully (debounce/rate-limit) without creating multiple accounts.

### 3. Personal Details

**Seed:** `seed.spec.ts`

#### 3.1. View and Update Personal Details — Happy Path

**File:** `specs/personal/update-details.spec.ts`

**Steps:**
  1. Assumption: Authenticated test user with existing profile data.
  2. Navigate to Personal Details / Profile page.
  3. Verify current values are displayed correctly.
  4. Edit fields (name, phone, address) with valid new values and save.
  5. Reload page and/or re-open profile to verify persisted changes.

**Expected Results:**
  - Fields update successfully and changes persist on reload.
  - Backend reflects updated values (if test can query API). Failure: changes revert or validation blocks saving.

#### 3.2. Personal Details — Field Validations and Edge Cases

**File:** `specs/personal/details-validation.spec.ts`

**Steps:**
  1. Assumption: Authenticated user on Personal Details page.
  2. Attempt to enter overly long values (beyond allowed length).
  3. Enter invalid phone/email formats in optional fields and attempt save.
  4. Leave required fields empty and attempt save.

**Expected Results:**
  - Length limits enforced with errors or truncation.
  - Invalid formats show inline validation errors and prevent save.
  - Required field omission prevents save with clear messages.

### 4. Navigation & Stability

**Seed:** `seed.spec.ts`

#### 4.1. Primary Navigation Flow

**File:** `specs/navigation/primary-nav.spec.ts`

**Steps:**
  1. Assumption: Fresh state or authenticated state as appropriate per flow.
  2. Click each top-level navigation item in sequence, ensuring the app navigates and loads the target page.
  3. Use browser back and forward to verify consistent state and no console errors.
  4. Open the same flows in a new tab and confirm session continuity (if applicable).

**Expected Results:**
  - Each nav item loads the expected page and content.
  - Back/forward preserve expected state without errors.
  - Multi-tab behavior maintains consistent session and does not produce race conditions.

#### 4.2. Resilience — Network and Slow Responses

**File:** `specs/navigation/resilience.spec.ts`

**Steps:**
  1. Assumption: Test harness can simulate network throttling or response delays.
  2. Apply network slow or offline mode, then navigate to a primary page.
  3. Attempt form submission during intermittent connectivity.
  4. Return network to normal and verify recovery (requests retried or error messages shown).

**Expected Results:**
  - UI shows loading indicators and helpful offline messages.
  - Form submissions either queue or fail with clear retry instructions.
  - App recovers gracefully when connectivity returns without data corruption.

### 5. Accessibility & Visual Regression

**Seed:** `seed.spec.ts`

#### 5.1. Automated Accessibility Checks (a11y)

**File:** `specs/accessibility/accessibility.spec.ts`

**Steps:**
  1. Assumption: Fresh/a known page state to inspect (home, login, registration, profile).
  2. Run automated accessibility audit (e.g., axe-core via Playwright) on key pages: home, login, register, personal details, dashboard.
  3. Record violations, severity, and suggested fixes.

**Expected Results:**
  - No critical accessibility violations. Any violations are documented with reproduction steps and a severity rating. Failure: critical a11y issues block release.

#### 5.2. Visual Regression — Critical Screenshots

**File:** `specs/visual/visual-regression.spec.ts`

**Steps:**
  1. Assumption: Baseline screenshots exist or will be created in CI for master baseline.
  2. Capture full-page and component screenshots for critical pages (home, login, dashboard, personal details).
  3. Compare screenshots against baseline with a small acceptable threshold.
  4. Flag and triage any unintended diffs.

**Expected Results:**
  - No unexpected visual diffs beyond acceptable thresholds. Legitimate changes are reviewed and baseline updated intentionally.
