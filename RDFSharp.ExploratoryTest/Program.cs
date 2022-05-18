using RDFSharp.Model;
using RDFSharp.Query;

var graph = new RDFGraph().SetContext(new Uri("ex:DataGraph"));

graph.AddTriple(new RDFTriple(new RDFResource("ex:Man"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
graph.AddTriple(new RDFTriple(new RDFResource("ex:Woman"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
graph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Woman")));
graph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Man")));
graph.AddTriple(new RDFTriple(new RDFResource("ex:Steve"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Man")));
graph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Bob")));
graph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.FOAF.AGENT, new RDFResource("ex:Alice")));
graph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Alice")));
graph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.FOAF.AGENT, new RDFResource("ex:Bob")));
graph.AddTriple(new RDFTriple(new RDFResource("ex:Steve"), RDFVocabulary.FOAF.AGENT, new RDFResource("ex:Steve")));
graph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), new RDFResource("http://xmlns.com/foaf/0.1/age"), new RDFResource("ex:85")));
graph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), new RDFResource("http://xmlns.com/foaf/0.1/age"), new RDFResource("ex:55")));
graph.AddTriple(new RDFTriple(new RDFResource("ex:Steve"), new RDFResource("http://xmlns.com/foaf/0.1/age"), new RDFResource("ex:49")));

var ages = new RDFResource("http://xmlns.com/foaf/0.1/age");

RDFVariable age = new RDFVariable("age");
RDFVariable name = new RDFVariable("name");
RDFSelectQuery query = new RDFSelectQuery()
 .AddPrefix(RDFNamespaceRegister.GetByPrefix("dc"))
 .AddPrefix(RDFNamespaceRegister.GetByPrefix("foaf"))
 .AddPatternGroup(new RDFPatternGroup("PG1")
 .AddPattern(new RDFPattern(name, ages, age))
 .AddPattern(new RDFPattern(name, RDFVocabulary.RDF.TYPE, new RDFResource("ex:Man"))))
 .AddModifier(new RDFOrderByModifier(name, RDFQueryEnums.RDFOrderByFlavors.ASC))
 .AddModifier(new RDFLimitModifier(5))
 .AddProjectionVariable(name)
 .AddProjectionVariable(age);

RDFSelectQueryResult selectResult = query.ApplyToGraph(graph);
selectResult.ToSparqlXmlResult(Console.OpenStandardOutput());

RDFVariable alice = new RDFVariable("alice");
RDFVariable person = new RDFVariable("person");
RDFSelectQuery query2 = new RDFSelectQuery()
 .AddPrefix(RDFNamespaceRegister.GetByPrefix("dc"))
 .AddPrefix(RDFNamespaceRegister.GetByPrefix("foaf"))
 .AddPatternGroup(new RDFPatternGroup("PG1")
 .AddPattern(new RDFPattern(alice, RDFVocabulary.FOAF.AGENT, new RDFResource("ex:Alice")))
 .AddPattern(new RDFPattern(alice, RDFVocabulary.FOAF.KNOWS, person)))
 .AddModifier(new RDFOrderByModifier(person, RDFQueryEnums.RDFOrderByFlavors.ASC))
 .AddModifier(new RDFLimitModifier(5))
 .AddProjectionVariable(alice)
 .AddProjectionVariable(person);

RDFSelectQueryResult selectResult2 = query2.ApplyToGraph(graph);
selectResult2.ToSparqlXmlResult(Console.OpenStandardOutput());

RDFVariable steve = new RDFVariable("steve");
RDFVariable p = new RDFVariable("person");
RDFSelectQuery query3 = new RDFSelectQuery()
 .AddPrefix(RDFNamespaceRegister.GetByPrefix("dc"))
 .AddPrefix(RDFNamespaceRegister.GetByPrefix("foaf"))
 .AddPatternGroup(new RDFPatternGroup("PG1")
 .AddPattern(new RDFPattern(steve, RDFVocabulary.FOAF.AGENT, new RDFResource("ex:Steve")))
 .AddPattern(new RDFPattern(steve, RDFVocabulary.FOAF.KNOWS, p)))
 .AddModifier(new RDFOrderByModifier(p, RDFQueryEnums.RDFOrderByFlavors.ASC))
 .AddModifier(new RDFLimitModifier(5))
 .AddProjectionVariable(steve)
 .AddProjectionVariable(p);

RDFSelectQueryResult selectResult3 = query3.ApplyToGraph(graph);
selectResult3.ToSparqlXmlResult(Console.OpenStandardOutput());