# Continuous Integration

## Travis CI

A **Travis**t nem sikerült beüzemelni. Bár adminisztrátori jogot kaptam és a **Travis** webes felületén
megjelent a repository-nk, a szervezet negatív kreditegyenlege miatt a projekteket nem buildelte az eszköz.

![image](https://user-images.githubusercontent.com/46872866/169278245-75127340-479b-4d3e-926b-34fbd9b4b82d.png)

Hiába állítottam be megfelelően a _.travis.yml_ konfigurációs fájlt, a **Travis** dashboardján nem történt
változás. Ennek fényében egy másik folyamatos integrációt támogató rendszert választottam.

## GitHub Actions

A lemásolt projektben már szerepelt két **GitHub Actions** workflow, de ezek a tesztek futtatásán kívül
nem hajtottak végre más feladatokat. Kezdetben egy új workflow-t hoztam létre, ami az _RDFSharp_ library-t
egy **NuGet** package-be csomagolta össze. Később pedig egyesítettem az általam és a csapattársaim által
létrehozott workflow-kat az eredetiekkel.
