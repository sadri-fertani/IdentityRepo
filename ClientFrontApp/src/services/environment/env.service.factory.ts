import { EnvService } from './env.service';

export const EnvServiceFactory = () => {

    const env = new EnvService();

    if (window.location.href.indexOf('localhost:4200') !== -1) {
        // Dev environment
        env.enableDebug = true;
        env.envName = 'dev';
        env.clientId = 'ng_client_1';
        env.appAddress = 'http://localhost:4200/';
    } else {
        // Production environment
        env.enableDebug = false;
        env.envName = 'prd';
        env.clientId = 'ng_client_prod_1';        
        env.appAddress = 'http://homeserver/HomeApp/';        
    }

    env.apiAddress = 'http://homeserver/HomeAPI/api/';
    env.identityServerAddress = 'http://homeserver/IdentityServer';

    return env;
};