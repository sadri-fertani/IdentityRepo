import { EnvService } from './env.service';

export interface IBrowserWindow extends Window {
    __env?: EnvService
}