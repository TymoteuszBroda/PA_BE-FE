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
  };

  isEditing: boolean = false;
  errorMessage = '';

  constructor(
    private licenceService: LicenceService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      const id = params.get('id');
      if (id) {
        this.isEditing = true;
        this.licenceService.getLicenceById(parseInt(id)).subscribe({
          next: (result) => {
            this.licence = result;
          },
          error: (err) => {
            console.error('Error loading licence', err);
          },
        });
      }
    });
  }

  onSubmit(): void {
    if (!this.isEditing) {
      this.licenceService.createLicence(this.licence).subscribe({
        next: (response) => {
          this.router.navigate(['/']); // Adjust the navigation path as needed
        },
        error: (err) => {
          console.error(err);
          this.errorMessage = `Error during creation: ${err.status} - ${err.message}`;
        },
      });
    } else {
      this.licenceService.editLicence(this.licence).subscribe({
        next: (response) => {
          this.router.navigate(['/']); // Adjust the navigation path as needed
        },
        error: (err) => {
          console.error(err);
          this.errorMessage = `Error during update: ${err.status} - ${err.message}`;
        },
      });
    }
  }
}
