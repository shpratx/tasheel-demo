import unittest
from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.chrome.options import Options
from BridgeNowFinanceHomePage import BridgeNowFinanceHomePage
from BridgeNowFinanceCompliancePage import BridgeNowFinanceCompliancePage

class BridgeNowFinanceTestSuite(unittest.TestCase):
    def setUp(self):
        chrome_options = Options()
        chrome_options.add_argument('--headless')
        chrome_options.add_argument('--no-sandbox')
        chrome_options.add_argument('--disable-dev-shm-usage')
        self.driver = webdriver.Chrome(service=Service(), options=chrome_options)

    def tearDown(self):
        self.driver.quit()

    def test_landing_page_displayed(self):
        """
        Test Case 594: VRVTEMP-390 TS-001 TC-001
        Steps:
        1. Launch the application.
        2. Select BridgeNow Finance option.
        3. Assert landing page is displayed.
        """
        home_page = BridgeNowFinanceHomePage(self.driver)
        home_page.launch_application()
        home_page.select_bridgenow_finance_option()
        self.assertTrue(home_page.is_landing_page_displayed(), "Landing page is not displayed.")

    def test_compliance_checklist(self):
        """
        Test Case 595: VRVTEMP-390 TS-002 TC-001
        Steps:
        1. Launch the application.
        2. Navigate to BridgeNow Finance landing page.
        3. Review page content for compliance.
        4. Assert compliance using checklist.
        """
        compliance_page = BridgeNowFinanceCompliancePage(self.driver)
        compliance_page.launch_application()
        compliance_page.navigate_to_bridgenow_finance_landing()
        compliance_page.review_page_content_for_compliance()
        # Sample compliance checklist
        compliance_checklist = [
            "Privacy Policy visible",
            "Terms and Conditions accessible",
            "Financial disclosures present",
            "User consent required"
        ]
        compliance_results = compliance_page.compare_with_compliance_checklist(compliance_checklist)
        self.assertTrue(all(compliance_results.values()), f"Compliance failed: {compliance_results}")

if __name__ == "__main__":
    unittest.main()
