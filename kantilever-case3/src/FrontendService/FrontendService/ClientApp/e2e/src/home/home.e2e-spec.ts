import {HomePage} from "./home.po";

describe('App', () => {
  let page: HomePage;

  beforeEach(() => {
    page = new HomePage();
  });

  it('should display the page title', () => {
    // Arrange
    page.navigateTo();

    // Act
    const title = page.getTitle().getText();

    // Assert
    expect(title).toContain("Kantilever");
  });
});
