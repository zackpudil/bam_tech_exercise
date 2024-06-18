import { Component, OnInit, inject } from '@angular/core';
import { StargateService } from '../../services/stargate.service';
import { Observable } from 'rxjs';
import { PersonAstronaut } from '../../models/PersonAstronaut';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AddPersonDialogComponent } from '../add-person/add-person.component';
import { ViewDutyHistoryComponent } from '../view-duty-history/view-duty-history.component';

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  imports: [CommonModule, MatCardModule, MatButtonModule, MatIcon, MatDialogModule],
  providers: [StargateService]
})
export class AppComponent implements OnInit {
  public personAstronauts$: Observable<PersonAstronaut[]> = new Observable<PersonAstronaut[]>();

  readonly dialog = inject(MatDialog);

  constructor(private stargateService: StargateService) {}


  ngOnInit(): void {
    this.personAstronauts$ = this.stargateService.getAstronauts();
  }

  addOrUpdate(isNew: boolean = false, name: string = '') {
    const ref = this.dialog.open(AddPersonDialogComponent, {
      data: {
        isNew,
        existingName: name
      }
    });

    ref.afterClosed().subscribe(() => {
      this.personAstronauts$ = this.stargateService.getAstronauts(); 
    });
  }

  viewDutyHistory(name: string) {
    this.dialog.open(ViewDutyHistoryComponent, {
      data: name
    });
  }

}
