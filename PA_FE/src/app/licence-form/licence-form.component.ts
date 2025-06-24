import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Licence } from '../../_models/Licence';
import { LicenceService } from '../../_services/licence.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-licence-form',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './licence-form.component.html',
  styleUrl: './licence-form.component.css',
})
export class LicenceFormComponent implements OnInit {
  licence: Licence = {
    id: 0,
    applicationName: '',
    quantity: 0,
    validTo: new Date(Date.now() + 365 * 24 * 60 * 60 * 1000)
      .toISOString()
      .substring(0, 10)
  };

  isEditing: boolean = false;
  errorMessage = '';

  existingNames: string[] = [];

  constructor(
    private licenceService: LicenceService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.licenceService.getLicences().subscribe(ls => {
      this.existingNames = ls.map(l => l.applicationName);
    });
    this.route.paramMap.subscribe((params) => {
      const id = params.get('id');
      if (id) {
        this.isEditing = true;
        this.licenceService.getLicenceById(parseInt(id)).subscribe({
          next: (result) => {
            this.licence = {
              ...result,
              validTo: result.validTo.substring(0, 10),
            };
          },
          error: (err) => {
            console.error('Error loading licence', err);
          },
        });
      }
    });
  }

  onNameChange(value: string): void {
    if (value === 'New license name') {
      this.licence.applicationName = '';
    } else if (this.existingNames.includes(value)) {
      this.licence.applicationName = value;
    }
  }

  onSubmit(): void {
    if (!this.isEditing) {
      this.licenceService.createLicence(this.licence).subscribe({
        next: (response) => {
          // Navigate back to the home page after creating a licence
          this.router.navigate(['/']);
        },
        error: (err) => {
          console.error(err);
          this.errorMessage = `Error during creation: ${err.status} - ${err.message}`;
        },
      });
    } else {
      this.licenceService.editLicence(this.licence).subscribe({
        next: (response) => {
          // Navigate back to the home page after editing a licence
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
