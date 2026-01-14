// spec: specs/test-plan.md

import { test, expect } from '@playwright/test';

test.describe('Test Suite', () => {
  test('test', async ({ page }) => {
    // 1. Navigate to `/login`.
    await page.goto('/login');

    // 2. Verify presence of `input[type="email"]` and `input[type="password"]`.
    await expect(page.locator('input[type="email"]')).toBeVisible();
    await expect(page.locator('input[type="password"]')).toBeVisible();

    // 3. Fill email and password with test values and submit if a submit button exists.
    await page.fill('input[type="email"]', 'test@example.com').catch(() => {});
    await page.fill('input[type="password"]', 'Password123!').catch(() => {});
    const submit = page.locator('button[type="submit"], input[type="submit"]');
    if (await submit.count() > 0) await submit.first().click();
  });
});
