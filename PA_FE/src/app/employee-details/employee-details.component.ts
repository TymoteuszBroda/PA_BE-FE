import { Component, OnInit } from '@angular/core';
import { Employee } from '../../_models/Employee';
import { AssignLicenseDTO } from '../../_models/AssignLicenseDTO';
import { EmployeeService } from '../../_services/employee.service';
import { LicenseService } from '../../_services/license.service';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-employee-details',
  templateUrl: './employee-details.component.html',
  imports: [CommonModule, RouterModule],
  styleUrls: ['./employee-details.component.css'],
})
export class EmployeeDetailsComponent implements OnInit {
  employee: Employee = {
    id: 0,
    firstName: '',
    lastName: '',
    phone: '',
    email: '',
    position: '',
  };

  assignedLicenses: AssignLicenseDTO[] = [];
  errorMessage = '';

  constructor(
    private employeeService: EmployeeService,
    private licenseService: LicenseService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      const id = params.get('id');
      if (id) {
        this.loadEmployee(+id);
        this.loadAssignedLicenses(+id);
      }
    });
  }

  loadEmployee(id: number): void {
    this.employeeService.getEmployeeById(id).subscribe({
      next: (employee) => (this.employee = employee),
      error: (err) => {
        console.error('Error loading employee:', err);
        this.errorMessage = 'Failed to load employee details';
      },
    });
  }

  loadAssignedLicenses(employeeId: number): void {
    this.licenseService.getLicensesByEmployeeId(employeeId).subscribe({
      next: (licenses) => (this.assignedLicenses = licenses),
      error: (err) =>
        (this.errorMessage = `Error loading licenses: ${err.message}`),
    });
  }

  onDeleteAssignment(assignmentId: number): void {
    if (confirm('Are you sure you want to remove this license assignment?')) {
      this.licenseService.deleteAssignedLicense(assignmentId).subscribe({
        next: () => {
          this.assignedLicenses = this.assignedLicenses.filter(
            (al) => al.id !== assignmentId
          );
        },
        error: (err) => {
          this.errorMessage = `Error deleting assignment: ${err.message}`;
        },
      });
    }
  }

  isExpiringSoon(validTo?: string): boolean {
    if (!validTo) {
      return false;
    }
    const expiry = new Date(validTo);
    const now = new Date();
    const twoWeeksAhead = new Date();
    twoWeeksAhead.setDate(now.getDate() + 14);
    return expiry <= twoWeeksAhead;
  }
}
