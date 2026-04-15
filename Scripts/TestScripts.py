import unittest
from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.chrome.options import Options
from BridgeNowFinanceHomePage import BridgeNowFinanceHomePage
from BridgeNowFinanceCompliancePage import BridgeNowFinanceCompliancePage
from Pages.BridgeNowFinanceLandingPage import BridgeNowFinanceLandingPage
from Pages.ApplicationFormPage import ApplicationFormPage

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

    def test_landing_page_layout_and_cta(self):
        """
        Test Case 596: VRVTEMP-390 TS-003 TC-001
        Steps:
        1. Launch the application or navigate to the website.
        2. Navigate to the BridgeNow Finance landing page.
        3. Review the page layout and content.
        4. Locate the call-to-action (CTA) to apply.
        Expected: A clear and prominent CTA to apply is visible on the landing page.
        """
        landing_page = BridgeNowFinanceLandingPage(self.driver)
        landing_page.launch_application()
        self.assertTrue(landing_page.is_landing_page_loaded(), "Landing page did not load.")
        self.assertTrue(landing_page.review_layout_and_content(), "Landing page layout/content is incomplete.")
        self.assertTrue(landing_page.locate_cta_apply(), "CTA to apply is not visible on the landing page.")

    def test_apply_and_stp_workflow(self):
        """
        Test Case 597: VRVTEMP-390 TS-004 TC-001
        Steps:
        1. Launch the application or navigate to the website.
        2. Navigate to the BridgeNow Finance landing page.
        3. Click on the 'Apply' CTA.
        4. Fill in the application form with eligible customer details.
        5. Submit the application.
        6. Observe the processing workflow.
        Expected: The application is processed automatically via the STP workflow without manual intervention.
        """
        landing_page = BridgeNowFinanceLandingPage(self.driver)
        landing_page.launch_application()
        self.assertTrue(landing_page.is_landing_page_loaded(), "Landing page did not load.")
        self.assertTrue(landing_page.locate_cta_apply(), "CTA to apply is not visible on the landing page.")
        landing_page.click_apply_cta()
        form_page = ApplicationFormPage(self.driver)
        self.assertTrue(form_page.is_form_loaded(), "Application form did not load.")
        # Example eligible customer data, should be replaced with valid data if required
        form_page.fill_application_form(name="John Doe", email="john.doe@example.com", phone="1234567890")
        form_page.submit_application()
        self.assertTrue(form_page.observe_processing_workflow(), "STP processing indicator not visible after submission.")

if __name__ == "__main__":
    unittest.main()
