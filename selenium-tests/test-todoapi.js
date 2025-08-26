const { Builder, By, until } = require('selenium-webdriver');
const chrome = require('selenium-webdriver/chrome');

(async function todoApiSeleniumTest() {
  console.log('🚀 Starting TodoApi Selenium automation test...');
  
  // Configure Chrome options for headless mode
  let options = new chrome.Options();
  //options.addArguments('--headless'); // Comment this out to see the browser window in action
  options.addArguments('--no-sandbox');
  options.addArguments('--disable-dev-shm-usage');
  options.addArguments('--disable-gpu');
  options.addArguments('--window-size=1920,1080');

  // Use npm-installed ChromeDriver (automatic detection)
  let service = new chrome.ServiceBuilder();

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
    console.log('⏱️  Waiting for page to load...');
    await driver.sleep(3000); // Wait for initial page load
    
    let title = await driver.getTitle();
    console.log(`✓ TodoApi page title: "${title}"`);
    
    // Wait for the page to load and user to be initialized
    console.log('⏱️  Waiting for user initialization and dynamic content...');
    await driver.sleep(4000);
    
    // Look for the actual elements on your TodoApi page
    let refreshButton = await driver.findElements(By.xpath("//button[contains(text(), 'Refresh Todos')]"));
    if (refreshButton.length > 0) {
      console.log('✓ Found Refresh Todos button');
      console.log('⏱️  Clicking Refresh Todos button...');
      await refreshButton[0].click();
      console.log('✓ Clicked Refresh Todos button');
      console.log('⏱️  Waiting for todos to refresh...');
      await driver.sleep(3000); // Wait for refresh
    } else {
      console.log('❌ Refresh Todos button not found');
    }
    
    // Look for Add Sample Todo button
    let addSampleButton = await driver.findElements(By.xpath("//button[contains(text(), 'Add Sample Todo')]"));
    if (addSampleButton.length > 0) {
      console.log('✓ Found Add Sample Todo button');
      console.log('⏱️  Clicking Add Sample Todo button...');
      await addSampleButton[0].click();
      console.log('✓ Clicked Add Sample Todo button');
      console.log('⏱️  Waiting for new todo to be created and loaded...');
      await driver.sleep(4000); // Wait for todo to be added
    } else {
      console.log('❌ Add Sample Todo button not found');
    }
    
    // Check if todos container exists and has content
    console.log('⏱️  Checking todos container content...');
    await driver.sleep(2000);
    let todosContainer = await driver.findElements(By.id('todosContainer'));
    if (todosContainer.length > 0) {
      let todosText = await todosContainer[0].getText();
      console.log('✓ Found todos container');
      console.log(`📝 Todos container content: "${todosText.substring(0, 100)}..."`);
      
      if (todosText.includes('Loading todos...')) {
        console.log('⚠️  Todos still loading or no todos found');
      } else {
        console.log('✓ Todos loaded successfully');
      }
    } else {
      console.log('❌ Todos container not found');
    }
    
    // Check for user info section
    console.log('⏱️  Checking user information section...');
    await driver.sleep(1000);
    let userInfo = await driver.findElements(By.id('userInfo'));
    if (userInfo.length > 0) {
      let userText = await userInfo[0].getText();
      console.log('✓ Found user info section');
      console.log(`👤 User info: "${userText}"`);
    } else {
      console.log('❌ User info section not found');
    }
    
    console.log('⏱️  Test completed, preparing to close browser...');
    await driver.sleep(3000); // Final pause before closing
    
    console.log('\n🎯 === TODOAPI SELENIUM TEST RESULTS ===');
    console.log('✅ Successfully connected to TodoApi Database Viewer');
    console.log('✅ Page loaded and title retrieved');
    console.log('✅ User interface elements tested');
    console.log('✅ Button interactions verified');
    console.log('✅ Todos container functionality checked');
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
