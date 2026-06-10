import { test, expect } from '@playwright/test';

test.describe('Authentication', () => {
  test('should display login page', async ({ page }) => {
    await page.goto('http://localhost:3000/login');
    await expect(page.locator('h1')).toContainText('AccessControl');
    await expect(page.locator('input[type="email"]')).toBeVisible();
    await expect(page.locator('input[type="password"]')).toBeVisible();
  });

  test('should show error on invalid login', async ({ page }) => {
    await page.goto('http://localhost:3000/login');
    await page.fill('input[type="email"]', 'invalid@test.com');
    await page.fill('input[type="password"]', 'wrongpassword');
    await page.click('button[type="submit"]');
    await expect(page.locator('text=Erro ao fazer login')).toBeVisible();
  });
});

test.describe('Dashboard', () => {
  test('should display dashboard stats', async ({ page }) => {
    await page.goto('http://localhost:3000/');
    await expect(page.locator('text=Dashboard')).toBeVisible();
  });
});
