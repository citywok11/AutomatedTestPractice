const { Builder, By, until } = require('selenium-webdriver');
const chrome = require('selenium-webdriver/chrome');
const assert = require('assert'); // Add this for assertions


(async function createNewUserMarkAsComplete() {

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
      await userHasBeenCreated(driver);
    } catch (error) {
      console.error('❌ Test failed:', error.message);
    } finally {
      await driver.quit();
      console.log('🔒 Browser closed. Test completed.');
    }
})();

async function userHasBeenCreated(driver) {

    const TODO_API_URL = 'http://localhost:5119'; // Update this to your actual URL
    await driver.get(TODO_API_URL);
    console.log('⏱️  Waiting for page to load...');
    await driver.sleep(3000); // Wait for initial page load



     // 1. Assert page title
    const title = await driver.getTitle();
    assert.strictEqual(title, 'TodoApi Database Viewer', 'Page title should match');
    console.log('✅ Page title assertion passed');






    
    var userName = await driver.findElement(By.css('[data-testid="user-name"]'))
    assert.strictEqual(await userName.getText(), 'John Doe', 'User name should be John Doe');
    console.log('✅ User name assertion passed');

    var addSampleButton = await driver.findElement(By.css('[data-testid="add-sample-todo-btn"]'))
    addSampleButton.click();
    addSampleButton.click();

    await driver.sleep(3000); 

    var TodoBox = await driver.findElement(By.css('[data-testid="todos-container"]'))
    assert(TodoBox, 'Todo box should exist');

    var TodoTitle = await driver.findElement(By.css('[data-testid="todo-title"]'))
    assert(TodoTitle, 'Todo title should exist');

    await driver.sleep(3000); // Wait for initial page load

    var CompletButton = await driver.findElement(By.css('[data-testid="mark-complete-btn"]'))
    CompletButton.click();

    await driver.sleep(3000); // Wait for initial page load

    var CompletedTimestamp = await driver.findElement(By.xpath('//*[@id="todosContainer"]/div[1]/div[3]/text()[3]'))
    assert(CompletedTimestamp, 'Completed timestamp should exist');
    console.log('✅ Completed timestamp assertion passed');
}

async function clickTodoBox(driver, selector) {
    
}
