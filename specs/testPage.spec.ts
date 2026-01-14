// spec: specs/test-plan.md

import { test, expect } from '@playwright/test';

test.describe('TestPage Suite', () => {
  test('testPage', async ({ page }) => {
    // 1. Navigate to the app root (`/`).
    await page.goto('/');

    // 2. Verify the page has finished loading (`document.readyState === 'complete`).
    await page.waitForLoadState('load');
    const ready = await page.evaluate(() => document.readyState);
    expect(ready).toBe('complete');

    // 3. Take a full-page screenshot.
    await page.screenshot({ path: 'Reports/Screenshots/testPage.png', fullPage: true });
  });
});
