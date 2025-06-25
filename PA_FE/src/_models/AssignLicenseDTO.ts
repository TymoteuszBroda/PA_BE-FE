export interface AssignLicenseDTO {
  id: number;
  employeeId: number;
  licenseId: number;
  employeeName: string;
  licenseName: string;
  assignedOn?: string;
  validTo?: string;
}
