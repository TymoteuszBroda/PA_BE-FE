import { Component, OnInit } from '@angular/core';
import { License } from '../../_models/License';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { LicenseService } from '../../_services/license.service';
@Component({
  selector: 'app-license-table',
  imports: [CommonModule],
  templateUrl: './license-table.component.html',
  styleUrl: './license-table.component.css',
})
export class LicenseTableComponent implements OnInit {
  licenses: License[] = [];

  constructor(private licenseService: LicenseService, private router: Router) {}

  ngOnInit() {
    this.licenseService.getLicenses().subscribe((data: License[]) => {
      this.licenses = data;

      console.log(data);
    });
  }
  deleteLicenses(id: number): void {
    this.licenseService.deleteLicenses(id).subscribe({
      next: (response) => {
        this.licenses = this.licenses.filter((e) => e.id !== id);
      },
      error: (err) => {
        console.error('Error deleting license', err);
      },
    });
  }
  editLicense(id: number): void{
    this.router.navigate(['editLicense/', id]);
  }

  showLicenseDetails(id: number): void {
    this.router.navigate(['licenseDetails/', id]);
  }

  decreaseQuantity(license: License): void {
    if (license.availableLicenses > 0) {
      this.licenseService.deleteLicenseInstance(license.id).subscribe(() => {
        license.availableLicenses--;
        license.quantity--;
      });
    }
  }

  isExpiringSoon(validTo: string): boolean {
    const expiry = new Date(validTo);
    const now = new Date();
    const twoWeeksAhead = new Date();
    twoWeeksAhead.setDate(now.getDate() + 14);
    return expiry <= twoWeeksAhead;
  }
}
