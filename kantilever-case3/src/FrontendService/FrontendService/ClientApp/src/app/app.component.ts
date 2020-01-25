import { Component, OnInit } from '@angular/core';
import { BestelService } from './services/bestel.service';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {

  constructor(
    private readonly _authService: AuthService,
    private readonly _bestelService: BestelService) { }

  ngOnInit(): void {
    if (this._authService.isLoggedIn) {
      this._bestelService.alleBestellingenVanKlant(this._authService.username).subscribe(
        result => {
          this._bestelService.laatMeldingZienVanNietAutomatischGoedgekeurdeBestellingen(this._authService.username);
        }
      );
    }

  }

}
