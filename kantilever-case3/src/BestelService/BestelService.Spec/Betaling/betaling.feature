Feature: Automatische goedkeuring van bestellingen bij betaling
  Om automatisch ongekeurde bestelling goed te keuren, die automatisch goedgekeurd kunnen worden
  Wil ik als sales medewerker
  Dat wanneer ik een betaling doorvoor er wordt gekeken of de eerst volgende bestelling goedgekeurd kan worden

  
  Scenario Outline: Ik voer een betaling in
    Given Er een goedgekeurde bestelling is met een openstaand bedrag van:  <openstaand bedrag> 
    Given En een bestaande ongekeurde bestelling is met een openstaand bedrag van: <ongekeurde bestelling bedrag> 
    When Ik een betaling doorvoor van <betaling> voor deze bestelling 
    Then Moet de ongekeurde bestaande bestelling '<wel of niet>' goedgekeurd worden

    Examples:
    | openstaand bedrag | ongekeurde bestelling bedrag | betaling | wel of niet |
    | 100.00            | 499.00                       | 100.00   | wel         |
    | 99.99             | 499.99                       | 99.99    | wel         |
    | 100.00            | 499.99                       | 99.99    | wel         |
    | 100.00            | 500.00                       | 99.99    | niet        |
