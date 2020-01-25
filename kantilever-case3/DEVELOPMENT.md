# Notes for the development team of the application

## Test DataRows

While researching the application, you might stumble across a lot of [DataRow] annotations that might not seem
all that useful or add anything to the test. These annotations are put in place because of our belief that if you
work semi-testdriven, all strings, ints and values an be hardcoded to 'cheat' tests. Given the following function:

`sum(int numberA, int numberB) => numberA + numberB`

This function can be tested using the test:

`Assert.AreEqual(10, sum(5,5))`

However, it is also possible to 'cheat' this test by rewriting the function's body to `=> 10`.
By adding several datarows, or in most cases 2, we make sure there is no hardcoded data in our methods
by throwing at last 2 datasets at it. For this reason you'll find a lot of datarows at places that
you wouldn't expect. For example, the GetById function with different ids and the Add function with 2 different values.numberA

## Test Coverage

For the coverage of this project we've decided to rely on coverlet for the calculations and SonarQube for reporting.
You'll also find quite a lot of [ExcludeFromCodeCoverage] attributes scattered around model classes and components
we've decided not to test.

We'd also like to ask you to calculate coverage without including the simple models and [ExcludeFromCodeCoverage] classes.

## Mutation testing

We use Stryker for mutation testing, each unit/component test project contains a config file for stryker that's used to
setup a failure threshold and files that should be excluded.

We used to have a stryker step in our pipeline, however this caused the pipelines to become extremely slow and on
release day we decided to disable this step permantently in the pipelines. It simply took more than an hour
to run through all tests in a simple service.

## UI Tests

This project contains two public-facing interfaces that are tested through UI and integration tests. The frontend is
tested through Angular e2e tests and the backoffice is tested through C# integration tests.

### HTTP commands and RPC

The existing services utilize HTTP requests to handle commands, however we found it much more convenient to use the
command functionality delivered by the Miffy library that one of us created. For this reason all communicating towards
the existing services are through HTTP requests and the internal services use commands over the message broker.

Obviously most of the communication with other services is abstracted away in agents using Dependency Inversion, so
HTTP and AMQP communication could be swapped if needed without much impact on the rest of the system.

### Lazy and Eager loading

Since most services communicate using JSON either through an API or a message broker, we decided it'd be best to just
eagerly load all the data from the database so that serialization does not need more roundtrips to the database than
required.

## System functionality

- Artikelen bestellen
- Artikelen aan winkelwagen toevoegen
- Aantal artikelen in winkelwagen aanpassen
- Artikelen uit winkelwagen halen
- Op artikelen zoeken
- Artikel details bekijken
- Gegevens automatisch invullen afleveradres
- Besteloverzicht bekijken
- Melding krijgen als bestelling niet automatisch goedgekeurd is
- Als klant inloggen
- Als klant registreren

- Factuur uitprinten
- Adreslabel uitprinten
- Bestelling klaarmelden
- Bestelregel afvinken
- Inloggen als magazijnmedewerker
- Eerstvolgende bestelling bekijken

- Wanbetalers inzien
- Betalingen registreren
- Bestelling goedkeuren
- Bestelling afkeuren
- Bestelling details inzien
- Bij te bestellen artikelen inzien
- Inloggen als sales medewerker

## Architecture

All architecture information can be found in the README.md file.
