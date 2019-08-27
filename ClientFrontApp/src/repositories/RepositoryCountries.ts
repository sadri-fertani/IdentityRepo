import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

import { ConfigurationService } from "../services/configuration/configuration.service";

import { IRepositoryBase } from './IRepositoryBase';

import { ICountry } from '../models/ICountry';

@Injectable()
export class RepositoryCountries implements IRepositoryBase<ICountry> {

    private get Route(): string {
        return this.configuration.ApiAddress + 'pays';
    }

    constructor(private http: HttpClient, private configuration: ConfigurationService) {
    }

    findAll(): Observable<ICountry[]> {
        return this.http.get<ICountry[]>(this.Route);
    }

    create(entity: ICountry): Observable<ICountry> {
        throw new Error("not Implemented")
    }

    update(entity: ICountry): Observable<ICountry> {
        throw new Error("not Implemented")
    }

    find(id: number): Observable<ICountry> {
        return this.http.get<ICountry>(this.Route + `/${id}`);
    }
}