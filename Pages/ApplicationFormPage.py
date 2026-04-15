from selenium.webdriver.common.by import By
from selenium.webdriver.remote.webdriver import WebDriver

class ApplicationFormPage:
    """
    PageClass for BridgeNow Finance Application Form Page.
    Handles filling eligible customer details, submitting, and observing workflow.
    """
    FORM_LOCATOR = (By.ID, "application-form")
    NAME_FIELD_LOCATOR = (By.ID, "customer-name")
    EMAIL_FIELD_LOCATOR = (By.ID, "customer-email")
    PHONE_FIELD_LOCATOR = (By.ID, "customer-phone")
    SUBMIT_BUTTON_LOCATOR = (By.XPATH, "//button[contains(@type, 'submit') and contains(text(), 'Submit')]")
    PROCESSING_INDICATOR_LOCATOR = (By.ID, "stp-processing-indicator")

    def __init__(self, driver: WebDriver):
        self.driver = driver

    def is_form_loaded(self):
        return self.driver.find_elements(*self.FORM_LOCATOR)

    def fill_application_form(self, name: str, email: str, phone: str):
        self.driver.find_element(*self.NAME_FIELD_LOCATOR).send_keys(name)
        self.driver.find_element(*self.EMAIL_FIELD_LOCATOR).send_keys(email)
        self.driver.find_element(*self.PHONE_FIELD_LOCATOR).send_keys(phone)

    def submit_application(self):
        self.driver.find_element(*self.SUBMIT_BUTTON_LOCATOR).click()

    def observe_processing_workflow(self):
        indicator = self.driver.find_elements(*self.PROCESSING_INDICATOR_LOCATOR)
        return len(indicator) > 0 and indicator[0].is_displayed()
