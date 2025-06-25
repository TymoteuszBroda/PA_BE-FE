import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { LicenseService } from '../../_services/license.service';
import { License } from '../../_models/License';
import { AssignLicenseDTO } from '../../_models/AssignLicenseDTO';
import { LicenseInstance } from '../../_models/LicenseInstance';

@Component({
  selector: 'app-license-details',
  templateUrl: './license-details.component.html',
  styleUrls: ['./license-details.component.css'],
  imports: [CommonModule, RouterModule],
})
export class LicenseDetailsComponent implements OnInit {
  license: License | null = null;
  assignedUsers: AssignLicenseDTO[] = [];
  instances: LicenseInstance[] = [];
  assignments: { instanceId?: number; validTo: string; assigned: boolean; employeeName?: string }[] = [];
  errorMessage = '';

  constructor(
    private licenseService: LicenseService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.loadLicense(+id);
        this.loadAssignedUsers(+id);
        this.loadInstances(+id);
      }
    });
  }

  loadLicense(id: number): void {
    this.licenseService.getLicenseById(id).subscribe({
      next: license => (this.license = license),
      error: err => {
        console.error('Error loading license', err);
        this.errorMessage = 'Failed to load license details';
      },
    });
  }

  loadAssignedUsers(licenseId: number): void {
    this.licenseService.getEmployeesByLicenseId(licenseId).subscribe({
      next: users => {
        this.assignedUsers = users;
        this.mergeAssignments();
      },
      error: err => {
        console.error('Error loading users', err);
        this.errorMessage = `Failed to load users: ${err.message}`;
      },
    });
  }

  loadInstances(id: number): void {
    this.licenseService.getLicenseInstances(id).subscribe({
      next: instances => {
        this.instances = instances;
        this.mergeAssignments();
      },
      error: err => console.error('Error loading instances', err)
    });
  }

  mergeAssignments(): void {
    if (!this.license) return;
    const seatCount = Math.max(
      this.instances.length,
      this.license.quantity,
      this.assignedUsers.length
    );

    this.assignments = Array.from({ length: seatCount }, (_, idx) => {
      const inst = this.instances[idx];
      const user = this.assignedUsers[idx];
      return {
        instanceId: inst ? inst.id : undefined,
        validTo: inst ? inst.validTo : this.license!.validTo,
        assigned: !!user,
        employeeName: user ? user.employeeName : undefined,
      };
    });
  }

  deleteInstance(instanceId: number | undefined): void {
    if (!instanceId || !this.license) return;
    this.licenseService.deleteLicenseInstanceById(instanceId).subscribe({
      next: () => {
        this.instances = this.instances.filter(i => i.id !== instanceId);
        this.license!.availableLicenses--;
        this.license!.quantity--;
        this.mergeAssignments();
      },
      error: err => console.error('Error deleting instance', err)
    });
  }
}
