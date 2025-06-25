import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { License } from '../_models/License';
import { AssignLicenseDTO } from '../_models/AssignLicenseDTO';
import { LicenseInstance } from '../_models/LicenseInstance';
@Injectable({
  providedIn: 'root',
})
export class LicenseService {
  private apiUrl = `${environment.apiUrl}/license`;

  constructor(private http: HttpClient) {}

  getLicenses(): Observable<License[]> {
    return this.http.get<License[]>(this.apiUrl);
  }

  createLicense(license: License): Observable<License> {
    return this.http.post<License>(this.apiUrl, license);
  }

  deleteLicenses(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  deleteLicenseInstance(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}/instance`);
  }

  deleteLicenseInstanceById(instanceId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/instance/${instanceId}`);
  }

  getLicenseById(id: number): Observable<License> {
    return this.http.get<License>(`${this.apiUrl}/${id}`);
  }

  editLicense(license: License): Observable<License> {
    return this.http.put<License>(`${this.apiUrl}/${license.id}`, license);
  }

  getLicensesByEmployeeId(employeeId: number): Observable<AssignLicenseDTO[]> {
    return this.http.get<AssignLicenseDTO[]>(
      `${this.apiUrl}/assigned-licenses/employee/${employeeId}`
    );
  }

  getEmployeesByLicenseId(licenseId: number): Observable<AssignLicenseDTO[]> {
    return this.http.get<AssignLicenseDTO[]>(
      `${this.apiUrl}/assigned-licenses/license/${licenseId}`
    );
  }

  deleteAssignedLicense(assignmentId: number): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/assigned-licenses/${assignmentId}`
    );
  }

  getLicenseInstances(licenseId: number): Observable<LicenseInstance[]> {
    return this.http.get<LicenseInstance[]>(`${this.apiUrl}/${licenseId}/instances`);
  }

  assignLicense(assignDto: AssignLicenseDTO): Observable<AssignLicenseDTO> {
    return this.http.post<AssignLicenseDTO>(
      `${this.apiUrl}/assign-license`,
      assignDto
    );
  }
}
