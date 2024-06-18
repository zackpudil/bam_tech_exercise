import { BaseResponse } from "./Response";

export interface PersonAstronautResponse extends BaseResponse {
    people: PersonAstronaut[]
};

export interface PersonAstronaut {
    personId: number;
    name: string;
    currentRank?: string;
    currentDutyTitle?: string;
    careerStartDate?: string;
    careerEndDate?: string;
}