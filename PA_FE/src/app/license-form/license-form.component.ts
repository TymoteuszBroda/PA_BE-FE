import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { License } from '../../_models/License';
import { LicenseService } from '../../_services/license.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-license-form',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './license-form.component.html',
  styleUrl: './license-form.component.css',
})
export class LicenseFormComponent implements OnInit {
  license: License = {
    id: 0,
    applicationName: '',
    availableLicenses: 0,
    quantity: 0,
    validTo: new Date(Date.now() + 365 * 24 * 60 * 60 * 1000)
      .toISOString()
      .substring(0, 10)
  };

  isEditing: boolean = false;
  errorMessage = '';

  existingNames: string[] = [];

  constructor(
    private licenseService: LicenseService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.licenseService.getLicenses().subscribe(ls => {
      this.existingNames = ls.map(l => l.applicationName);
    });
    this.route.paramMap.subscribe((params) => {
      const id = params.get('id');
      if (id) {
        this.isEditing = true;
        this.licenseService.getLicenseById(parseInt(id)).subscribe({
          next: (result) => {
            this.license = {
              ...result,
              validTo: result.validTo.substring(0, 10),
            };
          },
          error: (err) => {
            console.error('Error loading license', err);
          },
        });
      }
    });
  }

  onNameChange(value: string): void {
    if (value === 'New license name') {
      this.license.applicationName = '';
    } else if (this.existingNames.includes(value)) {
      this.license.applicationName = value;
    }
  }

  onSubmit(): void {
    if (!this.isEditing) {
      this.licenseService.createLicense(this.license).subscribe({
        next: (response) => {
          this.router.navigate(['/']);
        },
        error: (err) => {
          console.error(err);
          this.errorMessage = `Error during creation: ${err.status} - ${err.message}`;
        },
      });
    } else {
      this.licenseService.editLicense(this.license).subscribe({
        next: (response) => {
          this.router.navigate(['/']);
        },
        error: (err) => {
          console.error(err);
          this.errorMessage = `Error during update: ${err.status} - ${err.message}`;
        },
      });
    }
  }
}
