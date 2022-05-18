using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDFSharp.Model;
using RDFSharp.Query;
using RDFSharp.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireMock.Server;

namespace RDFSharp.Test.Query
{
    [TestClass]
    public class RDFDescribeQueryTest
    {
        private WireMockServer server;

        [TestInitialize]
        public void Initialize() { server = WireMockServer.Start(); }

        [TestCleanup]
        public void Cleanup() { server.Stop(); server.Dispose(); }

        #region Tests

        [TestMethod]
        public void ShouldCreateDescribeQuery()
        {
            RDFDescribeQuery query = new RDFDescribeQuery();

            Assert.IsNotNull(query);
            Assert.IsNotNull(query.QueryMembers);
            Assert.IsTrue(query.QueryMembers.Count == 0);
            Assert.IsNotNull(query.Prefixes);
            Assert.IsTrue(query.Prefixes.Count == 0);
            Assert.IsTrue(query.IsEvaluable);
            Assert.IsFalse(query.IsOptional);
            Assert.IsFalse(query.JoinAsUnion);
            Assert.IsFalse(query.IsSubQuery);
            Assert.IsTrue(query.ToString().Equals("DESCRIBE *" + Environment.NewLine + "WHERE {" + Environment.NewLine + "}"));
            Assert.IsTrue(query.QueryMemberID.Equals(RDFModelUtilities.CreateHash(query.QueryMemberStringID)));
            Assert.IsTrue(query.GetEvaluableQueryMembers().Count() == 0);
            Assert.IsTrue(query.GetPatternGroups().Count() == 0);
            Assert.IsTrue(query.GetSubQueries().Count() == 0);
            Assert.IsTrue(query.GetValues().Count() == 0);
            Assert.IsTrue(query.GetModifiers().Count() == 0);
            Assert.IsTrue(query.GetPrefixes().Count() == 0);
        }

        [TestMethod]
        public void ShouldCreateDescribeQueryWithQueryMembers()
        {
            RDFDescribeQuery query = new RDFDescribeQuery();
            query.AddPrefix(RDFNamespaceRegister.GetByPrefix("rdf"));
            query.AddPatternGroup(
                new RDFPatternGroup("PG1")
                    .AddPattern(new RDFPattern(new RDFVariable("?S"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDFS.CLASS))
                    .AddFilter(new RDFIsUriFilter(new RDFVariable("?S"))));
            query.AddSubQuery(
                new RDFSelectQuery()
                    .AddPrefix(RDFNamespaceRegister.GetByPrefix("owl"))
                    .AddPatternGroup(
                        new RDFPatternGroup("PG1")
                            .AddPattern(new RDFPattern(new RDFVariable("?S"), new RDFVariable("?P"), RDFVocabulary.OWL.CLASS))
                            .AddValues(new RDFValues().AddColumn(new RDFVariable("?S"), new List<RDFPatternMember>() { RDFVocabulary.RDFS.CLASS })))
                    .AddProjectionVariable(new RDFVariable("?S")));
            var a = query.ToString();
            Assert.IsTrue(query.ToString().Equals("PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>" + Environment.NewLine + "PREFIX owl: <http://www.w3.org/2002/07/owl#>" + Environment.NewLine + Environment.NewLine + "DESCRIBE *" + Environment.NewLine + "WHERE {" + Environment.NewLine + "  {" + Environment.NewLine + "    ?S rdf:type <http://www.w3.org/2000/01/rdf-schema#Class> ." + Environment.NewLine + "    FILTER ( ISURI(?S) ) " + Environment.NewLine + "  }" + Environment.NewLine + "  {" + Environment.NewLine + "    SELECT ?S" + Environment.NewLine + "    WHERE {" + Environment.NewLine + "      {" + Environment.NewLine + "        ?S ?P owl:Class ." + Environment.NewLine + "        VALUES ?S { <http://www.w3.org/2000/01/rdf-schema#Class> } ." + Environment.NewLine + "      }" + Environment.NewLine + "    }" + Environment.NewLine + "  }" + Environment.NewLine + "}"));
            Assert.IsTrue(query.QueryMemberID.Equals(RDFModelUtilities.CreateHash(query.QueryMemberStringID)));
            Assert.IsTrue(query.GetEvaluableQueryMembers().Count() == 2); //SPARQL Values is managed by Mirella
            Assert.IsTrue(query.GetPatternGroups().Count() == 1);
            Assert.IsTrue(query.GetSubQueries().Count() == 1);
            Assert.IsTrue(query.GetValues().Count() == 1);
            Assert.IsTrue(query.GetModifiers().Count() == 0);
            Assert.IsTrue(query.GetPrefixes().Count() == 2);
        }

        [TestMethod]
        public void AddDescribeTermShouldAddNewTerm() 
        {
            RDFDescribeQuery query = new ();
            RDFResource describeTerm = new ();

            var result = query.AddDescribeTerm (describeTerm);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.DescribeTerms.Count);
            Assert.AreEqual(describeTerm, result.DescribeTerms[0]);
        }

