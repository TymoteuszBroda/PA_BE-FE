import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { LicenceService } from '../../_services/licence.service';
import { Licence } from '../../_models/Licence';
import { AssignLicenceDTO } from '../../_models/AssignLicenceDTO';

@Component({
  selector: 'app-licence-details',
  templateUrl: './licence-details.component.html',
  styleUrls: ['./licence-details.component.css'],
  imports: [CommonModule, RouterModule],
})
export class LicenceDetailsComponent implements OnInit {
  licence: Licence | null = null;
  assignedUsers: AssignLicenceDTO[] = [];
  errorMessage = '';

  constructor(
    private licenceService: LicenceService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.loadLicence(+id);
        this.loadAssignedUsers(+id);
      }
    });
  }

  loadLicence(id: number): void {
    this.licenceService.getLicenceById(id).subscribe({
      next: licence => (this.licence = licence),
      error: err => {
        console.error('Error loading licence', err);
        this.errorMessage = 'Failed to load licence details';
      },
    });
  }

  loadAssignedUsers(licenceId: number): void {
    this.licenceService.getEmployeesByLicenceId(licenceId).subscribe({
      next: users => (this.assignedUsers = users),
      error: err => {
        console.error('Error loading users', err);
        this.errorMessage = `Failed to load users: ${err.message}`;
      },
    });
  }
}
