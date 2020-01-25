import { Routes } from '@angular/router';
import { ArtikelOverzichtComponent } from './pages/artikel-overzicht/artikel-overzicht.component';
import { BestelgegevensInvulComponent } from './pages/bestelgegevens-invullen/bestelgegevens-invullen.component';
import { BestellingOverzichtComponent } from './pages/bestelling-overzicht/bestelling-overzicht.component';
import { WinkelwagenOverzichtPageComponent } from './pages/winkelwagen-overzicht-page/winkelwagen-overzicht-page.component';
import { RegistreerComponent } from './pages/registreer/registreer.component';
import { LoginCallbackComponent } from './pages/login-callback/login-callback.component';
import { OidcAuthGuard } from './guards/oidc-auth-guard.service';
import { ArtikelPageComponent } from './pages/artikel-page/artikel-page.component';
import { NotfoundComponent } from './pages/notfound/notfound.component';

export const routes: Routes = [
  {
    path: '',
    component: ArtikelOverzichtComponent
  },
  {
    path: 'registreer',
    component: RegistreerComponent,
  },
  {
    path: 'auth-callback',
    component: LoginCallbackComponent
  },
  {
    path: 'bestelgegevens-invoeren',
    component: BestelgegevensInvulComponent,
    canActivate: [OidcAuthGuard]
  },
  {
    path: 'bestel-overzicht',
    component: BestellingOverzichtComponent,
    canActivate: [OidcAuthGuard]
  },
  {
    path: 'winkelwagen',
    component: WinkelwagenOverzichtPageComponent
  },
  {
    path: 'artikel/:artikelId',
    component: ArtikelPageComponent,
  },
  {
    path: '**',
    component: NotfoundComponent,
  },
];
