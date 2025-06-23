import { Component, OnInit } from '@angular/core';
import { Licence } from '../../_models/Licence';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { LicenceService } from '../../_services/licence.service';
@Component({
  selector: 'app-licence-table',
  imports: [CommonModule],
  templateUrl: './licence-table.component.html',
  styleUrl: './licence-table.component.css',
})
export class LicenceTableComponent {
  licences: Licence[] = [];

  constructor(private licenceService: LicenceService, private router: Router) {}

  ngOnInit() {
    this.licenceService.getLicences().subscribe((data: Licence[]) => {
      this.licences = data;

      console.log(data);
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

    decreaseQuantity(licence: Licence): void {
    if (licence.quantity > 0) {
      const updatedLicence = { ...licence, quantity: licence.quantity - 1 };

      this.licenceService.editLicence(updatedLicence).subscribe(() => {
        licence.quantity--;
      });
    }
  }
}
