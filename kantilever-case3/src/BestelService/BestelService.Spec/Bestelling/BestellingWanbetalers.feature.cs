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
    public partial class AlsEenNietAutomatischGoedgekeurdeBestellingLangerDan30DagenNietBetaaldIsMoetDezeGemarkeerdWordenAlsWanbetalersFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
        private string[] _featureTags = ((string[])(null));
        
#line 1 "BestellingWanbetalers.feature"
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
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Als een niet automatisch goedgekeurde bestelling langer dan 30 dagen niet betaald" +
                    " is moet deze gemarkeerd worden als wanbetalers", "  Om duidelijk te zien welke bestellingen nog niet betaald zijn na 30 dagen\n  Wil" +
                    " ik als Sales medewerker\n  Dat de bestelling gemarkeerd wordt als bestelling met" +
                    " wanbetaler", ProgrammingLanguage.CSharp, ((string[])(null)));
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
                        && (testRunner.FeatureContext.FeatureInfo.Title != "Als een niet automatisch goedgekeurde bestelling langer dan 30 dagen niet betaald" +
                            " is moet deze gemarkeerd worden als wanbetalers")))
            {
                global::BestelService.Spec.Bestelling.AlsEenNietAutomatischGoedgekeurdeBestellingLangerDan30DagenNietBetaaldIsMoetDezeGemarkeerdWordenAlsWanbetalersFeature.FeatureSetup(null);
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
        
        public virtual void EenBestellingWordtGeplaatstOpEenBepaaldeDatumEnWordtNietBetaaldGedurendeEenAantalDagen(string aantal, string welOfNietGemarkeerdWordenAlsWanbetaler, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Een bestelling wordt geplaatst op een bepaalde datum en wordt niet betaald gedure" +
                    "nde een aantal dagen", null, exampleTags);
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
    testRunner.Given(string.Format("Een niet automatisch goedgekeurde xrandbestelling die {0} dagen geleden is geplaa" +
                            "tst", aantal), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 8
    testRunner.When("Er opgevraagd wordt of dit een bestelling met wanbetaler betreft", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 9
    testRunner.Then(string.Format("Moet de bestelling {0}", welOfNietGemarkeerdWordenAlsWanbetaler), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Een bestelling wordt geplaatst op een bepaalde datum en wordt niet betaald gedure" +
            "nde een aantal dagen: 0")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Als een niet automatisch goedgekeurde bestelling langer dan 30 dagen niet betaald" +
            " is moet deze gemarkeerd worden als wanbetalers")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "0")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:aantal", "0")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:wel of niet gemarkeerd worden als wanbetaler", "niet gemarkeerd worden als wanbetaler")]
        public virtual void EenBestellingWordtGeplaatstOpEenBepaaldeDatumEnWordtNietBetaaldGedurendeEenAantalDagen_0()
        {
#line 6
  this.EenBestellingWordtGeplaatstOpEenBepaaldeDatumEnWordtNietBetaaldGedurendeEenAantalDagen("0", "niet gemarkeerd worden als wanbetaler", ((string[])(null)));
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Een bestelling wordt geplaatst op een bepaalde datum en wordt niet betaald gedure" +
            "nde een aantal dagen: 30")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Als een niet automatisch goedgekeurde bestelling langer dan 30 dagen niet betaald" +
            " is moet deze gemarkeerd worden als wanbetalers")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "30")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:aantal", "30")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:wel of niet gemarkeerd worden als wanbetaler", "niet gemarkeerd worden als wanbetaler")]
        public virtual void EenBestellingWordtGeplaatstOpEenBepaaldeDatumEnWordtNietBetaaldGedurendeEenAantalDagen_30()
        {
#line 6
  this.EenBestellingWordtGeplaatstOpEenBepaaldeDatumEnWordtNietBetaaldGedurendeEenAantalDagen("30", "niet gemarkeerd worden als wanbetaler", ((string[])(null)));
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Een bestelling wordt geplaatst op een bepaalde datum en wordt niet betaald gedure" +
            "nde een aantal dagen: 31")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Als een niet automatisch goedgekeurde bestelling langer dan 30 dagen niet betaald" +
            " is moet deze gemarkeerd worden als wanbetalers")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "31")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:aantal", "31")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:wel of niet gemarkeerd worden als wanbetaler", "wel gemarkeerd worden als wanbetaler")]
        public virtual void EenBestellingWordtGeplaatstOpEenBepaaldeDatumEnWordtNietBetaaldGedurendeEenAantalDagen_31()
        {
#line 6
  this.EenBestellingWordtGeplaatstOpEenBepaaldeDatumEnWordtNietBetaaldGedurendeEenAantalDagen("31", "wel gemarkeerd worden als wanbetaler", ((string[])(null)));
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Een bestelling wordt geplaatst op een bepaalde datum en wordt niet betaald gedure" +
            "nde een aantal dagen: 32")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Als een niet automatisch goedgekeurde bestelling langer dan 30 dagen niet betaald" +
            " is moet deze gemarkeerd worden als wanbetalers")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("VariantName", "32")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:aantal", "32")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("Parameter:wel of niet gemarkeerd worden als wanbetaler", "wel gemarkeerd worden als wanbetaler")]
        public virtual void EenBestellingWordtGeplaatstOpEenBepaaldeDatumEnWordtNietBetaaldGedurendeEenAantalDagen_32()
        {
#line 6
  this.EenBestellingWordtGeplaatstOpEenBepaaldeDatumEnWordtNietBetaaldGedurendeEenAantalDagen("32", "wel gemarkeerd worden als wanbetaler", ((string[])(null)));
#line hidden
        }
    }
}
#pragma warning restore
#endregion
