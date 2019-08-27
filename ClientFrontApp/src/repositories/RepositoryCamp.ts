import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

import { ConfigurationService } from "../services/configuration/configuration.service";

import { IRepositoryBase } from './IRepositoryBase';

import { ICamp } from '../models/ICamp';

@Injectable()
export class RepositoryCamp implements IRepositoryBase<ICamp> {

    private get Route(): string {
        return this.configuration.ApiAddress + 'camps';
    }

    constructor(private http: HttpClient, private configuration: ConfigurationService) {
    }

    findAll(): Observable<ICamp[]> {
        return this.http.get<ICamp[]>(this.Route);
    }

    create(entity: ICamp): Observable<ICamp> {
        return this.http.put<any>(this.Route, entity);
    }

    update(entity: ICamp): Observable<ICamp> {
        return this.http.put<any>(this.Route + `/${entity.moniker}`, entity);
    }

    find(id: string): Observable<ICamp> {
        return this.http.get<ICamp>(this.Route + `/${id}`);
    }
}