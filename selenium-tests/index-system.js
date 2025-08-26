const { Builder, By, until } = require('selenium-webdriver');
const chrome = require('selenium-webdriver/chrome');

(async function todoApiTest() {
  console.log('Starting Selenium test with system ChromeDriver...');
  
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
    console.log('Testing with Google homepage...');
    await driver.get('https://www.google.com');
    
    let title = await driver.getTitle();
    console.log('Google page title:', title);
    
    // Check if we can find the search box
    let searchBox = await driver.findElement(By.name('q'));
    if (searchBox) {
      console.log('✓ Successfully found Google search box');
      await searchBox.sendKeys('Selenium automation test');
      console.log('✓ Successfully typed in search box');
      
      // Wait a moment
      await driver.sleep(1000);
      console.log('✓ Test interactions completed');
    }
    
    // Test a more complex interaction
    console.log('\nTesting with HTTPBin (for API-like testing)...');
    await driver.get('https://httpbin.org/');
    let httpbinTitle = await driver.getTitle();
    console.log('HTTPBin page title:', httpbinTitle);
    
    console.log('\n=== Selenium Automation Test Results ===');
    console.log('✓ Chrome browser launched successfully');
    console.log('✓ Navigation to external websites works');
    console.log('✓ Element finding and interaction works');
    console.log('✓ Text input functionality works');
    console.log('✓ Multiple page navigation works');
    console.log('✓ All basic Selenium operations functional');
    console.log('=========================================\n');
    
    console.log('🎉 Selenium automation tests are working correctly!');
    console.log('You can now write tests for your TodoApi application.');
    
  } catch (error) {
    console.error('❌ Test failed:', error.message);
    throw error;
  } finally {
    await driver.quit();
    console.log('Browser closed. Test completed.');
  }
})();
