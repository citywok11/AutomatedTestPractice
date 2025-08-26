const { Builder, By, until } = require('selenium-webdriver');
require('chromedriver');

(async function todoApiTest() {
  let driver = await new Builder().forBrowser('chrome').build();
  try {
    // Replace with your actual API URL or web app URL
    const url = 'http://localhost:5000';
    await driver.get(url);

    // Example: Check page title
    let title = await driver.getTitle();
    console.log('Page title:', title);

    // Example: Find an element (update selector as needed)
    // let element = await driver.findElement(By.css('h1'));
    // let text = await element.getText();
    // console.log('Header text:', text);

    // Add more Selenium tests here as needed
  } finally {
    await driver.quit();
  }
})();
