# Voorbesprekeing macedonieje

1. Hoe lang zijn onze sprints?
1 week (3 sprints in totaal)

2. Wie is de scrummaster?
_TODO_

3. Hebben we een PO-by-proxy nodig?
Er wordt verwacht dat de product owner genoeg bereikbaar is om te overleggen, om deze reden kiezen we ervoor geen PO-by-proxy te nemen.

4. Wat is onze Generic DoD?
- De mutation score van code moet minimaal 65% zijn, Log en hashcode methoden uitgezonderd
- De coverage van de testen moeten minmaal 80% zijn
- Het item voldoet aan de acceptatiecriteria
- Product Owner heeft het item geaccepteerd
- Er zijn unit testen geschreven die het item testen
- Alle acceptatie, unit en integratietesten zijn uitgevoerd en slagen
- De code is gereviewed door een ander teamlid dan degene die het geschreven heeft
- Code staat in de Git repository
- Indien nodig is het architectuurmodel aangepast

5. Wanneer vergaderen we?
'S ochtends een daily scrum, 9:10 uur. 15:45 daily standdown.

6. Gebruiken we planningpoker voor schattingen?
Er zal aan het begin van de sprint maximaal een uur besteed worden aan het uitrekenen wat de schattingen zijn voor
backlogitems.

7. Gebruiken we een digitaal scrumboard
Digitaal houden we een backlog bij, scrumboard zal alleen op papier komen. Scrumboard heeft alleen de huidige sprint.

8. Checklist voor code Reviews?
- Build slaagt
- Goede architectuur aangehouden
- Nieuwe code is testbaar en moeten getest zijn, operational logging hoeft niet getest te worden
- Code conventies aangehouden (casing, readonly, geen this)
- Testen hebben de naam van de methode, een underscore, en wat er getest wordt. Dus Initialize_CallsPublish bv.

9. Overige proceszaken

**Werktijden**
Iedereen is standaard om 9 uur aanwezig, van maandag tot donderdag.
Om 16:00 is de dag officieel afgelopen.
Lunchpauze is vanaf 12 uur tot 13 uur.

**Te laat aanwezig**
Wanneer het bekend is dat een lid vertraagd zal zijn moet deze dit aangeven in de Whatsapp groep, max. 15 minuten voor 9 uur.

**Code reviews**
Code review vindt plaats nadat er een feature branch gemerged is in de master branch en er een SonarQube analyse overheen
gegaan is. Alle _Tasks in QA_ van een dag moeten uiterlijk 15:30 nagekeken zijn.

**Hoe gaan we te werk met Git branches**
We gebruiken een master branch met feature branches.
We maken gebruik van pull requests.
Branches worden na een merge verwijderd.
Commitberichten hebben geen eisen.

**Retrospectives en reviews**
Aan het einde van de sprint vindt er een sprint review en sprint retrospective plaats waar de PO ook aanwezig
zal zijn.

10. Wie vindt wat leuk om te doen naast programmeren?
_Souffian_:
_Dirk-Jan_:
_Sebastiaan_:
_Ian_:
_Maarten_: Pipelines, Miffy en DevOps

11. Wie kan wat goed?
_Souffian_:
_Dirk-Jan_:
_Sebastiaan_:
_Ian_:
_Maarten_: Pipelines, Miffy en DevOps

12. Wat moeten we nog meer afstemmen over het team?

**Communicatie**:
Er is een Whatsapp groep aangemaakt om zowel binnen als buiten werkuren te communiceren binnen het team.

13. Doen we TDD?
Ja, we vinden het belangrijk dat alle functionaliteiten goed getest zijn om stabiele software te leveren.

14. Gebruiken we MoQ?
Ja.

15. Gebruiken we EF Core?
Ja.

16. Gebruiken we onze eigen bus-implementatie? (Hoe?)
We maken gebruik van het Miffy framework, dit is een abstractie op de C# RabbitMQ Client.
Iedereen in het team heeft toegang tot de source code van dit framework. Het framework
wordt via NuGet gedistributeerd.

17. Wat moeten we nog meer afstemmen over tools & frameworks?

We gaan gebruik maken van:
- Gherkin voor specificaties
- Specflow voor acceptatie testen
- Angular de frontend service(s)
- ASP.NET Core WebAPI voor backend service(s) en/of Miffy
- RabbitMQ als event bus met Miffy framework
- Stryker als mutation test framework, zonder naar Log en GetHashCode methoden te kijken.
- MSTest als unit test framework
- Git voor versiebeheer
- Azure DevOps voor pipelines, git repository, backlog
- Kubernetes als deployment framework
- Azure Kubernetescluster
- SonarQube voor het waarborgen van code kwaliteit
- Docker om docker images te bouwen
- Docker-compose om snel images te testen
- ElasticSearch, FluentD, Kibana voor operational logging