        [TestMethod]
        public void AddDescribeTermShouldNotAddNullTerm()
        {
            RDFDescribeQuery query = new();
            RDFResource describeTerm = null;

            var result = query.AddDescribeTerm(describeTerm);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.DescribeTerms.Count);
        }

        [TestMethod]
        public void AddDescribeTermShouldNotAddSameTermTwice()
        {
            RDFDescribeQuery query = new();
            RDFResource describeTerm = new();

            var result = query
                .AddDescribeTerm(describeTerm)
                .AddDescribeTerm(describeTerm);           

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.DescribeTerms.Count);
            Assert.AreEqual(describeTerm, result.DescribeTerms[0]);
        }

        [TestMethod]
        public void AddDescribeTermShouldAddNewVariable()
        {
            RDFDescribeQuery query = new();
            RDFVariable describeVar = new("stubVariable");

            var result = query.AddDescribeTerm(describeVar);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Variables.Count);
            Assert.AreEqual(describeVar, result.Variables[0]);
            Assert.AreEqual(1, result.DescribeTerms.Count);
            Assert.AreEqual(describeVar, result.DescribeTerms[0]);
        }

        [TestMethod]
        public void AddDescribeTermShouldNotAddNullVariable()
        {
            RDFDescribeQuery query = new();
            RDFVariable describeVar = null;

            var result = query.AddDescribeTerm(describeVar);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Variables.Count);
            Assert.AreEqual(0, result.DescribeTerms.Count);
        }

        [TestMethod]
        public void AddDescribeTermShouldNotAddSameVariableTwice()
        {
            RDFDescribeQuery query = new();
            RDFVariable describeVar = new("stubVariable");

            var result = query
                .AddDescribeTerm(describeVar)
                .AddDescribeTerm(describeVar);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Variables.Count);
            Assert.AreEqual(describeVar, result.Variables[0]);
            Assert.AreEqual(1, result.DescribeTerms.Count);
            Assert.AreEqual(describeVar, result.DescribeTerms[0]);
        }

        [TestMethod]
        public void ShouldApplyDescribeQueryToGraphAndHaveOneResult()
        {
            RDFGraph graph = new RDFGraph();
            graph.AddTriple(new RDFTriple(new RDFResource("ex:flower"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDFS.CLASS));
            RDFDescribeQuery query = new RDFDescribeQuery()
                .AddPrefix(RDFNamespaceRegister.GetByPrefix(RDFVocabulary.RDF.PREFIX))
                .AddPatternGroup(new RDFPatternGroup("PG1")
                    .AddPattern(new RDFPattern(new RDFVariable("?S"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDFS.CLASS)));
            
            var result = query.ApplyToGraph(graph);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.DescribeResultsCount);
            Assert.AreEqual(1, result.DescribeResults.Rows.Count);
        }

        [TestMethod]
        public void ShouldApplyDescribeQueryToGraphAndHaveZeroResult()
        {
            RDFGraph graph = new RDFGraph();
            graph.AddTriple(new RDFTriple(new RDFResource("ex:flower"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDFS.DATATYPE));
            RDFDescribeQuery query = new RDFDescribeQuery()
                .AddPrefix(RDFNamespaceRegister.GetByPrefix(RDFVocabulary.RDF.PREFIX))
                .AddPatternGroup(new RDFPatternGroup("PG1")
                    .AddPattern(new RDFPattern(new RDFVariable("?S"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDFS.CLASS)));
            
            var result = query.ApplyToGraph(graph);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.DescribeResultsCount);
            Assert.AreEqual(0, result.DescribeResults.Rows.Count);
        }

        [TestMethod]
        public void ShouldApplyAskQueryToNullGraphAndHaveZeroResult()
        {
            RDFDescribeQuery query = new RDFDescribeQuery()
                .AddPrefix(RDFNamespaceRegister.GetByPrefix(RDFVocabulary.RDF.PREFIX))
                .AddPatternGroup(new RDFPatternGroup("PG1")
                    .AddPattern(new RDFPattern(new RDFVariable("?S"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDFS.CLASS)));
            
            var result = query.ApplyToGraph(null);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.DescribeResultsCount);
            Assert.AreEqual(0, result.DescribeResults.Rows.Count);
        }

        [TestMethod]
        public void ShouldApplyDescribeQueryToStoreAndHaveResult()
        {
            RDFMemoryStore store = new RDFMemoryStore();
            store.AddQuadruple(new RDFQuadruple(new RDFContext(), new RDFResource("ex:flower"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDFS.CLASS));
            RDFDescribeQuery query = new RDFDescribeQuery()
                .AddPrefix(RDFNamespaceRegister.GetByPrefix(RDFVocabulary.RDF.PREFIX))
                .AddPatternGroup(new RDFPatternGroup("PG1")
                    .AddPattern(new RDFPattern(new RDFVariable("?C"), new RDFVariable("?S"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDFS.CLASS)));
            
            var result = query.ApplyToStore(store);

            Assert.IsNotNull(result);
            Assert.IsTrue(0 < result.DescribeResultsCount);
            Assert.IsTrue(0 < result.DescribeResults.Rows.Count);
        }

        [TestMethod]
        public void ShouldApplyDescribeQueryToStoreAndHaveZeroResult()
        {
            RDFMemoryStore store = new RDFMemoryStore();
            store.AddQuadruple(new RDFQuadruple(new RDFContext(), new RDFResource("ex:flower"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDFS.CLASS));
            RDFDescribeQuery query = new RDFDescribeQuery()
                .AddPrefix(RDFNamespaceRegister.GetByPrefix(RDFVocabulary.RDF.PREFIX))
                .AddPatternGroup(new RDFPatternGroup("PG1")
                    .AddPattern(new RDFPattern(new RDFContext("ex:ctx"), new RDFVariable("?S"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDFS.CLASS)));
            
            var result = query.ApplyToStore(store);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.DescribeResultsCount);
            Assert.AreEqual(0, result.DescribeResults.Rows.Count);
        }

        [TestMethod]
        public void ShouldApplyDescribeQueryToNullStoreAndHaveZeroResult()
        {
            RDFDescribeQuery query = new RDFDescribeQuery()
                .AddPrefix(RDFNamespaceRegister.GetByPrefix(RDFVocabulary.RDF.PREFIX))
                .AddPatternGroup(new RDFPatternGroup("PG1")
                    .AddPattern(new RDFPattern(new RDFVariable("?S"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDFS.CLASS)));
            
            var result = query.ApplyToStore(null);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.DescribeResultsCount);
            Assert.AreEqual(0, result.DescribeResults.Rows.Count);
        }

        [TestMethod]
        public void ShouldApplyDescribeQueryToFederationAndHaveResult()
        {
            RDFGraph graph = new RDFGraph();
            graph.AddTriple(new RDFTriple(new RDFResource("ex:flower"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDFS.CLASS));
            RDFFederation federation = new RDFFederation().AddGraph(graph);
            RDFDescribeQuery query = new RDFDescribeQuery()
                .AddPrefix(RDFNamespaceRegister.GetByPrefix(RDFVocabulary.RDF.PREFIX))
                .AddPatternGroup(new RDFPatternGroup("PG1")
                    .AddPattern(new RDFPattern(new RDFVariable("?S"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDFS.CLASS)));
            
            var result = query.ApplyToFederation(federation);

            Assert.AreEqual(1, result.DescribeResultsCount);
            Assert.AreEqual(1, result.DescribeResults.Rows.Count);
        }

        [TestMethod]
        public void ShouldApplyDescribeQueryToFederationAndHaveZeroResult()
        {
            RDFGraph graph = new RDFGraph();
            graph.AddTriple(new RDFTriple(new RDFResource("ex:flower"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDFS.DATATYPE));
            RDFFederation federation = new RDFFederation().AddGraph(graph);
            RDFDescribeQuery query = new RDFDescribeQuery()
                .AddPrefix(RDFNamespaceRegister.GetByPrefix(RDFVocabulary.RDF.PREFIX))
                .AddPatternGroup(new RDFPatternGroup("PG1")
                    .AddPattern(new RDFPattern(new RDFVariable("?S"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDFS.CLASS)));
            
            var result = query.ApplyToFederation(federation);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.DescribeResultsCount);
            Assert.AreEqual(0, result.DescribeResults.Rows.Count);
        }

        [TestMethod]
        public void ApplyToFederationShouldApplyDescribeQueryToNullFederationAndHaveZeroResult()
        {
            RDFDescribeQuery query = new RDFDescribeQuery()
                .AddPrefix(RDFNamespaceRegister.GetByPrefix(RDFVocabulary.RDF.PREFIX))
                .AddPatternGroup(new RDFPatternGroup("PG1")
                    .AddPattern(new RDFPattern(new RDFVariable("?S"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDFS.CLASS)));
            
            var result = query.ApplyToFederation(null);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.DescribeResultsCount);
            Assert.AreEqual(0, result.DescribeResults.Rows.Count);
        }

        [TestMethod]
        public void ApplyToSPARQLEndpointShouldReturnEmptyResultForNullEndpoint() 
        {
            RDFDescribeQuery query = new();
            RDFSPARQLEndpoint endpoint = null;
            RDFSPARQLEndpointQueryOptions options = new();

            var result = query.ApplyToSPARQLEndpoint(endpoint, options);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.DescribeResultsCount);
            Assert.AreEqual(0, result.DescribeResults.Rows.Count);
        }

        #endregion
    }
}
