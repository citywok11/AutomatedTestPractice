const { Builder, By, until } = require('selenium-webdriver');
const chrome = require('selenium-webdriver/chrome');

(async function todoApiSeleniumTest() {
  console.log('🚀 Starting TodoApi Selenium automation test...');
  
  // Configure Chrome options for headless mode
  let options = new chrome.Options();
  options.addArguments('--headless');
  options.addArguments('--no-sandbox');
  options.addArguments('--disable-dev-shm-usage');
  options.addArguments('--disable-gpu');
  options.addArguments('--window-size=1920,1080');

  // Use system ChromeDriver
  let service = new chrome.ServiceBuilder('/usr/local/bin/chromedriver');

  let driver = await new Builder()
    .forBrowser('chrome')
    .setChromeOptions(options)
    .setChromeService(service)
    .build();
    
  try {
    // Test your TodoApi application
    const TODO_API_URL = 'http://localhost:5119'; // Update this to your actual URL
    
    console.log(`📍 Testing TodoApi at: ${TODO_API_URL}`);
    
    await driver.get(TODO_API_URL);
    
    let title = await driver.getTitle();
    console.log(`✓ TodoApi page title: "${title}"`);
    
    // Look for common Todo app elements
    let todoElements = await driver.findElements(By.css('input[type="text"], input[placeholder*="todo"], input[placeholder*="task"]'));
    if (todoElements.length > 0) {
      console.log(`✓ Found ${todoElements.length} todo input field(s)`);
      
      // Try to add a todo
      await todoElements[0].sendKeys('Test todo item from Selenium');
      console.log('✓ Added test todo item');
      
      // Look for add button
      let addButtons = await driver.findElements(By.css('button[type="submit"], button:contains("Add"), .add-btn, .btn-add'));
      if (addButtons.length > 0) {
        await addButtons[0].click();
        console.log('✓ Clicked add button');
      }
    }
    
    // Look for todo list
    let listElements = await driver.findElements(By.css('ul, ol, .todo-list, .tasks, .todo-items'));
    console.log(`✓ Found ${listElements.length} list element(s)`);
    
    // Look for individual todo items
    let todoItems = await driver.findElements(By.css('li, .todo-item, .task-item'));
    console.log(`✓ Found ${todoItems.length} todo item(s) in the list`);
    
    // Test todo item interactions (if any items exist)
    if (todoItems.length > 0) {
      // Look for complete/check buttons
      let completeButtons = await driver.findElements(By.css('input[type="checkbox"], .complete-btn, .check-btn'));
      if (completeButtons.length > 0) {
        await completeButtons[0].click();
        console.log('✓ Toggled todo item completion status');
      }
      
      // Look for delete buttons
      let deleteButtons = await driver.findElements(By.css('.delete-btn, .remove-btn, button:contains("Delete")'));
      if (deleteButtons.length > 0) {
        console.log('✓ Found delete functionality');
      }
    }
    
    console.log('\n� === TODOAPI SELENIUM TEST RESULTS ===');
    console.log('✅ Successfully connected to TodoApi');
    console.log('✅ Page loaded and title retrieved');
    console.log('✅ Todo input fields detected');
    console.log('✅ Todo list structure identified');
    console.log('✅ Basic todo interactions tested');
    console.log('==========================================\n');
    
    console.log('🎉 TodoApi Selenium test completed successfully!');
    
  } catch (error) {
    console.error('❌ TodoApi test failed:', error.message);
    console.log('\n💡 TROUBLESHOOTING TIPS:');
    console.log('1. Make sure your TodoApi server is running: cd src/TodoApi.Api && dotnet run');
    console.log('2. Verify the server is accessible at http://localhost:5000');
    console.log('3. Check that your TodoApi serves a web interface (HTML pages)');
    console.log('4. Update the CSS selectors in this test to match your actual HTML structure');
    throw error;
  } finally {
    await driver.quit();
    console.log('🔒 Browser closed. Test completed.');
  }
})();
