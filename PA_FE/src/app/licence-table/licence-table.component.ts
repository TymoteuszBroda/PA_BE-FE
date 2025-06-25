import { Component, OnInit } from '@angular/core';
import { Licence } from '../../_models/Licence';
import { LicenceInstance } from '../../_models/LicenceInstance';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { LicenceService } from '../../_services/licence.service';
@Component({
  selector: 'app-licence-table',
  imports: [CommonModule],
  templateUrl: './licence-table.component.html',
  styleUrl: './licence-table.component.css',
})
export class LicenceTableComponent implements OnInit {
  licences: Licence[] = [];
  expiryAlerts: { [key: number]: boolean } = {};

  constructor(private licenceService: LicenceService, private router: Router) {}

  ngOnInit() {
    this.licenceService.getLicences().subscribe((data: Licence[]) => {
      this.licences = data;
      for (const licence of data) {
        this.licenceService.getLicenceInstances(licence.id).subscribe({
          next: (instances: LicenceInstance[]) => {
            this.expiryAlerts[licence.id] = instances.some((inst) =>
              this.isDateWithinTwoWeeks(inst.validTo)
            );
          },
          error: () => {
            this.expiryAlerts[licence.id] = this.isDateWithinTwoWeeks(licence.validTo);
          },
        });
      }
    });
  }
  deleteLicences(id: number): void {
    this.licenceService.deleteLicences(id).subscribe({
      next: (response) => {
        this.licences = this.licences.filter((e) => e.id !== id);
      },
      error: (err) => {
        console.error('Error deleting licence', err);
      },
    });
  }
  editLicence(id: number): void{
    this.router.navigate(['editLicence/', id]);
  }

  showLicenceDetails(id: number): void {
    this.router.navigate(['licenceDetails/', id]);
  }

  decreaseQuantity(licence: Licence): void {
    if (licence.availableLicences > 0) {
      this.licenceService.deleteLicenceInstance(licence.id).subscribe(() => {
        licence.availableLicences--;
        licence.quantity--;
      });
    }
  }

  private isDateWithinTwoWeeks(date: string): boolean {
    const expiry = new Date(date);
    const now = new Date();
    const twoWeeksAhead = new Date();
    twoWeeksAhead.setDate(now.getDate() + 14);
    return expiry <= twoWeeksAhead;
  }

  isExpiringSoon(id: number): boolean {
    return this.expiryAlerts[id];
  }
}
