import { Component, inject } from '@angular/core';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TitleCasePipe } from '@angular/common';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterModule, FormsModule, BsDropdownModule, TitleCasePipe],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'employee-management-app';
  accountService = inject(AccountService);
  private router = inject(Router);
  private toastr = inject(ToastrService)

  model : any = {};

  login(){
    this.accountService.login(this.model).subscribe({
      next: (response) =>{
        this.router.navigateByUrl('/')
      },
      error: (error) =>{
        this.toastr.error(error.error)
      },
    });
  }

  logout(){
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
