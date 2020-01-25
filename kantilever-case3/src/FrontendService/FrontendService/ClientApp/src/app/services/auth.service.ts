import { Inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { User, UserManager, UserManagerSettings } from 'oidc-client';
import { RegistreerModel } from '../../app/models/registreerModel';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { AppConfigProvider } from '../../app/providers/app-config-provider';
import { endpoints } from '../../constants/endpoints';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  get username() {
    return this.user ? this.user.profile.preferred_username : null;
  }

  get token() {
    return this.user ? this.user.access_token : null;
  }

  get tokenType() {
    return this.user ? this.user.token_type : null;
  }

  get isLoggedIn(): boolean {
    return this.user !== null && !this.user.expired;
  }

  constructor(
    private readonly _http: HttpClient,
    private readonly _router: Router,
    @Inject('BASE_URL') private readonly _baseUrl: string,
    private readonly _toastrService: ToastrService,
    private readonly _appConfigProvider: AppConfigProvider) {

    this._buildUserSettings();

    if (AuthService._loadUserFromStorage()) {
      this.user = AuthService._loadUserFromStorage();
    }
  }

  private _userManager: UserManager;
  public user: User = null;

  private static _loadUserFromStorage(): User {
    return localStorage.getItem('user') ? User.fromStorageString(localStorage.getItem('user')) : null;
  }

  private static _clearUserFromStorage() {
    localStorage.removeItem('user');
  }

  private _buildUserSettings() {
    this._appConfigProvider.getConfig().subscribe(appconfig => {
      const settings: UserManagerSettings = {
        client_id: appconfig.angular_clientid,
        authority: appconfig.angular_authority,
        redirect_uri: appconfig.angular_redirect_uri,
        post_logout_redirect_uri: appconfig.angular_post_logout_redirect_uri,
        response_type: appconfig.angular_response_type,
        scope: appconfig.angular_scope,
      };
      this._userManager = new UserManager(settings);
    });

  }

  public async login(): Promise<void> {
    await this._userManager.signinRedirect();
  }

  public async logout(): Promise<void> {
    await this._userManager.signoutRedirect();
    this.user = undefined;
    AuthService._clearUserFromStorage();
  }

  public async completeLogin(): Promise<void> {
    try {
      this.user = await this._userManager.signinRedirectCallback();
    } finally {
      await this._router.navigate(['']);
    }
    this._saveUserToStorage();
    this.isLoggedIn ? this._loginSuccesToast() : this._loginErrorToast();
  }

  public registreer(model: RegistreerModel): Observable<Object> {
    return this._http.post(`${this._baseUrl}${endpoints.registreren}`, model);
  }

  private _saveUserToStorage(): void {
    if (this.user !== null) {
      localStorage.setItem('user', this.user.toStorageString());
    }
  }

  private _loginSuccesToast(): void {
    this._toastrService.success('Succesvol ingelogd!');
  }

  private _loginErrorToast(): void {
    this._toastrService.error('Er ging iets fout bij het inloggen!');
  }
}
