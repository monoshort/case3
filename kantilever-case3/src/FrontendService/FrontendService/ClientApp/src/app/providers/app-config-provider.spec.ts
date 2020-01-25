import { async, TestBed, getTestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AppConfigProvider, AppConfig } from './app-config-provider';
import { Subject } from 'rxjs';

describe('ArtikelenDataService', () => {
    let httpTestingController: HttpTestingController;
    let injector: TestBed;
    let service: AppConfigProvider;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [
                RouterTestingModule,
                ReactiveFormsModule,
                HttpClientTestingModule,

            ],
            declarations: [],
            providers: [
                AppConfigProvider,
                {
                    provide: 'BASE_URL',
                    useValue: 'http://localhost:5000/'
                },
                AppConfigProvider
            ]
        });
    }));

    beforeEach(() => {
        injector = getTestBed();
        httpTestingController = injector.get(HttpTestingController);
        service = injector.get(AppConfigProvider);
    });

    it('should be able to make a call to http://localhost:5000/api/config', () => {
        // Act
        service.loadConfig();

        // Assert
        const req = httpTestingController.expectOne(`api/config`);

        req.flush([]);
    });

    it('should be able to get the config ', () => {
        // Arrange
        service.$config = new Subject<AppConfig>();

        // Act
        service.$config.next({ angular_authority: 'test' } as AppConfig);
        const config = service.getConfig();

        // Assert
        config.subscribe(data => {
            expect(data.angular_authority).toBe('test');
        });
    });
});
