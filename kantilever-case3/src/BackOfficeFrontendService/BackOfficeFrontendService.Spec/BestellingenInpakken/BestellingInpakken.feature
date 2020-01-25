Feature: Het eerstvolgende bestellingoverzicht van magazijnmedewerkers
  Om te zien welk artikel ik in moet pakken
  wil ik als magazijnmedewerker
  de eerstvolgende bestelling bekijken

Scenario: Een goedgekeurde bestelling die nog niet ingepakt is, is te zien op het scherm
  Given Een goedgekeurde bestelling die nog niet ingepakt is
  When Ik de eerstvolgende bestelling pagina open
  Then Wil ik de eerstvolgende bestelling zien

Scenario: Er zijn geen goedgekeurde nog-in-te-pakken bestellingen
  Given Geen goedgekeurde nog-in-te-pakken bestellingen
  When Ik de eerstvolgende bestelling pagina open
  Then Zie ik een melding dat er geen bestellingen zijn

Scenario: Een nog niet goedgekeurde bestelling die nog niet ingepakt is, is niet te zien op het scherm
  Given Een niet goedgekeurde bestelling die nog niet ingepakt is
  When Ik de eerstvolgende bestelling pagina open
  Then Zie ik een melding dat er geen bestellingen zijn

