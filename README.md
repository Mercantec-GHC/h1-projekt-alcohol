# Markedsplads C2C Projekt

## Overblik
Dette projekt er en C2C (Consumer-to-Consumer) markedsplads bygget med Blazor, hvor brugere kan købe og sælge produkter direkte til hinanden.

## Projekt forklaring
Hjemmesiden indledes med en alderskontrol, som skal registrere, om brugeren er over eller under 18. Dette lille pop-up-vindue vil fortsat vise sig, hver gang man åbner frosiden.

Årsagen til det, er fordi vi ikke vil have at programmet skal gemme brugerens svar, da vi gerne vil anvende funktion til vores forklaring. Vi ved, hvordan koden kan ændres, så alderskontrollen
kan gemmer brugeres svar. Dog, er det et valg, ikke at implementere funktion, indtil videre.
____________________________________________________
Hvis man bliver omdirigeret til loginsiden, når man forsøger at få adgang til Annonce, Opret en Annonce og Profil siden, skyldes det, at man skal være logget ind. Hvis man ikke har en profil, skal man registerer sig først.
___________________________________________________
For at købe et produkt skal man kontakte sælgerens personlige telefonnummer, som helst bør være angivet ved produktet i indkøbslisten

### Installation
1. Klon repository'et

2. Åbn projektet i Visual Studio eller VS Code

3. Kør projektet
```
dotnet run
```

## Projektstruktur
- `Blazor-Markedsplads/` - Hovedprojektet
- `Dokumentation/` - Indeholder diagrammer, mockups og andre dokumenter
  - `Database/` - Database-diagrammer og -design
  - `UI/` - UI/UX mockups og wireframes
  - `API/` - API-dokumentation

## Bidrag
Følg disse trin for at bidrage til projektet:
1. Fork repository'et
2. Opret en feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit dine ændringer (`git commit -m 'Add some AmazingFeature'`)
4. Push til branch'en (`git push origin feature/AmazingFeature`)
5. Åbn en Pull Request 
