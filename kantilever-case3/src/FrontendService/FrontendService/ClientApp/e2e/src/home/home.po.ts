import {browser, by} from 'protractor';

export class HomePage {
  navigateTo() {
    return browser.get('/');
  }

  getTitle() {
    return browser.findElement(by.className("navbar-brand"));
  }
}
