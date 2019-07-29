import { ISpeaker } from "./ISpeaker";

export interface ITalk {
    talkId: number;
    title: string;
    abstract: string;
    level: string;
    speaker: ISpeaker;
}