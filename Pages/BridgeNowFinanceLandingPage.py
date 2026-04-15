import time
from selenium.webdriver.common.by import By
from selenium.webdriver.remote.webdriver import WebDriver

class BridgeNowFinanceLandingPage:
    """
    PageClass for BridgeNow Finance Landing Page.
    Handles navigation, layout review, and locating the Apply CTA.
    """
    URL = "https://bridgenowfinance.com/"  # Placeholder, update as needed
    CTA_APPLY_LOCATOR = (By.XPATH, "//a[contains(@class, 'cta-apply') or contains(text(), 'Apply')]" )

    def __init__(self, driver: WebDriver):
        self.driver = driver

    def launch_application(self):
        self.driver.get(self.URL)
        time.sleep(2)  # Wait for page to load

    def is_landing_page_loaded(self):
        return "BridgeNow Finance" in self.driver.title

    def review_layout_and_content(self):
        # Example: Check for key elements
        header = self.driver.find_elements(By.TAG_NAME, "header")
        footer = self.driver.find_elements(By.TAG_NAME, "footer")
        main = self.driver.find_elements(By.TAG_NAME, "main")
        return bool(header) and bool(footer) and bool(main)

    def locate_cta_apply(self):
        cta = self.driver.find_elements(*self.CTA_APPLY_LOCATOR)
        return len(cta) > 0 and cta[0].is_displayed()

    def click_apply_cta(self):
        cta = self.driver.find_element(*self.CTA_APPLY_LOCATOR)
        cta.click()
