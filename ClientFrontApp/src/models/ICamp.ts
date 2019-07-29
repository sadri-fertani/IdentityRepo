import { ITalk } from "./ITalk";

export interface ICamp {
    name: string;
    moniker: string;
    eventDate: Date;
    length: number;
    venue: string;
    locationAddress1: string;
    locationAddress2: string;
    locationAddress3: string;
    locationCityTown: string;
    locationStateProvince: string;
    locationPostalCode: string;
    locationCountry: string;
    talks:Array<ITalk>;
}
