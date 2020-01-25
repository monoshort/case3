Feature: Als een niet automatisch goedgekeurde bestelling langer dan 30 dagen niet betaald is moet deze gemarkeerd worden als wanbetalers
  Om duidelijk te zien welke bestellingen nog niet betaald zijn na 30 dagen
  Wil ik als Sales medewerker
  Dat de bestelling gemarkeerd wordt als bestelling met wanbetaler

  Scenario Outline: Een bestelling wordt geplaatst op een bepaalde datum en wordt niet betaald gedurende een aantal dagen
    Given Een niet automatisch goedgekeurde xrandbestelling die <aantal> dagen geleden is geplaatst
    When Er opgevraagd wordt of dit een bestelling met wanbetaler betreft
    Then Moet de bestelling <wel of niet gemarkeerd worden als wanbetaler>

    Examples:
    | aantal | wel of niet gemarkeerd worden als wanbetaler  |
    | 0      | niet gemarkeerd worden als wanbetaler         |
    | 30     | niet gemarkeerd worden als wanbetaler         |
    | 31     | wel gemarkeerd worden als wanbetaler          |
    | 32     | wel gemarkeerd worden als wanbetaler          |
