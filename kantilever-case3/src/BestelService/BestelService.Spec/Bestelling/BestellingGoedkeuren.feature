Feature: Automatische goedkeuring van bestellingen bij plaatsing
  Om risico's te verminderen bij bestellingen
  Wil ik als sales medewerker
  Bestellingen die een subtotaal inclusief btw boven de 500.00 hebben handmatig goedkeuren

  Scenario Outline: Er wordt een bestelling geplaatst met een bepaald subtotaal
    Given Een bestelling van <subtotaal inclusief btw>
    When Deze geplaatst wordt
    Then Moet deze <wel of niet goedgekeurd> worden

    Examples:
    | subtotaal inclusief btw | wel of niet goedgekeurd       |
    | 499.00                   | wel automatisch goedgekeurd   |
    | 499.99                   | wel automatisch goedgekeurd   |
    | 500.00                   | wel automatisch goedgekeurd  |
    | 500.99                   | niet automatisch goedgekeurd  |

  Scenario Outline: Er worden twee bestellingen geplaatst met een bepaald subtotaal
    Given Een bestelling van een klant met een subtotaal inclusief btw van <subtotaal inclusief btw 1>
    And Een bestelling van dezelfde klant met een subtotaal inclusief btw van <subtotaal inclusief btw 2>
    When Deze achter elkaar geplaatst worden
    Then Moet bestelling een <wel of niet goedgekeurd 1> worden en bestelling twee <wel of niet goedgekeurd 2> worden

    Examples:
    | subtotaal inclusief btw 1 | subtotaal inclusief btw 2 | wel of niet goedgekeurd 1   | wel of niet goedgekeurd 2     |
    | 200.00                     | 200.00                      | wel automatisch goedgekeurd | wel automatisch goedgekeurd   |
    | 400.00                     | 200.00                      | wel automatisch goedgekeurd | niet automatisch goedgekeurd  |
    | 500.00                     | 200.00                      | wel automatisch goedgekeurd | niet automatisch goedgekeurd |
    | 500.00                     | 500.00                      | wel automatisch goedgekeurd | niet automatisch goedgekeurd |
    | 300.00                     | 200.00                      | wel automatisch goedgekeurd | wel automatisch goedgekeurd |
    | 300.00                     | 200.01                      | wel automatisch goedgekeurd | niet automatisch goedgekeurd |
