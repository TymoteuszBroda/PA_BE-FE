import { Routes } from '@angular/router';
import { EmployeeTableComponent } from './employee-table/employee-table.component';
import { EmployeeFormComponent } from './employee-form/employee-form.component';
import { LicenceTableComponent } from './licence-table/licence-table.component';
import { LicenceFormComponent } from './licence-form/licence-form.component';
import { EmployeeDetailsComponent } from './employee-details/employee-details.component';
import { AssignLicenseComponent } from './assign-license/assign-license.component';

export const routes: Routes = [
  { path: '', component: EmployeeTableComponent },
  { path: 'create', component: EmployeeFormComponent },
  { path: 'licences', component: LicenceTableComponent },
  { path: 'editEmployee/:id', component: EmployeeFormComponent },
  { path: 'editLicence/:id', component: LicenceFormComponent },
  { path: 'createLicence', component: LicenceFormComponent },
  { path: 'employeeDetails/:id', component: EmployeeDetailsComponent },
  { path: 'assign-license/:employeeId', component: AssignLicenseComponent},]
;
