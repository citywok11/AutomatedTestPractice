# Selenium Automation Tests for TodoApi

This directory contains Selenium automation tests written in JavaScript specifically for testing your TodoApi application.

## Setup Complete ✅

- ✅ Node.js and npm configured
- ✅ Selenium WebDriver installed
- ✅ Google Chrome browser installed
- ✅ ChromeDriver (version 139) installed and configured
- ✅ TodoApi-specific test script created

## Available Test Scripts

### Main Test
- `npm test` - Runs TodoApi automation tests (requires running server)

## How to Use

### 1. Start Your TodoApi Server
```bash
cd src/TodoApi.Api
dotnet run
```

### 2. Run the TodoApi Tests
```bash
cd selenium-tests
npm test
```

### 3. Customize for Your Application
Edit `test-todoapi.js` to:
- Update the `TODO_API_URL` to match your server (default: http://localhost:5000)
- Modify CSS selectors to match your HTML structure
- Add specific test cases for your Todo application features

## Test Features

The current test suite covers:
- ✅ Navigation to your TodoApi application
- ✅ Page title verification
- ✅ Todo input field detection and interaction
- ✅ Add todo functionality testing
- ✅ Todo list structure verification
- ✅ Todo item completion/deletion testing

## Environment Details

- **Operating System**: Ubuntu 24.04.2 LTS (in dev container)
- **Node.js**: v22.17.0
- **Chrome**: 139.0.7258.138
- **ChromeDriver**: 139.0.7258.138
- **Selenium WebDriver**: ^4.20.0

## Test Configuration

The tests are configured to run in headless mode with the following Chrome options:
- `--headless` - Runs without GUI (perfect for CI/CD)
- `--no-sandbox` - Required for container environments
- `--disable-dev-shm-usage` - Prevents memory issues
- `--disable-gpu` - Reduces resource usage
- `--window-size=1920,1080` - Sets consistent window size

## Customization Guide

To adapt the tests for your specific TodoApi implementation:

1. **Update URL**: Change `TODO_API_URL` in `test-todoapi.js`
2. **Modify Selectors**: Update CSS selectors to match your HTML:
   - Todo input fields: `input[type="text"], input[placeholder*="todo"]`
   - Add buttons: `button[type="submit"], .add-btn`
   - Todo lists: `.todo-list, .tasks, .todo-items`
   - Todo items: `li, .todo-item, .task-item`
   - Complete buttons: `input[type="checkbox"], .complete-btn`
   - Delete buttons: `.delete-btn, .remove-btn`

3. **Add New Tests**: Extend the test with additional scenarios like:
   - User authentication
   - Todo editing
   - Filtering (completed/active todos)
   - Todo priorities
   - Multiple todo lists

## Troubleshooting

If you encounter issues:

1. **Server Not Running**: Make sure your TodoApi server is running at the specified URL
2. **Element Not Found**: Update CSS selectors to match your actual HTML structure
3. **ChromeDriver Issues**: Ensure ChromeDriver version matches your Chrome browser version
4. **Network Issues**: Verify your TodoApi is accessible via the browser manually

## Integration with CI/CD

These tests can be easily integrated into your build pipeline by running:
```bash
npm test
```

The headless configuration makes them suitable for automated testing environments.

Happy testing! 🚀
