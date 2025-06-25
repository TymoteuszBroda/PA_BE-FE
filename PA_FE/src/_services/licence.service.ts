import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { Licence } from '../_models/Licence';
import { AssignLicenceDTO } from '../_models/AssignLicenceDTO';
import { LicenceInstance } from '../_models/LicenceInstance';
@Injectable({
  providedIn: 'root',
})
export class LicenceService {
  private apiUrl = `${environment.apiUrl}/licence`;

  constructor(private http: HttpClient) {}

  getLicences(): Observable<Licence[]> {
    return this.http.get<Licence[]>(this.apiUrl);
  }

  createLicence(licence: Licence): Observable<Licence> {
    return this.http.post<Licence>(this.apiUrl, licence);
  }

  deleteLicences(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  deleteLicenceInstance(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}/instance`);
  }

  getLicenceById(id: number): Observable<Licence> {
    return this.http.get<Licence>(`${this.apiUrl}/${id}`);
  }

  editLicence(licence: Licence): Observable<Licence> {
    return this.http.put<Licence>(`${this.apiUrl}/${licence.id}`, licence);
  }

  getLicencesByEmployeeId(employeeId: number): Observable<AssignLicenceDTO[]> {
    return this.http.get<AssignLicenceDTO[]>(
      `${this.apiUrl}/assigned-licences/employee/${employeeId}`
    );
  }

  getEmployeesByLicenceId(licenceId: number): Observable<AssignLicenceDTO[]> {
    return this.http.get<AssignLicenceDTO[]>(
      `${this.apiUrl}/assigned-licences/licence/${licenceId}`
    );
  }

  deleteAssignedLicence(assignmentId: number): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/assigned-licences/${assignmentId}`
    );
  }

  getLicenceInstances(licenceId: number): Observable<LicenceInstance[]> {
    return this.http.get<LicenceInstance[]>(`${this.apiUrl}/${licenceId}/instances`);
  }

  assignLicence(assignDto: AssignLicenceDTO): Observable<AssignLicenceDTO> {
    return this.http.post<AssignLicenceDTO>(
      `${this.apiUrl}/assign-licence`,
      assignDto
    );
  }
}
