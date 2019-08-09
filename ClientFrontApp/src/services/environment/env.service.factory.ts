import { IBrowserWindow } from './IBrowserWindow';

export const EnvServiceFactory = () => {
    const browserWindow: IBrowserWindow = window;

    return browserWindow.hasOwnProperty('__env') ? browserWindow.__env : null;
};