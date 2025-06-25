import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { LicenceService } from '../../_services/licence.service';
import { Licence } from '../../_models/Licence';
import { AssignLicenceDTO } from '../../_models/AssignLicenceDTO';
import { LicenceInstance } from '../../_models/LicenceInstance';

@Component({
  selector: 'app-licence-details',
  templateUrl: './licence-details.component.html',
  styleUrls: ['./licence-details.component.css'],
  imports: [CommonModule, RouterModule],
})
export class LicenceDetailsComponent implements OnInit {
  licence: Licence | null = null;
  assignedUsers: AssignLicenceDTO[] = [];
  instances: LicenceInstance[] = [];
  assignments: { instanceId?: number; validTo: string; assigned: boolean; employeeName?: string }[] = [];
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
        this.loadInstances(+id);
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
    this.licenceService.getLicenceInstances(id).subscribe({
      next: instances => {
        this.instances = instances;
        this.mergeAssignments();
      },
      error: err => console.error('Error loading instances', err)
    });
  }

  mergeAssignments(): void {
    if (!this.licence) return;
    const seatCount = Math.max(
      this.instances.length,
      this.licence.quantity,
      this.assignedUsers.length
    );

    this.assignments = Array.from({ length: seatCount }, (_, idx) => {
      const inst = this.instances[idx];
      const user = this.assignedUsers[idx];
      return {
        instanceId: inst ? inst.id : undefined,
        validTo: inst ? inst.validTo : this.licence!.validTo,
        assigned: !!user,
        employeeName: user ? user.employeeName : undefined,
      };
    });
  }

  deleteInstance(instanceId: number | undefined): void {
    if (!instanceId || !this.licence) return;
    this.licenceService.deleteLicenceInstanceById(instanceId).subscribe({
      next: () => {
        this.instances = this.instances.filter(i => i.id !== instanceId);
        this.licence!.availableLicences--;
        this.licence!.quantity--;
        this.mergeAssignments();
      },
      error: err => console.error('Error deleting instance', err)
    });
  }
}
