import { Component, OnInit, inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from "@angular/material/dialog";
import { StargateService } from "../../services/stargate.service";
import { Observable, map } from "rxjs";
import { AstronautDuty } from "../../models/AstronautDuty";
import { CommonModule } from "@angular/common";
import { MatListModule } from "@angular/material/list";
import { MatIconModule } from "@angular/material/icon";

@Component({
    selector: 'app-view-duty-history',
    templateUrl: './view-duty-history.component.html',
    standalone: true,
    imports: [MatDialogModule, CommonModule, MatListModule, MatIconModule],
    providers: [StargateService]
})
export class ViewDutyHistoryComponent implements OnInit {
    name = inject<string>(MAT_DIALOG_DATA);

    constructor(private stargateService: StargateService) {}

    astronautDuties$: Observable<AstronautDuty[]> = new Observable<AstronautDuty[]>();

    ngOnInit(): void {
        this.astronautDuties$ = this.stargateService.getDuties(this.name)
            .pipe(map(response => response.astronautDuties))
    }
}