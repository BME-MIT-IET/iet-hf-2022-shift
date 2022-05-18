using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDFSharp.Model;
using System;
using TechTalk.SpecFlow;

namespace RDFSharp.BDDTest.Features
{
    [Binding]
    public class RDFGraphBDDTestSteps
    {

        private readonly ScenarioContext sc;

        private RDFGraph graphOne;
        private RDFGraph graphTwo;
        private RDFGraph union;

        public RDFGraphBDDTestSteps(ScenarioContext scen)
        {
            sc = scen;
            graphOne = new RDFGraph();
            graphTwo = new RDFGraph();
            union = new RDFGraph();
        }

        #region Given
        [Given(@"the uri is (.*)")]
        public void GivenTheUriIs(string uri)
        {
            sc["uri"] = uri;
        }

        [Given(@"an empty graph")]
        public void GivenAnEmptyGraph()
        {
            graphOne = new RDFGraph();
        }

        [Given(@"another empty graph")]
        public void GivenAnotherEmptyGraph()
        {
            graphTwo = new RDFGraph();
        }

        [Given(@"we add a triple with the values (.*), (.*), (.*)")]
        public void GivenWeAddATripleWithTheValues(string a, string b, string c)
        {
            RDFTriple triple = new RDFTriple(new RDFResource(a), new RDFResource(b), new RDFResource(c));
            graphOne.AddTriple(triple);
        }

        [Given(@"we add to the second graph a triple with the values (.*), (.*), (.*)")]
        public void GivenWeAddToTheSecondGraphATripleWithTheValuesHttpSubjHttpPredHttpObj(string a, string b, string c)
        {
            RDFTriple triple = new RDFTriple(new RDFResource(a), new RDFResource(b), new RDFResource(c));
            graphTwo.AddTriple(triple);
        }
        #endregion

        #region When

        [When(@"the context is set")]
        public void WhenTheContextIsSet()
        {
            string s = (string)sc["uri"];
            graphOne = graphOne.SetContext(new Uri(s));
        }

        [When(@"we compare them")]
        public void WhenWeCompareThem()
        {
            sc["Result"] = graphOne.Equals(graphTwo);
        }

        [When(@"we compare it to null")]
        public void WhenWeCompareItToNull()
        {
            sc["Result"] = graphOne.Equals(null);
        }

        [When(@"we add a triple with the values (.*), (.*), (.*)")]
        public void WhenWeAddATripleWithTheValues(string a, string b, string c)
        {
            RDFTriple triple = new RDFTriple(new RDFResource(a), new RDFResource(b), new RDFResource(c));
            graphOne.AddTriple(triple);
        }

        [When(@"we union them")]
        public void WhenWeUnionThem()
        {
            union = graphOne.UnionWith(graphTwo);
        }

        [When(@"we try to remove a triple with the subject (.*)")]
        public void WhenWeTryToRemoveATripleWithTheSubject(string sub)
        {
            graphOne.RemoveTriplesBySubject(new RDFResource(sub));
        }

        [When(@"we try to remove a triple with the predicate (.*)")]
        public void WhenWeTryToRemoveATripleWithThePredicate(string pred)
        {
            graphOne.RemoveTriplesByPredicate(new RDFResource(pred));
        }

        [When(@"we try to remove a triple with the object (.*)")]
        public void WhenWeTryToRemoveATripleWithTheObject(string obj)
        {
            graphOne.RemoveTriplesByObject(new RDFResource(obj));
        }
        #endregion

        #region Then
        [Then(@"the context should be (.*)")]
        public void ThenTheContextShouldBe(string uri2)
        {
            Assert.IsTrue(graphOne.Context.Equals(new Uri(uri2)));
        }

        [Then(@"the context should not be (.*)")]
        public void ThenTheContextShouldNotBeBnode(string uri2)
        {
            Assert.IsFalse(graphOne.Context.Equals(new Uri(uri2)));
        }


        [Then(@"it should succeed")]
        public void ThenItShouldSucceed()
        {
            var result = (bool)sc["Result"];
            Assert.IsTrue(result);
        }

        [Then(@"it should not succeed")]
        public void ThenItShouldNotSucceed()
        {
            var result = (bool)sc["Result"];
            Assert.IsFalse(result);
        }

        [Then(@"they should be equal")]
        public void ThenTheyShouldBeEqual()
        {
            var result = (bool)sc["Result"];
            Assert.IsTrue(result);
        }

        [Then(@"they should not be equal")]
        public void ThenTheyShouldNotBeEqual()
        {
            var result = (bool)sc["Result"];
            Assert.IsFalse(result);
        }

        [Then(@"the string representation should be (.*)")]
        public void ThenTheStringRepresentationShouldBe(string uri)
        {
            if (uri == "default")
            {
                Assert.IsTrue(graphOne.ToString().Equals(RDFNamespaceRegister.DefaultNamespace.ToString()));
            }
            else
                Assert.IsTrue(graphOne.ToString().Equals(uri));
        }

        [Then(@"the graph should have (.*) triple")]
        public void ThenTheGraphShouldHaveOneTriple(int amount)
        {
            Assert.IsTrue(graphOne.TriplesCount == amount);
        }

        [Then(@"the union should have (.*) triple")]
        public void ThenTheUnionShouldHaveTriple(int amount)
        {
            Assert.IsTrue(union.TriplesCount == amount);
        }
        #endregion
    }
}
