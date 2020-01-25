Feature: Verzendkosten worden over het subtotaal berekend
  Om te zien hoeveel verzendkosten bij het subtotaal van een bestelling komen
  Wil ik als directeur
  Dat bij het subtotaal de verzendkosten berekend worden met en zonder btw

  Scenario Outline: Er komt een bestelling binnen met een subtotaal
    Given Een bestelling met een subtotaal van <subtotaal>
    When Ik het subtotaal met verzendkosten inclusief en exclusief bereken
    Then Zou het subtotaal met verzendkosten <subtotaal met verzendkosten> moeten zijn
    And Zou het subtotaal exclusief btw met verzendkosten <subtotaal exclusief btw met verzendkosten> moeten zijn

    Examples:
    | subtotaal | subtotaal met verzendkosten | subtotaal exclusief btw met verzendkosten |
    | 1.21      | 7.21                          | 5.96                                        |
    | 2.42      | 8.42                          | 6.96                                        |
    | 12.10     | 18.10                         | 14.96                                       |
    | 24.20     | 30.20                         | 24.96                                       |
