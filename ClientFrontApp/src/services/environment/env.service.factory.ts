import { EnvService } from './env.service';

export const EnvServiceFactory = () => {

    const env = new EnvService();

    env.apiUrl = window.location.href;

    if (env.apiUrl.indexOf('localhost:5002') !== -1) {
        // Dev environment
        env.enableDebug = true;
        env.envName = 'dev';
        env.clientId = 'ng';
        env.apiAddress = 'http://localhost:5001/api/';
        env.identityServerAddress = 'http://localhost/IdentityServer';
    } else {
        // Production environment
        env.enableDebug = false;
        env.envName = 'prd';
        env.clientId = 'ngProd';
        env.apiAddress = 'http://localhost/HomeAPI/';
        env.identityServerAddress = 'http://localhost/IdentityServer';
    }

    return env;
};