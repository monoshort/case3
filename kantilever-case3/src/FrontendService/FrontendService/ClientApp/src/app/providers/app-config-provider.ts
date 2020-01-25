import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Subject } from 'rxjs';

export interface AppConfig {
    angular_authority: string;
    angular_clientid: string;
    angular_response_type: string;
    angular_redirect_uri: string;
    angular_scope: string;
    angular_post_logout_redirect_uri: string;
}

@Injectable()
export class AppConfigProvider {

    public $config: Subject<AppConfig> = new Subject<AppConfig>();

    constructor(
        private readonly httpClient: HttpClient
    ) { }

    loadConfig() {
        return this.httpClient.get<AppConfig>('api/config').toPromise().then(config => {
            this.$config.next(config);
        });
    }

    getConfig(): Subject<AppConfig> {
        return this.$config;
    }
}
