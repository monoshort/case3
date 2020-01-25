import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../app/services/auth.service';
import { systemVariables } from '../../../constants/system-variables';

@Component({
  selector: 'app-login-callback',
  template: ''
})
export class LoginCallbackComponent implements OnInit {

  constructor(private readonly _authService: AuthService) { }

  ngOnInit() {
    // to give the browser time to parse the URL
    setTimeout(() => {
      this._authService.completeLogin();
    }, systemVariables.loginCallbackTimeout);
  }

}
