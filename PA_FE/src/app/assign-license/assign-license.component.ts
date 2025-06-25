import { Component, OnInit } from '@angular/core';
import { License } from '../../_models/License';
import { AssignLicenseDTO } from '../../_models/AssignLicenseDTO';
import { LicenseService } from '../../_services/license.service';
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
  licenses: License[] = [];
  employeeId!: number;
  selectedLicenseId: number | null = null;
  errorMessage = '';

  constructor(
    private licenseService: LicenseService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      const id = params.get('employeeId');
      if (id) {
        this.employeeId = +id;
        this.loadAvailableLicenses();
      }
    });
  }

  loadAvailableLicenses(): void {
    this.licenseService.getLicenses().subscribe({
      next: (licenses) => {
        this.licenses = licenses.filter((license) => license.availableLicenses > 0);
      },
      error: (err) => {
        console.error('Error loading licenses:', err);
        this.errorMessage = 'Failed to load available licenses.';
      },
    });
  }

  assignLicense(): void {
    if (!this.selectedLicenseId) return;

    const assignDto: AssignLicenseDTO = {
      employeeId: this.employeeId,
      licenseId: this.selectedLicenseId,
      employeeName: '',
      licenseName: '',
      id: 0,
    };

    this.licenseService.assignLicense(assignDto).subscribe({
      next: () => {
        const assignedLicense = this.licenses.find(
          (l) => l.id === this.selectedLicenseId
        );
        if (assignedLicense) {
          assignedLicense.availableLicenses--;

          if (assignedLicense.availableLicenses === 0) {
            this.licenses = this.licenses.filter(
              (l) => l.id !== this.selectedLicenseId
            );
          }
        }

        this.selectedLicenseId = null;
      },
      error: (err) => {
        this.errorMessage = `Error assigning license: ${err.message}`;
      },
    });
  }
}
