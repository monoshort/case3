import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NotfoundComponent } from './notfound.component';
import { AngularFontAwesomeModule } from 'angular-font-awesome';

describe('NotfoundComponent', () => {
  let component: NotfoundComponent;
  let fixture: ComponentFixture<NotfoundComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        NotfoundComponent
      ],
      imports: [
        AngularFontAwesomeModule
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    // Arrange
    fixture = TestBed.createComponent(NotfoundComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    // Assert
    expect(component).toBeTruthy();
  });
});
