import { Injectable } from "@angular/core";
import { Observable, map } from "rxjs";
import { PersonAstronaut, PersonAstronautResponse } from "../models/PersonAstronaut";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { BaseResponse } from "../models/Response";
import { AstronautDutiesResponse } from "../models/AstronautDuty";

@Injectable({
    providedIn: 'root'
})
export class StargateService {
    private baseUrl: string;

    public constructor(private http: HttpClient) {
        this.baseUrl = "https://localhost:7204"
    }

    getAstronauts(): Observable<PersonAstronaut[]> {
        return this.http.get<PersonAstronautResponse>(`${this.baseUrl}/Person`).pipe(map(res => res.people));
    }

    addPerson(name: string): Observable<BaseResponse> {
        return this.http.post<BaseResponse>(
            `${this.baseUrl}/Person`,
            `"${name}"`,
            {
                headers: new HttpHeaders({ 
                    'Content-Type': 'text/json'
                })
            });
    }

    getDuties(name: string): Observable<AstronautDutiesResponse> {
        return this.http.get<AstronautDutiesResponse>(`${this.baseUrl}/AstronautDuty/${name}`);
    }

    addDuty(name: string, rank: string, dutyTitle: string, dutyStartDate: string): Observable<BaseResponse> {
        return this.http.post<BaseResponse>(
            `${this.baseUrl}/AstronautDuty`,
            {
                name, rank, dutyTitle, dutyStartDate
            }
        );
    }
}