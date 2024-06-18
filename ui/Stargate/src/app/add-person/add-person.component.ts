import { Component, inject } from "@angular/core";
import { MatButtonModule } from "@angular/material/button";
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from "@angular/material/dialog";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { StargateService } from "../../services/stargate.service";
import { FormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { switchMap } from "rxjs";
import { provideNativeDateAdapter } from "@angular/material/core";
import { AddPersonDialogData } from "../../models/Response";

@Component({
    selector: 'app-add-person-dialog',
    templateUrl: './add-person.component.html',
    standalone: true,
    imports: [MatButtonModule, MatDialogModule, MatFormFieldModule, MatInputModule, FormsModule, CommonModule, MatDatepickerModule],
    providers: [StargateService, provideNativeDateAdapter()]
})
export class AddPersonDialogComponent {
    data = inject<AddPersonDialogData>(MAT_DIALOG_DATA);
    constructor(private stargateService: StargateService, private dialog: MatDialogRef<AddPersonDialogComponent>) { }

    name: string = "";
    rank: string = "";
    duty: string = "";
    startDate: string = "";

    error: string = "";

    addPerson() {
        if (this.data.isNew) {
            this.stargateService
                .addPerson(this.name)
                .pipe(
                    switchMap(() => this.stargateService.addDuty(this.name, this.rank, this.duty, this.startDate))
                )
                .subscribe({
                    complete: () => this.dialog.close(),
                    error: ({ error }) => this.error = error.message
                });
        } else {
            this.stargateService
                .addDuty(this.data.existingName, this.rank, this.duty, this.startDate)
                .subscribe(() => this.dialog.close(), ({ error }) => this.error = error.message);
        }
            
    }
}