import { Component,inject,OnDestroy, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import {
  AdminApiAuthApiClient,
  ForgotPasswordRequest,
  RegisterRequest,
  RegistrationResponse,
} from 'src/app/api/admin-api.service.generated';
import { AlertService } from 'src/app/shared/services/alert.service';
import { UrlConstants } from 'src/app/shared/constants/url.constants';
import { Subject, takeUntil } from 'rxjs';
import { IconDirective } from '@coreui/icons-angular';
import { ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective } from '@coreui/angular';
import { UtilityService } from 'src/app/shared/services/utility.service';

@Component({
    selector: 'app-forgetPassword',
    templateUrl: './forgetPassword.component.html',
    styleUrls: ['./forgetPassword.component.scss'],
    standalone: true,
    imports: [ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective,ReactiveFormsModule]
})
export class ForgetPasswordComponent implements OnDestroy,OnInit{
  forgetPasswordForm: FormGroup;
  private ngUnsubscribe = new Subject<void>();
  constructor(private fb: FormBuilder,
    private utilService: UtilityService,
    private authApiClient: AdminApiAuthApiClient,
    private alertService: AlertService,
    private router: Router) { 
      this.forgetPasswordForm = this.fb.group({
        email: new FormControl('', Validators.required)
      });
    }
    ngOnInit(): void {
    }
    
    ngOnDestroy(): void {
      this.ngUnsubscribe.next();
      this.ngUnsubscribe.complete();
    }
    forgetPassword() {
      let email = this.forgetPasswordForm.controls['email'].value;
      if (email == null || email == '') {
        this.alertService.showError('Email không được để trống');
        return;
      }
      if(!this.utilService.validateEmail(email)){
        this.alertService.showError('Email không đúng định dạng');
        return;
      }

      var request: ForgotPasswordRequest = new ForgotPasswordRequest({
        email: this.forgetPasswordForm.controls['email'].value
      }); 
      this.authApiClient.forgotPassword(request)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (res: any) => {
          //Redirect to dashboard
          this.alertService.showSuccess("Đã gửi link đổi mật khẩu vòa email của bạn");
        },
        error: (error: any) => {         
          this.alertService.showError(error);                
        },
      });
    }
}
