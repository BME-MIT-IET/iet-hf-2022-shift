Feature: Graph
	Testing the features of graphs.

@Graph
Scenario: Setting the context of the graph
	Given an empty graph
	And the uri is http://example2.org/
	When the context is set
	Then the context should be http://example2.org/

Scenario: Not setting the context of the graph because of blank node
	Given an empty graph
	And the uri is bnode:1234567
	When the context is set
	Then the context should not be bnode:1234567

Scenario: Comparing empty graphs
	Given an empty graph
	And another empty graph
	When we compare them
	Then they should be equal

Scenario: Comparing an empty graph to null
	Given an empty graph
	When we compare it to null
	Then they should not be equal

Scenario: Getting the graph's string representation
	Given an empty graph
	And the uri is http://example2.org/
	When the context is set
	Then the string representation should be http://example2.org/

Scenario: Getting the graph's string representation when the context is not set
	Given an empty graph
	And the uri is bnode:1234567
	When the context is set
	Then the string representation should be default

Scenario: Adding a triple
	Given an empty graph
	When we add a triple with the values http://subj/, http://pred/, http://obj/
	Then the graph should have 1 triple

Scenario: Trying to add two triples
	Given an empty graph
	And we add a triple with the values http://subj/, http://pred/, http://obj/
	When we add a triple with the values http://a/, http://b/, http://c/
	Then the graph should have 2 triple

Scenario: Trying to add duplicate triples
	Given an empty graph
	And we add a triple with the values http://subj/, http://pred/, http://obj/
	When we add a triple with the values http://subj/, http://pred/, http://obj/
	Then the graph should have 1 triple

Scenario: Union
	Given an empty graph
	And we add a triple with the values http://subj/, http://pred/, http://obj/
	And we add a triple with the values http://subj2/, http://pred2/, http://obj2/
	And another empty graph
	And we add to the second graph a triple with the values http://subj3/, http://pred3/, http://obj3/ 
	And we add to the second graph a triple with the values http://subj4/, http://pred4/, http://obj4/ 
	When we union them
	Then the union should have 4 triple

Scenario: Union with duplicates
	Given an empty graph
	And we add a triple with the values http://subj/, http://pred/, http://obj/
	And we add a triple with the values http://subj2/, http://pred2/, http://obj2/
	And another empty graph
	And we add to the second graph a triple with the values http://subj3/, http://pred3/, http://obj3/ 
	And we add to the second graph a triple with the values http://subj2/, http://pred2/, http://obj2/ 
	When we union them
	Then the union should have 3 triple

Scenario: Removing a triple by subject
	Given an empty graph
	And we add a triple with the values http://subj/, http://pred/, http://obj/
	And we add a triple with the values http://subj2/, http://pred2/, http://obj2/
	When we try to remove a triple with the subject http://subj/
	Then the graph should have 1 triple

Scenario: Trying to remove a triple by subject that the graph doesn't have
	Given an empty graph
	And we add a triple with the values http://subj/, http://pred/, http://obj/
	And we add a triple with the values http://subj2/, http://pred2/, http://obj2/
	When we try to remove a triple with the subject http://subj3/
	Then the graph should have 2 triple



Scenario Outline: Removing by predicate
	Given an empty graph
	And we add a triple with the values http://subj/, http://pred/, http://obj/
	And we add a triple with the values http://subj2/, http://pred2/, http://obj2/
	When we try to remove a triple with the predicate <predicate>
	Then the graph should have <amount> triple

	Examples: 
	| predicate     | amount |
	| http://pred/  | 1      |
	| http://pred3/ | 2      |


Scenario Outline: Removing by object
	Given an empty graph
	And we add a triple with the values http://subj/, http://pred/, http://obj/
	And we add a triple with the values http://subj2/, http://pred2/, http://obj2/
	When we try to remove a triple with the object <object>
	Then the graph should have <amount> triple

	Examples: 
	| object       | amount |
	| http://obj/  | 1      |
	| http://obj3/ | 2      |