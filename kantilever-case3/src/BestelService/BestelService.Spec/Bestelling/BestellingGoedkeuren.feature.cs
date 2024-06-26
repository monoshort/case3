// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:3.1.0.0
//      SpecFlow Generator Version:3.1.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace BestelService.Spec.Bestelling
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class AutomatischeGoedkeuringVanBestellingenBijPlaatsingFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
        private string[] _featureTags = ((string[])(null));
        
#line 1 "BestellingGoedkeuren.feature"
#line hidden
        
        public virtual Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext
        {
            get
            {
                return this._testContext;
            }
            set
            {
                this._testContext = value;
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Automatische goedkeuring van bestellingen bij plaatsing", "  Om risico\'s te verminderen bij bestellingen\n  Wil ik als sales medewerker\n  Bes" +
                    "tellingen die een subtotaal inclusief btw boven de 500.00 hebben handmatig goedk" +
                    "euren", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((testRunner.FeatureContext != null) 
                        && (testRunner.FeatureContext.FeatureInfo.Title != "Automatische goedkeuring van bestellingen bij plaatsing")))
            {
                global::BestelService.Spec.Bestelling.AutomatischeGoedkeuringVanBestellingenBijPlaatsingFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Microsoft.VisualStudio.TestTools.UnitTesting.TestContext>(_testContext);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void ErWordtEenBestellingGeplaatstMetEenBepaaldSubtotaal(string subtotaalInclusiefBtw, string welOfNietGoedgekeurd, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Er wordt een bestelling geplaatst met een bepaald subtotaal", null, exampleTags);
#line 6
  this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 7
    testRunner.Given(string.Format("Een bestelling van {0}", subtotaalInclusiefBtw), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 8
    testRunner.When("Deze geplaatst wordt", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 9
    testRunner.Then(string.Format("Moet deze {0} worden", welOfNietGoedgekeurd), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Er wordt een bestelling geplaatst met een bepaald subtotaal: 499.00")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Automatische goedkeuring van bestellingen bij plaatsing")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "499.00")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:subtotaal inclusief btw", "499.00")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:wel of niet goedgekeurd", "wel automatisch goedgekeurd")]
        public virtual void ErWordtEenBestellingGeplaatstMetEenBepaaldSubtotaal_499_00()
        {
#line 6
  this.ErWordtEenBestellingGeplaatstMetEenBepaaldSubtotaal("499.00", "wel automatisch goedgekeurd", ((string[])(null)));
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Er wordt een bestelling geplaatst met een bepaald subtotaal: 499.99")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Automatische goedkeuring van bestellingen bij plaatsing")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "499.99")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:subtotaal inclusief btw", "499.99")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:wel of niet goedgekeurd", "wel automatisch goedgekeurd")]
        public virtual void ErWordtEenBestellingGeplaatstMetEenBepaaldSubtotaal_499_99()
        {
#line 6
  this.ErWordtEenBestellingGeplaatstMetEenBepaaldSubtotaal("499.99", "wel automatisch goedgekeurd", ((string[])(null)));
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Er wordt een bestelling geplaatst met een bepaald subtotaal: 500.00")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Automatische goedkeuring van bestellingen bij plaatsing")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "500.00")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:subtotaal inclusief btw", "500.00")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:wel of niet goedgekeurd", "wel automatisch goedgekeurd")]
        public virtual void ErWordtEenBestellingGeplaatstMetEenBepaaldSubtotaal_500_00()
        {
#line 6
  this.ErWordtEenBestellingGeplaatstMetEenBepaaldSubtotaal("500.00", "wel automatisch goedgekeurd", ((string[])(null)));
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Er wordt een bestelling geplaatst met een bepaald subtotaal: 500.99")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Automatische goedkeuring van bestellingen bij plaatsing")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "500.99")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:subtotaal inclusief btw", "500.99")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:wel of niet goedgekeurd", "niet automatisch goedgekeurd")]
        public virtual void ErWordtEenBestellingGeplaatstMetEenBepaaldSubtotaal_500_99()
        {
#line 6
  this.ErWordtEenBestellingGeplaatstMetEenBepaaldSubtotaal("500.99", "niet automatisch goedgekeurd", ((string[])(null)));
#line hidden
        }
        
        public virtual void ErWordenTweeBestellingenGeplaatstMetEenBepaaldSubtotaal(string subtotaalInclusiefBtw1, string subtotaalInclusiefBtw2, string welOfNietGoedgekeurd1, string welOfNietGoedgekeurd2, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Er worden twee bestellingen geplaatst met een bepaald subtotaal", null, exampleTags);
#line 18
  this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 19
    testRunner.Given(string.Format("Een bestelling van een klant met een subtotaal inclusief btw van {0}", subtotaalInclusiefBtw1), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 20
    testRunner.And(string.Format("Een bestelling van dezelfde klant met een subtotaal inclusief btw van {0}", subtotaalInclusiefBtw2), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 21
    testRunner.When("Deze achter elkaar geplaatst worden", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 22
    testRunner.Then(string.Format("Moet bestelling een {0} worden en bestelling twee {1} worden", welOfNietGoedgekeurd1, welOfNietGoedgekeurd2), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Er worden twee bestellingen geplaatst met een bepaald subtotaal: Variant 0")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Automatische goedkeuring van bestellingen bij plaatsing")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "Variant 0")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:subtotaal inclusief btw 1", "200.00")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:subtotaal inclusief btw 2", "200.00")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:wel of niet goedgekeurd 1", "wel automatisch goedgekeurd")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:wel of niet goedgekeurd 2", "wel automatisch goedgekeurd")]
        public virtual void ErWordenTweeBestellingenGeplaatstMetEenBepaaldSubtotaal_Variant0()
        {
#line 18
  this.ErWordenTweeBestellingenGeplaatstMetEenBepaaldSubtotaal("200.00", "200.00", "wel automatisch goedgekeurd", "wel automatisch goedgekeurd", ((string[])(null)));
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Er worden twee bestellingen geplaatst met een bepaald subtotaal: Variant 1")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Automatische goedkeuring van bestellingen bij plaatsing")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "Variant 1")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:subtotaal inclusief btw 1", "400.00")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:subtotaal inclusief btw 2", "200.00")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:wel of niet goedgekeurd 1", "wel automatisch goedgekeurd")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:wel of niet goedgekeurd 2", "niet automatisch goedgekeurd")]
        public virtual void ErWordenTweeBestellingenGeplaatstMetEenBepaaldSubtotaal_Variant1()
        {
#line 18
  this.ErWordenTweeBestellingenGeplaatstMetEenBepaaldSubtotaal("400.00", "200.00", "wel automatisch goedgekeurd", "niet automatisch goedgekeurd", ((string[])(null)));
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Er worden twee bestellingen geplaatst met een bepaald subtotaal: Variant 2")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Automatische goedkeuring van bestellingen bij plaatsing")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "Variant 2")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:subtotaal inclusief btw 1", "500.00")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:subtotaal inclusief btw 2", "200.00")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:wel of niet goedgekeurd 1", "wel automatisch goedgekeurd")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:wel of niet goedgekeurd 2", "niet automatisch goedgekeurd")]
        public virtual void ErWordenTweeBestellingenGeplaatstMetEenBepaaldSubtotaal_Variant2()
        {
#line 18
  this.ErWordenTweeBestellingenGeplaatstMetEenBepaaldSubtotaal("500.00", "200.00", "wel automatisch goedgekeurd", "niet automatisch goedgekeurd", ((string[])(null)));
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Er worden twee bestellingen geplaatst met een bepaald subtotaal: Variant 3")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Automatische goedkeuring van bestellingen bij plaatsing")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "Variant 3")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:subtotaal inclusief btw 1", "500.00")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:subtotaal inclusief btw 2", "500.00")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:wel of niet goedgekeurd 1", "wel automatisch goedgekeurd")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:wel of niet goedgekeurd 2", "niet automatisch goedgekeurd")]
        public virtual void ErWordenTweeBestellingenGeplaatstMetEenBepaaldSubtotaal_Variant3()
        {
#line 18
  this.ErWordenTweeBestellingenGeplaatstMetEenBepaaldSubtotaal("500.00", "500.00", "wel automatisch goedgekeurd", "niet automatisch goedgekeurd", ((string[])(null)));
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Er worden twee bestellingen geplaatst met een bepaald subtotaal: Variant 4")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Automatische goedkeuring van bestellingen bij plaatsing")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "Variant 4")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:subtotaal inclusief btw 1", "300.00")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:subtotaal inclusief btw 2", "200.00")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:wel of niet goedgekeurd 1", "wel automatisch goedgekeurd")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:wel of niet goedgekeurd 2", "wel automatisch goedgekeurd")]
        public virtual void ErWordenTweeBestellingenGeplaatstMetEenBepaaldSubtotaal_Variant4()
        {
#line 18
  this.ErWordenTweeBestellingenGeplaatstMetEenBepaaldSubtotaal("300.00", "200.00", "wel automatisch goedgekeurd", "wel automatisch goedgekeurd", ((string[])(null)));
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Er worden twee bestellingen geplaatst met een bepaald subtotaal: Variant 5")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Automatische goedkeuring van bestellingen bij plaatsing")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "Variant 5")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:subtotaal inclusief btw 1", "300.00")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:subtotaal inclusief btw 2", "200.01")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:wel of niet goedgekeurd 1", "wel automatisch goedgekeurd")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:wel of niet goedgekeurd 2", "niet automatisch goedgekeurd")]
        public virtual void ErWordenTweeBestellingenGeplaatstMetEenBepaaldSubtotaal_Variant5()
        {
#line 18
  this.ErWordenTweeBestellingenGeplaatstMetEenBepaaldSubtotaal("300.00", "200.01", "wel automatisch goedgekeurd", "niet automatisch goedgekeurd", ((string[])(null)));
#line hidden
        }
    }
}
#pragma warning restore
#endregion
