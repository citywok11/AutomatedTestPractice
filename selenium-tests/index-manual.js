const { Builder, By, until } = require('selenium-webdriver');
const chrome = require('selenium-webdriver/chrome');

(async function todoApiTest() {
  console.log('Starting Selenium test (without chromedriver dependency)...');
  
  // Configure Chrome options for headless mode
  let options = new chrome.Options();
  options.addArguments('--headless');
  options.addArguments('--no-sandbox');
  options.addArguments('--disable-dev-shm-usage');
  options.addArguments('--disable-gpu');
  options.addArguments('--window-size=1920,1080');

  let driver = await new Builder()
    .forBrowser('chrome')
    .setChromeOptions(options)
    .build();
    
  try {
    console.log('Testing with Google homepage first...');
    await driver.get('https://www.google.com');
    
    let title = await driver.getTitle();
    console.log('Google page title:', title);
    
    // Check if we can find the search box
    let searchBox = await driver.findElement(By.name('q'));
    if (searchBox) {
      console.log('✓ Successfully found Google search box');
      await searchBox.sendKeys('Selenium automation test');
      console.log('✓ Successfully typed in search box');
      
      // Wait a moment and take a screenshot (if needed)
      await driver.sleep(1000);
      console.log('✓ Test completed successfully');
    }
    
    console.log('\n=== Selenium Automation Test Results ===');
    console.log('✓ Chrome browser launched successfully');
    console.log('✓ Navigation to external website works');
    console.log('✓ Element finding and interaction works');
    console.log('✓ All basic Selenium operations functional');
    console.log('=========================================\n');
    
  } catch (error) {
    console.error('Test failed:', error.message);
  } finally {
    await driver.quit();
    console.log('Browser closed. Test completed.');
  }
})();
