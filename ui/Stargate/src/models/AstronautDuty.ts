import { PersonAstronaut } from "./PersonAstronaut";
import { BaseResponse } from "./Response";

export interface AstronautDutiesResponse extends BaseResponse {
    person: PersonAstronaut;
    astronautDuties: AstronautDuty[]
};

export interface AstronautDuty {
    rank: string;
    dutyTitle: string;
    dutyStartDate: string;
    dutyEndDate?: string;
}