import { Component,OnDestroy, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import {
  AdminApiAuthApiClient,
  RegisterRequest,
  RegistrationResponse,
} from 'src/app/api/admin-api.service.generated';
import { AlertService } from 'src/app/shared/services/alert.service';
import { UrlConstants } from 'src/app/shared/constants/url.constants';
import { Subject, takeUntil } from 'rxjs';
import { IconDirective } from '@coreui/icons-angular';
import { ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective } from '@coreui/angular';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.scss'],
    standalone: true,
    imports: [ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective,ReactiveFormsModule]
})
export class RegisterComponent implements OnDestroy,OnInit{
  registerForm: FormGroup;
  private ngUnsubscribe = new Subject<void>();
  constructor(private fb: FormBuilder,
    private authApiClient: AdminApiAuthApiClient,
    private alertService: AlertService,
    private router: Router) { 
      this.registerForm = this.fb.group({
        userName: new FormControl('', Validators.required),
        password: new FormControl('', Validators.required),
        email: new FormControl('', Validators.required),
        confirmPassword: new FormControl('', Validators.required),
      });
    }
    ngOnInit(): void {
      
    }
    
    ngOnDestroy(): void {
      this.ngUnsubscribe.next();
      this.ngUnsubscribe.complete();
    }
    register() {
      var request: RegisterRequest = new RegisterRequest({
        userName: this.registerForm.controls['userName'].value,
        password: this.registerForm.controls['password'].value,
        email: this.registerForm.controls['email'].value,
        confirmPassword: this.registerForm.controls['confirmPassword'].value,
      });
  
      this.authApiClient.register(request)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (res: RegistrationResponse) => {
          //Redirect to dashboard
          this.router.navigate([UrlConstants.LOGIN]);
  
        },
        error: (error: any) => {
          console.log(error);
          this.alertService.showError('Có lỗi xảy ra');
        },
      });
    }

}
