import { Routes } from '@angular/router';
import { EmployeeTableComponent } from './employee-table/employee-table.component';
import { EmployeeFormComponent } from './employee-form/employee-form.component';
import { LicenseTableComponent } from './license-table/license-table.component';
import { LicenseFormComponent } from './license-form/license-form.component';
import { EmployeeDetailsComponent } from './employee-details/employee-details.component';
import { AssignLicenseComponent } from './assign-license/assign-license.component';
import { LicenseDetailsComponent } from './license-details/license-details.component';

export const routes: Routes = [
  { path: '', component: EmployeeTableComponent },
  { path: 'create', component: EmployeeFormComponent },
  { path: 'licenses', component: LicenseTableComponent },
  { path: 'editEmployee/:id', component: EmployeeFormComponent },
  { path: 'editLicense/:id', component: LicenseFormComponent },
  { path: 'createLicense', component: LicenseFormComponent },
  { path: 'employeeDetails/:id', component: EmployeeDetailsComponent },
  { path: 'licenseDetails/:id', component: LicenseDetailsComponent },
  { path: 'assign-license/:employeeId', component: AssignLicenseComponent},]
;
