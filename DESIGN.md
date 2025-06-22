# Designkeuzes EPDConsole

## Clean Architecture
Het project is opgezet volgens het Clean Architecture-principe. Dit zorgt voor een duidelijke scheiding tussen domein, infrastructuur, applicatielogica en presentatie. Hierdoor is de code onderhoudbaar, testbaar en eenvoudig uitbreidbaar.

- **Core**: bevat domein-entiteiten en interfaces.
- **Infrastructure**: bevat de database-implementatie (Entity Framework).
- **Application**: bevat businesslogica, CQRS-handlers en validatie.
- **EPDConsole**: bevat de gebruikersinterface (console).

## CQRS & MediatR
Voor alle mutaties en queries wordt het CQRS-patroon toegepast, ondersteund door MediatR. Commands en Queries worden afgehandeld door aparte handlers. Dit maakt de logica overzichtelijk en testbaar.

## Validatie met FluentValidation
Alle input wordt gevalideerd met FluentValidation. Validatieregels zijn per command gescheiden en worden automatisch uitgevoerd via een MediatR pipeline. Dit voorkomt duplicatie en zorgt voor consistente foutafhandeling.

## Dependency Injection
Alle afhankelijkheden worden via Dependency Injection aangeboden. Dit maakt het mogelijk om eenvoudig te testen met mocks en zorgt voor een flexibele, uitbreidbare architectuur.

## SOLID-principes
De code volgt de SOLID-principes:
- **Single Responsibility**: elke klasse heeft één duidelijke verantwoordelijkheid.
- **Open/Closed**: logica is uitbreidbaar via nieuwe handlers/validators zonder bestaande code te wijzigen.
- **Liskov Substitution**: alle implementaties van interfaces (zoals IApplicationDbContext) zijn uitwisselbaar en gedragen zich zoals verwacht, zowel in productie als in tests.
- **Interface Segregation**: interfaces zijn klein en doelgericht gehouden; er zijn geen onnodig brede interfaces. Voor deze schaal is IApplicationDbContext overzichtelijk en functioneel.
- **Dependency Inversion**: afhankelijkheden worden via interfaces aangeboden.

## Testen
De Application-laag is volledig afgedekt met unittests (xUnit, Moq, FluentAssertions). Hierdoor is regressie snel zichtbaar en blijft de code betrouwbaar bij refactoring.

## .NET 8 & Moderne Pakketten
Het project is geüpdatet naar .NET 8 en maakt gebruik van moderne, goed ondersteunde pakketten zoals MediatR, FluentValidation en Moq.

---

Deze keuzes zorgen samen voor een toekomstbestendige, onderhoudbare en goed geteste applicatie. 