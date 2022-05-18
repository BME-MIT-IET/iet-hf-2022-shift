# BDD Tesztek
 A BDD (Behavior-Driven Development) a TDD (Test-Driven Development, vagyis Teszt-Vezérelt Fejlesztés) kiterjesztése ahol a user-story-kon van a hangsúly és a kódolás valós problémákra ad megoldást.
 A BDD nagy hangsúlyt fektet a csapat együttműködésére és a kereszt-funkcionális munkafolyamatokra. A sikeres BDD szerves részét képezi annak biztosítása, hogy a felhasználói történetek és a viselkedések az üzleti oldalról eljussanak a technikai oldalra.

 A teszteléshez Specflowt használtunk

 ## Features

A feature fájlokba írhatunk user-storykat, ezek a *Gherkin* nyelvet használják.
Itt scenariokat hozhatunk létre, ezek felelnek meg a tesztesetnek.
*Given* kulcsszó után adhatjuk meg a kezdeti állapotot, előfeltételeket.  
*And* kulcsszó után további előfeltételeket adhatunk meg, ezek a sorok egyenértékűek a *Given* sorokkal. A *When* kulcsszó után adhatjuk meg a tesztelendő eseményt.
A *Then* kulcsszó sorában adhatjuk meg a végállapotot.

Létrehozhatunk *Scenario Outline*-okat, amelyekben konkrét értékek helyére változókat írhatunk, és ezek értékeit az *Examples* kulcsszó után paraméterként megadhatjuk. Így ugyanazt a tesztet könnyen, több értékkel is végrehajthatjuk.

## Steps
A step fájlokban adhatjuk meg, az egyes Feature fájl sorokat a program hogy értelmezze.

## RDFGraph

Az *RDFGraph* osztály *Set*, *Add*, *Remove*, *Union* funkcióit teszteltük BDD tesztek segítségével