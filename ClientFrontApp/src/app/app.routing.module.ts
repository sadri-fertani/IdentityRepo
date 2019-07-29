import { Routes, RouterModule } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';

import { HomeComponent } from './home/home.component';
import { PageNotFoundComponent } from './page.not.found.component';
import { CampsComponent } from './camps/camps.component';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';

export const routes: Routes = [
    {
        path: 'home',
        component: HomeComponent
    },
    {
        path: 'camps',
        component: CampsComponent
    },
    {
        path: 'unauthorized',
        component: UnauthorizedComponent
    },
    {
        path: '',
        redirectTo: '/home',
        pathMatch: 'full'
    },
    { 
        path: '**', 
        component: PageNotFoundComponent 
    }
];

export const RoutingModule: ModuleWithProviders = RouterModule.forRoot(routes, { enableTracing: false });