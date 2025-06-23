import { Component, OnInit } from '@angular/core';
import { Employee } from '../../_models/Employee';
import { AssignLicenceDTO } from '../../_models/AssignLicenceDTO';
import { EmployeeService } from '../../_services/employee.service';
import { LicenceService } from '../../_services/licence.service';
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

  assignedLicences: AssignLicenceDTO[] = [];
  errorMessage = '';

  constructor(
    private employeeService: EmployeeService,
    private licenceService: LicenceService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      const id = params.get('id');
      if (id) {
        this.loadEmployee(+id);
        this.loadAssignedLicences(+id);
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

  loadAssignedLicences(employeeId: number): void {
    this.licenceService.getLicencesByEmployeeId(employeeId).subscribe({
      next: (licences) => (this.assignedLicences = licences),
      error: (err) =>
        (this.errorMessage = `Error loading licences: ${err.message}`),
    });
  }

  onDeleteAssignment(assignmentId: number): void {
    if (confirm('Are you sure you want to remove this licence assignment?')) {
      this.licenceService.deleteAssignedLicence(assignmentId).subscribe({
        next: () => {
          this.assignedLicences = this.assignedLicences.filter(
            (al) => al.id !== assignmentId
          );
        },
        error: (err) => {
          this.errorMessage = `Error deleting assignment: ${err.message}`;
        },
      });
    }
  }
}
