import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class EnvService {
    public enableDebug = false;
    public envName = '';
    public clientId = '';
    public apiAddress = '';
    public appAddress = '';
    public identityServerAddress = '';
}