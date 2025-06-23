import { Component, OnInit } from '@angular/core';
import { Licence } from '../../_models/Licence';
import { AssignLicenceDTO } from '../../_models/AssignLicenceDTO';
import { LicenceService } from '../../_services/licence.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-assign-license',
  templateUrl: './assign-license.component.html',
  styleUrls: ['./assign-license.component.css'],
  imports: [CommonModule, FormsModule],
})
export class AssignLicenseComponent implements OnInit {
  licences: Licence[] = [];
  employeeId!: number;
  selectedLicenceId: number | null = null;
  errorMessage = '';

  constructor(
    private licenceService: LicenceService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      const id = params.get('employeeId');
      if (id) {
        this.employeeId = +id;
        this.loadAvailableLicences();
      }
    });
  }

  loadAvailableLicences(): void {
    this.licenceService.getLicences().subscribe({
      next: (licences) => {
        this.licences = licences.filter((licence) => licence.quantity > 0);
      },
      error: (err) => {
        console.error('Error loading licenses:', err);
        this.errorMessage = 'Failed to load available licenses.';
      },
    });
  }

  assignLicence(): void {
    if (!this.selectedLicenceId) return;

    const assignDto: AssignLicenceDTO = {
      employeeId: this.employeeId,
      licenceId: this.selectedLicenceId,
      employeeName: '',
      licenceName: '',
      id: 0,
    };

    this.licenceService.assignLicence(assignDto).subscribe({
      next: () => {
        const assignedLicence = this.licences.find(
          (l) => l.id === this.selectedLicenceId
        );
        if (assignedLicence) {
          assignedLicence.quantity--;

          if (assignedLicence.quantity === 0) {
            this.licences = this.licences.filter(
              (l) => l.id !== this.selectedLicenceId
            );
          }
        }

        this.selectedLicenceId = null;
      },
      error: (err) => {
        this.errorMessage = `Error assigning license: ${err.message}`;
      },
    });
  }
}
