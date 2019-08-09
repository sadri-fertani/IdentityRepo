import { EnvService } from './env.service';
import { EnvServiceFactory } from './env.service.factory';

export const EnvServiceProvider = {
    provide: EnvService,
    useFactory: EnvServiceFactory,
    deps: []
}