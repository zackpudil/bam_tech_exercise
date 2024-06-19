import { Component, OnInit, inject } from '@angular/core';
import { StargateService } from '../../services/stargate.service';
import { Observable, tap } from 'rxjs';
import { PersonAstronaut } from '../../models/PersonAstronaut';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AddPersonDialogComponent } from '../add-person/add-person.component';
import { ViewDutyHistoryComponent } from '../view-duty-history/view-duty-history.component';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatDividerModule } from '@angular/material/divider';

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIcon,
    MatDialogModule,
    MatProgressSpinner,
    MatDividerModule
  ],
  providers: [StargateService]
})
export class AppComponent implements OnInit {

  readonly dialog = inject(MatDialog);
  constructor(private stargateService: StargateService) {}

  personAstronauts$: Observable<PersonAstronaut[]> = new Observable<PersonAstronaut[]>();
  loading: boolean = true;

  ngOnInit(): void {
    this.personAstronauts$ = this.stargateService.getAstronauts()
      .pipe(tap(() => this.loading = false));
  }

  addOrUpdate(isNew: boolean, name: string = '') {
    const ref = this.dialog.open(AddPersonDialogComponent, {
      data: {
        isNew,
        existingName: name
      }
    });

    ref.afterClosed().subscribe(() => {
      this.loading = true;
      this.personAstronauts$ = this.stargateService.getAstronauts()
        .pipe(tap(() => this.loading = false)); 
    });
  }

  viewDutyHistory(name: string) {
    this.dialog.open(ViewDutyHistoryComponent, {
      data: name
    });
  }

}
