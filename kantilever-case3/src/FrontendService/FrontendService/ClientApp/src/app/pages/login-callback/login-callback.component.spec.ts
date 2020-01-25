import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginCallbackComponent } from './login-callback.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ToastrModule } from 'ngx-toastr';
import { AuthService } from '../../services/auth.service';
import { AppConfigProvider } from '../../../app/providers/app-config-provider';

describe('LoginCallbackComponent', () => {
  let component: LoginCallbackComponent;
  let fixture: ComponentFixture<LoginCallbackComponent>;
  let authService: AuthService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [LoginCallbackComponent],
      imports: [
        HttpClientTestingModule,
        RouterTestingModule,
        ToastrModule.forRoot()
      ],
      providers: [
        AuthService,
        { provide: 'BASE_URL', useValue: 'http://1.2.3.4:1234' },
        AppConfigProvider
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginCallbackComponent);
    component = fixture.componentInstance;

    authService = TestBed.get(AuthService);

    fixture.detectChanges();
  });

  it('should create', () => {
    // Assert
    expect(component).toBeTruthy();
  });

  it('should call the login method of the Authservice onInit', (done) => {
    // Arrange
    const spy = spyOn(authService, 'completeLogin');

    // Act
    component.ngOnInit();

    // Assert
    setTimeout(() => {
      expect(spy).toHaveBeenCalled();
      done();
    }, 150);
  });
});
