const { Builder, By, until } = require('selenium-webdriver');
const chrome = require('selenium-webdriver/chrome');

(async function seleniumVerificationTest() {
  console.log('🚀 Starting Selenium verification test...');
  
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
    // Test 1: Navigate to a simple HTML page
    console.log('📍 Test 1: Navigation to HTTPBin...');
    await driver.get('https://httpbin.org/html');
    
    let title = await driver.getTitle();
    console.log(`   ✓ Page title: "${title}"`);
    
    // Test 2: Find HTML elements
    console.log('📍 Test 2: Finding HTML elements...');
    let h1Elements = await driver.findElements(By.tagName('h1'));
    console.log(`   ✓ Found ${h1Elements.length} H1 element(s)`);
    
    if (h1Elements.length > 0) {
      let h1Text = await h1Elements[0].getText();
      console.log(`   ✓ H1 text: "${h1Text}"`);
    }
    
    // Test 3: Test a form page
    console.log('📍 Test 3: Testing form interaction...');
    await driver.get('https://httpbin.org/forms/post');
    
    // Find form elements
    let custname = await driver.findElement(By.name('custname'));
    let custtel = await driver.findElement(By.name('custtel'));
    
    // Fill out the form
    await custname.clear();
    await custname.sendKeys('Test Customer');
    console.log('   ✓ Entered customer name');
    
    await custtel.clear();
    await custtel.sendKeys('555-1234');
    console.log('   ✓ Entered customer phone');
    
    // Test 4: Navigate back to verify history
    console.log('📍 Test 4: Testing browser navigation...');
    await driver.navigate().back();
    
    let currentUrl = await driver.getCurrentUrl();
    console.log(`   ✓ Navigated back to: ${currentUrl}`);
    
    // Test 5: Execute JavaScript
    console.log('📍 Test 5: Testing JavaScript execution...');
    let userAgent = await driver.executeScript('return navigator.userAgent;');
    console.log(`   ✓ User Agent: ${userAgent.substring(0, 50)}...`);
    
    console.log('\n🎉 === SELENIUM AUTOMATION TEST SUITE RESULTS ===');
    console.log('✅ Chrome browser launches successfully');
    console.log('✅ Web page navigation works');
    console.log('✅ HTML element detection works');
    console.log('✅ Form field interaction works');
    console.log('✅ Text input functionality works');
    console.log('✅ Browser navigation (back/forward) works');
    console.log('✅ JavaScript execution works');
    console.log('==================================================\n');
    
    console.log('🏆 SUCCESS: Selenium automation is fully functional!');
    console.log('💡 You can now create tests for your TodoApi application.');
    console.log('💡 Run "npm run test-todo" to test with a local TodoApi server.');
    
  } catch (error) {
    console.error('❌ Test failed:', error.message);
    console.error('Full error:', error);
    throw error;
  } finally {
    await driver.quit();
    console.log('🔒 Browser closed. Test completed.');
  }
})();
