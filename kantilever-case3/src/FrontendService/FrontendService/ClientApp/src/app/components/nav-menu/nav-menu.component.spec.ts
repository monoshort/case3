import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NavMenuComponent } from './nav-menu.component';
import { RouterTestingModule } from '@angular/router/testing';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { HttpClientTestingModule, } from '@angular/common/http/testing';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppConfigProvider } from '../../../app/providers/app-config-provider';

describe('NavMenuComponent', () => {
  let component: NavMenuComponent;
  let fixture: ComponentFixture<NavMenuComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [NavMenuComponent],
      imports: [
        RouterTestingModule,
        AngularFontAwesomeModule,
        HttpClientTestingModule,
        BrowserAnimationsModule,
        ToastrModule.forRoot()
      ],
      providers: [{ provide: 'BASE_URL', useValue: 'http://example.com' }, AppConfigProvider]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NavMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    // Assert
    expect(component).toBeTruthy();
  });

  it('should change isExpanded from true to false on toggle', () => {
    // Arrange
    component.isExpanded = true;

    // Act
    component.toggle();

    // Assert
    expect(component.isExpanded).toEqual(false);
  });

  it('should change isExpanded from false to true on toggle', () => {
    // Arrange
    component.isExpanded = false;

    // Act
    component.toggle();

    // Assert
    expect(component.isExpanded).toEqual(true);
  });
});
