import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { ArtikelOverzichtComponent } from './pages/artikel-overzicht/artikel-overzicht.component';
import { BestelgegevensInvulComponent } from './pages/bestelgegevens-invullen/bestelgegevens-invullen.component';
import { BestellingOverzichtComponent } from './pages/bestelling-overzicht/bestelling-overzicht.component';
import { WinkelwagenOverzichtComponent } from './components/winkelwagen-overzicht/winkelwagen-overzicht.component';
import { routes } from './app-routing.module';
import { ArtikelComponent } from './components/artikel/artikel.component';
import { AdresComponent } from './components/adres/adres.component';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { WinkelwagenOverzichtPageComponent } from './pages/winkelwagen-overzicht-page/winkelwagen-overzicht-page.component';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { RegistreerComponent } from './pages/registreer/registreer.component';
import { LoginCallbackComponent } from './pages/login-callback/login-callback.component';
import { ArtikelPageComponent } from './pages/artikel-page/artikel-page.component';
import { ZoekfilterPipe } from './pipes/zoekfilter.pipe';
import { AuthHttpInterceptor } from './interceptors/http-token.interceptor';
import { AppConfigProvider } from './providers/app-config-provider';
import { NotfoundComponent } from './pages/notfound/notfound.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    ArtikelOverzichtComponent,
    BestelgegevensInvulComponent,
    WinkelwagenOverzichtPageComponent,
    WinkelwagenOverzichtComponent,
    BestellingOverzichtComponent,
    ArtikelComponent,
    AdresComponent,
    RegistreerComponent,
    LoginCallbackComponent,
    ArtikelPageComponent,
    ZoekfilterPipe,
    NotfoundComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    RouterModule.forRoot(routes),
    ToastrModule.forRoot(),
    AngularFontAwesomeModule,
    FormsModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthHttpInterceptor,
      multi: true,
    },
    AppConfigProvider,
    {
      provide: APP_INITIALIZER,
      useFactory: (appConfigProvider: AppConfigProvider) => {
        return () => appConfigProvider.loadConfig();
      },
      multi: true,
      deps: [AppConfigProvider]
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
